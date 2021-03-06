﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iExchange.Common;
using System.Xml;
using System.Diagnostics;
using System.Threading;
using System.Collections;
using System.Data;
using Trader.Server.Bll;
using Trader.Server.Util;
using Trader.Server.TypeExtension;
using Trader.Common;
using log4net;
using System.Xml.Linq;
using System.Collections.Concurrent;
using Trader.Server.SessionNamespace;
namespace Trader.Server.Service
{
    public class CommandManager
    {
        public static readonly CommandManager Default = new CommandManager();
        private CommandQueue _Commands;
        private ILog _Logger = LogManager.GetLogger(typeof(CommandManager));
        private AutoResetEvent _Event = new AutoResetEvent(false);
        private AutoResetEvent _StopEvent = new AutoResetEvent(false);
        private AutoResetEvent[] _Events;
        private ConcurrentQueue<Command> _Queue = new ConcurrentQueue<Command>();
        private volatile bool _IsStop;
        private volatile bool _IsStart;

        private CommandManager()
        {
            this._Commands= new CommandQueue(new TimeSpan(0, 20, 0));
            this._Events = new[] { this._Event,this._StopEvent};
          
        }


        public void Start()
        {
            if (this._IsStart)
            {
                return;
            }
            Thread thread = new Thread(ProcessCommand);
            thread.IsBackground = true;
            thread.Start();
            this._IsStart = true;
        }

        public void Stop()
        {
            this._IsStop = true;
            this._StopEvent.Set();
        }

        public void Send(Command command)
        {
            this._Queue.Enqueue(command);
            this._Event.Set();
        }

        private void ProcessCommand()
        {
            while (true)
            {
                if (this._IsStop)
                {
                    break;
                }
                WaitHandle.WaitAny(this._Events);
                Command command;
                while (this._Queue.TryDequeue(out command))
                {
                    QuotationCommand quotation = command as QuotationCommand;
                    if (quotation != null)
                    {
                        QuotationDispatcher.Default.Add(quotation);
#if TEST
                        QuotationTester.Default.Add();
#else
#endif
                    }
                    else
                    {
                        CompositeCommand compositeCommand = command as CompositeCommand;
                        if (compositeCommand != null)
                        {
                            foreach (var cmd in compositeCommand.Commands)
                            {
                                this._Commands.Add(cmd);
                                Application.Default.AgentController.SendCommand(command: cmd);
                            }
                        }
                        else
                        {
                            this._Commands.Add(command);
                            Application.Default.AgentController.SendCommand(command: command);
                        }
                    }
                }
            }
        }



        public int LastSequence
        {
            get
            {
                return this._Commands.LastSequence;
            }
        }

        public void AddCommand(Command command)
        {
            this.Send(command);
        }


        public void AddQuotation(QuotationCommand quotation)
        {
            this._Commands.Add(quotation);
            Application.Default.AgentController.SendCommand(quotation);
        }


        public XmlNode VerifyRefrence(Session session,State state, XmlNode xmlCommands, out bool changed)
        {
            changed = false;
            if (!(state is TraderState)) return xmlCommands;

            TraderState tradingConsoleState = (TraderState)state;

            List<Guid> instrumentIDs = new List<Guid>();

            try
            {
                foreach (XmlElement xmlElement in xmlCommands.ChildNodes)
                {
                    XmlElement tran = null;
                    Guid instrumentID = Guid.Empty;
                    switch (xmlElement.Name)
                    {
                        case "Execute2":
                            tran = (XmlElement)xmlElement.GetElementsByTagName("Transaction")[0];
                            instrumentID = XmlConvert.ToGuid(tran.Attributes["InstrumentID"].Value);
                            if (!tradingConsoleState.InstrumentsView.ContainsKey(instrumentID))
                            {
                                instrumentIDs.Add(instrumentID);
                                changed = true;
                            }
                            break;
                        case "Place":
                            tran = (XmlElement)xmlElement.GetElementsByTagName("Transaction")[0];
                            instrumentID = XmlConvert.ToGuid(tran.Attributes["InstrumentID"].Value);
                            if (!tradingConsoleState.Instruments.ContainsKey(instrumentID))
                            {
                                instrumentIDs.Add(instrumentID);
                                changed = true;
                            }
                            break;
                    }
                }

                if (instrumentIDs.Count > 0)
                {
                    DataSet dataSet = this.GetInstruments(session,instrumentIDs);
                    XmlDocument xmlDocument = xmlCommands.OwnerDocument;
                    XmlElement parameters = xmlDocument.CreateElement("Parameters");

                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    System.IO.StringWriter instrumentWriter = new System.IO.StringWriter(stringBuilder);
                    System.Xml.XmlWriter instrumentXmlWriter = new XmlTextWriter(instrumentWriter);

                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(DataSet));
                    xmlSerializer.Serialize(instrumentXmlWriter, dataSet);

                    parameters.SetAttribute("DataSet", stringBuilder.ToString());
                    xmlCommands.InsertBefore(parameters, xmlCommands.FirstChild);
                    changed = true;
                }
            }
            catch (System.Exception ex)
            {
                _Logger.Error(ex);
            }

            return xmlCommands;
        }
        private XmlNode VerifyRefrence(Session session,State state, XmlNode xmlCommands)
        {
            bool changed;
            return this.VerifyRefrence(session,state, xmlCommands, out changed);
        }

        public DataSet GetInstruments(Session session,List<Guid> instrumentIDs)
        {
            try
            {
                TradingConsoleState state = SessionManager.Default.GetTradingConsoleState(session);
                Token token = SessionManager.Default.GetToken(session);
                DataSet dataSet = Application.Default.TradingConsoleServer.GetInstruments(token, instrumentIDs, state);
                return dataSet;
            }
            catch (System.Exception exception)
            {
                _Logger.Error(exception);
                return null;
            }
        }


        public XElement GetLostCommands(Session session,int firstSequence, int lastSequence)
        {
            XmlNode xmlCommands = null;
            try
            {
                State state = SessionManager.Default.GetTradingConsoleState(session);
                Token token = SessionManager.Default.GetToken(session);
                if (Command.CompareSequence(firstSequence, lastSequence) > 0 )
                {
                    _Logger.Warn(string.Format("{0},range == [{1},{2}]", token, firstSequence, lastSequence));
                    return null;
                }
                xmlCommands = this._Commands.GetCommands(token, state, firstSequence, lastSequence);
                xmlCommands = this.VerifyRefrence(session, state, xmlCommands);
                _Logger.Warn(string.Format("{0},  range == [{1},{2}]\n{3}", token, firstSequence, lastSequence,
                    xmlCommands.OuterXml));
                return XmlResultHelper.NewResult(xmlCommands.OuterXml);
            }
            catch (System.Exception ex)
            {
                _Logger.Error(ex);
                return XmlResultHelper.NewErrorResult();
            }
           
        }

        
    }
}

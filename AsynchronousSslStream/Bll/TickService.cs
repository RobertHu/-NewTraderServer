﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using iExchange.Common;
using log4net;
using Trader.Server.SessionNamespace;
using System.Data;
using Trader.Server.Util;
using Trader.Server.TypeExtension;
using System.Net;
using System.Diagnostics;
using Trader.Server.Service;
using System.Runtime.Serialization.Json;
using System.IO;
using Trader.Common;
namespace Trader.Server.Bll
{
    public static class TickService
    {
        private static ILog _Logger = LogManager.GetLogger(typeof (TickService));
        public static XElement GetTickByTickHistoryData(Session session, Guid instrumentId, DateTime from, DateTime to)
        {
            TraderState state = SessionManager.Default.GetTradingConsoleState(session);
            XElement  result = null;
            if (state.InstrumentsView.ContainsKey(instrumentId))
            {
                Guid quotePolicyId = state.InstrumentsView[instrumentId];
                DataSet ds = Application.Default.TradingConsoleServer.GetTickByTickHistoryDatas2(instrumentId, quotePolicyId, from, to);
                result = XmlResultHelper.NewResult( ds.ToXml());
            }
            else
            {
                result = XmlResultHelper.NewResult(string.Empty);
            }
            return result;
        }


        public static XElement  GetChartData(Guid asyncResultId)
        {
            try
            {
                DataSet ds = (DataSet)Application.Default.AsyncResultManager.GetResult(asyncResultId);
                return XmlResultHelper.NewResult(ds.ToXml());
            }
            catch (Exception exception)
            {
                _Logger.Error(exception);
                return XmlResultHelper.NewErrorResult();
            }
        }


        public static XElement  AsyncGetChartData2(Session session, Guid instrumentId, DateTime from, DateTime to, string dataCycleParameter)
        {
            try
            {
                AsyncResult asyncResult = new AsyncResult("AsyncGetChartData2", session.ToString());
                Application.Default.AssistantOfCreateChartData2.AddTask(asyncResult, new ChartDataArgument2(instrumentId, dataCycleParameter, from, to, asyncResult, session),CreateChartData2);
                return XmlResultHelper.NewResult(asyncResult.Id.ToString());
            }
            catch (System.Exception exception)
            {
                _Logger.Error(exception);
                return XmlResultHelper.NewErrorResult();
            }
            
        }

        public static XElement AsyncGetChartData2ForMobile(Session session, Guid instrumentId, DateTime from, DateTime to, string dataCycleParameter)
        {
            try
            {
                AsyncResult asyncResult = new AsyncResult("AsyncGetChartData2ForMobile", session.ToString());
                Application.Default.AssistantOfCreateChartData2.AddTask(asyncResult, new ChartDataArgument2(instrumentId, dataCycleParameter, from, to, asyncResult, session), CreateChartData2ForMobile);
                //CommandManager.Default.AddCommand(new AsyncCommand(0, chartDataArgument.AsyncResult));
                return XmlResultHelper.NewResult(asyncResult.Id.ToString());
            }
            catch (System.Exception exception)
            {
                _Logger.Error(exception);
                return XmlResultHelper.NewErrorResult();
            }

        }

        private static void CreateChartData2(object state)
        {
            ChartDataArgument2 chartDataArgument = (ChartDataArgument2)state;
            try
            {
                //DataSet dataSet = this.GetChartData2(chartDataArgument.InstrumentId, chartDataArgument.DataCycle, chartDataArgument.From, chartDataArgument.To);
                //SChart.Datas.StockData stockData = new SChart.Datas.StockData(cookieContainer);
                //DataSet dataSet = stockData.GetChartData2(chartDataArgument.InstrumentId, chartDataArgument.DataCycle, chartDataArgument.From, chartDataArgument.To);
                TradingConsoleState tradingConsoleState = chartDataArgument.TradingConsoleState;
                Guid quotePolicyId = (Guid)tradingConsoleState.Instruments[chartDataArgument.InstrumentId];
                TradingConsoleServer tradingConsoleServer = chartDataArgument.TradingConsoleServer;
                DataSet dataSet = tradingConsoleServer.GetChartData2(chartDataArgument.InstrumentId, quotePolicyId, chartDataArgument.DataCycle, chartDataArgument.From, chartDataArgument.To);
                AsyncResultManager asyncResultManager = chartDataArgument.AsyncResultManager;
                asyncResultManager.SetResult(chartDataArgument.AsyncResult, dataSet);
                CommandManager.Default.AddCommand(new AsyncCommand(0, chartDataArgument.AsyncResult));
                //else
                //{
                //    string userIdString = string.Empty;
                //    if (chartDataArgument.Token != null)
                //    {
                //        Token token = chartDataArgument.Token;
                //        userIdString = token.UserID.ToString();
                //    }
                //    AppDebug.LogEvent("TradingConsole.Service.CreateChartData2", "CookieContainer Timeout" + " UserId: " + userIdString, EventLogEntryType.Warning);
                //    Commands commands = chartDataArgument.Commands;
                //    commands.Add(chartDataArgument.Token, new AsyncCommand(0, chartDataArgument.AsyncResult, true, null));
                //}
            }
            catch (Exception e)
            {
                _Logger.Error(e);
                CommandManager.Default.AddCommand( new AsyncCommand(0, chartDataArgument.AsyncResult, true, e));
            }
        }

        private static void CreateChartData2ForMobile(object state)
        {
            ChartDataArgument2 chartDataArgument = (ChartDataArgument2)state;
            try
            {
                TradingConsoleState tradingConsoleState = chartDataArgument.TradingConsoleState;
                Guid quotePolicyId = (Guid)tradingConsoleState.Instruments[chartDataArgument.InstrumentId];
                TradingConsoleServer tradingConsoleServer = chartDataArgument.TradingConsoleServer;
                //DataSet dataSet = tradingConsoleServer.GetChartData2(chartDataArgument.InstrumentId, quotePolicyId, chartDataArgument.DataCycle, chartDataArgument.From, chartDataArgument.To);

                string dataCycle = chartDataArgument.DataCycle.ToLower();
                string dateFormat = ChartQuotation.FormatMinute;
                
                //DataSet dataSet = DataAccess.GetChartData(instrumentId, quotePolicyId, dataCycle, fromTime.Value, toTime, commandTimeOut);
                DataSet dataSet = tradingConsoleServer.GetChartData2(chartDataArgument.InstrumentId, quotePolicyId, chartDataArgument.DataCycle, chartDataArgument.From, chartDataArgument.To);

                ChartQuotationCollection chartQuotationCollection = ChartQuotationCollection.Create(dataSet, dataCycle, dateFormat);

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ChartQuotationCollection));
                string result;
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.WriteObject(stream, chartQuotationCollection);
                    stream.Seek(0, SeekOrigin.Begin);
                    StreamReader streamReader = new StreamReader(stream);
                    result = streamReader.ReadToEnd();
                }                
                
                AsyncResultManager asyncResultManager = chartDataArgument.AsyncResultManager;
                XElement element = XmlResultHelper.NewResult("ChartData", result);
                asyncResultManager.SetResult(chartDataArgument.AsyncResult, element);
                AsyncCommand command = new AsyncCommand(0, chartDataArgument.AsyncResult);
                command.Content = element.ToXmlNode();
                CommandManager.Default.AddCommand(command);
            }
            catch (Exception e)
            {
                _Logger.Error(e);
                CommandManager.Default.AddCommand(new AsyncCommand(0, chartDataArgument.AsyncResult, true, e));
            }

        }
    }
}

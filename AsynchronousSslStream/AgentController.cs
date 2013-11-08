using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using log4net;
using Trader.Server.Bll;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Trader.Server.SessionNamespace;
using iExchange.Common;
using Trader.Common;
using Trader.Server.Ssl;
using Trader.Server._4BitCompress;
using Trader.Server.Service;
using Trader.Server.ValueObjects;
using Trader.Server.Core;
namespace Trader.Server
{
   
    public class AgentController
    {
        private readonly ILog _Logger = LogManager.GetLogger(typeof(AgentController));
        private IClientsProcessor _ClientsProcessor;
        private ICommandTransfer _CommandTransfer;
        private IDisconnectedClientProccessor _DisconnectedClientProccessor;
        public AgentController(IClientsProcessor clientsProccessor, ICommandTransfer commandTransfer, IDisconnectedClientProccessor disconnectedClientProccessor)
        {
            _ClientsProcessor = clientsProccessor;
            _CommandTransfer = commandTransfer;
            _DisconnectedClientProccessor = disconnectedClientProccessor;
        }
        public void Add(Session session, ReceiveAgent receiver, Client sender)
        {
            _ClientsProcessor.Add(session, receiver, sender);
        }

        public void Remove(Session session)
        {
            _ClientsProcessor.Remove(session);
        }

        public bool RecoverConnection(Session originSession, Session currentSession)
        {
            _ClientsProcessor.Remove(originSession);
            SenderReceiverPair currentRelation;
            if (!_ClientsProcessor.Remove(currentSession, out currentRelation))
                return false;
            if (!_ClientsProcessor.Add(originSession, currentRelation))
                return false;
            currentRelation.Sender.UpdateClientID(originSession);
            return true;
        }

        public Client GetSender(Session session)
        {
            return _ClientsProcessor.GetSender(session);
        }


        public ReceiveAgent GetReceiver(Session session)
        {
            return _ClientsProcessor.GetReceiver(session);
        }

        public void Start()
        {
            try
            {
                _DisconnectedClientProccessor.Start();
                _CommandTransfer.Start();
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
            }
        }

        public void Stop()
        {
            _CommandTransfer.Stop();
            _DisconnectedClientProccessor.Stop();
        }

        public void EnqueueDisconnectSession(Session session)
        {
            _DisconnectedClientProccessor.Add(session);
        }

        public void SendCommand(QuotationCommand quotationCommand=null,Command command =null)
        {
            if (_ClientsProcessor.IsEmpty()) return;
            var target = new CommandForClient(quotationCommand:quotationCommand,command:command);
            _CommandTransfer.Send(target);
        }

        public void KickoutAllClient()
        {
           _ClientsProcessor.ParallelForEach(p =>
            {
                p.Value.Sender.Send(new CommandForClient(data: NamedCommands.GetKickoutPacket()));
                Application.Default.SessionMonitor.Remove(p.Key);
            });
        }
    }

}
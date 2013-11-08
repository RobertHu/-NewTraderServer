using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Trader.Server.ValueObjects;
using Trader.Common;
using Trader.Server.Ssl;
using System.Threading.Tasks;

namespace Trader.Server.Core
{
    public class ClientsCenter:IClientsProcessor
    {
        private readonly ConcurrentDictionary<Session, SenderReceiverPair> _Clients = new ConcurrentDictionary<Session, SenderReceiverPair>();
        public bool Remove(Session session)
        {
            SenderReceiverPair relation;
           return _Clients.TryRemove(session, out relation);
        }

        public bool Remove(Session session, out SenderReceiverPair pair)
        {
            return _Clients.TryRemove(session, out pair);
        }
        

        public bool Add(Session session, ReceiveAgent receiver,Client sender)
        {
           return _Clients.TryAdd(session, new SenderReceiverPair(sender, receiver));
        }

        public bool Add(Session session, SenderReceiverPair pair)
        {
            return _Clients.TryAdd(session, pair);
        }

        public ReceiveAgent GetReceiver(Session session)
        {
            ReceiveAgent result = null;
            SenderReceiverPair relation;
            if (_Clients.TryGetValue(session, out relation))
            {
                result = relation.Receiver;
            }
            return result;
        }

        public Client GetSender(Session session)
        {
            Client result = null;
            SenderReceiverPair relation;
            if (_Clients.TryGetValue(session, out relation))
            {
                result = relation.Sender;
            }
            return result;
        }

        public void ParallelForEach(Action<KeyValuePair<Session, SenderReceiverPair>> callback)
        {
            Parallel.ForEach(_Clients, callback);
        }

        public bool IsEmpty()
        {
            return _Clients.Count() == 0;
        }
    }
}

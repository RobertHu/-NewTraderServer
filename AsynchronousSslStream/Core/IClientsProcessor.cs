using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Common;
using Trader.Server.Ssl;
using Trader.Server.ValueObjects;

namespace Trader.Server.Core
{
    public interface IClientsProcessor
    {
        bool Remove(Session session);
        bool Remove(Session session, out SenderReceiverPair pair);
        bool Add(Session session, ReceiveAgent receiver, Client sender);
        bool Add(Session session, SenderReceiverPair pair);
        bool IsEmpty();
        ReceiveAgent GetReceiver(Session session);
        Client GetSender(Session session);
        void ParallelForEach(Action<KeyValuePair<Session,SenderReceiverPair>> callback);
    }
}

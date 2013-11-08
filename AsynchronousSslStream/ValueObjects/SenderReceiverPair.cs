using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.Ssl;

namespace Trader.Server.ValueObjects
{
    public struct SenderReceiverPair
    {
        private ReceiveAgent _Receiver;
        private Client _Sender;
        public SenderReceiverPair(Client sender, ReceiveAgent receiver)
        {
            this._Receiver = receiver;
            this._Sender = sender;
        }
        public ReceiveAgent Receiver { get { return _Receiver; } }
        public Client Sender { get { return _Sender; } }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.Log
{
    public class LogInfo
    {
        public string LoginName { get; set; }
        public Guid UserId { get; set; }
        public string Ip { get; set; }
        public string UserType { get; set; }
        public string Msg { get; set; }
        public DateTime Timestamp { get; set; }
        public string EventType { get; set; }
        public Guid? TransactionId { get; set; }
        public Guid? AccountId { get; set; }
    }
}

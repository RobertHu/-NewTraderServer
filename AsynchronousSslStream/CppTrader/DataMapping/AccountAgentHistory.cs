using System;

namespace Trader.Server.CppTrader.DataMapping
{
    public class AccountAgentHistory
    {
        public Guid AccountId
        {
            get;
            set;
        }

        public Guid AgentAccountId
        {
            get;
            set;
        }

        public DateTime AgentBeginTime
        {
            get;
            set;
        }

        public DateTime AgentEndTime
        {
            get;
            set;
        }
    }
}
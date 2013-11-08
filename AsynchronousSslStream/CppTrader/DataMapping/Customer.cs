using System;
using Trader.Server.CppTrader.DataMapping.Enums;

namespace Trader.Server.CppTrader.DataMapping
{
    public class Customer
    {
        public Guid Id
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }
        
        public string Name
        {
            get;
            set;
        }
        
        public string Email
        {
            get;
            set;
        }

        public bool IsDisplayLedger
        {
            get;
            set;
        }

		public bool AllowFreeAgent
        {
            get;
            set;
        }

        public AccountOrderType SingleAccountOrderType
        {
            get;
            set;
        }
        
        public AccountOrderType MultiAccountsOrderType
        {
            get;
            set;
        }

        public TimeSpan DQOrderOutTime
        {
            get;
            set;
        }

        public int AssignOrderType
        {
            get;
            set;
        }

        public bool IsCalculateFloat
        {
            get;
            set;
        }

		public bool IsSendOrderMail
        {
            get;
            set;
        }

        public bool DisallowTrade
        {
            get;
            set;
        }

        public int DisplayAlert
        {
            get;
            set;
        }
        
        public bool IsNoShowAccountStatus
        {
            get;
            set;
        }

        public bool ShowLog
        {
            get;
            set;
        }
        
        public DateTime LastLogTime
        {
            get;
            set;
        }
        
        public Guid? PublicQuotePolicyId
        {
            get;
            set;
        }
            
        public Guid? PrivateQuotePolicyId
        {
            get;
            set;
        }

        public Guid? DealingPolicyId
        {
            get;
            set;
        }

        public bool IsEmployee
        {
            get;
            set;
        }

    }
}
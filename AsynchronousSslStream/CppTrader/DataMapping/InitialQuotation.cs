using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping
{
    public class InitialQuotation
    {
        public Guid InstrumentId
        {
            get;
            set;
        }

        public string Ask
        {
            get;
            set;
        }

        public string Bid
        {
            get;
            set;
        }

        public string High
        {
            get;
            set;
        }

        public string Low
        {
            get;
            set;
        }

        public DateTime Timestamp
        {
            get;
            set;
        }
        
        public string Open
        {
            get;
            set;
        }

        public string PreClose
        {
            get;
            set;
        }

        public string Volume
        {
            get;
            set;
        }
        
        public string TotalVolume
        {
            get;
            set;
        }

		public bool IsPrivateOpen
        {
            get;
            set;
        }
        
        public bool IsPrivateClose
        {
            get;
            set;
        }
        
        public bool IsPrivateDailyAsk
        {
            get;
            set;
        }

        public bool IsPrivateDailyBid
        {
            get;
            set;
        }
    }
}
using System;

namespace Trader.Server.CppTrader.DataMapping
{
    public class InitializeData
    {
        public string OrganizationName
        {
            get;
            set;
        }

        public long LastSequence
        {
            get;
            set;
        }

        public SettingSet SettingSet
        {
            get;
            set;
        }

        public TradingSet TradingSet
        {
            get;
            set;
        }
    }
}
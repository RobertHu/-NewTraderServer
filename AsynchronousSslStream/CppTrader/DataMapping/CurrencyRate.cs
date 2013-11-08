using System;

namespace Trader.Server.CppTrader.DataMapping
{
    public class CurrencyRate
    {
        public Guid SourceCurrencyId
        {
            get;
            set;
        }

        public Guid TargetCurrencyId
        {
            get;
            set;
        }

        public decimal RateIn
        {
            get;
            set;
        }

        public decimal RateOut
        {
            get;
            set;
        }
    }
}
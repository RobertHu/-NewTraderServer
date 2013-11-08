using System;

namespace Trader.Server.CppTrader.DataMapping
{
    public class PriceRange
    {
        public Guid InstrumentId;
        public double? HighLimitPrice;
        public double? LowLimitPrice;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping.Response
{

    public enum AggressorSide
    {
        Buy,
        Sell,
        Unknown
    }

    public class Trade
    {
        public Guid InstrumentId;
        public DateTime Timestamp;
        public decimal Price;
        public AggressorSide AggressorSide;
        public double Volume;
        public double TradeVolume;
        public double TotalVolume;
    }

    public class TradeDistribution
    {
        public Guid InstrumentId { get; set; }
        public decimal Price { get; set; }
        public AggressorSide AggressorSide { get; set; }
        public double Volume { get; set; }
        public int Transactions { get; set; }
    }
}

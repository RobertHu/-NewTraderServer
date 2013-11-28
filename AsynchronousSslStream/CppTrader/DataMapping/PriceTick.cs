using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping
{
    public class PriceTick
    {
        public string PriceTickSetIndex { get; set; }
        public int Sequence { get; set; }
        public double? MinimumPrice { get; set; }  // null express negative infinity
        public double Tick { get; set; }
    }
}

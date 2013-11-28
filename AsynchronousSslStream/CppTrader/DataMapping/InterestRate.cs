using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping
{
    public class InterestRate
    {
        public InterestRate[] InterestRates;
        public Guid OrderId;
        public Guid InterestId;
        public decimal? Buy;
        public decimal? Sell;
    }
}

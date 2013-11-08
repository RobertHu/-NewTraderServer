using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping.Enums
{
    public enum PhysicalTradeSide
    {
        None = 0,
        Buy = 1,
        Sell = 2,
        ShortSell = 4,
        Delivery = 8,
        Deposit = 16
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping.Enums
{
    public enum OrderType
    {
        SpotTrade,
        Limit,
        Market,
        MarketOnOpen,
        MarketOnClose,
        OneCancelOther,
        Risk,
        Stop,
        MultipleClose,
        MarketToLimit,
        StopLimit,
        FAK_Market
    }
}

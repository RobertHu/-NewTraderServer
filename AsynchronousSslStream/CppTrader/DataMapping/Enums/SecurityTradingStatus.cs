using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping.Enums
{
    public enum SecurityTradingStatus
    {
        TradingHalt = 2,
        PriceIndication = 5,
        ReadyToTrade = 17,
        NotAvailableForTrading = 18,
        UnknownOrInvalid = 20,
        PreOpen = 21,
        OpeningRotation = 22,
        PreCross = 24,
        Cross = 25,
        NoCancel = 26
    }
}

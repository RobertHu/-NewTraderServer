using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping.Enums
{
    public enum ExpireType
    {
        Day,
        GTC,
        IOC,
        GTD,
        Session,
        FillOrKill,
        FillAndKill,

        //below are not realy expire type, all these will bo convert to GTD
        GoodTillMonthDay,
        GTF,
        GTM,
        GoodTillMonthSession
    }
}

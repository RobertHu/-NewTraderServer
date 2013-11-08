using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping.Enums
{
    public enum RecalculateRateType
    {
        //利息重算类型
        //1:下月重算 2:下一年重算 
        RecalucateNextMonth = 1,
        RecalucateNextYear = 2,
    }
}

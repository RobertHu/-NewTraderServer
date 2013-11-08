using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping.Enums
{
    public enum Frequence
    {
        //偿还频率
        //-1：不定期，0:月，1:季度，2:双周 
        Occasional = -1,
        Month = 0,
        Quarterly = 1,
        Fortnight = 2
    }
}

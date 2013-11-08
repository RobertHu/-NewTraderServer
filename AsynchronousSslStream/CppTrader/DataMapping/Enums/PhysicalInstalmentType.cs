using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping.Enums
{
    public enum PhysicalInstalmentType
    {
        //还款类型
        //0:全款 1:等额本金 2：等额本息
        None = 0,
        EqualPrincipal = 1,
        EqualInstalment = 2
    }
}

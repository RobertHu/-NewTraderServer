using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping.Enums
{
    public enum AdministrationFeeBaseType
    {
        //手续费
        //0:Fixed Amount(数量*比例）, 1:%(价值*比例), 2:LumpSum（固定金额)
        FixedAmount = 0,
        Percentage = 1,
        LumpSum = 2
    }
}

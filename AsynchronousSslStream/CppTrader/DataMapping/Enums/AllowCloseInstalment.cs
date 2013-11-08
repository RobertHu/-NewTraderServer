using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping.Enums
{
    public enum AllowCloseInstalment
    {
        //提前还款
        //0,不容許；1，任何情況下容許； 2，不存在欠款(逾期未还)下容許; 3,允许提前还款(此项是给backoffice用的)
        DisAllow = 0,
        AllowAll = 1,
        AllowWhenNoOutstandingInstalment = 2,
        AllowPrepayment = 3
    }
}

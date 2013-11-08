using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping.Enums
{
    public enum AccountType
    {
        Common,
        Agent,
        Company,
        Transit,//used by trader, to hide history open order and disable close order
        BlackList//Is same as AccountType.Common except add notify message
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping.Enums
{
    public enum ScrapDepositStatus
    {
        //0,待化验；1已化验；2已产生转仓单;100，已取消
        ExaminationInProcess = 0,
        ExaminationCompleted = 1,
        ContractConverted = 2,
        Canceled = 100,
    }
}

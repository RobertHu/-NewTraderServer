using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping.Enums
{
    public enum ContractTerminateType
    {
        //終止合約扣费類別
        //0:还款金额比例; 1:  扣一期; 2:Fixed Amount(手数*金额）；3:LumpSum（固定金额)，
        RepaymentRatio = 0,
        DeductOneInstalment = 1,
        FixedAmount = 2,
        LumpSum = 3
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping.Enums
{
    public enum DeliveryRequestStatus
    {
        //0,已接受；1，已审批； 2，货已备好；3，已提货；4已产生转仓单；5，已对冲；100，已取消
        Accepted = 0,
        Approved = 1,
        Stocked = 2,
        Delivered = 3,
        OrderCreated = 4,
        Hedge = 5,
        Cancelled = 100
    }
}

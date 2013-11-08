using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping.Enums
{
    public enum TransactionType
    {
        Single,
        Pair,
        OneCancelOther,
        Mapping,
        MultipleClose,
        Assign = 100,//AssigningOrderID == AssigningOrderID (the id of order been assigned from)
    }

    public enum TransactionSubType
    {
        None = 0,
        Amend,  //AssigningOrderID == AmendedOrderId (the id of order been amended)
        IfDone, //AssigningOrderID == IfOrderId (the id of order used as condition)
        Match,
        Assign, //AssigningOrderID == AssigningOrderID (id of the order been assigned from) //NotImplemented
        Mapping,
    }



    public enum Phase
    {
        Placing = 255,
        Placed = 0,
        Canceled,
        Executed,
        Completed,
        Deleted
    }
}

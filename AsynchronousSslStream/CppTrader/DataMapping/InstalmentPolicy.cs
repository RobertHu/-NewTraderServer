using System;
using Trader.Server.CppTrader.DataMapping.Enums;

namespace Trader.Server.CppTrader.DataMapping
{
    public class InstalmentPolicy
    {        
        public Guid Id
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public int AllowedInstalmentTypes
        {
            get;
            set;
        }

        public int RecalculateRateTypes
        {
            get;
            set;
        }

        public decimal ValueDiscountAsMargin
        {
            get;
            set;
        }

        public AllowCloseInstalment AllowClose
        {
            get;
            set;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping
{
    public class DayPLNotValued
    {
        public Guid OrderId
        {
            get;
            set;
        }

        public decimal Interest
        {
            get;
            set;
        }

        public decimal Storage
        {
            get;
            set;
        }
    }
}
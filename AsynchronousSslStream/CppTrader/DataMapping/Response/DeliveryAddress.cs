using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping.Response
{
    public class DeliveryAddress
    {
        public Guid Id
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }
    }
}

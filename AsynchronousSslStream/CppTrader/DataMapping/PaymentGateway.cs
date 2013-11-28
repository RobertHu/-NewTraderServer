using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping
{
    public class PaymentGateway
    {
        public string MerchantAcctId { get; set; }
        public string MerchantKey { get; set; }
        public Guid OrganizationId { get; set; }
    }
}

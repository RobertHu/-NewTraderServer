using System;

namespace Trader.Server.CppTrader.DataMapping
{
    public class PaymentInstructionRemark
    {
        public Guid? OrganizationId { get; set; }

        public string Remark { get; set; }
    }
}
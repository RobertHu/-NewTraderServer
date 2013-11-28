using System;
using System.Net;
using System.Runtime.Serialization;
using Trader.Server.CppTrader.DataMapping.Enums;

namespace Trader.Server.CppTrader.DataMapping.Response
{
    public class OrderInstalment
    {
        public Guid OrderId
        {
            get;
            set;
        }

        public int Sequence
        {
            get;
            set;
        }

        public decimal Principal
        {
            get;
            set;
        }

        public decimal Interest
        {
            get;
            set;
        }

        public decimal DebitInterest
        {
            get;
            set;
        }

        public DateTime PaymentDateTimeOnPlan
        {
            get;
            set;
        }

        public DateTime? PaidDateTime
        {
            get;
            set;
        }

        public decimal InterestRate
        {
            get;
            set;
        }

        public decimal InstalmentAmount
        {
            get;
            set;
        }

        public PaymentStatus PaymentStatus
        {
            get;
            set;
        }
    }
}

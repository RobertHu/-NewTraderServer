using System;
using Trader.Server.CppTrader.DataMapping.Enums;

namespace Trader.Server.CppTrader.DataMapping
{
    public class InstalmentPolicyDetail
    {
        public Guid InstalmentPolicyId
        {
            get;
            set;
        }

        public int Period
        {
            get;
            set;
        }

        public decimal MinDownPayment
        {
            get;
            set;
        }

        public decimal MaxDownPayment
        {
            get;
            set;
        }

        public decimal InterestRate
        {
            get;
            set;
        }

        public AdministrationFeeBaseType AdministrationFeeBase
        {
            get;
            set;
        }

        public decimal AdministrationFee
        {
            get;
            set;
        }

        public int Frequence
        {
            get;
            set;
        }

        public int DebitInterestType
        {
            get;
            set;
        }

        public decimal DebitInterestRatio
        {
            get;
            set;
        }

        public int DebitFreeDays
        {
            get;
            set;
        }

        public ContractTerminateType ContractTerminateType
        {
            get;
            set;
        }

        public decimal ContractTerminateFee
        {
            get;
            set;
        }

        public int DownPaymentBasis
        {
            get;
            set;
        }

        public bool IsActive { get; set; }
    }
}
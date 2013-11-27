using System;

namespace Trader.Server.CppTrader.DataMapping
{
    public class SettingSet
    {
        public string OrganizationName
        {
            get;
            set;
        }

        public Account[] Accounts
        {
            get;
            set;
        }

        public AccountAgentHistory[] AccountAgentHistories
        {
            get;
            set;
        }

        public Currency[] Currencies
        {
            get;
            set;
        }

        public CurrencyRate[] CurrencyRates
        {
            get;
            set;
        }

        public Customer Customer
        {
            get;
            set;
        }

        public Customer[] Customers
        {
            get;
            set;
        }

        public Instrument[] Instruments
        {
            get;
            set;
        }

        public ScrapInstrument[] ScrapInstruments
        {
            get;
            set;
        }

        public QuotePolicyDetail[] QuotePolicyDetails
        {
            get;
            set;
        }

        public SystemParameter SystemParameter
        {
            get;
            set;
        }

        public TradeDay TradeDay
        {
            get;
            set;
        }

        public TradePolicy[] TradePolicies
        {
            get;
            set;
        }

        public TradePolicyDetail[] TradePolicyDetails
        {
            get;
            set;
        }

        public VolumeNecessary[] VolumeNecessaries
        {
            get;
            set;
        }

        public VolumeNecessaryDetail[] VolumeNecessaryDetails
        {
            get;
            set;
        }

        public TradingTime[] TradingTimes
        {
            get;
            set;
        }

        public DealingPolicyDetail[] DealingPolicyDetails
        {
            get;
            set;
        }

        public Guid? InterestRateID
        {
            get;
            set;
        }

        public PaymentInstructionRemark[] PaymentInstructionRemarks
        {
            get;
            set;
        }

        public DeliveryCharge[] DeliveryCharges
        {
            get;
            set;
        }

        public DeliveryHolidays[] DeliveryHolidaies
        {
            get;
            set;
        }

        public InstalmentPolicy[] InstalmentPolicies
        {
            get;
            set;
        }

        public InstalmentPolicyDetail[] InstalmentPolicyDetails
        {
            get;
            set;
        }
    }
}
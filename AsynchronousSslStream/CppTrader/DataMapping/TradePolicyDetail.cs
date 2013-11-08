using System;

namespace Trader.Server.CppTrader.DataMapping
{
    public class TradePolicyDetail
    {
        public Guid TradePolicyId
        {
            get;
            set;
        }

        public Guid InstrumentId
        {
            get;
            set;
        }

        public Guid? VolumeNecessaryId
        {
            get;
            set;
        }

        public decimal ContractSize
        {
            get;
            set;
        }

        public bool IsTradeActive
        {
            get;
            set;
        }

        public decimal CommissionCloseD
        {
            get;
            set;
        }

        public decimal CommissionCloseO
        {
            get;
            set;
        }

        public decimal MinCommissionClose
        {
            get;
            set;
        }

        public decimal MinCommissionOpen
        {
            get;
            set;
        }

        public decimal MinOpen
        {
            get;
            set;
        }

        public decimal OpenMultiplier
        {
            get;
            set;
        }

        public decimal MinClose
        {
            get;
            set;
        }

        public decimal CloseMultiplier
        {
            get;
            set;
        }

        public decimal DefaultLot
        {
            get;
            set;
        }

        public int Option
        {
            get;
            set;
        }

        public bool IsAcceptNewStop
        {
            get;
            set;
        }

        public bool IsAcceptNewLimit
        {
            get;
            set;
        }

        public bool IsAcceptNewMOOMOC
        {
            get;
            set;
        }

        public bool AllowIfDone
        {
            get;
            set;
        }

        public Guid? InterestRateId
        {
            get;
            set;
        }

        public decimal? InterestRateBuy
        {
            get;
            set;
        }

        public decimal? InterestRateSell
        {
            get;
            set;
        }

        public int DQMaxMove
        {
            get;
            set;
        }

        public decimal? PairRelationFactor
        {
            get;
            set;
        }

        public bool GoodTillDate
        {
            get;
            set;
        }

        public bool GoodTillMonthDayOrder
        {
            get;
            set;
        }

        public bool GoodTillMonthSession
        {
            get;
            set;
        }

        public bool GoodTillMonthGTM
        {
            get;
            set;
        }

        public bool GoodTillMonthGTF
        {
            get;
            set;
        }

        public bool MultipleCloseAllowed
        {
            get;
            set;
        }

        public bool CanPlaceMatchOrder
        {
            get;
            set;
        }

        public bool ChangePlacedOrderAllowed
        {
            get;
            set;
        }

        public decimal MarginD
        {
            get;
            set;
        }

        public decimal MarginO
        {
            get;
            set;
        }

        public decimal MarginLockedD
        {
            get;
            set;
        }

        public decimal MarginLockedO
        {
            get;
            set;
        }

        public int NecessaryRound
        {
            get;
            set;
        }

        public bool AllowNewOCO
        {
            get;
            set;
        }

        public int? AllowedPhysicalTradeSides
        {
            get;
            set;
        }
        
        public decimal? DiscountOfOdd
        {
            get;
            set;
        }

        public decimal? ValueDiscountAsMargin
        {
            get;
            set;
        }
        
        public decimal? PhysicalMinDeliveryQuantity
        {
            get;
            set;
        }

        public decimal? PhysicalDeliveryIncremental
        {
            get;
            set;
        }

        public Guid? DeliveryChargeId
        {
            get;
            set;
        }

        public Guid? InstalmentPolicyId
        {
            get;
            set;
        }

        public decimal? InstalmentPledgeDiscount
        {
            get;
            set;
        }        
    }
}
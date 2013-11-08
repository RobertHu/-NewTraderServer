using System;
using Trader.Server.CppTrader.DataMapping.Enums;

namespace Trader.Server.CppTrader.DataMapping
{
    public class Instrument
    {
        public Guid Id
        {
            get;
            set;
        }

        public string OriginCode
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public int? Denominator
        {
            get;
            set;
        }

        public int? NumeratorUnit
        {
            get;
            set;
        }

        public byte? CommissionFormula
        {
            get;
            set;
        }

        public byte? TradePLFormula
        {
            get;
            set;
        }

        public int? OrderTypeMask
        {
            get;
            set;
        }

        public bool? IsNormal
        {
            get;
            set;
        }

        public decimal? MaxDQLot
        {
            get;
            set;
        }

        public decimal? MaxOtherLot
        {
            get;
            set;
        }

        public Guid? CurrencyId
        {
            get;
            set;
        }

        public int? PriceValidTime
        {
            get;
            set;
        }

        public decimal? DqQuoteMinLot
        {
            get;
            set;
        }

        public bool? IsSinglePrice
        {
            get;
            set;
        }

        public int? AcceptLmtVariation
        {
            get;
            set;
        }

        public int? AcceptCloseLmtVariation
        {
            get;
            set;
        }

        public int? AcceptIfDoneVariation
        {
            get;
            set;
        }

        public int? CancelLmtVariation
        {
            get;
            set;
        }

        public bool? IsActive
        {
            get;
            set;
        }

        public int? Sequence
        {
            get;
            set;
        }

        public int? SequenceForQuotatoin
        {
            get;
            set;
        }

        public PriceType? PriceType
        {
            get;
            set;
        }

        public TimeSpan? LastAcceptTimeSpan
        {
            get;
            set;
        }

        public DateTime? BeginTime
        {
            get;
            set;
        }

        public DateTime? EndTime
        {
            get;
            set;
        }

        public int? AcceptDQVariation
        {
            get;
            set;
        }

        //public boolean _isHasDocument;
        public DateTime? DayOpenTime
        {
            get;
            set;
        }

        public DateTime? DayCloseTime
        {
            get;
            set;
        }

        //Added by Michael on 2008-04-23
        public DateTime? LastDayCloseTime
        {
            get;
            set;
        }

        public DateTime? NextDayOpenTime
        {
            get;
            set;
        }

        public DateTime? MocTime
        {
            get;
            set;
        }

        public DateTime? LastTradeDay
        {
            get;
            set;
        }

        public InstrumentCategory? Category
        {
            get;
            set;
        }

        public Guid? GroupId
        {
            get;
            set;
        }

        public bool? CanPlacePendingOrderAtAnyTime
        {
            get;
            set;
        }

        public string ExternalExchangeCode
        {
            get;
            set;
        }

        public byte? MarginFormula
        {
            get;
            set;
        }
        public int? AllowedNewTradeSides
        {
            get;
            set;
        }

        public string Unit
        {
            get;
            set;
        }

        public byte? PhysicalLotDecimal
        {
            get;
            set;
        }

        public string Narrative
        {
            get;
            set;
        }

        public string QuoteDescription
        {
            get;
            set;
        }

        public int? DeliveryTimeBeginDay
        {
            get;
            set;
        }

        public int? DeliveryTimeEndDay
        {
            get;
            set;
        }

        public Guid? DeliveryPointGroupId
        {
            get;
            set;
        }
    }
}
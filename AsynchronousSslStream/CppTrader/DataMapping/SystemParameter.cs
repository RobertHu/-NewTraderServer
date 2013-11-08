using System;

namespace Trader.Server.CppTrader.DataMapping
{
    public class SystemParameter
    {
        public double OrderValidDuration { get; set; }

        public double MarketOrderValidDuration { get; set; }

        public double MooMocAcceptDuration { get; set; }

        public double MooMocCancelDuration { get; set; }

        public bool DisplayLmtStopPoints { get; set; }

        public int EnquiryOutTime { get; set; }

        public int ExceptionEnquiryOutTime { get; set; }

        public bool HighBid { get; set; }

        public bool LowBid { get; set; }

        public bool AllowInstantPayment { get; set; }

        public bool? CaculateChangeWithDenominator { get; set; }

        public bool? TradinPanelGridFirst { get; set; }

        public bool? ShowAccountName { get; set; }

        public string TraderNameInEnglish { get; set; }

        public string TraderNameInSimplifiedChinese { get; set; }

        public string TraderNameInTraditionalChinese { get; set; }

        public bool UseNightNecessaryWhenBreak { get; set; }

        public bool? EnableModifyTelephoneIdentification { get; set; }

        public bool? EnablePalceLotNnemonic { get; set; }

        public Guid? CnyCurrencyId { get; set; }

        public bool? BalanceDeficitAllowPay { get; set; }

        public bool? AllowMixNewLimitStop { get; set; }

        public bool? EnableModifyLeverage { get; set; }

        public int? MinLeverage { get; set; }

        public int? MaxLeverage { get; set; }

        public int? LeverageStep { get; set; }

        public TimeSpan? PlaceConfirmMinTime { get; set; }

        public bool? EnableMarginPin { get; set; }

        public bool? BankAccountNameMustSameWithAccountName { get; set; }

        public bool? BankAccountOnly { get; set; }

        public bool? DQMaxLotApplyAccount { get; set; }

        public bool? NeedSelectAccount { get; set; }

        public int? MaxCustomerBankNo { get; set; }

        public bool? ShowReportInBrowserWhenOOB { get; set; }

        public bool? EnableTrendSheetChart { get; set; }

        public bool? AllowEditBankAccountInTrader { get; set; }

        public int TimeOptionInTraderLogAndConfirmWindow { get; set; }

        public bool ShowPriceChangedBeforeCloseConfirmWindow { get; set; }

        public DateTime TradeDayBeginTime { get; set; }

        public bool? ClosingUseCustomerQuotePolicy { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Trader.Server.CppTrader.DataMapping.Enums;
using Trader.Server._4BitCompress;

namespace Trader.Server.CppTrader.DataMapping.Util
{
    internal static class InitializationHelper
    {
        internal delegate void Initiliaze<T>(T value, DataRow row);
        internal static T Create<T>(DataSet dataSet, string tableName, Initiliaze<T> initailize)
            where T : new()
        {
            if (dataSet.Tables.Contains(tableName))
            {
                DataTable table = dataSet.Tables[tableName];

                T value = new T();
                initailize(value, table.Rows[0]);

                return value;
            }
            else
            {
                return default(T);
            }
        }

        internal static T[] CreateArray<T>(DataSet dataSet, string tableName, Initiliaze<T> initailize)
            where T : new()
        {
            if (dataSet.Tables.Contains(tableName))
            {
                DataTable table = dataSet.Tables[tableName];

                DataRowCollection rows = table.Rows;
                T[] values = new T[rows.Count];

                int index = 0;
                foreach (DataRow row in rows)
                {
                    T value = new T();
                    initailize(value, row);
                    values[index++] = value;
                }
                return values;
            }
            else
            {
                return null;
            }
        }

        internal static T GetItemValue<T>(this DataRow row, string name, T defaul)
        {
            return row[name] == DBNull.Value ? defaul : (T)row[name];
        }
    }

    public static class SettingsSetHelper
    {
        public static void Initialize(this SettingSet settingSet, DataSet dataSet)
        {
            settingSet.Customer = InitializationHelper.Create<Customer>(dataSet, "Customer", Initialize);
            settingSet.TradeDay = InitializationHelper.Create<TradeDay>(dataSet, "TradeDay", Initialize);
            settingSet.SystemParameter = InitializationHelper.Create<SystemParameter>(dataSet, "SystemParameter", Initialize);
            settingSet.Currencies = InitializationHelper.CreateArray<Currency>(dataSet, "Currency", Initialize);
            settingSet.CurrencyRates = InitializationHelper.CreateArray<CurrencyRate>(dataSet, "CurrencyRate", Initialize);
            settingSet.TradePolicies = InitializationHelper.CreateArray<TradePolicy>(dataSet, "TradePolicy", Initialize);
            settingSet.TradePolicyDetails = InitializationHelper.CreateArray<TradePolicyDetail>(dataSet, "TradePolicyDetail", Initialize);
            settingSet.VolumeNecessaries = InitializationHelper.CreateArray<VolumeNecessary>(dataSet, "VolumeNecessary", Initialize);
            settingSet.VolumeNecessaryDetails = InitializationHelper.CreateArray<VolumeNecessaryDetail>(dataSet, "VolumeNecessaryDetail", Initialize);
            settingSet.Accounts = InitializationHelper.CreateArray<Account>(dataSet, "Account", Initialize);
            settingSet.AccountAgentHistories = InitializationHelper.CreateArray<AccountAgentHistory>(dataSet, "AccountAgentHistory", Initialize);
            settingSet.Instruments = InitializationHelper.CreateArray<Instrument>(dataSet, "Instrument", Initialize);
            settingSet.DealingPolicyDetails = InitializationHelper.CreateArray<DealingPolicyDetail>(dataSet, "DealingPolicyDetail", Initialize);
            settingSet.QuotePolicyDetails = InitializationHelper.CreateArray<QuotePolicyDetail>(dataSet, "QuotePolicyDetail", Initialize);
            settingSet.TradingTimes = InitializationHelper.CreateArray<TradingTime>(dataSet, "TradingTime", Initialize);
            settingSet.PaymentInstructionRemarks = InitializationHelper.CreateArray<PaymentInstructionRemark>(dataSet, "PaymentInstructionRemark", Initialize);
            settingSet.ScrapInstruments = InitializationHelper.CreateArray<ScrapInstrument>(dataSet, "ScrapInstrument", Initialize);
            settingSet.DeliveryCharges = InitializationHelper.CreateArray<DeliveryCharge>(dataSet, "DeliveryCharge", Initialize);
            settingSet.DeliveryHolidaies = InitializationHelper.CreateArray<DeliveryHolidays>(dataSet, "DeliveryHolidays", Initialize);
            settingSet.InstalmentPolicies = InitializationHelper.CreateArray<InstalmentPolicy>(dataSet, "InstalmentPolicy", Initialize);
            settingSet.InstalmentPolicyDetails = InitializationHelper.CreateArray<InstalmentPolicyDetail>(dataSet, "InstalmentPolicyDetail", Initialize);
        }

        private static void Initialize(Customer customer, DataRow dataRow)
        {
            customer.Id = (Guid)dataRow["ID"];
            customer.Code = (string)dataRow["Code"];
            customer.Name = dataRow.GetItemValue<string>("Name", null);
            customer.Email = dataRow.GetItemValue<string>("Email", null);
            customer.IsDisplayLedger = (bool)dataRow["IsDisplayLedger"];
            customer.AllowFreeAgent = (int)dataRow["AllowFreeAgent"] == 1;
            customer.SingleAccountOrderType = dataRow["SingleAccountOrderType"].ToEnum<AccountOrderType>();
            customer.MultiAccountsOrderType = dataRow["MultiAccountsOrderType"].ToEnum<AccountOrderType>();
            customer.DQOrderOutTime = TimeSpan.FromSeconds((int)dataRow["DQOrderOutTime"]);
            customer.AssignOrderType = (int)dataRow["AssignOrderType"];
            customer.IsCalculateFloat = (bool)dataRow["IsCalculateFloat"];
            customer.IsSendOrderMail = (bool)dataRow["IsSendOrderMail"];
            customer.DisallowTrade = (bool)dataRow["DisallowTrade"];
            customer.DisplayAlert = (int)dataRow["DisplayAlert"];
            customer.IsNoShowAccountStatus = (bool)dataRow["IsNoShowAccountStatus"];
            customer.ShowLog = dataRow["ShowLog"] == DBNull.Value ? false : (bool)dataRow["ShowLog"];
            customer.LastLogTime = (DateTime)dataRow["LastLogTime"];
            if (dataRow.Table.Columns.Contains("PrivateQuotePolicyID"))
            {
                customer.PrivateQuotePolicyId = (Guid)dataRow["PrivateQuotePolicyID"];
            }
            if (dataRow.Table.Columns.Contains("PublicQuotePolicyID"))
            {
                customer.PublicQuotePolicyId = (Guid)dataRow["PublicQuotePolicyID"];
            }
            if (dataRow.Table.Columns.Contains("DealingPolicyID"))
            {
                customer.DealingPolicyId = dataRow.GetItemValue<Guid?>("DealingPolicyID", null);
            }
            customer.IsEmployee = (bool)dataRow["IsEmployee"];
        }

        private static void Initialize(TradeDay tradeDay, DataRow dataRow)
        {
            tradeDay.CurrentDay = (DateTime)dataRow["TradeDay"];
            tradeDay.BeginTime = (DateTime)dataRow["BeginTime"];
            tradeDay.EndTime = (DateTime)dataRow["EndTime"];
            tradeDay.LastDay = (DateTime)dataRow["LastTradeDay"];
        }

        private static void Initialize(SystemParameter systemParameter, DataRow dataRow)
        {
            systemParameter.OrderValidDuration = (int)dataRow["OrderValidDuration"];
            systemParameter.MooMocAcceptDuration = (int)dataRow["MooMocAcceptDuration"];
            systemParameter.MooMocCancelDuration = (int)dataRow["MooMocCancelDuration"];
            systemParameter.DisplayLmtStopPoints = dataRow.GetItemValue<bool>("DisplayLmtStopPoints", true);
            systemParameter.EnquiryOutTime = (int)dataRow["EnquiryOutTime"];
            systemParameter.ExceptionEnquiryOutTime = (int)dataRow["ExceptionEnquiryOutTime"];
            systemParameter.CaculateChangeWithDenominator = (bool)dataRow["CaculateChangeWithDenominator"];
            systemParameter.MaxCustomerBankNo = (int)dataRow["MaxCustomerBankNo"];

            if (dataRow.Table.Columns.Contains("HighBid"))
            {
                systemParameter.HighBid = (bool)dataRow["HighBid"];
            }
            if (dataRow.Table.Columns.Contains("LowBid"))
            {
                systemParameter.LowBid = (bool)dataRow["LowBid"];
            }
            if (dataRow.Table.Columns.Contains("TradinPanelGridFirst"))
            {
                systemParameter.TradinPanelGridFirst = (bool)dataRow["TradinPanelGridFirst"];
            }
            if (dataRow.Table.Columns.Contains("ShowAccountName"))
            {
                systemParameter.ShowAccountName = (bool)dataRow["ShowAccountName"];
            }
            if (dataRow.Table.Columns.Contains("TraderNameInEnglish")
                && dataRow["TraderNameInEnglish"] != DBNull.Value)
            {
                systemParameter.TraderNameInEnglish = (string)dataRow["TraderNameInEnglish"];
            }
            if (dataRow.Table.Columns.Contains("TraderNameInSimplifiedChinese")
                && dataRow["TraderNameInSimplifiedChinese"] != DBNull.Value)
            {
                systemParameter.TraderNameInSimplifiedChinese = (string)dataRow["TraderNameInSimplifiedChinese"];
            }
            if (dataRow.Table.Columns.Contains("TraderNameInTraditionalChinese")
                && dataRow["TraderNameInTraditionalChinese"] != DBNull.Value)
            {
                systemParameter.TraderNameInTraditionalChinese = (string)dataRow["TraderNameInTraditionalChinese"];
            }
            if (dataRow.Table.Columns.Contains("EnablePalceLotNnemonic")
                && dataRow["EnablePalceLotNnemonic"] != DBNull.Value)
            {
                systemParameter.EnablePalceLotNnemonic = (bool)dataRow["EnablePalceLotNnemonic"];
            }
            if (dataRow.Table.Columns.Contains("UseNightNecessaryWhenBreak")
                && dataRow["UseNightNecessaryWhenBreak"] != DBNull.Value)
            {
                systemParameter.UseNightNecessaryWhenBreak = (bool)dataRow["UseNightNecessaryWhenBreak"];
            }

            if (dataRow.Table.Columns.Contains("EnableModifyTelephoneIdentificationCode")
                && dataRow["EnableModifyTelephoneIdentificationCode"] != DBNull.Value)
            {
                systemParameter.EnableModifyTelephoneIdentification = (bool)dataRow["EnableModifyTelephoneIdentificationCode"];
            }
            if (dataRow.Table.Columns.Contains("CnyCurrencyId")
                && dataRow["CnyCurrencyId"] != DBNull.Value)
            {
                systemParameter.CnyCurrencyId = (Guid)dataRow["CnyCurrencyId"];
            }

            if (dataRow.Table.Columns.Contains("BalanceDeficitAllowPay"))
            {
                systemParameter.BalanceDeficitAllowPay = (bool)dataRow["BalanceDeficitAllowPay"];
            }
            if (dataRow.Table.Columns.Contains("AllowMixNewLimitStop"))
            {
                systemParameter.AllowMixNewLimitStop = (bool)dataRow["AllowMixNewLimitStop"];
            }

            if (dataRow.Table.Columns.Contains("EnableModifyLeverage"))
            {
                systemParameter.EnableModifyLeverage = (bool)dataRow["EnableModifyLeverage"];
                systemParameter.MinLeverage = (int)dataRow["MinLeverage"];
                systemParameter.MaxLeverage = (int)dataRow["MaxLeverage"];
                systemParameter.LeverageStep = (int)dataRow["LeverageStep"];
            }
            else
            {
                systemParameter.EnableModifyLeverage = false;
            }

            if (dataRow.Table.Columns.Contains("PlaceConfirmMinTime"))
            {
                systemParameter.PlaceConfirmMinTime = TimeSpan.FromSeconds((int)dataRow["PlaceConfirmMinTime"]);
            }
            else
            {
                systemParameter.PlaceConfirmMinTime = TimeSpan.Zero;
            }

            if (dataRow.Table.Columns.Contains("EnableMarginPin"))
            {
                systemParameter.EnableMarginPin = (bool)dataRow["EnableMarginPin"];
            }
            else
            {
                systemParameter.EnableMarginPin = false;
            }

            if (dataRow.Table.Columns.Contains("BankAccountNameMustSameWithAccountName"))
            {
                systemParameter.BankAccountNameMustSameWithAccountName = (bool)dataRow["BankAccountNameMustSameWithAccountName"];
            }
            else
            {
                systemParameter.BankAccountNameMustSameWithAccountName = false;
            }

            if (dataRow.Table.Columns.Contains("BankAccountOnly"))
            {
                systemParameter.BankAccountOnly = (bool)dataRow["BankAccountOnly"];
            }
            else
            {
                systemParameter.BankAccountOnly = false;
            }

            if (dataRow.Table.Columns.Contains("DQMaxLotApplyAccount"))
            {
                systemParameter.DQMaxLotApplyAccount = (bool)dataRow["DQMaxLotApplyAccount"];
            }
            else
            {
                systemParameter.DQMaxLotApplyAccount = false;
            }

            if (dataRow.Table.Columns.Contains("NeedSelectAccount"))
            {
                systemParameter.NeedSelectAccount = (bool)dataRow["NeedSelectAccount"];
            }
            else
            {
                systemParameter.NeedSelectAccount = false;
            }

            if (dataRow.Table.Columns.Contains("ShowReportInBrowserWhenOOB")
               && dataRow["ShowReportInBrowserWhenOOB"] != DBNull.Value)
            {
                systemParameter.ShowReportInBrowserWhenOOB = (bool)dataRow["ShowReportInBrowserWhenOOB"];
            }
            else 
            {
                systemParameter.ShowReportInBrowserWhenOOB = false;
            }

            if (dataRow.Table.Columns.Contains("EnableTrendSheetChart"))
            {
                systemParameter.EnableTrendSheetChart = (bool)dataRow["EnableTrendSheetChart"];
            }

            if (dataRow.Table.Columns.Contains("AllowEditBankAccountInTrader"))
            {
                systemParameter.AllowEditBankAccountInTrader = (bool)dataRow["AllowEditBankAccountInTrader"];
            }

            if (dataRow.Table.Columns.Contains("TOITLACW"))
            {
                systemParameter.TimeOptionInTraderLogAndConfirmWindow = Convert.ToInt16(dataRow["TOITLACW"]);
            }

            if (dataRow.Table.Columns.Contains("SPCBCCW"))
            {
                systemParameter.ShowPriceChangedBeforeCloseConfirmWindow = (bool)dataRow["SPCBCCW"];
            }

            if (dataRow.Table.Columns.Contains("TradeDayBeginTime"))
            {
                systemParameter.TradeDayBeginTime = (DateTime)dataRow["TradeDayBeginTime"];
            }

            if (dataRow.Table.Columns.Contains("ClosingUseCustomerQuotePolicy"))
            {
                systemParameter.ClosingUseCustomerQuotePolicy = (bool)dataRow["ClosingUseCustomerQuotePolicy"];
            }            
        }

        private static void Initialize(Currency currency, DataRow dataRow)
        {
            currency.Id = (Guid)dataRow["ID"];
            currency.Code = (string)dataRow["Code"];
            currency.Name = (string)dataRow["Name"];
            currency.Decimals = (short)dataRow["Decimals"];
            if (dataRow["MinDeposit"] != DBNull.Value)
            {
                currency.MinDeposit = (decimal)dataRow["MinDeposit"];
            }
        }

        private static void Initialize(this CurrencyRate currencyRate, DataRow dataRow)
        {
            currencyRate.SourceCurrencyId = (Guid)dataRow["SourceCurrencyID"];
            currencyRate.TargetCurrencyId = (Guid)dataRow["TargetCurrencyID"];
            currencyRate.RateIn = (decimal)(double)dataRow["RateIn"];
            currencyRate.RateOut = (decimal)(double)dataRow["RateOut"];
        }

        private static void Initialize(TradePolicy tradePolicy, DataRow dataRow)
        {
            tradePolicy.Id = (Guid)dataRow["ID"];
            if (dataRow.Table.Columns.Contains("AlertLevel1"))
            {
                tradePolicy.AlertLevel1 = dataRow.GetItemValue<decimal>("AlertLevel1", 0);
            }
            if (dataRow.Table.Columns.Contains("AlertLevel2"))
            {
                tradePolicy.AlertLevel2 = dataRow.GetItemValue<decimal>("AlertLevel2", 0);
            }
            if (dataRow.Table.Columns.Contains("AlertLevel3"))
            {
                tradePolicy.AlertLevel3 = dataRow.GetItemValue<decimal>("AlertLevel3", 0);
            }
        }

        private static void Initialize(TradePolicyDetail tradePolicyDetail, DataRow dataRow)
        {
            tradePolicyDetail.TradePolicyId = (Guid)dataRow["TradePolicyID"];
            tradePolicyDetail.InstrumentId = (Guid)dataRow["InstrumentID"];

            tradePolicyDetail.ContractSize = (decimal)dataRow["ContractSize"];
            tradePolicyDetail.IsTradeActive = (bool)dataRow["IsTradeActive"];
            tradePolicyDetail.CommissionCloseD = dataRow.GetItemValue<decimal>("CommissionCloseD", 0);
            tradePolicyDetail.CommissionCloseO = dataRow.GetItemValue<decimal>("CommissionCloseO", 0);
            tradePolicyDetail.MinCommissionClose = dataRow.GetItemValue<decimal>("MinCommissionClose", 0);
            tradePolicyDetail.MinCommissionOpen = dataRow.GetItemValue<decimal>("MinCommissionOpen", 0);
            tradePolicyDetail.MinOpen = dataRow.GetItemValue<decimal>("MinOpen", 0);
            tradePolicyDetail.OpenMultiplier = dataRow.GetItemValue<decimal>("OpenMultiplier", 0);
            tradePolicyDetail.MinClose = dataRow.GetItemValue<decimal>("MinClose", 0);
            tradePolicyDetail.DefaultLot = dataRow.GetItemValue<decimal>("DefaultLot", 0);
            tradePolicyDetail.CloseMultiplier = dataRow.GetItemValue<decimal>("CloseMultiplier", 0);
            tradePolicyDetail.Option = (int)dataRow["Option"];

            tradePolicyDetail.IsAcceptNewStop = (bool)dataRow["IsAcceptNewStop"];
            tradePolicyDetail.IsAcceptNewLimit = (bool)dataRow["IsAcceptNewLimit"];
            tradePolicyDetail.IsAcceptNewMOOMOC = (bool)dataRow["IsAcceptNewMOOMOC"];
            tradePolicyDetail.MultipleCloseAllowed = (bool)dataRow["MultipleCloseAllowed"];
            tradePolicyDetail.AllowIfDone = (bool)dataRow["AllowIfDone"];
            tradePolicyDetail.AllowNewOCO = (bool)dataRow["AllowNewOCO"];

            tradePolicyDetail.InterestRateId = dataRow.GetItemValue<Guid?>("InterestRateID", null);
            tradePolicyDetail.InterestRateBuy = dataRow.GetItemValue<decimal?>("InterestRateBuy", null);
            tradePolicyDetail.InterestRateSell = dataRow.GetItemValue<decimal?>("InterestRateSell", null);

            tradePolicyDetail.DQMaxMove = (int)dataRow["DQMaxMove"];
            tradePolicyDetail.PairRelationFactor = dataRow.GetItemValue<decimal?>("PairRelationFactor", null);

            tradePolicyDetail.GoodTillDate = (bool)dataRow["GoodTillDate"];
            tradePolicyDetail.GoodTillMonthDayOrder = (bool)dataRow["GoodTillMonthDayOrder"];
            tradePolicyDetail.GoodTillMonthSession = (bool)dataRow["GoodTillMonthSession"];
            tradePolicyDetail.GoodTillMonthGTM = (bool)dataRow["GoodTillMonthGTM"];
            tradePolicyDetail.GoodTillMonthGTF = (bool)dataRow["GoodTillMonthGTF"];
            tradePolicyDetail.CanPlaceMatchOrder = (bool)dataRow["CanPlaceMatchOrder"];
            tradePolicyDetail.ChangePlacedOrderAllowed = dataRow.Table.Columns.Contains("ChangePlacedOrderAllowed") ? (bool)dataRow["ChangePlacedOrderAllowed"] : false;

            tradePolicyDetail.MarginD = (decimal)dataRow["MarginD"];
            tradePolicyDetail.MarginO = (decimal)dataRow["MarginO"];
            tradePolicyDetail.MarginLockedD = (decimal)dataRow["MarginLockedD"];
            tradePolicyDetail.MarginLockedO = (decimal)dataRow["MarginLockedO"];
            tradePolicyDetail.NecessaryRound = (int)dataRow["NecessaryRound"];

            if (dataRow.Table.Columns.Contains("VolumeNecessaryId"))
            {
                tradePolicyDetail.VolumeNecessaryId = dataRow.GetItemValue<Guid?>("VolumeNecessaryId", null);
            }
            tradePolicyDetail.AllowedPhysicalTradeSides = (int)dataRow["AllowedPhysicalTradeSides"];
            tradePolicyDetail.DiscountOfOdd = (decimal)dataRow["DiscountOfOdd"];
            tradePolicyDetail.ValueDiscountAsMargin = (decimal)dataRow["ValueDiscountAsMargin"];
            tradePolicyDetail.PhysicalMinDeliveryQuantity = (decimal)dataRow["PhysicalMinDeliveryQuantity"];
            tradePolicyDetail.PhysicalDeliveryIncremental = (decimal)dataRow["PhysicalDeliveryIncremental"];
            if (dataRow.Table.Columns.Contains("DeliveryChargeId"))
            {
                tradePolicyDetail.DeliveryChargeId = dataRow.GetItemValue<Guid?>("DeliveryChargeId", null);
            }
            if (dataRow.Table.Columns.Contains("InstalmentPolicyId"))
            {
                tradePolicyDetail.InstalmentPolicyId = dataRow.GetItemValue<Guid?>("InstalmentPolicyId", null);
            }
            if (dataRow.Table.Columns.Contains("PartPaidPhysicalNecessary"))
            {
                tradePolicyDetail.PartPaidPhysicalNecessary = dataRow.GetItemValue<decimal?>("PartPaidPhysicalNecessary", null);
            }            
        }

        private static void Initialize(VolumeNecessary volumeNecessary, DataRow dataRow)
        {
            volumeNecessary.Id = (Guid)dataRow["ID"];
            volumeNecessary.Option = Convert.ToInt16(dataRow["Option"]);
        }

        private static void Initialize(VolumeNecessaryDetail volumeNecessaryDetail, DataRow dataRow)
        {
            volumeNecessaryDetail.Id = (Guid)dataRow["Id"];
            volumeNecessaryDetail.VolumeNecessaryId = (Guid)dataRow["VolumeNecessaryId"];
            volumeNecessaryDetail.From = (decimal)dataRow["From"];
            volumeNecessaryDetail.MarginD = (decimal)dataRow["MarginD"];
            volumeNecessaryDetail.MarginO = (decimal)dataRow["MarginO"];
        }

        private static void Initialize(Account account, DataRow dataRow)
        {
            account.Id = (Guid)dataRow["ID"];
            account.CustomerEmail = dataRow.GetItemValue<string>("CustomerEmail", null);
            account.Code = (string)dataRow["Code"];
            account.Name = (string)dataRow["Name"];
            account.OrganizationCode = (string)dataRow["OrganizationCode"];
            account.OrganizationName = (string)dataRow["OrganizationName"];
            account.OrganizationId = (Guid)dataRow["OrganizationID"];

            account.EnableAgentRegistration = (bool)dataRow["EnableAgentRegistration"];
            account.EnableCMExtension = (bool)dataRow["EnableCMExtension"];
            account.EnableOwnerRegistration = (bool)dataRow["EnableOwnerRegistration"];
            account.EnablePI = (bool)dataRow["EnablePI"];
            account.EnablePICash = (bool)dataRow["EnablePICash"];
            account.EnablePIInterACTransfer = (bool)dataRow["EnablePIInterACTransfer"];

            account.RateLotMin = dataRow.GetItemValue<decimal>("RateLotMin", 1);
            account.RateLotMultiplier = dataRow.GetItemValue<decimal>("RateLotMultiplier", 1);
            account.RateDefaultLot = dataRow.GetItemValue<decimal>("RateDefaultLot", 1);

            account.Type = (AccountType)dataRow["Type"];
            account.IsAutoClose = (bool)dataRow["IsAutoClose"];
            account.IsMultiCurrency = (bool)dataRow["IsMultiCurrency"];
            account.CustomerId = (Guid)dataRow["CustomerID"];
            account.CurrencyId = (Guid)dataRow["CurrencyId"];
            account.RateCommission = dataRow.GetItemValue<decimal>("RateCommission", 1);
            account.AgentId = dataRow.GetItemValue<Guid?>("AgentID", null);
            account.AgentName = dataRow.GetItemValue<string>("AgentName", "");
            account.AgentCode = dataRow.GetItemValue<string>("AgentCode", "");

            account.AgentEmail = dataRow.GetItemValue<string>("AgentEmail", null);
            account.IsTradingAllowed = (bool)dataRow["IsTradingAllowed"];
            account.BeginTime = (DateTime)dataRow["BeginTime"];
            account.EndTime = (DateTime)dataRow["EndTime"];
            account.IsLocked = (bool)dataRow["IsLocked"];
            account.GroupId = (Guid)dataRow["GroupID"];
            account.TradePolicyId = (Guid)dataRow["TradePolicyID"];
            account.CreditAmount = dataRow.GetItemValue<decimal>("CreditAmount", 0);
            account.AllowAddNewPosition = (bool)dataRow["AllowAddNewPosition"];

            if (dataRow["Leverage"] != DBNull.Value)
                account.Leverage = (int)dataRow["Leverage"];
            account.UserRelation = (int)dataRow["UserRelation"];
            account.AllowSalesTrading = (bool)dataRow["AllowSalesTrading"];
            account.AllowManagerTrading = (bool)dataRow["AllowManagerTrading"];
            account.IsTradingAllowedOnAccount = (bool)dataRow["IsTradingAllowedOnAccount"];
            if (dataRow.Table.Columns.Contains("RateMarginD"))
            {
                account.RateMarginD = dataRow.GetItemValue<decimal>("RateMarginD", 1);
            }
            if (dataRow.Table.Columns.Contains("RateMarginO"))
            {
                account.RateMarginO = dataRow.GetItemValue<decimal>("RateMarginO", 1);
            }
            if (dataRow.Table.Columns.Contains("RateMarginLockD"))
            {
                account.RateMarginLockD = dataRow.GetItemValue<decimal>("RateMarginLockD", 1);
            }
            if (dataRow.Table.Columns.Contains("RateMarginLockO"))
            {
                account.RateMarginLockO = dataRow.GetItemValue<decimal>("RateMarginLockO", 1);
            }
            if (dataRow.Table.Columns.Contains("BankAccountDefaultCountryId"))
            {
                account.BankAccountDefaultCountryId = dataRow.GetItemValue<long?>("BankAccountDefaultCountryId", null);
            }
            account.AllowChangePasswordInTrader = (bool)dataRow["AllowChangePasswordInTrader"];
            if (dataRow.Table.Columns.Contains("RiskActionMinimumEquity"))
            {
                account.RiskActionMinimumEquity = dataRow.GetItemValue<decimal?>("RiskActionMinimumEquity", null);
            }
        }

        private static void Initialize(AccountAgentHistory accountAgentHistory, DataRow dataRow)
        {
            accountAgentHistory.AccountId = (Guid)dataRow["AccountID"];
            accountAgentHistory.AgentAccountId = (Guid)dataRow["AgentAccountID"];
            accountAgentHistory.AgentBeginTime = (DateTime)dataRow["AgentBeginTime"];
            accountAgentHistory.AgentEndTime = dataRow.GetItemValue<DateTime>("AgentEndTime", accountAgentHistory.AgentBeginTime);
        }

        private static void Initialize(ScrapInstrument scrapInstrument, DataRow dataRow)
        {
            scrapInstrument.Id = (Guid)dataRow["Id"];
            scrapInstrument.Unit = dataRow.GetItemValue<string>("Unit", "");
            scrapInstrument.QuantityDecimalDigits = dataRow.GetItemValue<int>("QuantityDecimalDigits", 0);
            scrapInstrument.Description = dataRow.GetItemValue<string>("Description", "");
        }

        private static void Initialize(DeliveryCharge deliveryCharge, DataRow dataRow)
        {
            deliveryCharge.Id = (Guid)dataRow["Id"];
            deliveryCharge.Code = dataRow.GetItemValue<string>("Code", "");
            deliveryCharge.ChargeBasis = dataRow["ChargeBasis"].ToEnum<PhysicalChargeBasis>();
            deliveryCharge.ChargeRate = dataRow.GetItemValue<decimal>("ChargeRate", 0);
            deliveryCharge.MinCharge = dataRow.GetItemValue<decimal>("MinCharge", 0);
        }

        private static void Initialize(DeliveryHolidays deliveryHolidays, DataRow dataRow)
        {
            deliveryHolidays.BeginDate = (DateTime)dataRow["BeginDate"];
            deliveryHolidays.EndDate = (DateTime)dataRow["EndDate"];
        }

        private static void Initialize(InstalmentPolicy instalmentPolicy, DataRow dataRow)
        {
            instalmentPolicy.Id = (Guid)dataRow["ID"];
            instalmentPolicy.Code = (string)dataRow["Code"];
            instalmentPolicy.AllowedInstalmentTypes = (int)dataRow["AllowedInstalmentTypes"];
            instalmentPolicy.RecalculateRateTypes = (int)dataRow["RecalculateRateTypes"];
            instalmentPolicy.ValueDiscountAsMargin = (decimal)dataRow["ValueDiscountAsMargin"];
            instalmentPolicy.AllowClose = dataRow["AllowClose"].ToEnum<AllowCloseInstalment>();
        }

        private static void Initialize(InstalmentPolicyDetail instalmentPolicyDetail, DataRow dataRow)
        {
            instalmentPolicyDetail.InstalmentPolicyId = (Guid)dataRow["InstalmentPolicyId"];
            instalmentPolicyDetail.Period = (int)dataRow["Period"];
            instalmentPolicyDetail.MinDownPayment = dataRow.GetItemValue<decimal>("MinDownPayment", 0);
            instalmentPolicyDetail.MaxDownPayment = dataRow.GetItemValue<decimal>("MaxDownPayment", 0);
            instalmentPolicyDetail.InterestRate = dataRow.GetItemValue<decimal>("InterestRate", 0); 
            instalmentPolicyDetail.AdministrationFeeBase = dataRow["AdministrationFeeBase"].ToEnum<AdministrationFeeBaseType>();
            instalmentPolicyDetail.AdministrationFee = dataRow.GetItemValue<decimal>("AdministrationFee", 0);
            instalmentPolicyDetail.ContractTerminateType = dataRow["ContractTerminateType"].ToEnum<ContractTerminateType>();
            instalmentPolicyDetail.ContractTerminateFee = dataRow.GetItemValue<decimal>("ContractTerminateFee", 0);
            instalmentPolicyDetail.DownPaymentBasis = (int)dataRow["DownPaymentBasis"];
            instalmentPolicyDetail.Frequence = (int)dataRow["Frequence"];
            instalmentPolicyDetail.IsActive = (bool)dataRow["IsActive"];
            //instalmentPolicyDetail.IsActive = false;
        }

        private static void Initialize(Instrument instrument, DataRow dataRow)
        {
            instrument.Id = (Guid)dataRow["ID"];
            instrument.OriginCode = (string)dataRow["OriginCode"];
            instrument.Code = (string)dataRow["Code"];
            instrument.Description = (string)dataRow["Description"];
            instrument.Denominator = (int)dataRow["Denominator"];
            instrument.NumeratorUnit = (int)dataRow["NumeratorUnit"];
            instrument.CommissionFormula = (byte)dataRow["CommissionFormula"];
            instrument.TradePLFormula = (byte)dataRow["TradePLFormula"];
            instrument.OrderTypeMask = (int)dataRow["OrderTypeMask"];
            instrument.IsNormal = (bool)dataRow["IsNormal"];
            instrument.MaxDQLot = dataRow.GetItemValue<decimal>("MaxDQLot", 0);
            instrument.MaxOtherLot = dataRow.GetItemValue<decimal>("MaxOtherLot", 0);
            instrument.CurrencyId = (Guid)dataRow["CurrencyID"];
            instrument.PriceValidTime = (int)dataRow["PriceValidTime"];
            instrument.DqQuoteMinLot = dataRow.GetItemValue<decimal>("DQQuoteMinLot", 0);
            instrument.IsSinglePrice = (bool)dataRow["IsSinglePrice"];
            instrument.LastAcceptTimeSpan = TimeSpan.FromMinutes((int)dataRow["LastAcceptTimeSpan"]);
            instrument.BeginTime = (DateTime)dataRow["BeginTime"];
            instrument.EndTime = (DateTime)dataRow["EndTime"];
            instrument.AcceptDQVariation = (int)dataRow["AcceptDQVariation"];
            instrument.AcceptLmtVariation = (int)dataRow["AcceptLmtVariation"];
            instrument.AcceptCloseLmtVariation = (int)dataRow["AcceptCloseLmtVariation"];
            instrument.CancelLmtVariation = (int)dataRow["CancelLmtVariation"];
            instrument.AcceptIfDoneVariation = (int)dataRow["AcceptIfDoneVariation"];
            instrument.PriceType = dataRow["PriceType"].ToEnum<PriceType>();
            //instrument.IsHasDocument = (bool)dataRow["IsHasDocument"];
            instrument.Sequence = (int)dataRow["Sequence"];
            instrument.DayOpenTime = dataRow.GetItemValue<DateTime?>("DayOpenTime", null);
            instrument.DayCloseTime = dataRow.GetItemValue<DateTime?>("DayCloseTime", null);
            instrument.LastDayCloseTime = dataRow.GetItemValue<DateTime?>("LastDayCloseTime", null);
            instrument.NextDayOpenTime = dataRow.GetItemValue<DateTime?>("NextDayOpenTime", null);
            instrument.MocTime = dataRow.GetItemValue<DateTime?>("MOCTime", null);
            instrument.LastTradeDay = dataRow.GetItemValue<DateTime?>("LastTradeDay", null);
            instrument.IsActive = (bool)dataRow["IsActive"];
            instrument.Category = dataRow["Category"].ToEnum<InstrumentCategory>();
            instrument.CanPlacePendingOrderAtAnyTime = (bool)dataRow["CanPlacePendingOrderAtAnyTime"];
            if (dataRow.Table.Columns.Contains("ExternalExchangeCode"))
            {
                instrument.ExternalExchangeCode = dataRow.GetItemValue<string>("ExternalExchangeCode", null);
            }
            if (dataRow.Table.Columns.Contains("GroupID"))
            {
                instrument.GroupId = dataRow.GetItemValue<Guid?>("GroupID", null);
            }
            instrument.MarginFormula = (byte)dataRow["MarginFormula"];
            instrument.SequenceForQuotatoin = GuidMapping.Add(instrument.Id);
            instrument.AllowedNewTradeSides = Convert.ToInt16(dataRow["AllowedNewTradeSides"]);
            instrument.Unit = dataRow.GetItemValue<string>("Unit", "");
            instrument.PhysicalLotDecimal = dataRow.GetItemValue<byte>("PhysicalLotDecimal", 0);
            instrument.Narrative = dataRow.GetItemValue<string>("Narrative", "");
            instrument.QuoteDescription = dataRow.GetItemValue<string>("QuoteDescription", "");
            if (dataRow.Table.Columns.Contains("DeliveryTimeBeginDay"))
            {
                instrument.DeliveryTimeBeginDay = dataRow.GetItemValue<int>("DeliveryTimeBeginDay", 0);
            }
            if (dataRow.Table.Columns.Contains("DeliveryTimeEndDay"))
            {
                instrument.DeliveryTimeEndDay = dataRow.GetItemValue<int>("DeliveryTimeEndDay", 0);
            }
            if (dataRow.Table.Columns.Contains("DeliveryPointGroupId") && dataRow["DeliveryPointGroupId"] != DBNull.Value)
            {
                instrument.DeliveryPointGroupId = (Guid)dataRow["DeliveryPointGroupId"];
            }
        }

        private static void Initialize(DealingPolicyDetail dealingPolicyDetail, DataRow dataRow)
        {
            dealingPolicyDetail.DealingPolicyId = (Guid)(dataRow["DealingPolicyID"]);
            dealingPolicyDetail.InstrumentId = (Guid)(dataRow["InstrumentID"]);
            dealingPolicyDetail.MaxDQLot = dataRow.GetItemValue<decimal>("MaxDQLot", 0);
            dealingPolicyDetail.MaxOtherLot = dataRow.GetItemValue<decimal>("MaxOtherLot", 0);
            dealingPolicyDetail.DQQuoteMinLot = dataRow.GetItemValue<decimal>("DQQuoteMinLot", 0);

            dealingPolicyDetail.AcceptDQVariation = dataRow.GetItemValue<int>("AcceptDQVariation", 0);
            dealingPolicyDetail.AcceptLmtVariation = dataRow.GetItemValue<int>("AcceptLmtVariation", 0);
            dealingPolicyDetail.CancelLmtVariation = dataRow.GetItemValue<int>("CancelLmtVariation", 0);
            dealingPolicyDetail.AllowedNewTradeSides = Convert.ToInt16(dataRow["AllowedNewTradeSides"]);
            dealingPolicyDetail.AcceptCloseLmtVariation = dataRow.GetItemValue<int>("AcceptCloseLmtVariation", 0);
            dealingPolicyDetail.PriceValidTime = dataRow.GetItemValue<int>("PriceValidTime", 0);
        }

        private static void Initialize(QuotePolicyDetail quotePolicyDetail, DataRow dataRow)
        {
            quotePolicyDetail.QuotePolicyId = (Guid)dataRow["QuotePolicyID"];
            quotePolicyDetail.InstrumentId = (Guid)dataRow["InstrumentID"];
            quotePolicyDetail.IsOriginHiLo = ((bool)dataRow["IsOriginHiLo"]);
            quotePolicyDetail.HiLoSpread = ((int)dataRow["HiLoSpread"]);
            quotePolicyDetail.Spread = ((int)dataRow["SpreadPoints"]);
        }

        private static void Initialize(TradingTime tradingTime, DataRow dataRow)
        {
            tradingTime.InstrumentId = (Guid)dataRow["InstrumentID"];
            tradingTime.BeginTime = (DateTime)dataRow["BeginTime"];
            tradingTime.EndTime = (DateTime)dataRow["EndTime"];
        }

        private static void Initialize(PaymentInstructionRemark paymentInstructionRemark, DataRow dataRow)
        {
            paymentInstructionRemark.Remark = (string)dataRow["Remark"];
            paymentInstructionRemark.OrganizationId = (Guid)dataRow["OrganizationID"];
        }
    }
}

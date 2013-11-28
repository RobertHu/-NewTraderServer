using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Trader.Server.CppTrader.DataMapping.Enums;
using Trader.Server.CppTrader.DataMapping.Util;

namespace Trader.Server.CppTrader.DataMapping.Util
{
    public static class TradingSetHelper
    {
        public static void Initialize(this TradingSet tradingSet, DataSet dataSet)
        {
            tradingSet.Quotations = InitializationHelper.CreateArray<InitialQuotation>(dataSet, "Quotation", Initialize);
            tradingSet.Transactions = InitializationHelper.CreateArray<Transaction>(dataSet, "Transaction", Initialize);
            tradingSet.Orders = InitializationHelper.CreateArray<Order>(dataSet, "Order", Initialize);
            tradingSet.OrderRelations = InitializationHelper.CreateArray<OrderRelation>(dataSet, "OrderRelation", Initialize);
            tradingSet.AccountCurrencies = InitializationHelper.CreateArray<AccountCurrency>(dataSet, "AccountCurrency", Initialize);
            tradingSet.DayPLNotValueds = InitializationHelper.CreateArray<DayPLNotValued>(dataSet, "DayPLNotValued", Initialize);

            tradingSet.BestPendings = InitializationHelper.CreateArray<InitialPendingItem>(dataSet, "BestPending", Initialize);
            tradingSet.TimeAndSales = InitializationHelper.CreateArray<InitialTimeAndSale>(dataSet, "TimeAndSale", Initialize);
            tradingSet.HasMessage = TradingSetHelper.GetHasMessage(dataSet);

            tradingSet.GroupStatus = InitializationHelper.CreateArray<IdStatus>(dataSet, "InstrumentGroupState", Initialize);
            tradingSet.InstrumentStatus = InitializationHelper.CreateArray<IdStatus>(dataSet, "InstrumentState", Initialize);

            tradingSet.DeliveryRequests = InitializationHelper.CreateArray<DeliveryRequest>(dataSet, "DeliveryRequest", Initialize);
            tradingSet.ScrapDeposits = InitializationHelper.CreateArray<ScrapDeposit>(dataSet, "ScrapDeposit", Initialize);
            tradingSet.DeliveryRequestOrderRelation = InitializationHelper.CreateArray<DeliveryRequestOrderRelation>(dataSet, "DeliveryRequestOrderRelation", Initialize);
        }

        public static void InitializeMessages(this TradingSet tradingSet, DataSet messagesDataSet)
        {
            tradingSet.Messages = InitializationHelper.CreateArray<Chat>(messagesDataSet, "Messages", Initialize);
        }

        private static void Initialize(Chat chat, DataRow dataRow)
        {
            chat.Id = (Guid)dataRow["ID"];
            chat.Title = (string)dataRow["Title"];
            chat.Content = dataRow["Content"] == DBNull.Value ? null : (String)dataRow["Content"];
            chat.PublishTime = (DateTime)dataRow["PublishTime"];
        }

        private static void Initialize(IdStatus item, DataRow dataRow)
        {
            item.Id = (Guid)dataRow["Id"];
            item.Status = (SecurityTradingStatus)(int)dataRow["Status"];
        }

        private static void Initialize(AccountCurrency item, DataRow dataRow)
        {
            item.AccountId = (Guid)dataRow["AccountID"];
            item.CurrencyId = (Guid)dataRow["CurrencyID"];
            item.UnclearAmount = (decimal)dataRow["UnclearAmount"];
        }

        private static void Initialize(InitialQuotation item, DataRow dataRow)
        {
            item.InstrumentId = (Guid)dataRow["InstrumentID"];
            item.Timestamp = (DateTime)dataRow["Timestamp"];

            item.Bid = dataRow.GetItemValue<string>("Bid", null);
            item.Ask = dataRow.GetItemValue<string>("Ask", null);
            item.High = dataRow.GetItemValue<string>("High", null);
            item.Low = dataRow.GetItemValue<string>("Low", null);
            item.Open = dataRow.GetItemValue<string>("Open", null);
            if (dataRow.Table.Columns.Contains("Volume") && dataRow["Volume"] != DBNull.Value && (double)dataRow["Volume"] > 0) item.Volume = ((double)dataRow["Volume"]).ToString();
            if (dataRow.Table.Columns.Contains("TotalVolume") && dataRow["TotalVolume"] != DBNull.Value && (double)dataRow["TotalVolume"] > 0) item.TotalVolume = ((double)dataRow["TotalVolume"]).ToString();
            if (dataRow.Table.Columns.Contains("IsPrivateDailyAsk")) item.IsPrivateDailyAsk = dataRow.GetItemValue<bool>("IsPrivateDailyAsk", false);
            if (dataRow.Table.Columns.Contains("IsPrivateDailyBid")) item.IsPrivateDailyBid = dataRow.GetItemValue<bool>("IsPrivateDailyBid", false);
            item.IsPrivateOpen = dataRow.GetItemValue<bool>("IsPrivateOpen", false);
            item.PreClose = dataRow.GetItemValue<string>("PrevClose", null);
            item.IsPrivateClose = dataRow.GetItemValue<bool>("IsPrivateClose", false);
        }

        private static void Initialize(Transaction item, DataRow dataRow)
        {
            item.Id = (Guid)dataRow["ID"];
            item.AccountId = (Guid)dataRow["AccountID"];
            item.InstrumentId = (Guid)dataRow["InstrumentID"];
            item.Code = dataRow.GetItemValue<string>("Code", null);
            item.Type = dataRow["Type"].ToEnum<TransactionType>();
            if (dataRow.Table.Columns.Contains("TransactionSubType"))
            {
                item.SubType = dataRow["TransactionSubType"].ToEnum<TransactionSubType>();
            }
            item.Phase = dataRow["Phase"].ToEnum<Phase>();
            item.BeginTime = (DateTime)dataRow["BeginTime"];
            item.EndTime = (DateTime)dataRow["EndTime"];
            if (dataRow.Table.Columns.Contains("ExpireType"))
            {
                item.ExpireType = dataRow["ExpireType"].ToEnum<ExpireType>();
            }
            item.SubmitTime = (DateTime)dataRow["SubmitTime"];
            item.SubmitorId = dataRow.GetItemValue<Guid>("SubmitorID", Guid.Empty);
            item.ExecuteTime = dataRow.GetItemValue<DateTime?>("ExecuteTime", null);
            item.AssigningOrderId = dataRow.GetItemValue<Guid?>("AssigningOrderID", null);
            item.OrderType = dataRow["OrderType"].ToEnum<OrderType>();
            item.ContractSize = (decimal)dataRow["ContractSize"];
            if (dataRow.Table.Columns.Contains("InstrumentCategory"))
            {
                item.InstrumentCategory = dataRow["InstrumentCategory"].ToEnum<InstrumentCategory>(); 
            }
        }

        private static void Initialize(Order item, DataRow dataRow)
        {
            item.TransactionId = (Guid)dataRow["TransactionID"];
            item.Id = (Guid)dataRow["ID"];
            item.Code = dataRow.GetItemValue<string>("Code", null);
            item.Lot = (decimal)dataRow["Lot"];
            item.MinLot = dataRow.GetItemValue<decimal?>("MinLot", null);
            item.MaxShow = dataRow.GetItemValue<decimal?>("MaxShow", null);

            item.IsOpen = (bool)dataRow["IsOpen"];
            item.IsBuy = (bool)dataRow["IsBuy"];
            item.SetPrice = dataRow.GetItemValue<string>("SetPrice", null);
            item.SetPrice2 = dataRow.GetItemValue<string>("SetPrice2", null);
            item.ExecutePrice = dataRow.GetItemValue<string>("ExecutePrice", null);

            item.TradeOption = dataRow["TradeOption"].ToEnum<TradeOption>();
            item.DQMaxMove = dataRow.GetItemValue<int>("DQMaxMove", 0);
            item.ExecuteTradeDay = dataRow.GetItemValue<DateTime?>("ExecuteTradeDay", null);

            item.PhysicalPaidAmount = dataRow.GetItemValue<decimal?>("PhysicalPaidAmount", null);
            item.PhysicalTradeSide = dataRow["PhysicalTradeSide"].ToEnum<PhysicalTradeSide>();
            item.PhysicalRequestId = dataRow.GetItemValue<Guid?>("PhysicalRequestId", null);
            if (dataRow.Table.Columns.Contains("ValueAsMargin"))
                item.PedgeAmount = dataRow.GetItemValue<decimal?>("ValueAsMargin", null);
            if (dataRow.Table.Columns.Contains("InstalmentPolicyId"))
                item.InstalmentPolicyId = dataRow.GetItemValue<Guid?>("InstalmentPolicyId", null);
            if (dataRow.Table.Columns.Contains("PhysicalInstalmentType") && dataRow["PhysicalInstalmentType"] != DBNull.Value)
                item.PhysicalInstalmentType = dataRow["PhysicalInstalmentType"].ToEnum<PhysicalInstalmentType>();
            if (dataRow.Table.Columns.Contains("InstalmentFrequence") && dataRow["InstalmentFrequence"] != DBNull.Value)
                item.InstalmentFrequence = dataRow["InstalmentFrequence"].ToEnum<Frequence>();
            if (dataRow.Table.Columns.Contains("Period"))
                item.Period = dataRow.GetItemValue<int?>("Period", null);
            if (dataRow.Table.Columns.Contains("DownPayment"))
                item.DownPayment = dataRow.GetItemValue<decimal?>("DownPayment", null);
            if (dataRow.Table.Columns.Contains("RecalculateRateType") && dataRow["RecalculateRateType"] != DBNull.Value)
                item.RecalculateRateType = dataRow["RecalculateRateType"].ToEnum<RecalculateRateType>();
            if (dataRow.Table.Columns.Contains("IsInstalmentOverdue"))
                item.IsInstalmentOverdue = dataRow.GetItemValue<bool?>("IsInstalmentOverdue", null);
            if (dataRow.Table.Columns.Contains("PhysicalOriginValue"))
                item.PhysicalOriginValue = dataRow.GetItemValue<decimal?>("PhysicalOriginValue", null);
            if (dataRow.Table.Columns.Contains("PhysicalOriginValueBalance"))
                item.PhysicalOriginValueBalance = dataRow.GetItemValue<decimal?>("PhysicalOriginValueBalance", null);            
            if (dataRow.Table.Columns.Contains("PaidPledge"))
                item.PaidPledge = dataRow.GetItemValue<decimal?>("PaidPledge", null);
            if (dataRow.Table.Columns.Contains("PaidPledgeBalance"))
                item.PaidPledgeBalance = dataRow.GetItemValue<decimal?>("PaidPledgeBalance", null);
        }

        private static void Initialize(DeliveryRequest item, DataRow dataRow)
        {            
            item.Id = (Guid)dataRow["Id"];
            item.Code = dataRow.GetItemValue<string>("Code", "");
            item.AccountId = (Guid)dataRow["AccountId"];
            item.InstrumentId = (Guid)dataRow["InstrumentId"];
            //item.Instrument = dataRow.GetItemValue<string>("Instrument", null);
            item.RequireQuantity = (decimal)dataRow["RequireQuantity"];
            item.Unit = dataRow.GetItemValue<string>("Unit", "");
            item.AvailableTime = dataRow.GetItemValue<DateTime?>("AvailableTime", null);
            item.DeliveryTime = dataRow.GetItemValue<DateTime?>("DeliveryTime", null);
            item.Status = dataRow["Status"].ToEnum<DeliveryRequestStatus>();
            item.SubmitTime = dataRow.GetItemValue<DateTime?>("SubmitTime", null);
        }

        private static void Initialize(ScrapDeposit item, DataRow dataRow)
        {
            item.Id = (Guid)dataRow["Id"];
            item.Code = dataRow.GetItemValue<string>("Code", "");
            item.AccountId = (Guid)dataRow["AccountId"];
            //item.InstrumentId = (Guid)dataRow["InstrumentId"];
            if (dataRow.Table.Columns.Contains("ScrapInstrumentId"))
                item.ScrapInstrumentId = (Guid)dataRow["ScrapInstrumentId"];
            if (dataRow.Table.Columns.Contains("TradeInstrumentId"))
                item.TradeInstrumentId = (Guid)dataRow["TradeInstrumentId"];
            item.RawQuantity = (decimal)dataRow["RawQuantity"];
            item.Unit = dataRow.GetItemValue<string>("Unit", "");
            item.AcceptTime = dataRow.GetItemValue<DateTime?>("AcceptTime", null);
            item.Status = dataRow["Status"].ToEnum<ScrapDepositStatus>();
            item.SubmitTime = dataRow.GetItemValue<DateTime?>("SubmitTime", null);
            if (dataRow.Table.Columns.Contains("AdjustedQuantity"))
                item.AdjustedQuantity = dataRow.GetItemValue<decimal>("AdjustedQuantity", 0);
            if (dataRow.Table.Columns.Contains("FinalQuantity"))
                item.FinalQuantity = dataRow.GetItemValue<decimal>("FinalQuantity", 0);
        }

        private static void Initialize(DeliveryRequestOrderRelation item, DataRow dataRow)
        {
            item.DeliveryRequestId = (Guid)dataRow["DeliveryRequestId"];
            item.OpenOrderId = (Guid)dataRow["OpenOrderId"];
            item.DeliveryLot = (decimal)dataRow["DeliveryLot"];
            item.DeliveryQuantity = (decimal)dataRow["DeliveryQuantity"];
        }

        private static void Initialize(OrderRelation item, DataRow dataRow)
        {
            item.OrderId1 = (Guid)dataRow["OpenOrderId"];
            item.OrderId2 = (Guid)dataRow["CloseOrderId"];
            item.Lot = (decimal)dataRow["ClosedLot"];
        }

        private static void Initialize(DayPLNotValued item, DataRow dataRow)
        {
            item.OrderId = (Guid)dataRow["OrderID"];
            item.Interest = (decimal)dataRow["DayInterestPLNotValued"];
            item.Storage = (decimal)dataRow["DayStoragePLNotValued"];
        }

        private static void Initialize(InitialPendingItem item, DataRow dataRow)
        {
            item.OrganizationId = (Guid)dataRow["OrganizationId"];
            item.InstrumentId = (Guid)dataRow["InstrumentId"];
            item.IsBuy = (bool)dataRow["IsBuy"];
            item.Sequence = (long)dataRow["Sequence"];

            item.Price = (string)dataRow["Price"];
            item.Quantity = (decimal)dataRow["Quantity"];
        }

        private static void Initialize(InitialTimeAndSale item, DataRow dataRow)
        {
            item.OrganizationId = (Guid)dataRow["OrganizationId"];
            item.InstrumentId = (Guid)dataRow["InstrumentId"];

            item.Timestamp = (DateTime)dataRow["Timestamp"];
            item.Price = (string)dataRow["Price"];
            item.Quantity = (decimal)dataRow["Quantity"];
        }

        private static bool GetHasMessage(DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables["Message"];
            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if ((int)dataRow["MessageCount"] > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal static void Initialize(this Contract item, DataRow dataRow)
        {
            item.OriginOrderId = (Guid)dataRow["ID"];
            item.Lot = (decimal)dataRow["Lot"];
            item.LotBalance = (decimal)dataRow["LotBalance"];
            item.ExecutePrice = dataRow.GetItemValue<string>("ExecutePrice", null); ;

            item.CommissionSum = dataRow.GetItemValue<decimal>("CommissionSum", 0);
            item.LevySum = dataRow.GetItemValue<decimal>("LevySum", 0);
            item.InterestPerLot = dataRow.GetItemValue<decimal>("InterestPerLot", 0);
            item.StoragePerLot = dataRow.GetItemValue<decimal>("StoragePerLot", 0);
            item.InterestRate = dataRow.GetItemValue<decimal?>("InterestRate", null);

            //item.PeerOrderCodes = (String)dataRow["PeerOrderCodes"];
            if (dataRow.Table.Columns.Contains("TradePL")) item.TradePL = dataRow.GetItemValue<decimal>("TradePL", 0);

            if (dataRow.Table.Columns.Contains("TradePLFloat")) item.TradePLFloat = dataRow.GetItemValue<decimal>("TradePLFloat", 0);
            if (dataRow.Table.Columns.Contains("InterestPLFloat")) item.InterestPLFloat = dataRow.GetItemValue<decimal>("InterestPLFloat", 0);
            if (dataRow.Table.Columns.Contains("StoragePLFloat")) item.StoragePLFloat = dataRow.GetItemValue<decimal>("StoragePLFloat", 0);

            if (dataRow.Table.Columns.Contains("InterestPLNotValued")) item.InterestPLNotValued = dataRow.GetItemValue<decimal>("InterestPLNotValued", 0);
            if (dataRow.Table.Columns.Contains("StoragePLNotValued")) item.StoragePLNotValued = dataRow.GetItemValue<decimal>("StoragePLNotValued", 0);
            if (dataRow.Table.Columns.Contains("AutoLimitPriceString")) item.AutoLimitPriceString = dataRow.GetItemValue<string>("AutoLimitPriceString", null);
            if (dataRow.Table.Columns.Contains("AutoStopPriceString")) item.AutoStopPriceString = dataRow.GetItemValue<string>("AutoStopPriceString", null);

            if (dataRow.Table.Columns.Contains("Necessary")) item.Necessary = dataRow.GetItemValue<decimal>("Necessary", 0);
            if (dataRow.Table.Columns.Contains("ValueAsMargin")) item.PedgeAmount = dataRow.GetItemValue<decimal>("ValueAsMargin", 0);
            if (dataRow.Table.Columns.Contains("PhysicalInstalmentType"))
                item.CanInstalment = (dataRow["PhysicalInstalmentType"] == DBNull.Value || (int)dataRow["PhysicalInstalmentType"] == 0) ? false : true;
            if (dataRow.Table.Columns.Contains("IsInstalmentOverdue"))
                item.IsInstalmentOverdue = dataRow.GetItemValue<bool?>("IsInstalmentOverdue", null);
            if (dataRow["PhysicalInstalmentType"] == DBNull.Value)
            {
                item.IsPayoff = true;
            }
            else
            {
                //if ((dataRow.Table.Columns.Contains("PaidPledge")
                //&& dataRow.Table.Columns.Contains("PhysicalOriginValue")
                //&& dataRow["PaidPledge"] != DBNull.Value
                //&& dataRow["PhysicalOriginValue"] != DBNull.Value)
                //||
                //(dataRow.Table.Columns.Contains("PaidPledgeBalance")
                //&& dataRow.Table.Columns.Contains("PhysicalOriginValue")
                //&& dataRow["PaidPledgeBalance"] != DBNull.Value
                //&& dataRow["PhysicalOriginValue"] != DBNull.Value)
                //)
                //{
                //    item.IsPayoff = item.CanInstalment ? Math.Abs((decimal)dataRow["PaidPledgeBalance"]) == 0 : (Math.Abs((decimal)dataRow["PaidPledge"]) == (decimal)dataRow["PhysicalOriginValue"]);
                //}
                if ((dataRow.Table.Columns.Contains("PaidPledgeBalance")
                && dataRow.Table.Columns.Contains("PhysicalOriginValueBalance")
                && dataRow["PaidPledgeBalance"] != DBNull.Value
                && dataRow["PhysicalOriginValueBalance"] != DBNull.Value)
                )
                {
                    item.IsPayoff = (Math.Abs((decimal)dataRow["PaidPledgeBalance"]) == (decimal)dataRow["PhysicalOriginValueBalance"]);
                }
                else 
                {
                    item.IsPayoff = true;
                }
            }
        }

    }
}
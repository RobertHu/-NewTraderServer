using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.CppTrader.DataMapping.Enums;

namespace Trader.Server.CppTrader.DataMapping.Response
{

    public enum MarginApproveResult
    {
        NotApproved,
        Successed,
        NoEnoughBalaceToPay,
        Failed,
    }

    public class ChangePhonePasswordResult
    {
        public ChangePhonePasswordResult() { }

        public ChangePhonePasswordResult(bool isSuccess, string message)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }


    public class AddMarginResult
    {
        public AddMarginResult()
        {
        }

        public AddMarginResult(bool isSuccess, string reference)
        {
            this.IsSuccess = isSuccess;
            this.Reference = reference;
        }

        public AddMarginResult(bool isSuccess, string reference, string errorMessage)
            : this(isSuccess, reference)
        {
            this.ErrorMessage = errorMessage;
        }

        public bool IsSuccess { get; set; }
        public string Reference { get; set; }
        public string ErrorMessage { get; set; }
        public Guid AccountId { get; set; }
        public MarginApproveResult ApproveResult { get; set; }
    }


    public class OrderQueryResult
    {
        public string Code { get; set; }
        public string AccountCode { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime? ExecuteTime { get; set; }
        public string InstrumentCode { get; set; }
        public OrderType OrderType { get; set; }
        public decimal Lot { get; set; }
        public string Price { get; set; }
        public OpenClose OpenClose { get; set; }
        public BuySell BuySell { get; set; }
        public Phase Phase { get; set; }
        public string Remarks { get; set; }

        public TradeOption TradeOption { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionSubType TransactionSubType { get; set; }
        public iExchange.Common.ExchangeSystem ExchangeSystem { get; set; }

        //public string SetPrice { get; set; }
        //public string SetPrice2 { get; set; }
        //public string ExecutePrice { get; set; }
        //public decimal CommissionSum { get; set; }
        //public decimal LevySum { get; set; }
        //public decimal InterestPerLot { get; set; }
    }


    public class OpenInterest
    {
        public Guid InstrumentId;
        public double OpenInterestQuantity;
    }



    public class DailyVwap
    {
        public DateTime TradeDay { get; set; }
        public decimal? LastPrice { get; set; }
        public double TotalVolume { get; set; }
        public double ContractValue { get; set; }
        public double VWAP { get; set; }
    }
}

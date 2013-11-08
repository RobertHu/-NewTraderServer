using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.CppTrader.DataMapping.Enums;

namespace Trader.Server.CppTrader.DataMapping
{
    public class Order
    {
        public Guid Id
        {
            get;
            set;
        }

        public Guid TransactionId
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public decimal Lot
        {
            get;
            set;
        }

        public decimal? MinLot
        {
            get;
            set;
        }

        public decimal? MaxShow
        {
            get;
            set;
        }

        public bool IsOpen
        {
            get;
            set;
        }

        public bool IsBuy
        {
            get;
            set;
        }

        public string SetPrice
        {
            get;
            set;
        }

        public string ExecutePrice
        {
            get;
            set;
        }

        public DateTime? PriceTimestamp
        {
            get;
            set;
        }

        public bool IsQuotedPrice
        {
            get;
            set;
        }

        public string SetPrice2
        {
            get;
            set;
        }

        public string LimitPriceForIfDone
        {
            get;
            set;
        }

        public string StopPriceForIfDone
        {
            get;
            set;
        }

        public TradeOption TradeOption
        {
            get;
            set;
        }

        public int DQMaxMove
        {
            get;
            set;
        }

        public DateTime? ExecuteTradeDay
        {
            get;
            set;
        }
        
        public decimal? PhysicalPaidAmount
        {
            get;
            set;
        }

        public PhysicalTradeSide? PhysicalTradeSide
        {
            get;
            set;
        }

        public Guid? PhysicalRequestId
        {
            get;
            set;
        }

        public decimal? PedgeAmount
        {
            get;
            set;
        }
                
        public Guid? InstalmentPolicyId
        {
            get;
            set;
        }

        public PhysicalInstalmentType? PhysicalInstalmentType
        {
            get;
            set;
        }

        public RecalculateRateType? RecalculateRateType
        {
            get;
            set;
        }
        
        public int? Period
        {
            get;
            set;
        }

        public decimal? DownPayment
        {
            get;
            set;
        }
        
        public bool? IsInstalmentOverdue
        {
            get;
            set;
        }

        public Frequence? InstalmentFrequence
        {
            get;
            set;
        }

        public decimal? PhysicalOriginValue
        {
            get;
            set;
        }
        
        public int? DownPaymentBasis
        {
            get;
            set;
        }
 
        public override string ToString()
        {
            return string.Format("Id={0}, TransactionId={1}, Code={2}, Lot={3}, MinLot={4}, IsOpen={5}, IsBuy={6}, SetPrice={7}, SetPrice2={8}, LimitPriceForIfDone={9}, StopPriceForIfDone={10}, TradeOption={11}, DQMaxMove={12}, MaxShow={13}",
                Id, TransactionId, Code, Lot, MinLot, IsOpen, IsBuy, SetPrice, SetPrice2, LimitPriceForIfDone, StopPriceForIfDone, TradeOption, DQMaxMove, MaxShow);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping
{
    public class Contract
    {
        public Guid OriginOrderId
        {
            get;
            set;
        }

        public decimal Lot
        {
            get;
            set;
        }

        public decimal LotBalance
        {
            get;
            set;
        }

        public string ExecutePrice
        {
            get;
            set;
        }
        
        public decimal CommissionSum
        {
            get;
            set;
        }

        public decimal LevySum
        {
            get;
            set;
        }        

        public decimal InterestPerLot
        {
            get;
            set;
        }

        public decimal StoragePerLot
        {
            get;
            set;
        }

        public decimal? InterestRate
        {
            get;
            set;
        }

        public decimal TradePL
        {
            get;
            set;
        }

        public decimal InterestPL
        {
            get;
            set;
        }

        public decimal StoragePL
        {
            get;
            set;
        }

        public decimal TradePLFloat
        {
            get;
            set;
        } 

        public decimal InterestPLFloat
        {
            get;
            set;
        }

        public decimal StoragePLFloat
        {
            get;
            set;
        }

        public decimal InterestPLNotValued
        {
            get;
            set;
        }

        public decimal StoragePLNotValued
        {
            get;
            set;
        }       

        public string LivePrice
        {
            get;
            set;
        }

        public string AutoLimitPriceString
        {
            get;
            set;
        }

        public string AutoStopPriceString
        {
            get;
            set;
        }
        public decimal Necessary
        {
            get;
            set;
        }
        public decimal PedgeAmount
        {
            get;
            set;
        }

        public bool? IsPayoff
        {
            get;
            set;
        }

        public bool? IsInstalmentOverdue
        {
            get;
            set;
        }

        public bool CanInstalment
        {
            get;
            set;
        }
    }
}
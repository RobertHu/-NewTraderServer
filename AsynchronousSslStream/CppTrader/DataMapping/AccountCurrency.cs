using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping
{
    public class AccountCurrency
    {
        public Guid AccountId
        {
            get;
            set;
        }

        public Guid CurrencyId
        {
            get;
            set;
        }

        public decimal? Balance
        {
            get;
            set;
        }

        public decimal? Necessary
        {
            get;
            set;
        }

        public decimal? InterestPLNotValued
        {
            get;
            set;
        }

        public decimal? StoragePLNotValued
        {
            get;
            set;
        }

        public decimal? TradePLNotValued
        {
            get;
            set;
        }

        public decimal? InterestPLFloat
        {
            get;
            set;
        }

        public decimal? StoragePLFloat
        {
            get;
            set;
        }

        public decimal? TradePLFloat
        {
            get;
            set;
        }

        public decimal? UnclearAmount
        {
            get;
            set;
        }

        public decimal? PedgeAmount
        {
            get;
            set;
        }

        public decimal? FrozenFund
        {
            get;
            set;
        }

        public decimal? TotalPaidAmount
        {
            get;
            set;
        }

        public decimal? PartialPaymentPhysicalNecessary
        {
            get;
            set;
        }

        public override string ToString()
        {
            return string.Format("AccountId={0}, CurrencyId={1}, Balance={2}, Necessary={3}, InterestPLNotValued={4}, StoragePLNotValued={5}, TradePLNotValued={6}, InterestPLFloat={7}, StoragePLFloat={8}, TradePLFloat={9}, UnclearAmount={10}, PedgeAmount={11}, FrozenFund={12}",
                AccountId, CurrencyId == null ? "NULL" : CurrencyId.ToString(), Balance, Necessary, InterestPLNotValued, StoragePLNotValued, TradePLNotValued, InterestPLFloat, StoragePLFloat, TradePLFloat, UnclearAmount, PedgeAmount, FrozenFund);
        }
    }
}
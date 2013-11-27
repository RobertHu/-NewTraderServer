using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.CppTrader.DataMapping.Enums;

namespace Trader.Server.CppTrader.DataMapping
{
    public class TradingSet
    {
        public bool HasMessage
        {
            get;
            set;
        }

        public AccountBalance[] AccountBalances
        {
            get;
            set;
        }

        public InitialQuotation[] Quotations
        {
            get;
            set;
        }

        public AccountCurrency[] AccountCurrencies
        {
            get;
            set;
        }

        public DayPLNotValued[] DayPLNotValueds
        {
            get;
            set;
        }

        public Transaction[] Transactions
        {
            get;
            set;
        }

        public Order[] Orders
        {
            get;
            set;
        }

        public Contract[] Contracts
        {
            get;
            set;
        }

        public OrderRelation[] OrderRelations
        {
            get;
            set;
        }

        public DeliveryRequest[] DeliveryRequests
        {
            get;
            set;
        }

        public ScrapDeposit[] ScrapDeposits
        {
            get;
            set;
        }

        public DeliveryRequestOrderRelation[] DeliveryRequestOrderRelation
        {
            get;
            set;
        }

        public InitialPendingItem[] BestPendings { get; set; }

        public InitialTimeAndSale[] TimeAndSales { get; set; }

        public IdStatus[] GroupStatus { get; set; }
        public IdStatus[] InstrumentStatus { get; set; }
        public PriceRange[] PriceRanges { get; set; }

        public Chat[] Messages { get; set; }
    }

    public class IdStatus
    {
        public Guid Id { get; set; }
        public SecurityTradingStatus Status { get; set; }
    }
}
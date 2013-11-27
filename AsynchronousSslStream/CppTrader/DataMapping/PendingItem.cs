using System;

namespace Trader.Server.CppTrader.DataMapping
{
    public class PendingItem
    {
        public PendingItem() { }

        public PendingItem(string price, decimal quantity)
        {
            this.Price = price;
            this.Quantity = quantity;
        }

        public string Price
        {
            get;
            set;
        }

        public decimal Quantity
        {
            get;
            set;
        }
    }

    public class InitialPendingItem
    {
        public Guid OrganizationId { get; set; }
        public Guid InstrumentId { get; set; }
        public string Price { get; set; }
        public decimal Quantity { get; set; }
        public bool IsBuy { get; set; }
        public long Sequence { get; set; }
    }
}
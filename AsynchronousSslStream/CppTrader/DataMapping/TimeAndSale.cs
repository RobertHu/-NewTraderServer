using System;

namespace Trader.Server.CppTrader.DataMapping
{
    public class TimeAndSale
    {
        public TimeAndSale() { }

        public TimeAndSale(DateTime timestamp, string price, decimal quantity)
        {
            this.Timestamp = timestamp;
            this.Price = price;
            this.Quantity = quantity;
        }

        public DateTime Timestamp
        {
            get;
            set;
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

        public override string ToString()
        {
            return string.Format("Timestamp={0}, Price={1}, Quantity={2}", Timestamp, Price, Quantity);
        }
    }

    public class InitialTimeAndSale
    {
        public Guid OrganizationId { get; set; }

        public Guid InstrumentId { get; set; }

        public string Price { get; set; }

        public DateTime Timestamp { get; set; }

        public decimal Quantity { get; set; }

        public override string ToString()
        {
            return string.Format("OrganizationId={0}, InstrumentId={1}, Timestamp={2}, Price={3}, Quantity={4}", OrganizationId, InstrumentId, Timestamp, Price, Quantity);
        }
    }
}

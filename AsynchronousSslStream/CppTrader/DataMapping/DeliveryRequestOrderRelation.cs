using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping
{
    public class DeliveryRequestOrderRelation
    {
        public Guid OpenOrderId { get; set; }

        public decimal DeliveryQuantity { get; set; }

        public decimal DeliveryLot { get; set; }

        public Guid? DeliveryRequestId { get; set; }
        
        public override string ToString()
        {
            return string.Format("OpenOrderId={0}, DeliveryQuantity={1}, DeliveryLot={2}", OpenOrderId, DeliveryQuantity, DeliveryLot);
        }
    }    
}
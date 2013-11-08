using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using Trader.Server.CppTrader.DataMapping.Enums;

namespace Trader.Server.CppTrader.DataMapping
{
    public class DeliveryRequest
    {
        public Guid Id
        {
            get;
            set;
        }

        public Guid AccountId
        {
            get;
            set;
        }
        
        public Guid InstrumentId
        {
            get;
            set;
        }

        public Guid ChargeCurrencyId
        {
            get;
            set;
        }

        public decimal Charge
        {
            get;
            set;
        }

        public string Instrument
        {
            get;
            set;
        }

        public decimal RequireLot
        {
            get;
            set;
        }

        public decimal RequireQuantity
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public string Unit
        {
            get;
            set;
        }

        public DateTime? AvailableTime
        {
            get;
            set;
        }

        public DateTime? DeliveryTime
        {
            get;
            set;
        }

        public DeliveryRequestStatus? Status
        {
            get;
            set;
        }

        public DateTime? SubmitTime
        {
            get;
            set;
        }

        public Guid? DeliveryAddressId
        {
            get;
            set;
        }

        public DeliveryRequestOrderRelation[] DeliveryRequestOrderRelations { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Id=");
            stringBuilder.Append(Id);
            stringBuilder.Append(",");

            stringBuilder.Append("AccountId=");
            stringBuilder.Append(AccountId);
            stringBuilder.Append(",");

            stringBuilder.Append("InstrumentId=");
            stringBuilder.Append(InstrumentId);
            stringBuilder.Append(",");

            stringBuilder.Append("RequireQuantity=");
            stringBuilder.Append(RequireQuantity);
            stringBuilder.Append(",");
            
            stringBuilder.Append("DeliveryAddressId=");
            stringBuilder.Append(DeliveryAddressId);
            stringBuilder.Append(",");

            if (DeliveryRequestOrderRelations != null && DeliveryRequestOrderRelations.Length > 0)
            {
                stringBuilder.Append("DeliveryRequestRelations:");
                foreach (DeliveryRequestOrderRelation deliveryRequestOrderRelation in DeliveryRequestOrderRelations)
                {
                    stringBuilder.Append(deliveryRequestOrderRelation);
                    stringBuilder.Append(";");
                }
            }

            return stringBuilder.ToString();
        }
    }

    public static class DeliveryRequestSerializationHelper
    {
        public static byte[] ToByteArray(this DeliveryRequest deliveryRequest)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(DeliveryRequest));
                serializer.WriteObject(stream, deliveryRequest);
                return stream.ToArray();
            }
        }

        public static DeliveryRequest From(byte[] deliveryRequestData)
        {
            using (MemoryStream stream = new MemoryStream(deliveryRequestData))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(DeliveryRequest));
                DeliveryRequest deliveryRequest = (DeliveryRequest)serializer.ReadObject(stream);
                return deliveryRequest;
            }
        }
    }
}
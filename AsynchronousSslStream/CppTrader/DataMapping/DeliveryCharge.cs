using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using Trader.Server.CppTrader.DataMapping.Enums;

namespace Trader.Server.CppTrader.DataMapping
{
    public class DeliveryCharge
    {
        public Guid Id
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public PhysicalChargeBasis ChargeBasis
        {
            get;
            set;
        }

        public decimal ChargeRate
        {
            get;
            set;
        }

        public decimal MinCharge
        {
            get;
            set;
        }
        
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Id=");
            stringBuilder.Append(Id);
            stringBuilder.Append(",");

            stringBuilder.Append("Code=");
            stringBuilder.Append(Code);
            stringBuilder.Append(",");
            
            stringBuilder.Append("PriceType=");
            stringBuilder.Append(ChargeBasis);
            stringBuilder.Append(",");

            stringBuilder.Append("ChargeRate=");
            stringBuilder.Append(ChargeRate);
            stringBuilder.Append(",");

            stringBuilder.Append("MinCharge=");
            stringBuilder.Append(MinCharge);
            
            return stringBuilder.ToString();
        }
    }
}
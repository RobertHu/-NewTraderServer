using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace Trader.Server.CppTrader.DataMapping
{
    public class ScrapInstrument
    {
        public Guid Id
        {
            get;
            set;
        }

        public string Unit
        {
            get;
            set;
        }

        public int QuantityDecimalDigits
        {
            get;
            set;
        }
                
        public string Description
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

            stringBuilder.Append("Unit=");
            stringBuilder.Append(Unit);
            stringBuilder.Append(",");

            stringBuilder.Append("QuantityDecimalDigits=");
            stringBuilder.Append(QuantityDecimalDigits);
            stringBuilder.Append(",");

            stringBuilder.Append("Description=");
            stringBuilder.Append(Description);
            
            return stringBuilder.ToString();
        }
    }
}
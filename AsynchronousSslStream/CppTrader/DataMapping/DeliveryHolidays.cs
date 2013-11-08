using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace Trader.Server.CppTrader.DataMapping
{
    public class DeliveryHolidays
    {
        public DateTime BeginDate
        {
            get;
            set;
        }

        public DateTime EndDate
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

            stringBuilder.Append("BeginDate=");
            stringBuilder.Append(BeginDate);
            stringBuilder.Append(",");

            stringBuilder.Append("EndDate=");
            stringBuilder.Append(EndDate);
            stringBuilder.Append(",");

            stringBuilder.Append("Description=");
            stringBuilder.Append(Description);
            
            return stringBuilder.ToString();
        }
    }
}
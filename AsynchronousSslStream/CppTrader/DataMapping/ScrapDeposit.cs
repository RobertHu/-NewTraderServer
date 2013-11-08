using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using Trader.Server.CppTrader.DataMapping.Enums;

namespace Trader.Server.CppTrader.DataMapping
{
    public class ScrapDeposit
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
        
        public Guid TradeInstrumentId
        {
            get;
            set;
        }

        public Guid ScrapInstrumentId
        {
            get;
            set;
        }

        public string ScrapInstrumentCode
        {
            get;
            set;
        }

        public int QuantityDecimalDigits
        {
            get;
            set;
        }

        public decimal RawQuantity
        {
            get;
            set;
        }

        public decimal? AdjustedQuantity
        {
            get;
            set;
        }

        public decimal? FinalQuantity
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

        public DateTime? AcceptTime
        {
            get;
            set;
        }

        public ScrapDepositStatus? Status
        {
            get;
            set;
        }

        public DateTime? SubmitTime
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

            stringBuilder.Append("AccountId=");
            stringBuilder.Append(AccountId);
            stringBuilder.Append(",");

            stringBuilder.Append("ScrapInstrumentId=");
            stringBuilder.Append(ScrapInstrumentId);
            stringBuilder.Append(",");

            stringBuilder.Append("TradeInstrumentId=");
            stringBuilder.Append(TradeInstrumentId);
            stringBuilder.Append(",");

            stringBuilder.Append("ScrapInstrumentCode=");
            stringBuilder.Append(ScrapInstrumentCode);
            stringBuilder.Append(",");

            stringBuilder.Append("RequireQuantity=");
            stringBuilder.Append(RawQuantity);
            stringBuilder.Append(",");

            stringBuilder.Append("Unit=");
            stringBuilder.Append(Unit);
            stringBuilder.Append(",");

            stringBuilder.Append("SettlementTime=");
            stringBuilder.Append(AcceptTime);
            stringBuilder.Append(",");

            stringBuilder.Append("Status=");
            stringBuilder.Append(Status);
            stringBuilder.Append(",");

            stringBuilder.Append("SubmitTime=");
            stringBuilder.Append(SubmitTime);
            
            return stringBuilder.ToString();
        }
    }
}
using System;

namespace Trader.Server.CppTrader.DataMapping
{
    public class VolumeNecessaryDetail
    {
        public Guid VolumeNecessaryId
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }

        public decimal From
        {
            get;
            set;
        }

        public decimal MarginD
        {
            get;
            set;
        }

        public decimal MarginO
        {
            get;
            set;
        }
    }
}
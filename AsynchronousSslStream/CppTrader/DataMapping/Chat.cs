using System;

namespace Trader.Server.CppTrader.DataMapping
{
    public class Chat
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public String Content { get; set; }
        public DateTime PublishTime { get; set; }
    }
}
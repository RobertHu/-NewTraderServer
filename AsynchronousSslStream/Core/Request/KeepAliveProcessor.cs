using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Trader.Server.Serialization;
using Trader.Server.Bll;

namespace Trader.Server.Core.Request
{
    public class KeepAliveProcessor : IRequestProcessor
    {
        public static readonly KeepAliveProcessor Default = new KeepAliveProcessor();
        private KeepAliveProcessor() { }
        public PacketContent Process(Serialization.SerializedInfo request)
        {
            Debug.Assert(request.Content.ContentType == ContentType.KeepAlivePacket);
            bool isExist = Application.Default.SessionMonitor.Exist(request.ClientInfo.Session);
            return new PacketContent(request.Content.KeepAlive.Packet, isExist);
        }
    }
}

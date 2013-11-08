using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Common;
using CommonUtil;
namespace Trader.Server.Service
{
    public class NamedCommands
    {
        private const string KickoutContent = "<KickoutCommand/>";
        private static object _Lock = new object();
        private static UnmanagedMemory _KickoutPacket;
        public static UnmanagedMemory GetKickoutPacket()
        {
            if (_KickoutPacket != null)
            {
                return _KickoutPacket;
            }
            lock (_Lock)
            {
                if (_KickoutPacket != null)
                {
                    return _KickoutPacket;
                }
                byte[] contentBytes = PacketConstants.ContentEncoding.GetBytes(KickoutContent);
                byte[] contentLengthBytes = contentBytes.Length.ToCustomerBytes();
                byte[] packet = new byte[PacketConstants.HeadLength + contentBytes.Length];
                Buffer.BlockCopy(contentLengthBytes, 0, packet, PacketConstants.ContentLengthIndex, contentLengthBytes.Length);
                Buffer.BlockCopy(contentBytes, 0, packet, PacketConstants.HeadLength, contentBytes.Length);
                _KickoutPacket = new UnmanagedMemory(packet);
                return _KickoutPacket;
            }

        }
    }
}

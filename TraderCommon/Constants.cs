using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtil;
namespace Trader.Common
{
    public static class PacketConstants
    {
        public const int SettingLength = 1;
        public const int SessionHeaderLength = 1;
        public const int ContentHeaderLength = 4;
        public const int SettingIndex = 0;
        public const int SessionLengthIndex = 1;
        public const int ContentLengthIndex = 2;
        public const int HeadLength = 6;
        public const int InvokeIdLength = 36;
        public static readonly Encoding SessionEncoding = Encoding.ASCII;
        public static readonly Encoding ContentEncoding = Encoding.UTF8;
        public static readonly Encoding ClientInvokeIDEncoding = Encoding.ASCII;
    }

    public static class PacketHelper
    {
        public static int GetContentLength(byte[] source, int index)
        {
            Byte[] bytes = new byte[PacketConstants.ContentHeaderLength];
            Buffer.BlockCopy(source, index, bytes, 0, PacketConstants.ContentHeaderLength);
            return bytes.ToCustomerInt();
        }

        public static int GetPacketLength(byte[] source, int index)
        {
            int sessionLength = source[index + PacketConstants.SessionLengthIndex];
            int contentLength = GetContentLength(source, index + PacketConstants.ContentLengthIndex);
            return PacketConstants.HeadLength + sessionLength + contentLength;
        }
    }
}

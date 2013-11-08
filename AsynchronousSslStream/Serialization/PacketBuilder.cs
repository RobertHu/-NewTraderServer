using System;
using CommonUtil;
using System.Xml.Linq;
using Trader.Common;
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace Trader.Server.Serialization
{
    public static class PacketBuilder
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);
        public  static UnmanagedMemory Build(SerializedObject response)
        {
            ContentType contentType = response.Content.ContentType;
            if (contentType==ContentType.KeepAlivePacket)
            {
                return BuildForKeepAlive(response);
            }

            if (contentType == ContentType.UnmanageMemory)
            {
                return  BuildForPointer(response.Content.UnmanageMem, response.ClientInfo.ClientInvokeId);
            }

            return BuildForGeneralFormat(response);
        }


        private static UnmanagedMemory BuildForGeneralFormat(SerializedObject response)
        {
             if (!string.IsNullOrEmpty(response.ClientInfo.ClientInvokeId))
            {
                AppendClientInvokeIdToContentNode(response.Content, response.ClientInfo.ClientInvokeId);
            }
            byte[] contentBytes = GetContentBytes(response.Content);
            byte[] sessionBytes = GetSessionBytes(response.ClientInfo.Session.ToString());
            byte sessionLengthByte = (byte)sessionBytes.Length;
            byte[] contentLengthBytes = contentBytes.Length.ToCustomerBytes();
            int packetLength = PacketConstants.HeadLength + sessionLengthByte + contentBytes.Length;
            var packet = new UnmanagedMemory(packetLength);
            const byte priceByte = 0;
            AddHeaderToPacket(packet, priceByte, sessionLengthByte, contentLengthBytes);
            AddSessionToPacket(packet, sessionBytes, PacketConstants.HeadLength);
            AddContentToPacket(packet, contentBytes, PacketConstants.HeadLength + sessionLengthByte);
            return packet;
        }


        private static UnmanagedMemory BuildForKeepAlive(SerializedObject response)
        {
            Debug.Assert(response.Content.ContentType == ContentType.KeepAlivePacket, "content type should be keepalive");
            KeepAlive keepAlive = response.Content.KeepAlive;
            keepAlive.Packet[PacketConstants.SettingIndex] = keepAlive.IsSuccess ? PacketFirstHeadByteValue.IsKeepAliveAndSuccessValue : PacketFirstHeadByteValue.IsKeepAliveAndFailedValue;
            UnmanagedMemory packet = new UnmanagedMemory(keepAlive.Packet);
            return packet;
        }


        private unsafe static UnmanagedMemory BuildForPointer(UnmanagedMemory source, string invokeID)
        {
            UnmanagedMemory content = ZlibHelper.ZibCompress(source.ToArray());
            source.Dispose();
            int contentLength = PacketConstants.InvokeIdLength + content.Length;
            int packetLength=PacketConstants.HeadLength + contentLength;
            UnmanagedMemory packet = new UnmanagedMemory(packetLength);
            byte[] contentLengthBytes = contentLength.ToCustomerBytes();
            packet.Handle[PacketConstants.SettingIndex] = PacketFirstHeadByteValue.IsPlainString;
            packet.Handle[PacketConstants.SessionLengthIndex] = 0;
            int offset = PacketConstants.ContentLengthIndex;
            Marshal.Copy(contentLengthBytes, 0, (IntPtr)(packet.Handle + offset), contentLengthBytes.Length);
            offset = PacketConstants.HeadLength;
            byte[] invokeIDBytes = PacketConstants.ClientInvokeIDEncoding.GetBytes(invokeID);
            Marshal.Copy(invokeIDBytes, 0, (IntPtr)(packet.Handle + offset), invokeIDBytes.Length);
            offset += invokeIDBytes.Length;
            CopyMemory((IntPtr)(packet.Handle + offset),(IntPtr)content.Handle, (uint)content.Length);
            content.Dispose();
            return packet;
        }



        public static UnmanagedMemory BuildPrice(byte[] price)
        {
            return BuildForCommandCommon(price, true);
          
        }

        public static UnmanagedMemory BuildForContentInBytesCommand(byte[] content)
        {
            return BuildForCommandCommon(content, false);
        }

        private unsafe static UnmanagedMemory BuildForCommandCommon(byte[] data, bool isPrice)
        {
            int packetLength = PacketConstants.HeadLength + data.Length;
            UnmanagedMemory packet =new UnmanagedMemory(packetLength);
            byte[] contentLengthBytes = CustomerIntCache.Get(data.Length);
            packet.Handle[PacketConstants.SettingIndex] = 0;
            if (isPrice)
            {
                byte priceByte = PacketFirstHeadByteValue.IsPrice;
                packet.Handle[PacketConstants.SettingIndex] = priceByte;
            }
            packet.Handle[PacketConstants.SessionLengthIndex] = 0;
            int offset = PacketConstants.ContentLengthIndex;
            Marshal.Copy(contentLengthBytes, 0, (IntPtr)(packet.Handle + offset), contentLengthBytes.Length);
            offset = PacketConstants.HeadLength;
            Marshal.Copy(data, 0, (IntPtr)(packet.Handle + offset), data.Length);
            return packet;
        }


        private static void AppendClientInvokeIdToContentNode(PacketContent contentNode,string invokeID)
        {
            if (contentNode.ContentType == ContentType.Xml)
            {
                contentNode.XmlContent.Add(new XElement(RequestConstants.InvokeIdNodeName, invokeID));
            }
            else if (contentNode.ContentType == ContentType.Json)
            {
                contentNode.JsonContent.Add(RequestConstants.InvokeIdNodeName, invokeID);
            }
            else
            {
                throw new NotSupportedException(string.Format("such contentType current not define {0}", contentNode.ContentType));
            }
        }


        private unsafe static void AddSessionToPacket(UnmanagedMemory packet,byte[] sessionBytes,int index)
        {
            if (sessionBytes != null)
            {
                Marshal.Copy(sessionBytes, 0, (IntPtr)(packet.Handle + index), sessionBytes.Length);
            }
        }

        private unsafe static void AddContentToPacket(UnmanagedMemory packet, byte[] contentBytes,int index)
        {
            Marshal.Copy(contentBytes, 0, (IntPtr)(packet.Handle + index), contentBytes.Length);
        }

        private unsafe static void AddHeaderToPacket(UnmanagedMemory packet, byte isPrice, byte sessionLength, byte[] contentLengthBytes)
        {
            packet.Handle[PacketConstants.SettingIndex] = isPrice;
            packet.Handle[PacketConstants.SessionLengthIndex] = sessionLength;
            const int startIndex = PacketConstants.ContentLengthIndex;
            Marshal.Copy(contentLengthBytes, 0, (IntPtr)(packet.Handle + startIndex), contentLengthBytes.Length);
        }

        private static byte[] GetSessionBytes(string sessionID)
        {
            if (sessionID.IsNullOrEmpty())
            {
                return new byte[0];
            }
            byte[] bytes = PacketConstants.SessionEncoding.GetBytes(sessionID);
            if (bytes.Length > 255) throw new ArgumentOutOfRangeException("the length of SessionID");
            return bytes;
        }

        private static byte[] GetContentBytes(PacketContent content)
        {
            Debug.Assert(content.ContentType == ContentType.Xml || content.ContentType == ContentType.Json);
            byte[] bytes;
            if (content.ContentType == ContentType.Xml)
            {
                bytes = PacketConstants.ContentEncoding.GetBytes(content.XmlContent.ToString());
            }
            else
            {
                bytes = PacketConstants.ContentEncoding.GetBytes(content.JsonContent.ToString());
            }
            return bytes;
        }
      
    }
}

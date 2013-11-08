using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtil;
using System.Xml;
using System.Xml.Linq;
using Trader.Common;
using log4net;
using System.IO;
using Newtonsoft.Json.Linq;
namespace Trader.Server.Serialization
{
    public static class PacketParser
    {
        private static ILog _Logger = LogManager.GetLogger(typeof(PacketParser));

        public static SerializedObject  Parse(byte[] packet)
        {
            try
            {
                bool isKeepAlive = (packet[PacketConstants.SettingIndex] & PacketFirstHeadByteValue.IsKeepAliveMask) == PacketFirstHeadByteValue.IsKeepAliveMask;
                if (isKeepAlive)
                {
                    return ParseForKeepAlive(packet);
                }
                return ParseForGeneral(packet);

            }
            catch (Exception ex)
            {
                _Logger.Error("parse packet", ex);
                return null;
            }
        }

        private static SerializedObject ParseForGeneral(byte[] packet)
        {
            byte sessionLength = packet[PacketConstants.SessionLengthIndex];
            byte[] contentLengthBytes = new byte[PacketConstants.ContentHeaderLength];
            Array.Copy(packet, PacketConstants.ContentLengthIndex, contentLengthBytes, 0, PacketConstants.ContentHeaderLength);
            int contentLength = contentLengthBytes.ToCustomerInt();
            byte[] contentBytes = new byte[contentLength];
            int contentIndex = PacketConstants.HeadLength + sessionLength;
            Array.Copy(packet, contentIndex, contentBytes, 0, contentLength);
            string sessionStr = PacketConstants.SessionEncoding.GetString(packet, PacketConstants.HeadLength, sessionLength);
            Session session = SessionMapping.Get(sessionStr);
            string content = PacketConstants.ContentEncoding.GetString(contentBytes);
            bool isContentInXmlFormat = (packet[PacketConstants.SettingIndex] & PacketFirstHeadByteValue.IsXmlFormatMask) == PacketFirstHeadByteValue.IsXmlFormatMask;
            if (isContentInXmlFormat)
            {
                return ParseXmlPacket(session, content);
            }
            bool isContentInJsonFormat = (packet[PacketConstants.SettingIndex] & PacketFirstHeadByteValue.IsJsonFormatMask) == PacketFirstHeadByteValue.IsJsonFormatMask;
            if (isContentInJsonFormat)
            {
                JObject jsonContent = JObject.Parse(content);
                var clientInvokeIdProperty = jsonContent[RequestConstants.InvokeIdNodeName];
                string clientInvokeId = clientInvokeIdProperty == null ? string.Empty : clientInvokeIdProperty.ToString();
                return null;
            }
            throw new InvalidOperationException("the packet type can't be recognized");
        }

        private static SerializedObject ParseXmlPacket(Session session, string content)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);
            using (var nodeReader = new XmlNodeReader(doc))
            {
                nodeReader.MoveToContent();
                XElement contentNode = XDocument.Load(nodeReader).Root;
                XElement clientInvokeNode = FetchClientInvokeNode(contentNode);
                string clientInvokeId = clientInvokeNode == null ? string.Empty : clientInvokeNode.Value;
                return SerializedObject.CreateForXml(session, clientInvokeId, contentNode);
            }
        }

        private static SerializedObject ParseForKeepAlive(byte[] packet)
        {
            byte sessionLength = packet[PacketConstants.SessionLengthIndex];
            string sessionStr = PacketConstants.SessionEncoding.GetString(packet, PacketConstants.HeadLength, sessionLength);
            Session session = SessionMapping.Get(sessionStr);
            return SerializedObject.CreateForKeepAlive(session, true, packet);
        }

        private static XElement FetchClientInvokeNode(XElement parent)
        {
            XElement result = null;
            foreach (XElement ele in parent.Descendants())
            {
                if (ele.Name == RequestConstants.InvokeIdNodeName)
                {
                    result = ele;
                    break;
                }
            }
            return result;
        }
    }
}

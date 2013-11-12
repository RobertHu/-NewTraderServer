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

        public static SerializedInfo  Parse(byte[] packet)
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

        private static SerializedInfo ParseForGeneral(byte[] packet)
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
            bool isContentInJsonFormat = (packet[PacketConstants.SettingIndex] & PacketFirstHeadByteValue.IsJsonFormatMask) == PacketFirstHeadByteValue.IsJsonFormatMask;
            if (isContentInJsonFormat)
            {
                return ParseJsonPacket(session, content);
            }
            return ParseXmlPacket(session, content);
        }

        private static SerializedInfo ParseXmlPacket(Session session, string content)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);
            using (var nodeReader = new XmlNodeReader(doc))
            {
                nodeReader.MoveToContent();
                XElement contentNode = XDocument.Load(nodeReader).Root;
                XElement clientInvokeNode = FetchClientInvokeNode(contentNode);
                string clientInvokeId = clientInvokeNode == null ? string.Empty : clientInvokeNode.Value;
                return SerializedInfo.CreateForXml(session, clientInvokeId, contentNode);
            }
        }

        private static SerializedInfo ParseJsonPacket(Session session, string content)
        {
            JObject jsonContent = JObject.Parse(content);
            var clientInvokeIdProperty = jsonContent[RequestConstants.InvokeIdNodeName];
            string clientInvokeId = clientInvokeIdProperty == null ? string.Empty : clientInvokeIdProperty.ToString();
            return SerializedInfo.CreateForJson(session, clientInvokeId, jsonContent);
        }

        private static SerializedInfo ParseForKeepAlive(byte[] packet)
        {
            byte sessionLength = packet[PacketConstants.SessionLengthIndex];
            string sessionStr = PacketConstants.SessionEncoding.GetString(packet, PacketConstants.HeadLength, sessionLength);
            Session session = SessionMapping.Get(sessionStr);
            return SerializedInfo.CreateForKeepAlive(session, true, packet);
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

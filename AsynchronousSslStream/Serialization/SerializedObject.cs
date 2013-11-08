using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Trader.Common;
using Trader.Server.Ssl;
using Trader.Server.SessionNamespace;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Trader.Server.Serialization
{
    public class SerializedObject
    {
        private SerializedObject() { }

        public static SerializedObject CreateForKeepAlive(Session session, bool isKeepAlive, byte[] keepAlivePacket)
        {
            SerializedObject target = new SerializedObject()
            {
                Content = new PacketContent(keepAlivePacket),
                ClientInfo = new ClientInfo(null, session)
            };
            return target;
        }
        public static SerializedObject CreateForXml(Session session, string clientInvokeId, XElement content)
        {
            SerializedObject target = new SerializedObject()
            {
                Content= new PacketContent(content),
                ClientInfo=new ClientInfo(clientInvokeId,session)
            };
            return target;
        }

        public static SerializedObject CreateForJson(Session session, string clientInvokeId, JObject content)
        {
            SerializedObject target = new SerializedObject()
            {
                Content=new PacketContent(content),
                ClientInfo=new ClientInfo(clientInvokeId,session)
            };
            return target;
        }

        public static SerializedObject CreateForUnmanageMemory(Session session, string clientInvokeId, UnmanagedMemory mem)
        {
            SerializedObject target = new SerializedObject()
            {
                Content=new PacketContent(mem),
                ClientInfo=new ClientInfo(clientInvokeId,session)
            };
            return target;
        }


        public PacketContent Content { get; set; }
        public ClientInfo ClientInfo { get; set; }
    }

    public enum ContentType
    {
        Xml,
        UnmanageMemory,
        Json,
        KeepAlivePacket
    }

    public class ClientInfo
    {
        public ClientInfo(string clientInvokeId,Session session)
        {
            this.ClientInvokeId = clientInvokeId;
            this.Session = session;
        }
        public string ClientInvokeId { get; private set; }
        public Session ClientId { get; set; }
        public Session Session { get; set; }
        public Client Sender { get; set; }
        public string RemoteIp { get; set; } 
    }

    public class KeepAlive
    {
        public KeepAlive(byte[] content)
        {
            Content=content;
        }
        public bool IsSuccess { get;  set; }
        public byte[] Content { get; private set; }
    }

    public class PacketContent
    {
        public PacketContent(XElement xmlContent)
        {
            this.XmlContent = xmlContent;
            this.ContentType = ContentType.Xml;
        }

        public PacketContent(UnmanagedMemory mem)
        {
            this.UnmanageMem = mem;
            this.ContentType = ContentType.UnmanageMemory;
        }

        public PacketContent(JObject jsonContent)
        {
            this.JsonContent = jsonContent;
            this.ContentType = ContentType.Json;
        }

        public PacketContent(byte[] keepAlivePacket)
        {
            this.KeepAlive = new KeepAlive(keepAlivePacket);
            this.ContentType = ContentType.KeepAlivePacket;
        }

        public XElement XmlContent { get; private set; }
        public UnmanagedMemory UnmanageMem { get; private set; }
        public JObject JsonContent { get; private set; }
        public KeepAlive KeepAlive { get; private set; }
        public ContentType ContentType { get; set; }
    }
}

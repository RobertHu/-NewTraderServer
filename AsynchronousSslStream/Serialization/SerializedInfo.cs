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
    public class SerializedInfo
    {
        private SerializedInfo() { }

        public PacketContent Content { get; private set; }
        public ClientInfo ClientInfo { get; private set; }

        public void UpdateContent(PacketContent content)
        {
            this.Content = content;
        }

        public static SerializedInfo CreateForKeepAlive(Session session, bool isKeepAlive, byte[] keepAlivePacket)
        {
            SerializedInfo target = new SerializedInfo()
            {
                Content = new PacketContent(keepAlivePacket),
                ClientInfo = new ClientInfo(null, session)
            };
            return target;
        }
        public static SerializedInfo CreateForXml(Session session, string clientInvokeId, XElement content)
        {
            SerializedInfo target = new SerializedInfo()
            {
                Content= new PacketContent(content),
                ClientInfo=new ClientInfo(clientInvokeId,session)
            };
            return target;
        }

        public static SerializedInfo CreateForJson(Session session, string clientInvokeId, JObject content)
        {
            SerializedInfo target = new SerializedInfo()
            {
                Content=new PacketContent(content),
                ClientInfo=new ClientInfo(clientInvokeId,session)
            };
            return target;
        }
       
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
        public Session ClientId { get; private set; }
        public Session Session { get; private set; }
        public Client Sender { get;private set; }
        public string RemoteIp { get; private set; }

        public void Initialize(Session clientId,Client sender,string remoteIp)
        {
            this.ClientId = clientId;
            this.Sender = sender;
            this.RemoteIp = remoteIp;
        }

        public void UpdateSession(Session session)
        {
            this.Session = session;
        }
    }

    public struct KeepAlive
    {
        private bool _IsSuccess;
        private byte[] _Packet;
        public KeepAlive(byte[] content,bool isSuccess)
        {
            _Packet=content;
            _IsSuccess = isSuccess;
        }
        public bool IsSuccess { get { return _IsSuccess; } }
        public byte[] Packet { get { return _Packet; } }
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

        public PacketContent(byte[] keepAlivePacket,bool isSuccess=false)
        {
            this.KeepAlive = new KeepAlive(keepAlivePacket,isSuccess);
            this.ContentType = ContentType.KeepAlivePacket;
        }

        public XElement XmlContent { get; private set; }
        public UnmanagedMemory UnmanageMem { get; private set; }
        public JObject JsonContent { get; private set; }
        public KeepAlive KeepAlive { get; private set; }
        public ContentType ContentType { get;private  set; }
    }
}

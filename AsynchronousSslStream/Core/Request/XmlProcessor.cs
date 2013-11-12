using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Trader.Common;
using Trader.Server.Serialization;

namespace Trader.Server.Core.Request
{
    public class XmlProcessor : IRequestProcessor
    {
        public static readonly XmlProcessor Default = new XmlProcessor();
        private XmlProcessor() { }

        public PacketContent Process(Serialization.SerializedInfo request)
        {
            XElement content = request.Content.XmlContent;
            if (content.Name != RequestConstants.RootNodeName)
                throw new InvalidOperationException("thre request is not valid");
            var methodNode = content.Descendants().Single(m => m.Name == RequestConstants.MethodNodeName);
            if (methodNode.Name == RequestConstants.MethodNodeName)
            {
                return MethodRequestProcessor.Process(request, methodNode.Value);
            }
            throw new NotSupportedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.Serialization;
using System.Diagnostics;
using Trader.Common;

namespace Trader.Server.Core.Request
{
    public class JsonProcessor : IRequestProcessor
    {
        public static readonly JsonProcessor Default = new JsonProcessor();
        private JsonProcessor() { }
        public PacketContent Process(SerializedInfo request)
        {
            Debug.Assert(request.Content.ContentType == ContentType.Json);
            var methodProperty = request.Content.JsonContent.Request[RequestConstants.MethodNodeName];
            if (methodProperty == null)
                throw new NotSupportedException();
            string methodName = methodProperty.ToString();
            return MethodRequestProcessor.Process(request, methodName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.CppTrader.CommunicationAbstract;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Trader.Server.TypeExtension;

namespace Trader.Server.CppTrader.Communication
{
    public class ResponseProvider : IResponseProvider
    {
        public JObject Generate<T>(T content) where T : class
        {
            return Generate(content, null);
        }

        public JObject Generate<T>(T content, string clientInvokeId) where T : class
        {
            string contentJson = JsonConvert.SerializeObject(content, Formatting.None);
            dynamic wholeContent = new JObject();
            wholeContent.Result = contentJson;
            if (clientInvokeId.HasValue())
                wholeContent.ClientInvokeId = clientInvokeId;
            return wholeContent;
        }
    }
}

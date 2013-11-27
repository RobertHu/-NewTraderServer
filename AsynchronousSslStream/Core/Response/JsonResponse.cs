using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Trader.Server.Serialization;
using Trader.Common;

namespace Trader.Server.Core.Response
{
    public static class JsonResponse
    {
        public static PacketContent NewErrorResult(string clientInvokeId,string error = "")
        {
            var result = new ErrorResult(error, clientInvokeId);
            return new PacketContent(Generate(result));
        }

        public static PacketContent NewResult(string clientInvokeId,object obj)
        {
            var result = new SuccessfulResult(obj, clientInvokeId);
            return new PacketContent(Generate(result));
        }

        private static JsonContent Generate(object obj)
        {
            string result = JsonConvert.SerializeObject(obj, Formatting.None);
            return new JsonContent(response: result);
        }

        private class SuccessfulResult
        {
            public SuccessfulResult(object result,string invokeId)
            {
                this.Result = result;
                this.InvokeId = invokeId;
            }
            public object Result { get; private set; }
            public string InvokeId { get; private set; }
        }

        private class ErrorResult
        {
            public ErrorResult(string error,string invokeId)
            {
                this.Error = error;
                this.InvokeId = invokeId;
            }
            public string Error { get; private set; }
            public string InvokeId { get; private set; }
        }


    }
    
}

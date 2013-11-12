using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Trader.Server.Serialization;

namespace Trader.Server.Core.Response
{
    public static class JsonResponse
    {
        public static PacketContent NewErrorResult(string error = "")
        {
            var errorResult = new ErrorResult { Error = error };
            return new PacketContent(Generate(errorResult));
        }

        public static PacketContent NewResult(object obj)
        {
            return new PacketContent(Generate(obj));
        }

        private static JObject Generate(object obj)
        {
            string result = JsonConvert.SerializeObject(obj, Formatting.None);
            dynamic content = new JObject();
            content.Result = result;
            return content;
        }

    }

    public class ErrorResult
    {
        public string Error{get;set;}
    }
    
}

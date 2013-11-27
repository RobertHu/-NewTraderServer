using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.Serialization;
using Trader.Common;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Trader.Server.Core.Request
{
    public class ArgumentsParser
    {
        public static List<string> Parse(PacketContent content)
        {
            if (content.ContentType == ContentType.Xml)
            {
                return ParseXml(content.XmlContent);
            }
            else if (content.ContentType == ContentType.Json)
            {
                return ParseJson(content.JsonContent.Request);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private static List<string> ParseXml(XElement content)
        {
            var args = content.Descendants(RequestConstants.ArgumentNodeName).SingleOrDefault();
            List<string> argList = new List<string>();
            if (args == null)
                throw new ArgumentException("arguments node can't be found");
            foreach (var node in args.Descendants())
            {
                argList.Add(node.Value);
            }
            return argList;
        }

        private static List<string> ParseJson(JObject content)
        {
            var args = content[RequestConstants.ArgumentNodeName];
            if (args == null)
                throw new ArgumentException("argument property can't be found");
            return JsonConvert.DeserializeObject<List<string>>(args.ToString());
        }
    }
}

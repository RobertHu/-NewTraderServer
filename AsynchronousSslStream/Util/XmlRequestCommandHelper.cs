using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Trader.Common;
using Trader.Server.Serialization;

namespace Trader.Server.Util
{
    public static class XmlRequestCommandHelper
    {
        public static List<string> GetArguments(PacketContent content)
        {
            if (content.ContentType != ContentType.Xml)
                throw new NotSupportedException();

            var args = content.XmlContent.Descendants(RequestConstants.ArgumentNodeName).SingleOrDefault();
            List<string> argList = new List<string>();
            if (args == null) { return null; }
            else
            {
                foreach (var node in args.Descendants())
                {
                    argList.Add(node.Value);
                }
                return argList;
            }
        }
    }
}

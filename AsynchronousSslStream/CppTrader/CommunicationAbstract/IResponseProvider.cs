using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Trader.Server.CppTrader.CommunicationAbstract
{
    public interface IResponseProvider
    {
        JObject Generate<T>(T content) where T : class;
        JObject Generate<T>(T content,string clientInvokeId) where T : class;
    }
}

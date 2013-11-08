using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.CppTrader.DataMapping.WebService;
using iExchange.Common;
using Wintellect.Threading.AsyncProgModel;

namespace Trader.Server.CppTrader.DataMappingAbstract
{
    public interface IInitDataProvider
    {
        event GetInitDataCompleteHandle Completed;
        IEnumerator<int> AsyncGetInitData(Token token, AsyncEnumerator ae);
    }
}

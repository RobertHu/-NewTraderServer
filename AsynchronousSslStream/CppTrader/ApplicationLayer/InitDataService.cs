using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.CppTrader.DataMapping.WebService;
using System.Data;
using Trader.Common;
using iExchange.Common;
using Trader.Server.SessionNamespace;
using System.Diagnostics;
using Wintellect.Threading.AsyncProgModel;
using Trader.Server.CppTrader.DataMappingAbstract;

namespace Trader.Server.CppTrader.ApplicationLayer
{
    public class InitDataService
    {
        public void GetInitData(Session session)
        {
            Token token = SessionManager.Default.GetToken(session);
            Debug.Assert(token != null, "token can't be null");
            IInitDataProvider provider = new InitDataProvider();
            provider.Completed += GetInitDataCompletedCallback ;
            var ae = new AsyncEnumerator();
            ae.BeginExecute(provider.AsyncGetInitData(token, ae), ae.EndExecute);
        }

        private void GetInitDataCompletedCallback(IInitDataProvider sender, DataSet initData)
        {
            sender.Completed -= GetInitDataCompletedCallback;
        }
    }
}

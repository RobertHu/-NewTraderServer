using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using iExchange.Common;
using Wintellect.Threading.AsyncProgModel;
using Trader.Server.Bll;
using log4net;
using Trader.Server.CppTrader.DataMappingAbstract;

namespace Trader.Server.CppTrader.DataMapping.WebService
{
    public delegate void GetInitDataCompleteHandle(IInitDataProvider sender, DataSet initData);

    public class InitDataProvider : IInitDataProvider
    {
        public event GetInitDataCompleteHandle Completed;
        private readonly ILog _Logger = LogManager.GetLogger(typeof(InitDataProvider));
        public IEnumerator<int> AsyncGetInitData(Token token,AsyncEnumerator ae)
        {
            Application.Default.StateServer.BeginGetInitData(token, null, ae.End(), null);
            yield return 1;
            try
            {
                int commandSequence;
                DataSet initData = Application.Default.StateServer.EndGetInitData(ae.DequeueAsyncResult(), out commandSequence);
                var handler = Completed;
                if (handler != null)
                    handler(this,initData);
            }
            catch (Exception ex)
            {
                _Logger.Error("async getInitData", ex);
                yield break;
            }
        }
    
    }
}

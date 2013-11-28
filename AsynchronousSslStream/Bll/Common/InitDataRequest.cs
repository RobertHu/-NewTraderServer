using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.Serialization;
using iExchange.Common;
using System.Xml.Linq;
using Wintellect.Threading.AsyncProgModel;
using Mobile = iExchange3Promotion.Mobile;
using Trader.Server.Util;
using Trader.Server.Core.Request;

namespace Trader.Server.Bll.Common
{
    public class InitDataRequest
    {
        private SerializedInfo _Request;
        private Token _Token;
        public InitDataRequest(SerializedInfo request, Token token)
        {
            _Request = request;
            _Token = token;
        }

        public XElement Execute()
        {
            if (_Token != null && _Token.AppType == AppType.Mobile)
            {
                return ExecuteMobileRequest();
            }
            else if (_Token != null && _Token.AppType == AppType.CppTrader)
            {
                ExecuteCppTraderRequest();
                return null;
            }
            else if (_Token != null && _Token.AppType == AppType.TradingConsole)
            {
                ExecuteJavaTraderRequest();
                return null;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private XElement ExecuteMobileRequest()
        {
            System.Data.DataSet initData = Mobile.Manager.GetInitData(_Token);
            List<string> argList =ArgumentsParser.Parse(_Request.Content);
            Guid selectedAccountId = (argList != null && argList.Count > 0 ? new Guid(argList[0]) : Guid.Empty);
            InitDataService.Init(_Request.ClientInfo.Session, initData);
            var result = Mobile.Manager.Initialize(_Token, initData, selectedAccountId);
            if (System.Configuration.ConfigurationManager.AppSettings["MobileDebug"] == "true")
            {
                Mobile.Server.Transaction transaction = iExchange3Promotion.Mobile.Manager.ConvertModifyRequest(_Token, new Guid("69B80BC0-90FA-46E3-AF08-1B6E67FFF506"), "1596", null, null, null, null, null, null, false);
                XElement element = new XElement("Result");
                ICollection<XElement> errorCodes = MobileHelper.GetPlaceResultForMobile(transaction, _Token);
            }
            return result;
        }

        private void ExecuteCppTraderRequest()
        {
            var initDataService = new CppTrader.InitDataService(_Request,_Token);
            initDataService.AsyncGetInitData();
        }

        private void ExecuteJavaTraderRequest()
        {
            var ae = new AsyncEnumerator();
            ae.BeginExecute(InitDataService.GetInitData(_Request, ae), ae.EndExecute);
        }


    }
}

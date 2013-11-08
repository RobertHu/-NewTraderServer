using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.Serialization;
using iExchange.Common;
using System.Xml.Linq;
using Trader.Server.Util;
using Trader.Server.CppTrader.DataMappingAbstract;
using Trader.Server.TypeExtension;
using Trader.Server.SessionNamespace;
using Trader.Common;
using Mobile = iExchange3Promotion.Mobile;
namespace Trader.Server.Bll.Common
{
    public class LoginRequest
    {
        private SerializedObject _Request;
        private Token _Token;
        public LoginRequest(SerializedObject request,Token token)
        {
            _Request = request;
            _Token = token;
        }
        public void Execute()
        {
            List<string> argList = XmlRequestCommandHelper.GetArguments(_Request.Content);
            int appType = argList[3].ToInt();
            LoginParameter loginParameter = new LoginParameter()
            {
                LoginId = argList[0],
                Password=argList[1],
                Version=argList[2],
                AppType=(AppType)argList[3].ToInt(),
                AsyncEnumerator=new Wintellect.Threading.AsyncProgModel.AsyncEnumerator()
            };
            LoginManager loginManager = new LoginManager();
            loginManager.StateLoadingCompleted += OnLoginStateLoadingCompleteCallback;
            loginManager.Login(loginParameter);
        }

        private void OnLoginStateLoadingCompleteCallback(LoginManager sender,AppType appType)
        {
            sender.StateLoadingCompleted -= OnLoginStateLoadingCompleteCallback;
            if (AppType.Mobile == appType)
            {
                Token token = SessionManager.Default.GetToken(_Request.ClientInfo.Session);
                _Request.Content = new PacketContent( Mobile.Manager.Login(token));
                SendCenter.Default.Send(_Request);
            }
            else if (appType == AppType.CppTrader)
            {

            }

            //test:
            if (System.Configuration.ConfigurationManager.AppSettings["MobileDebug"] == "true")
            {
                //Dictionary<Guid, Guid> quotePolicyIds = Mobile.Manager.UpdateInstrumentSetting(token, new string[] { });
            }
        }
    }
}

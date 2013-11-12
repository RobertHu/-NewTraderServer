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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Trader.Server.CppTrader.Communication.Model;
using Trader.Server.Core;
using Trader.Server.Core.Request;
using Trader.Server.Core.Response;
namespace Trader.Server.Bll.Common
{
    public class LoginRequest
    {
        private SerializedInfo _Request;
        private Token _Token;
        public LoginRequest(SerializedInfo request,Token token)
        {
            _Request = request;
            _Token = token;
        }
        public void Execute()
        {
            List<string> argList =ArgumentsParser.Parse(_Request.Content);
            int appType = argList[3].ToInt();
            LoginParameter loginParameter = new LoginParameter()
            {
                LoginId = argList[0],
                Password=argList[1],
                Version=argList[2],
                AppType=(AppType)argList[3].ToInt(),
                Request=_Request
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
                _Request.UpdateContent(new PacketContent( Mobile.Manager.Login(token)));
                SendCenter.Default.Send(_Request);
            }
            else if (appType == AppType.CppTrader)
            {
                LoginResult result = new LoginResult { SessionId = _Request.ClientInfo.Session.ToString(), ServerDateTime=DateTime.Now.ToStandrandDateTimeStr() };
                _Request.UpdateContent(JsonResponse.NewResult(result));
                SendCenter.Default.Send(_Request);
            }
            if (System.Configuration.ConfigurationManager.AppSettings["MobileDebug"] == "true")
            {
            }
        }
    }
}

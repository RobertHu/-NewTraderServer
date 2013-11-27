using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iExchange.Common;
using System.Xml;
using log4net;
using System.Data;
using System.IO;
using Trader.Server.SessionNamespace;
using System.Collections;
using System.Diagnostics;
using System.Xml.Linq;
using Trader.Server.TypeExtension;
using Trader.Server.Util;
using Wintellect.Threading;
using Wintellect.Threading.AsyncProgModel;
using Trader.Common;
using Trader.Server.Serialization;
using Trader.Server.CppTrader.DataMappingAbstract;
using Trader.Server.CppTrader.DataMapping.WebService;
using JavaLoginService = Trader.Server.Bll.JavaTrader.LoginService;
using Trader.Server.Bll.Common;
using Trader.Server.Core.Response;
namespace Trader.Server.Bll
{

    public class LoginManager
    {
        private readonly ILog _Logger = LogManager.GetLogger(typeof(LoginManager));
        private ILoginProvider _LoginProvider;
        public event Action<LoginManager,AppType> StateLoadingCompleted;
        public LoginManager()
        {
            _LoginProvider = new LoginProvider();
            _LoginProvider.Completed += AsyncLoginCompletedCallback;
        }

        public void Login(LoginParameter parameter)
        {
            AsyncEnumerator ae = new AsyncEnumerator();
            ae.BeginExecute(_LoginProvider.AsyncLogin(parameter, ae), ae.EndExecute);
        }

        private void OnStateLoadingComplete(AppType appType)
        {
            var handler = StateLoadingCompleted;
            if (handler != null)
            {
                handler(this, appType);
            }
        }


        private void AsyncLoginCompletedCallback(ILoginProvider provider,LoginInfo loginInfo)
        {
            provider.Completed -= AsyncLoginCompletedCallback;
            bool success = false;
            switch (loginInfo.Status)
            {
                case LoginStatus.None:
                    break;
                case LoginStatus.ExceedMaxRetryCount:
                    var loginId = loginInfo.Parameter.LoginId;
                    var password = loginInfo.Parameter.Password;
                    var ip = loginInfo.Parameter.Request.ClientInfo.RemoteIp;
                    _Logger.WarnFormat("{0} login failed: exceed max login retry times", loginId);
                    AuditHelper.AddIllegalLogin(AppType.TradingConsole, loginId, password, ip);
                    Application.Default.TradingConsoleServer.SaveLoginFail(loginId, password, ip);
                    break;
                case LoginStatus.LoginIdIsEmpty:
                    _Logger.Warn("LoginId is empty");
                    break;
                case LoginStatus.ParticipantServiceLoginFailed:
                    break;
                case LoginStatus.UserIdIsEmpty:
                    _Logger.WarnFormat("{0} is not a valid user", loginInfo.Parameter.LoginId);
                    break;
                case LoginStatus.CheckPermissionFailed:
                    break;
                case LoginStatus.NotAuthrized:
                    _Logger.WarnFormat("{0} doesn't have the right to login trader", loginInfo.Parameter.LoginId);
                    break;
                case LoginStatus.StateServerLoginFailed:
                    break;
                case LoginStatus.StateServerNotLogined:
                    break;
                case LoginStatus.Success:
                    success = true;
                    break;
            }
            if (!success)
            {
                LoginRetryTimeHelper.IncreaseFailedCount(loginInfo.Parameter.LoginId, ParticipantType.Customer, SettingManager.Default.ConnectionString);
                OnError(loginInfo.Parameter.Request, loginInfo.Parameter.AppType);
            }
            else
            {
                LoginRetryTimeHelper.ClearFailedCount(loginInfo.UserID, ParticipantType.Customer, SettingManager.Default.ConnectionString);
                ProcessPostAsyncLoginSuccess(loginInfo);
            }
        }

        private void ProcessPostAsyncLoginSuccess(LoginInfo loginInfo)
        {
            Session session = loginInfo.Parameter.Request.ClientInfo.Session;
            if (SetLoginInfo(loginInfo))
            {
                SessionManager.Default.AddSession(loginInfo.UserID, session);
                string version = loginInfo.Parameter.Version;
                var language = string.IsNullOrEmpty(version) ? "ENG" : version.Substring(version.Length - 3);
                var token = SessionManager.Default.GetToken(session);
                if (token == null)
                {
                    var tokenType = loginInfo.Parameter.AppType;
                    token = new Token(loginInfo.UserID, UserType.Customer, tokenType);
                    SessionManager.Default.AddToken(session, token);
                }
                token.Language = language;
                Application.Default.SessionMonitor.Add(session);
                switch (loginInfo.Parameter.AppType)
                {
                    case AppType.TradingConsole:
                        JavaLoginService service = new JavaLoginService(loginInfo);
                        service.AsyncGetLoginData();
                        break;
                    case AppType.Mobile:
                        OnStateLoadingComplete(AppType.Mobile);
                        break;
                    case AppType.CppTrader:
                        OnStateLoadingComplete(AppType.CppTrader);
                        break;
                    default:
                        throw new ArgumentException(string.Format("{0} is not recogized", loginInfo.Parameter.AppType), "AppType");
                }
            }
            else
            {
                SessionManager.Default.RemoveToken(session);
            }
        }

        private void OnError(SerializedInfo request,AppType appType)
        {
            if (appType != AppType.CppTrader)
            {
                request.UpdateContent(XmlResultHelper.ErrorResult);
            }
            else
            {
                Debug.Assert(appType == AppType.CppTrader);
                request.UpdateContent(JsonResponse.NewErrorResult(request.ClientInfo.ClientInvokeId));
            }
            SendCenter.Default.Send(request);
        }


        private bool SetLoginInfo(LoginInfo loginInfo)
        {
            var ds = Application.Default.TradingConsoleServer.GetLoginParameters(loginInfo.UserID, string.Empty);
            var row = ds.Tables[0].Rows[0];
            bool isPathPassed = (Boolean)row["IsPathPassed"];
            loginInfo.DisallowLogin = (Boolean)row["DisallowLogin"];
            loginInfo.IsActivateAccount = (Boolean)row["IsActivateAccount"];
            loginInfo.IsDisableJava30 = (Boolean)row["IsDisableJava30"];
            if (!loginInfo.CompanyName.HasValue())
            {
                var companyName2 = (String)row["Path"];
                if (companyName2 == string.Empty)
                    companyName2 = "MHL";
                if (Directory.Exists(LoginHelper.GetOrginazationDir(companyName2)))
                {
                    isPathPassed = true;
                    loginInfo.CompanyName = companyName2;
                }
            }
            return isPathPassed && !loginInfo.DisallowLogin;
        }

    }
}
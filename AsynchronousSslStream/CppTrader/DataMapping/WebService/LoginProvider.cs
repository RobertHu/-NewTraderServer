using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.CppTrader.DataMappingAbstract;
using log4net;
using iExchange.Common;
using Trader.Server.TypeExtension;
using Trader.Server.Bll;
using Trader.Server.SessionNamespace;

namespace Trader.Server.CppTrader.DataMapping.WebService
{
    public class LoginProvider : ILoginProvider
    {
        private readonly ILog _Logger = LogManager.GetLogger(typeof(LoginProvider));
        public event LoginCompletedHandler Completed;

        public IEnumerator<int> AsyncLogin(LoginParameter parameter)
        {
            string connectionString = SettingManager.Default.ConnectionString;
            LoginInfo loginInfo = new LoginInfo() { Parameter=parameter};
            if (LoginRetryTimeHelper.IsFailedCountExceeded(parameter.LoginId, ParticipantType.Customer, connectionString))
            {
                loginInfo.Status = LoginStatus.ExceedMaxRetryCount;
                OnCompleted(loginInfo);
                yield break;
            }
            if (!parameter.LoginId.HasValue())
            {
                loginInfo.Status = LoginStatus.LoginIdIsEmpty;
                OnCompleted(loginInfo);
                yield break;
            }

            Application.Default.ParticipantService.BeginLogin(parameter.LoginId, parameter.Password,parameter.AsyncEnumerator.End(), null);
            yield return 1;
            try
            {
                loginInfo.UserID = Application.Default.ParticipantService.EndLogin(parameter.AsyncEnumerator.DequeueAsyncResult());
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
                loginInfo.Status = LoginStatus.ParticipantServiceLoginFailed;
                OnCompleted(loginInfo);
                yield break;
            }

            if (loginInfo.UserID == Guid.Empty)
            {
                loginInfo.Status = LoginStatus.UserIdIsEmpty;
                OnCompleted(loginInfo);
                yield break;
            }

            Guid programID = new Guid(SettingManager.Default.GetJavaTraderSettings("TradingConsole"));
            Guid permissionID = new Guid(SettingManager.Default.GetJavaTraderSettings("Run"));
            Application.Default.SecurityService.BeginCheckPermission(loginInfo.UserID, programID, permissionID, "", "", loginInfo.UserID, parameter.AsyncEnumerator.End(), null);
            yield return 1;
            bool isAuthrized = false;
            try
            {
                string message;
                isAuthrized = Application.Default.SecurityService.EndCheckPermission(parameter.AsyncEnumerator.DequeueAsyncResult(), out message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
                loginInfo.Status = LoginStatus.CheckPermissionFailed;
                OnCompleted(loginInfo);
                yield break;
            }

            if (!isAuthrized)
            {
                loginInfo.Status = LoginStatus.NotAuthrized;
                OnCompleted(loginInfo);
                yield break;
            }

            var token = new Token(Guid.Empty, UserType.Customer, parameter.AppType);
            token.UserID = loginInfo.UserID;
            token.SessionID = parameter.Request.ClientInfo.Session.ToString();
            SessionManager.Default.AddToken(parameter.Request.ClientInfo.Session, token);
            Application.Default.StateServer.BeginLogin(token, parameter.AsyncEnumerator.End(), null);
            yield return 1;
            bool isStateServerLogined = false;
            try
            {
                isStateServerLogined = Application.Default.StateServer.EndLogin(parameter.AsyncEnumerator.DequeueAsyncResult());
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
                loginInfo.Status = LoginStatus.StateServerLoginFailed;
                OnCompleted(loginInfo);
                yield break;
            }

            if (!isStateServerLogined)
            {
                loginInfo.Status = LoginStatus.StateServerNotLogined;
                OnCompleted(loginInfo);
                yield break;
            }
            loginInfo.Status = LoginStatus.Success;
            OnCompleted(loginInfo);

        }

        private void OnCompleted(LoginInfo info)
        {
            var handle = Completed;
            if (handle != null)
                handle(this, info);
        }
    }
}

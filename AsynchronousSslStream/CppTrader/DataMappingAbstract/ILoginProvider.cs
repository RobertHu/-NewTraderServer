using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wintellect.Threading.AsyncProgModel;
using Trader.Server.Serialization;
using iExchange.Common;

namespace Trader.Server.CppTrader.DataMappingAbstract
{

    public enum LoginStatus
    {
        None,
        ExceedMaxRetryCount,
        LoginIdIsEmpty,
        ParticipantServiceLoginFailed,
        UserIdIsEmpty,
        CheckPermissionFailed,
        NotAuthrized,
        StateServerLoginFailed,
        StateServerNotLogined,
        Success
    }

    public class LoginInfo
    {
        public Guid UserID { get; set; }
        public string CompanyName { get; set; }
        public bool DisallowLogin { get; set; }
        public bool IsActivateAccount { get; set; }
        public bool IsDisableJava30 { get; set; }
        public LoginStatus Status { get; set; }
        public LoginParameter Parameter { get; set; }
    }

    public class LoginParameter
    {
        public SerializedObject Request { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string Version { get; set; }
        public AppType AppType { get; set; }
        public AsyncEnumerator AsyncEnumerator { get; set; }
    }

    public delegate void LoginCompletedHandler(ILoginProvider sender,LoginInfo info);

    public interface ILoginProvider
    {
        event LoginCompletedHandler Completed;
        IEnumerator<int> AsyncLogin(LoginParameter parameter);
    }
}

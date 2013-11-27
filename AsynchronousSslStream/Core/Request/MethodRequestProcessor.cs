using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.Serialization;
using Trader.Server.SessionNamespace;
using Trader.Server.Bll;
using Trader.Server.Util;
using Trader.Common;
using iExchange.Common;

namespace Trader.Server.Core.Request
{
    public static class MethodRequestProcessor
    {
        private const string LoginMethodName = "Login";
        public static PacketContent Process(SerializedInfo request, string methodName)
        {
            Token token = SessionManager.Default.GetToken(request.ClientInfo.Session);
            PacketContent result;
            if (!Application.Default.SessionMonitor.Exist(request.ClientInfo.Session))
            {
                ExecuteRequestWhenSessionNotExist(methodName, request, token, out result);
            }
            else
            {
                result = RequestTable.Default.Execute(methodName, request, token);
            }
            return result;
        }

        private static void ExecuteRequestWhenSessionNotExist(string methodName, SerializedInfo request, Token token, out PacketContent result)
        {
            result = XmlResultHelper.ErrorResult;
            if (methodName == LoginMethodName)
            {
                result = RequestTable.Default.Execute(methodName, request, token);
            }
            if (request.ClientInfo.ClientId != Session.InvalidSession)
                request.ClientInfo.UpdateSession(request.ClientInfo.ClientId);
        }

    }
}

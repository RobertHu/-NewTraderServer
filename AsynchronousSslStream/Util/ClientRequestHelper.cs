using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Common;
using System.Xml;
using Trader.Server.Bll;
using Trader.Server.Service;
using Trader.Server.TypeExtension;
using Mobile = iExchange3Promotion.Mobile;
using System.Xml.Linq;
using Trader.Server.Serialization;
using Trader.Server.SessionNamespace;
using iExchange.Common;

namespace Trader.Server.Util
{
    public static class ClientRequestHelper
    {
        public static void Process(SerializedObject request)
        {
            PacketContent content = null;
            try
            {
                if (request.Content.ContentType!=ContentType.KeepAlivePacket)
                {
                    content = ProcessForNormal(request);
                }
                else
                {
                    ProcessForKeepAlive(request);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                content =XmlResultHelper.NewErrorResult(ex.ToString()).ToPacketContent();
            }
            finally
            {
                Application.Default.SessionMonitor.Update(request.ClientInfo.Session);
                if (content != null)
                {
                    request.Content = content;
                    SendCenter.Default.Send(request);
                }
            }
        }

        private static void ProcessForKeepAlive(SerializedObject request)
        {
            request.Content.KeepAlive.IsSuccess = Application.Default.SessionMonitor.Exist(request.ClientInfo.Session);
            SendCenter.Default.Send(request);
        }

        private static PacketContent  ProcessForNormal(SerializedObject request)
        {
            var  result = XmlResultHelper.ErrorResult;
            if (request.Content.ContentType == ContentType.Xml)
            {
                XElement content = request.Content.XmlContent;
                if (content.Name != RequestConstants.RootNodeName) return result;
                var methodNode = content.Descendants().Single(m => m.Name == RequestConstants.MethodNodeName);
                if (methodNode.Name == RequestConstants.MethodNodeName)
                {
                    result =ProcessMethodReqeust(request, methodNode.Value);
                }
                return result;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private static PacketContent ProcessMethodReqeust(SerializedObject request,string methodName)
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

        private static void ExecuteRequestWhenSessionNotExist(string methodName, SerializedObject request, Token token,out PacketContent result)
        {
            result = XmlResultHelper.ErrorResult;
            if (methodName == "Login")
            {
                result = RequestTable.Default.Execute(methodName, request, token);
            }
            WhenSessionNotExistRecoveSessionToCurrentSession(request);
        }

        private static void WhenSessionNotExistRecoveSessionToCurrentSession(SerializedObject request)
        {
            if (request.ClientInfo.ClientId != Session.InvalidValue)
            {
                request.ClientInfo.Session = request.ClientInfo.ClientId;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Xml.Linq;
using Trader.Common;
using iExchange.Common;
using Trader.Server.SessionNamespace;
using Trader.Server.Util;
using Trader.Server.Serialization;
using Trader.Server.TypeExtension;

namespace Trader.Server.Bll.Common
{
    public static class LoginOutService
    {
        private readonly static ILog _Logger = LogManager.GetLogger(typeof(LoginOutService));
        public static PacketContent Logout(Session session)
        {
            try
            {
                Token token = SessionManager.Default.GetToken(session);
                if (token != null)
                {
                    TraderState state = SessionManager.Default.GetTradingConsoleState(session);
                    Application.Default.TradingConsoleServer.SaveLogoutLog(token, "", state != null && state.IsEmployee);
                    Application.Default.StateServer.Logout(token);
                    Application.Default.SessionMonitor.Remove(session);
                }

            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
            }
            return XmlResultHelper.NewResult("").ToPacketContent();
        }

    }
}

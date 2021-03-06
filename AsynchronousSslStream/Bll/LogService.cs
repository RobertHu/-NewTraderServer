﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iExchange.Common;
using System.Xml;
using Trader.Server.Util;
using Trader.Server.TypeExtension;
using System.Xml.Linq;
using Trader.Server.SessionNamespace;
using Trader.Common;
namespace Trader.Server.Bll
{
    public class LogService
    {
        public static XElement SaveLog(Session  session, string logCode, DateTime timestamp, string action, Guid transactionId)
        {
            try
            {
                Token token = SessionManager.Default.GetToken(session);
                TraderState state = SessionManager.Default.GetTradingConsoleState(session);
                bool isEmployee = state == null ? false : state.IsEmployee;
                Application.Default.TradingConsoleServer.SaveLog(token, isEmployee,"", logCode, timestamp, action, transactionId);
                return XmlResultHelper.NewResult("");
            }
            catch (System.Exception exception)
            {
                AppDebug.LogEvent("TradingConsole.SaveLog:", exception.ToString(), System.Diagnostics.EventLogEntryType.Error);
                return XmlResultHelper.NewErrorResult();
            }
        }
    }
}

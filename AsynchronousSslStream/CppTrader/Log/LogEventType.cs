using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.Log
{
    public enum LogEventType
    {
        LoginFail,
        Logon,
        Logout,
        Statement,
        Ledger,
        Quote,
        CancelQuote,
        ChangePassword,
        Activate
    }
}

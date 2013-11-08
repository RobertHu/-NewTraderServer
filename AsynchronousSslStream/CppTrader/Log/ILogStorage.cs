using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.Log
{
    public interface ILogStorage
    {
        bool Save(LogInfo info);
    }
}

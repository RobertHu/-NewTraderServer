using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Common;

namespace Trader.Server.Core
{
    public interface IDisconnectedClientProccessor
    {
        void Add(Session session);
        bool Start();
        bool Stop();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.ValueObjects;

namespace Trader.Server.Core
{
    public interface ICommandTransfer
    {
        void Send(CommandForClient command);
        bool Start();
        bool Stop();
    }
}

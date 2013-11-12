using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.Serialization;

namespace Trader.Server.Core.Request
{
    public interface IRequestProcessor
    {
        PacketContent Process(SerializedInfo request);
    }
}

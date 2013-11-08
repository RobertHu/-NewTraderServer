using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.CppTrader.CommunicationAbstract;
using Trader.Server.CppTrader.Communication;

namespace Trader.Server.Core
{
    public static class JsonServiceFactory
    {
        private static IResponseProvider _ResponseProvider;

        public static IResponseProvider CreateResponseProvider()
        {
            if (_ResponseProvider == null)
                _ResponseProvider = new ResponseProvider();
            return _ResponseProvider;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.Util;
using Trader.Server.TypeExtension;
using System.Xml;
using log4net;
using Trader.Common;
using System.Xml.Linq;
using Trader.Server.Serialization;
namespace Trader.Server.Bll
{
    public static class RecoverService
    {
        private static ILog _Logger = LogManager.GetLogger(typeof(RecoverService));
        public static PacketContent Recover(Session originSession,Session currentSession)
        {
            PacketContent result = XmlResultHelper.ErrorResult;
            try
            {
                if (Application.Default.AgentController.RecoverConnection(originSession, currentSession))
                {
                    result = XmlResultHelper.NewResult(StringConstants.OkResult).ToPacketContent();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _Logger.Error(ex);
            }
            return result;
        }
    }
}

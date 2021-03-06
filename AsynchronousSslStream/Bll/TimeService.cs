﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Time;
using iExchange.Common;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Trader.Server.Util;
using Trader.Server.TypeExtension;

namespace Trader.Server.Bll
{
    public static class TimeService
    {
        public static XElement GetTimeInfo()
        {
            try
            {
               TimeInfo info=((ITimeSyncService)Framework.Time.SystemTime.Default).GetTimeInfo();
               string xml = XmlSerializeHelper.ToXml(info.GetType(), info);
               return XmlResultHelper.NewResult(xml);
              
            }
            catch (System.Exception exception)
            {
                AppDebug.LogEvent("TradingConsole.GetTimeInfo:", exception.ToString(), System.Diagnostics.EventLogEntryType.Error);
                return XmlResultHelper.NewErrorResult();
            }
        }
    }
}

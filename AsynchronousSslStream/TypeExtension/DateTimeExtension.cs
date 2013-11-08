using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.TypeExtension
{
    public static class DateTimeExtension
    {
        public static string ToStandrandDateTimeStr(this DateTime source)
        {
            return source.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
        }
    }
}

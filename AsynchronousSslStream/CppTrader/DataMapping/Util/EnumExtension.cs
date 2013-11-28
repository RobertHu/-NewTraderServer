using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping.Util
{
    public static class EnumExtension
    {
        public static T ToEnum<T>(this object value) where T: struct
        {
            int underlineVaule;
            if(!int.TryParse(value.ToString(),out underlineVaule)
                || !Enum.IsDefined(typeof(T),value))
            {
                throw new InvalidCastException("value can't be cast to enum");
            }
            return (T)Enum.ToObject(typeof(T), underlineVaule);
        }
    }
}

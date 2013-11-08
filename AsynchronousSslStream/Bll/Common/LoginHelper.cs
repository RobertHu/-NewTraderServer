using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Trader.Server.Bll.Common
{
    public static class LoginHelper
    {
        public static string GetOrginazationDir(string companyCode)
        {
            return Path.Combine(SettingManager.Default.PhysicPath, companyCode);
        }
    }
}

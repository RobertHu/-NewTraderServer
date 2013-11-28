using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.CppTrader.DataMapping.Response
{
    public class AccountForSelection
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public bool IsSelected { get; set; }
        public int Sequence { get; set; }
    }
}

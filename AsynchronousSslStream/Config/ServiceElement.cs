using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Trader.Server.Config
{
    public class ServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("key",IsRequired=true)]
        public string Key
        {
            get { return this["key"] as string; }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return this["value"] as string; }
        }
    }

    public class ServiceCollection : ConfigurationElementCollection
    {

        public ServiceElement this[int index]
        {
            get
            {
                return BaseGet(index) as ServiceElement;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceElement)element).Key;
        }
    }

}

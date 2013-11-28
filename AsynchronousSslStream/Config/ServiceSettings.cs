using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
namespace Trader.Server.Config
{

    public class ServiceConfigurationSetting
    {
        public static readonly ServiceConfigurationSetting Defalut = new ServiceConfigurationSetting();
        private ServiceConfigurationSetting() 
        {
            var configSection = ServiceSettings.GetConfig();
            this.V3Services = FillSettins(configSection.V3Services);
            this.JavaTraderSettings = FillSettins(configSection.JavaTraderSettings);
            this.CppTraderSettings = FillSettins(configSection.CppTraderSettings);
        }

        public NameValueCollection V3Services { get; private set; }

        public NameValueCollection JavaTraderSettings { get; private set; }

        public NameValueCollection CppTraderSettings { get; private set; }


        private NameValueCollection FillSettins(ServiceCollection col)
        {
            var target = new NameValueCollection();
            for (int i = 0; i < col.Count; i++)
            {
                var item = col[i];
                target.Add(item.Key, item.Value);
            }
            return target;
        }
    }



    public class ServiceSettings : ConfigurationSection
    {
        private const string _SettingName= "ServiceSettings";
        private const string _V3ServiceSection = "V3Services";
        private const string _JavaTraderSection = "JavaTraderSettings";
        private const string _CppTraderSection = "CppTraderSettings";
        private static ServiceSettings _Settings = ConfigurationManager.GetSection(_SettingName) as ServiceSettings
                                            ?? new ServiceSettings();
        public static ServiceSettings GetConfig() { return _Settings; }

        [ConfigurationProperty(_V3ServiceSection)]
        public ServiceCollection V3Services
        {
            get
            {
                return (ServiceCollection)this[_V3ServiceSection] ?? new ServiceCollection();
            }
        }


        [ConfigurationProperty(_JavaTraderSection)]
        public ServiceCollection JavaTraderSettings
        {
            get
            {
                return (ServiceCollection)this[_JavaTraderSection] ?? new ServiceCollection();
            }
        }

        [ConfigurationProperty(_CppTraderSection)]
        public ServiceCollection CppTraderSettings
        {
            get
            {
                return (ServiceCollection)this[_CppTraderSection] ?? new ServiceCollection();
            }
        }

    }
}

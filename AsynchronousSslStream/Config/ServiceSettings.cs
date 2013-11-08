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
        private object _Lock = new object();
        private ServiceConfigurationSetting() { }

        private NameValueCollection _V3Services;

        public NameValueCollection V3Services
        {
            get
            {
                if (this._V3Services == null)
                {
                    lock (_Lock)
                    {
                        if (this._V3Services == null)
                        {
                            this._V3Services = FillSettins(ServiceSettings.GetConfig().V3Services);
                        }
                    }
                }
                return this._V3Services;
            }
        }


        private NameValueCollection _JavaTraderSettings;
        public NameValueCollection JavaTraderSettings
        {
            get
            {
                if (_JavaTraderSettings == null)
                {
                    lock (_Lock)
                    {
                        if (_JavaTraderSettings == null)
                        {
                            _JavaTraderSettings = FillSettins(ServiceSettings.GetConfig().JavaTraderSettings);
                        }
                    }
                }
                return _JavaTraderSettings;
            }
        }




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
        private const string _settingName= "ServiceSettings";
        private static ServiceSettings _settings = ConfigurationManager.GetSection(_settingName) as ServiceSettings
                                            ?? new ServiceSettings();
        public static ServiceSettings GetConfig() { return _settings; }

        [ConfigurationProperty("V3Services")]
        public ServiceCollection V3Services
        {
            get
            {
                return (ServiceCollection)this["V3Services"] ?? new ServiceCollection();
            }
        }


        [ConfigurationProperty("JavaTraderSettings")]
        public ServiceCollection JavaTraderSettings
        {
            get
            {
                return (ServiceCollection)this["JavaTraderSettings"] ?? new ServiceCollection();
            }
        }


        



    }
}

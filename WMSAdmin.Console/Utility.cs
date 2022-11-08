using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Console
{
    internal class Utility
    {
        private IServiceProvider _serviceProvider;
        private ILogger _logger;
        private Entity.Entities.Config.ConfigSetting _configSetting;
        private BusinessService.Configuration _bsConfig;
        private Language.Configuration _langConfig;
        private Dictionary<Type, BusinessService.BaseService> _businessServices;
        internal Utility(IServiceProvider serviceProvider, ILogger logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _businessServices = new Dictionary<Type, BusinessService.BaseService>();
            _configSetting = GetConfigSetting();
            _bsConfig = new BusinessService.Configuration
            {
                Setting = _configSetting,
                ServiceProvider = serviceProvider,
                Logger = _logger
            };

            _langConfig = new Language.Configuration
            {
                Logger = _logger,
                ServiceProvider = serviceProvider,
                Setting = _configSetting,
            };
        }

        public T GetBusinessService<T>() where T : BusinessService.BaseService
        {
            var type = typeof(T);
            _businessServices.TryGetValue(type, out var businessService);
            if (businessService != null) return (T)businessService;

            businessService = (T)Activator.CreateInstance(typeof(T), _bsConfig);
            _businessServices.Add(type, businessService);
            return (T)businessService;
        }

        public Entity.Entities.Config.ConfigSetting ConfigSetting => _configSetting;
        public Language.Configuration LanguageConfiguration => _langConfig;

        public Entity.Entities.Config.ConfigSetting GetConfigSetting()
        {
            var bsConfig = new BusinessService.Configuration
            {
                Setting = new Entity.Entities.Config.ConfigSetting
                {
                    Application = new Entity.Entities.Config.Application
                    {
                        SessionId = $"{DateTime.Now.Ticks}",
                        CacheExpiryInMinutes = Entity.Constants.Default.CacheExpiryInMinutes,
                    },
                    Pagination = new Entity.Entities.Config.Pagination
                    {
                        MaxRecordsPerPage = Entity.Constants.Default.Pagination_Max_RecordsPerpage,
                        RecordsPerPage = Entity.Constants.Default.Pagination_Max_RecordsPerpage,
                        MaxRecordsAllowedPerPage = Entity.Constants.Default.Pagination_Max_AllowedRecordsPerpage,
                    }
                },

                ServiceProvider = _serviceProvider,
                Logger = _logger
            };
            var configService = new BusinessService.ConfigService(bsConfig);
            var setting = configService.GetConfigSetting();
            return setting;
        }
    }
}

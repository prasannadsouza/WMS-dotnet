using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WMSAdmin.WebApp
{
    public class Utility
    {
        private IServiceProvider _serviceProvider; 
        private HttpContext _httpContext;
        private ILogger _logger;
        private Entity.Entities.Config.ConfigSetting _configSetting;
        private BusinessService.Configuration _bsConfig;
        private Dictionary<Type, BusinessService.BaseService> _businessServices;
        public Utility(HttpContext httpContext, IServiceProvider serviceProvider, ILogger logger)
        {
            _httpContext = httpContext;
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
        }

        private T GetBusinessService<T>() where T : BusinessService.BaseService
        {
            var type = typeof(T);
            _businessServices.TryGetValue(type, out var businessService);
            if (businessService != null) return (T) businessService;

            businessService = (T)Activator.CreateInstance(typeof(T), _bsConfig);
            _businessServices.Add(type, businessService);
            return (T)businessService;
        }

        public Entity.Entities.Config.ConfigSetting ConfigSetting => _configSetting;

        public Entity.Entities.Config.ConfigSetting GetConfigSetting()
        {
            var bsConfig = new BusinessService.Configuration
            {
                Setting = new Entity.Entities.Config.ConfigSetting
                {
                    Application = new Entity.Entities.Config.Application
                    {
                        SessionId = $"{_httpContext.TraceIdentifier}:{_httpContext.Session.Id}",
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
            var setting  = configService.GetConfigSetting();
            return setting;
        }
    }
}

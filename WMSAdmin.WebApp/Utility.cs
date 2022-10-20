using Microsoft.Extensions.Caching.Memory;

namespace WMSAdmin.WebApp
{
    public class Utility
    {
        private IServiceProvider _serviceProvider; 
        private HttpContext _httpContext;
        private IMemoryCache _memoryCache;
        protected ILogger _logger;
        public Utility(HttpContext httpContext, IServiceProvider serviceProvider, ILogger logger)
        {
            _httpContext = httpContext;
            _serviceProvider = serviceProvider;
            _memoryCache = serviceProvider.GetRequiredService<IMemoryCache>(); 
            _logger = logger;
        }

        public Entity.Entities.Config.ConfigSetting GetConfigSetting()
        {
            
            var bsConfig = new BusinessService.Configuration
            {
                Setting = new Entity.Entities.Config.ConfigSetting
                {
                    System = new Entity.Entities.Config.Application
                    {
                        SessionId = $"{_httpContext.TraceIdentifier}:{_httpContext.Session.Id}",
                    }
                },
                ServiceProvider = _serviceProvider,
                Logger = _logger
            };
            var appService = new BusinessService.ConfigService(bsConfig);
            var setting  = appService.GetConfigSetting();
            return setting;
        }
    }
}

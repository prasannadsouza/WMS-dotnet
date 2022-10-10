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
            _memoryCache = serviceProvider.GetRequiredService<IMemoryCache>(); ;
            _logger = logger;
        }

        public Entity.Entities.ConfigSetting GetConfigSetting()
        {
            var key = Entity.Constants.Cache.CONFIGSETTING;
            var isCached = _memoryCache.TryGetValue(key, out Entity.Entities.ConfigSetting cacheValue);
            if (isCached) return cacheValue;

            
                var appService = new BusinessService.Application(new BusinessService.BusinessServiceConfiguration { ServiceProvider = _serviceProvider } );
                cacheValue = appService.GetConfigSetting();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(60));

            _memoryCache.Set(Entity.Constants.Cache.CONFIGSETTING, cacheValue, cacheEntryOptions);
            return cacheValue;
            
        }
    }
}

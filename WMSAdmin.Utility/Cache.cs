using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WMSAdmin.Utility
{
    public class Cache
    {
        private Configuration Configuration { get; set; }
        private IMemoryCache MemoryCache { get; set; }
        protected ILogger Logger { get; private set; }

        public Cache(Configuration configuration)
        {
            Configuration = configuration;
            Logger = configuration.Logger;
            MemoryCache = configuration.ServiceProvider.GetRequiredService<IMemoryCache>();
        }

        private string _className;
        private string ClassName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_className) == false) return _className;
                _className = this.GetType().FullName;
                return _className;
            }
        }

        public T GetFromCache<T>(string key, out bool isCached)
        {
            isCached = MemoryCache.TryGetValue(key, out T cacheValue);
            if (isCached) return cacheValue;
            RemoveCacheKey(key);
            return default(T);
        }

        public void SaveToCache<T>(string key, T value, bool noexpiry = false, int? cacheExpiryInMinutes = null)
        {
            if (cacheExpiryInMinutes == null) cacheExpiryInMinutes = Configuration.Setting.Application.CacheExpiryInMinutes;
            SaveCacheKey(key);
            if (noexpiry)
            {
                MemoryCache.Set(key, value);
                return;
            }
            var cacheEntryOptions = new MemoryCacheEntryOptions()
               .SetSlidingExpiration(TimeSpan.FromMinutes(cacheExpiryInMinutes.Value));
            MemoryCache.Set(key, value, cacheEntryOptions);
        }

        public void RemoveFromCache(string key)
        {
            var isCached = MemoryCache.TryGetValue(key, out _);
            if (isCached == false) return;
            MemoryCache.Remove(key);
        }

        private void SaveCacheKey(string cacheKey)
        {
            var key = Entity.Constants.Cache.APPLICATION_CACHEKEYS;
            var isCached = MemoryCache.TryGetValue(key, out List<string> cacheValue);
            if (isCached == false) cacheValue = new List<string>();
            if (cacheValue.Contains(cacheKey) == false) cacheValue.Add(cacheKey);
            MemoryCache.Set(key, cacheValue);
        }

        public void RemoveCacheKey(string cacheKey)
        {
            var key = Entity.Constants.Cache.APPLICATION_CACHEKEYS;
            var isCached = MemoryCache.TryGetValue(key, out List<string> cacheValue);
            if (isCached == false) return;
            if (cacheValue.Contains(cacheKey) == false) return;
            cacheValue.Remove(cacheKey);
            if (cacheValue.Any() == false)
            {
                MemoryCache.Remove(key);
                return;
            }
            MemoryCache.Set(key, cacheValue);
        }

        public List<string> GetCacheKeys()
        {
            var key = Entity.Constants.Cache.APPLICATION_CACHEKEYS;
            var _ = MemoryCache.TryGetValue(key, out List<string> cacheValue);
            return cacheValue;
        }
    }
}

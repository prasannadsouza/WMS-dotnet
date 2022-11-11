using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace WMSAdmin.Utility
{
    public class Cache
    {
        private Configuration Configuration { get; set; }
        private IMemoryCache MemoryCache { get; set; }

        public Cache(Configuration configuration)
        {
            Configuration = configuration;
            MemoryCache = configuration.ServiceProvider.GetRequiredService<IMemoryCache>();
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
            var isCached = MemoryCache.TryGetValue(key, out Dictionary<string, DateTime> cacheValue);
            if (isCached == false) cacheValue = new Dictionary<string, DateTime>();
            if (cacheValue.ContainsKey(cacheKey) == false) cacheValue.Add(cacheKey, DateTime.Now);
            MemoryCache.Set(key, cacheValue);
        }

        public void RemoveCacheKey(string cacheKey)
        {
            var key = Entity.Constants.Cache.APPLICATION_CACHEKEYS;
            var isCached = MemoryCache.TryGetValue(key, out Dictionary<string, DateTime> cacheValue);
            if (isCached == false) return;
            if (cacheValue.ContainsKey(cacheKey) == false) return;
            cacheValue.Remove(cacheKey);
            if (cacheValue.Any() == false)
            {
                MemoryCache.Remove(key);
                return;
            }
            MemoryCache.Set(key, cacheValue);
        }

        public Dictionary<string, DateTime> GetCacheKeys()
        {
            var key = Entity.Constants.Cache.APPLICATION_CACHEKEYS;
            var _ = MemoryCache.TryGetValue(key, out Dictionary<string, DateTime> cacheValue);
            return cacheValue;
        }

        public DateTime? GetCachedTime(string cacheKey)
        {
            var key = Entity.Constants.Cache.APPLICATION_CACHEKEYS;
            var isCached = MemoryCache.TryGetValue(key, out Dictionary<string, DateTime> cacheValue);
            
            if (isCached == false) return null;
            if (cacheValue.ContainsKey(cacheKey) == false) return null;
            return cacheValue[cacheKey];
        }
    }
}

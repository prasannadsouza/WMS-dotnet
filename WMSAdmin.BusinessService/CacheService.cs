using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.BusinessService
{
    public class CacheService : BaseService
    {
        private RepoService _reposervice;
        public CacheService(Utility.Configuration configuration) : base(configuration)
        {
            _reposervice = new RepoService(configuration);
        }

        private DateTime GetTimeStamp(string cacheKey)
        {
            var timestampList = GetCacheTimestampList();
            var configTimeStamp = timestampList.FirstOrDefault(e => string.Compare(e.Code, cacheKey, true) == 0);

            if (configTimeStamp != null ) return configTimeStamp.TimeStamp.Value;

            configTimeStamp = _reposervice.Get(new Entity.Filter.ConfigTimeStamp { Code = cacheKey, }).Data.FirstOrDefault();

            if (configTimeStamp == null)
            {
                configTimeStamp = new Entity.Entities.ConfigTimeStamp
                {
                    Code = cacheKey,
                    TimeStamp = DateTime.MinValue,
                };

                _reposervice.Save(configTimeStamp);
            }

            timestampList = GetCacheTimestampList();
            timestampList.Add(configTimeStamp);
            CacheUtility.SaveToCache(Entity.Constants.Cache.CONFIGTIMESTAMPLIST, timestampList);
            return configTimeStamp.TimeStamp.Value;
        }

        public bool IsCacheChanged(string cacheKey)
        {
            var lastCachedTime = CacheUtility.GetCachedTime(cacheKey);
            if (lastCachedTime == null) return true;
            var lastChangedTime = GetTimeStamp(cacheKey);
            return lastChangedTime > lastCachedTime;
        }

        public List<Entity.Entities.ConfigTimeStamp> GetCacheTimestampList()
        {
            var key = Entity.Constants.Cache.CONFIGTIMESTAMPLIST;
            var cachedValue = CacheUtility.GetFromCache<List<Entity.Entities.ConfigTimeStamp>>(key, out bool isCached);
            if (isCached) return cachedValue;

            cachedValue = new List<Entity.Entities.ConfigTimeStamp>();
            var filter = new Entity.Filter.ConfigTimeStamp { Pagination = Utility.AppHelper.GetDefaultPagination(Configuration.Setting, true), };

            do
            {
                var data = _reposervice.Get(filter).Data;
                foreach (var item in data)
                {
                    cachedValue.Add(item);
                }

                filter.Pagination.CurrentPage++;
            }
            while (filter.Pagination.CurrentPage <= filter.Pagination.TotalPages);

            CacheUtility.SaveToCache(key, cachedValue, true);
            return cachedValue;
        }

        public void UpdateTimestamp(string cacheKey)
        {
            var configTimestamp = _reposervice.Get(new Entity.Filter.ConfigTimeStamp { Code = cacheKey,}).Data.FirstOrDefault();

            if (configTimestamp == null) configTimestamp = new Entity.Entities.ConfigTimeStamp { Code = cacheKey, };
            configTimestamp.TimeStamp = DateTime.Now;
            _reposervice.Save(configTimestamp);

            var timestampList = GetCacheTimestampList();
            var cachedTimestamp = timestampList.FirstOrDefault(e=> string.Compare(e.Code, cacheKey, true) == 0);

            if (cachedTimestamp != null)
            {
                cachedTimestamp.TimeStamp = configTimestamp.TimeStamp;
            }
            else
            {
                timestampList.Add(configTimestamp);
            }

            CacheUtility.SaveToCache(Entity.Constants.Cache.CONFIGTIMESTAMPLIST, timestampList);
        }

        public void ClearCache(string cacheKey)
        {
            CacheUtility.RemoveFromCache(cacheKey);
            CacheUtility.RemoveCacheKey(cacheKey);
        }

        public void ClearCache()
        {
            CacheUtility.ClearCache();
            CacheUtility.RemoveFromCache(Entity.Constants.Cache.CONFIGTIMESTAMPLIST);
        }
    }
}

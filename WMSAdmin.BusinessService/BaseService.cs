using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WMSAdmin.BusinessService
{
    public class BaseService
    {
        protected Utility.Configuration Configuration { get; private set; }
        
        private Dictionary<System.Type, Repository.BaseRepository> _repositories;
       

        protected Utility.Cache CacheUtility { get; private set; }

        protected BaseService(Utility.Configuration configuration)
        {
            Configuration = configuration;
            
            CacheUtility = new Utility.Cache(configuration);
            _repositories = new Dictionary<Type, Repository.BaseRepository>();
        }
       
        protected T GetRepository<T>() where T : Repository.BaseRepository
        {
            var type = typeof(T);
            _repositories.TryGetValue(type, out var repository);
            if (repository != null) return (T)repository;

            repository = Activator.CreateInstance(typeof(T), Configuration) as T;
            _repositories.Add(type, repository);
            return (T)repository;
        }

        public DateTime GetTimeStamp(string code)
        {
            var repo = GetRepository<Repository.AppConfig>();
            var data = repo.Get(new Entity.Filter.AppConfig
            {
                Code = code,
                AppConfigGroup = new Entity.Filter.AppConfigGroup
                {
                    Code = Entity.Constants.Config.GROUP_CONFIGTIMESTAMP
                },
            }).Data.FirstOrDefault();

            if (data == null) return DateTime.MinValue;
            return System.Text.Json.JsonSerializer.Deserialize<DateTime>(data.Value);
        }

        public bool IsCacheChanged(string cacheKey)
        {
            var lastCachedTime = CacheUtility.GetCachedTime(cacheKey);
            if (lastCachedTime == null) return true;
            var lastChangedTime = GetTimeStamp(cacheKey);
            return lastChangedTime > lastCachedTime;
        }
    }
}
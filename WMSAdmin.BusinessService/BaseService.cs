using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WMSAdmin.BusinessService
{
    public class BaseService
    {
        protected Utility.Configuration Configuration { get; private set; }
        
        private Dictionary<System.Type, Repository.BaseRepository> _repositories;
        protected IMemoryCache MemoryCache { get; private set; }
        protected ILogger Logger { get; private set; }

        protected Utility.Cache CacheUtility { get; private set; }

        protected BaseService(Utility.Configuration configuration)
        {
            Configuration = configuration;
            Logger = configuration.Logger;
            CacheUtility = new Utility.Cache(configuration);
            MemoryCache = configuration.ServiceProvider.GetRequiredService<IMemoryCache>();
            _repositories = new Dictionary<Type, Repository.BaseRepository>();
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

        protected T GetRepository<T>() where T : Repository.BaseRepository
        {
            var type = typeof(T);
            _repositories.TryGetValue(type, out var repository);
            if (repository != null) return (T)repository;

            repository = Activator.CreateInstance(typeof(T), Configuration) as T;
            _repositories.Add(type, repository);
            return (T)repository;
        }

        

        public DateTime GetConfigTimeStamp()
        {
            return GetTimeStamp(Entity.Constants.Config.CONFIGTIMESTAMP_CONFIGTIMESTAMP);
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
            return DateTime.Parse(data.Value, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
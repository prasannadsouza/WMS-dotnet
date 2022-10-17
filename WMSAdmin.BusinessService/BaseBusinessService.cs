using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WMSAdmin.BusinessService
{
    public class BaseBusinessService
    {
        protected BusinessServiceConfiguration Configuration { get; private set; }
        private Repository.RepoConfiguration _repoConfiguration;
        private Dictionary<System.Type, Repository.BaseRepository> _repositories;
        protected ILogger Logger { get; private set; }

        protected BaseBusinessService(BusinessServiceConfiguration configuration)
        {
            Configuration = configuration;
            _repositories = new Dictionary<Type, Repository.BaseRepository>();
            _repoConfiguration = new Repository.RepoConfiguration
            {
                ServiceProvider = Configuration.ServiceProvider,
                Setting = Configuration.Setting,
                Logger = Configuration.Logger,
            }; ;
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
            repository = (T)Activator.CreateInstance(typeof(T), _repoConfiguration);
            _repositories.Add(type, repository);
            return (T)repository;
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WMSAdmin.BusinessService
{
    public class BaseBusinessService
    {
        protected BaseBusinessService(BusinessServiceConfiguration configuration)
        {
            Configuration = configuration;
            _entitySortFieldMapper = GetEntitySortFieldMapper();
             Logger = Configuration.ServiceProvider.GetService<ILogger>();
        }

        internal BusinessServiceConfiguration Configuration { get; set; }
        private Dictionary<string, string> _entitySortFieldMapper;
        protected ILogger Logger { get; private set; }
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

        #region Repositories
        private Repository.RepoConfiguration GetRepoConfiguration()
        {
            return new Repository.RepoConfiguration
            {
                Setting = Configuration.Setting,
                ServiceProvider = Configuration.ServiceProvider,
            };
        }

        private Repository.AppConfig _repoAppConfig;
        protected Repository.AppConfig RepoAppConfig
        {
            get
            {
                if (_repoAppConfig != null) return _repoAppConfig;
                _repoAppConfig = new Repository.AppConfig(GetRepoConfiguration());
                return _repoAppConfig;
            }
        }

        private Repository.AppConfigGroup _repoAppConfigGroup;
        protected Repository.AppConfigGroup RepoAppConfigGroup
        {
            get
            {
                if (_repoAppConfigGroup != null) return _repoAppConfigGroup;
                _repoAppConfigGroup = new Repository.AppConfigGroup(GetRepoConfiguration());
                return _repoAppConfigGroup;
            }
        }
        #endregion

        private Dictionary<string, string> GetEntitySortFieldMapper()
        {
            var sortFieldMapper = new Dictionary<string, string>();
            return sortFieldMapper;
        }

    }
}
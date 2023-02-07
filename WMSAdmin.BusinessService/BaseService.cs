using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace WMSAdmin.BusinessService
{
    public class BaseService
    {
        protected Utility.Configuration Configuration { get; private set; }

        private Dictionary<System.Type, Repository.BaseRepository> _repositories;
        private Dictionary<System.Type, BaseService> _businessServices;
        private Dictionary<Type, Language.ResourceManager.BaseResourceManager> _resourceManagers;


        protected Utility.Cache CacheUtility { get; private set; }

        protected BaseService(Utility.Configuration configuration)
        {
            Configuration = configuration;

            CacheUtility = new Utility.Cache(configuration);
            _repositories = new Dictionary<Type, Repository.BaseRepository>();
            _businessServices = new Dictionary<Type, BaseService>();
            _resourceManagers = new Dictionary<Type, Language.ResourceManager.BaseResourceManager>();
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

        protected T GetBusinessService<T>() where T : BaseService
        {
            var type = typeof(T);
            _businessServices.TryGetValue(type, out var businessService);
            if (businessService != null) return (T)businessService;

            businessService = Activator.CreateInstance(typeof(T), Configuration) as T;
            _businessServices.Add(type, businessService);
            return (T)businessService;
        }

        public T GetResourceManager<T>() where T : Language.ResourceManager.BaseResourceManager
        {
            var type = typeof(T);
            _resourceManagers.TryGetValue(type, out var resourceManager);
            if (resourceManager != null) return (T)resourceManager;

            resourceManager = (T)Activator.CreateInstance(typeof(T), Configuration, Configuration.Culture)!;
            _resourceManagers.Add(type, resourceManager);
            return (T)resourceManager;
        }

        public Entity.Entities.Pagination GetDefaultPagination(bool forMaxRecordsPerPage = false)
        {
            return GetRepository<Repository.AppConfig>().GetDefaultPagination(forMaxRecordsPerPage);
        }
    }
}
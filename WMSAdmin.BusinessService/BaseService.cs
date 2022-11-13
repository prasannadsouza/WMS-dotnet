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

      
    }
}
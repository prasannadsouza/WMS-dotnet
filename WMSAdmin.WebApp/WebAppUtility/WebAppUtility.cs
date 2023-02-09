using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WMSAdmin.WebApp.WebAppUtility
{
    public class WebAppUtility
    {
        public const string APIRoute = "api/[controller]/[action]";

        private HttpContext _httpContext;
        public Utility.Configuration Configuration { get; private set; }
        private Dictionary<Type, BusinessService.BaseService> _businessServices;
        private Dictionary<Type, Language.ResourceManager.BaseResourceManager> _resourceManagers;
        public WebAppUtility(HttpContext httpContext, IServiceProvider serviceProvider)
        {
            _httpContext = httpContext;
            _businessServices = new Dictionary<Type, BusinessService.BaseService>();
            _resourceManagers = new Dictionary<Type, Language.ResourceManager.BaseResourceManager>();

            Configuration = new Utility.Configuration
            {
                Setting = GetConfigSetting(serviceProvider),
                ServiceProvider = serviceProvider,
                Culture = System.Globalization.CultureInfo.CurrentCulture,
            };
        }

        public T GetBusinessService<T>() where T : BusinessService.BaseService
        {
            var type = typeof(T);
            _businessServices.TryGetValue(type, out var businessService);
            if (businessService != null) return (T)businessService;

            businessService = (T)Activator.CreateInstance(typeof(T), Configuration)!;
            _businessServices.Add(type, businessService);
            return (T)businessService;
        }

        public Entity.Entities.Config.ConfigSetting ConfigSetting => Configuration.Setting;

        public Entity.Entities.Config.ConfigSetting GetConfigSetting(IServiceProvider serviceProvider)
        {
            var configuration = new Utility.Configuration
            {
                Setting = Utility.AppHelper.GetDefaultConfigSetting(),
                ServiceProvider = serviceProvider,
                Culture = System.Globalization.CultureInfo.CurrentCulture,
            }; 
            
            var configService = new BusinessService.ConfigService(configuration);
            var setting = configService.GetConfigSetting();
            setting.Application.SessionId = _httpContext.Session.Id;
            return setting;
        }

        public T GetResourceManager<T>() where T: Language.ResourceManager.BaseResourceManager
        {
            var type = typeof(T);
            _resourceManagers.TryGetValue(type, out var resourceManager);
            if (resourceManager != null) return (T)resourceManager;

            resourceManager = (T)Activator.CreateInstance(typeof(T), Configuration, System.Globalization.CultureInfo.CurrentCulture)!;
            _resourceManagers.Add(type, resourceManager);
            return (T)resourceManager;
        }

        public Entity.Entities.Pagination GetDefaultPagination(bool forMaxRecordsPerPage = false)
        {
            return GetBusinessService<BusinessService.RepoService>().GetDefaultPagination(forMaxRecordsPerPage);
        }

    }
}

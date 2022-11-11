using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WMSAdmin.WebApp
{
    public class WebUtility
    {
        private HttpContext _httpContext;
        public Utility.Configuration Configuration { get; private set; }
        private Dictionary<Type, BusinessService.BaseService> _businessServices;
        public WebUtility(HttpContext httpContext, IServiceProvider serviceProvider)
        {
            _httpContext = httpContext;
            _businessServices = new Dictionary<Type, BusinessService.BaseService>();

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
            return setting;
        }
    }
}

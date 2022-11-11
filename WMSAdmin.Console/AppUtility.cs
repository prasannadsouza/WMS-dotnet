using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Console
{
    internal class AppUtility
    {
        private Utility.Configuration Configuration;
        private IServiceProvider _serviceProvider;
        private Dictionary<Type, BusinessService.BaseService> _businessServices;
        private ILogger _logger;
        internal AppUtility(IServiceProvider serviceProvider)
        {
            _businessServices = new Dictionary<Type, BusinessService.BaseService>();
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetRequiredService<ILogger<AppUtility>>();

            var setting = GetConfigSetting();
            Configuration = new Utility.Configuration
            {
                Setting = setting,
                ServiceProvider = serviceProvider,
                Culture = new System.Globalization.CultureInfo(setting.Application.Locale),
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

        private Entity.Entities.Config.ConfigSetting GetConfigSetting()
        {
            var configuration = new Utility.Configuration
            {
                Setting = Utility.AppHelper.GetDefaultConfigSetting(),
                ServiceProvider = _serviceProvider,
                Culture = System.Globalization.CultureInfo.CurrentCulture,
            };
            var configService = new BusinessService.ConfigService(configuration);
            var setting = configService.GetConfigSetting();
            return setting;
        }

        public List<System.Globalization.CultureInfo> GetSupportedCultures()
        {
            var supportedCultures = new List<System.Globalization.CultureInfo>();
            var repoService = new BusinessService.RepoService(Configuration);
            var filter = new Entity.Filter.LanguageCulture { Pagination = Utility.AppHelper.GetDefaultPagination(Configuration.Setting, true) };

            do
            {
                var items = repoService.Get(filter).Data;

                foreach (var item in items)
                {
                    supportedCultures.Add(new System.Globalization.CultureInfo(item.Code));
                }

                filter.Pagination.CurrentPage++;
            }
            while (filter.Pagination.CurrentPage <= filter.Pagination.TotalPages);
            return supportedCultures;
        }
    }
}

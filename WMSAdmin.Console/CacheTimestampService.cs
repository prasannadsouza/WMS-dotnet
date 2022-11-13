using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WMSAdmin.BusinessService;

namespace WMSAdmin.Console
{
    internal class CacheTimestampService : ConsoleService
    {
        public CacheTimestampService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            // Constructor's parameters validations...
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation($"Service is starting.");

            stoppingToken.Register(() => Logger.LogDebug($"Background task is stopping."));
            RunService = true;
            
            while (!stoppingToken.IsCancellationRequested)
            {
                if (RunService)
                {
                    var configService = AppUtility.GetBusinessService<BusinessService.ConfigService>();
                    var setting = configService.GetConfigSetting();
                    UpdateTimeStamp();
                    setting = configService.GetConfigSetting();
                    setting = configService.GetConfigSetting();
                }
                break;
            }

            Logger.LogInformation($"Background task is stopping.");
        }

        private void UpdateTimeStamp()
        {
            var keys = new List<string>
            {
                Entity.Constants.Cache.CONFIGSETTING_APPLICATION,
                Entity.Constants.Cache.CONFIGSETTING_DEBUGTEST,
                Entity.Constants.Cache.CONFIGSETTING_EMAIL,
                Entity.Constants.Cache.CONFIGSETTING_PAGINATION,
                $"{Entity.Constants.Cache.LANGUAGETEXT}_WMSAdmin_{Entity.Constants.Language.LANGUAGEGROUP_GENERAL}_sv-SE",
                $"{Entity.Constants.Cache.LANGUAGETEXT}_WMSAdmin_{Entity.Constants.Language.LANGUAGEGROUP_GENERAL}_en-SE",
                $"{Entity.Constants.Cache.LANGUAGETEXT}_WMSAdmin_{Entity.Constants.Language.LANGUAGEGROUP_LOGIN}_sv-SE",
                $"{Entity.Constants.Cache.LANGUAGETEXT}_WMSAdmin_{Entity.Constants.Language.LANGUAGEGROUP_LOGIN}_en-SE",
            };
            
            var cacheService = AppUtility.GetBusinessService<BusinessService.CacheService>();

            foreach (var key in keys)
            {
                cacheService.UpdateTimestamp(key);
            }
        }
    }
}

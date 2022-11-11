using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
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
                $"{Entity.Constants.Cache.CONFIGSETTING_LANGUAGETEXT}_WMSAdmin_{Entity.Constants.Language.LANGUAGEGROUP_GENERAL}_sv-SE",
                $"{Entity.Constants.Cache.CONFIGSETTING_LANGUAGETEXT}_WMSAdmin_{Entity.Constants.Language.LANGUAGEGROUP_GENERAL}_en-SE",
                $"{Entity.Constants.Cache.CONFIGSETTING_LANGUAGETEXT}_WMSAdmin_{Entity.Constants.Language.LANGUAGEGROUP_LOGIN}_sv-SE",
                $"{Entity.Constants.Cache.CONFIGSETTING_LANGUAGETEXT}_WMSAdmin_{Entity.Constants.Language.LANGUAGEGROUP_LOGIN}_en-SE",
            };

            var repoService = AppUtility.GetBusinessService<BusinessService.RepoService>();
            var appConfigGroup = repoService.Get(new Entity.Filter.AppConfigGroup { Code = Entity.Constants.Config.GROUP_CONFIGTIMESTAMP }).Data.FirstOrDefault();
            if (appConfigGroup == null)
            {
                appConfigGroup = new Entity.Entities.AppConfigGroup
                {
                    Code = Entity.Constants.Config.GROUP_CONFIGTIMESTAMP,
                    Name = "Config Timestamp",
                    Description = "Group for Config Timestamp",
                    TimeStamp = DateTime.Now,
                };

                repoService.Save(appConfigGroup);
            }

            foreach (var key in keys)
            {
                var appConfig = repoService.Get(new Entity.Filter.AppConfig
                {
                    Code = key,
                    AppConfigGroup = new Entity.Filter.AppConfigGroup { Id = appConfigGroup.Id }
                }).Data.FirstOrDefault();

                if (appConfig == null) appConfig = new Entity.Entities.AppConfig
                {
                    Code = key,
                    Description = $"Timestamp for Key {key}",
                    AppConfigGroupId = appConfigGroup.Id,
                };

                appConfig.Value = System.Text.Json.JsonSerializer.Serialize(DateTime.Now);
                appConfig.TimeStamp = DateTime.Now;
                repoService.Save(appConfig);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace WMSAdmin.WebApp.Controllers
{
    public class ConfigController : BaseController
    {
        public ConfigController(IServiceProvider serviceProvider) :
          base(serviceProvider)
        {

        }

        [HttpGet]
        [Route(WebUtility.APIRoute)]
        public void UpdateTimeStamp()
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

        [HttpGet]
        [Route(WebUtility.APIRoute)]
        public Entity.Entities.Config.ConfigSetting GetConfigSetting()
        {
            var configsetting = AppUtility.ConfigSetting;
            return configsetting;
        }
    }
}

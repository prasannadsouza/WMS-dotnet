using Microsoft.AspNetCore.Mvc;
using WMSAdmin.BusinessService;

namespace WMSAdmin.WebApp.Controllers
{
    public class ConfigController : BaseController
    {
        BusinessService.CacheService _cacheService;
        public ConfigController(IServiceProvider serviceProvider) :
          base(serviceProvider)
        {
            _cacheService = AppUtility.GetBusinessService<BusinessService.CacheService>();
        }

        [HttpGet]
        [Route(WebUtility.APIRoute)]
        public void UpdateTimeStamp([FromQuery] string cacheKey)
        {
            _cacheService.UpdateTimestamp(cacheKey);
        }

        [HttpGet]
        [Route(WebUtility.APIRoute)]
        public OkObjectResult UpdateAllTimeStamp()
        {
            //var keys = new List<string>
            //{
            //    Entity.Constants.Cache.CONFIGSETTING_APPLICATION,
            //    Entity.Constants.Cache.CONFIGSETTING_DEBUGTEST,
            //    Entity.Constants.Cache.CONFIGSETTING_EMAIL,
            //    Entity.Constants.Cache.CONFIGSETTING_PAGINATION,
            //    $"{Entity.Constants.Cache.LANGUAGETEXT}_WMSAdmin_{Entity.Constants.Language.LANGUAGEGROUP_GENERAL}_sv-SE",
            //    $"{Entity.Constants.Cache.LANGUAGETEXT}_WMSAdmin_{Entity.Constants.Language.LANGUAGEGROUP_GENERAL}_en-SE",
            //    $"{Entity.Constants.Cache.LANGUAGETEXT}_WMSAdmin_{Entity.Constants.Language.LANGUAGEGROUP_LOGIN}_sv-SE",
            //    $"{Entity.Constants.Cache.LANGUAGETEXT}_WMSAdmin_{Entity.Constants.Language.LANGUAGEGROUP_LOGIN}_en-SE",
            //};

            var timeStampList = _cacheService.GetCacheTimestampList();
            foreach (var timeStamp in timeStampList)
            {
                _cacheService.UpdateTimestamp(timeStamp.Code);
            }

            return Ok(new Entity.Entities.Response<bool> { Data = true });
        }


        [HttpGet]
        [Route(WebUtility.APIRoute)]
        public OkObjectResult ClearCache()
        {
            _cacheService.ClearCache();
            return Ok(new Entity.Entities.Response<bool> { Data = true });
        }

        [HttpGet]
        [Route(WebUtility.APIRoute)]
        public OkObjectResult GetApplicationConfig()
        {
            var setting = AppUtility.ConfigSetting.Application;
            return Ok(new Entity.Entities.Response<Entity.Entities.Config.Client.Application>
            {
                Data = new Entity.Entities.Config.Client.Application
                {
                    ApplicationTitle = setting.ApplicationTitle,
                    BaseUrl = setting.BaseUrl,
                    CurrentVersion = setting.CurrentVersion,
                    LocaleCode = setting.LocaleCode,
                    UILocaleCode = setting.UILocaleCode,
                }
            });
        }

        [HttpGet]
        [Route(WebUtility.APIRoute)]
        public OkObjectResult GetPaginationConfig()
        {
            var setting = AppUtility.ConfigSetting.Pagination;
            return Ok(new Entity.Entities.Response<Entity.Entities.Config.Client.Pagination>
            {
                Data = new Entity.Entities.Config.Client.Pagination
                {
                    MaxRecordsPerPage = setting.MaxRecordsPerPage,
                    RecordsPerPage = setting.RecordsPerPage,
                    TotalPagesToJump = setting.TotalPagesToJump,
                },
            });
        }
    }
}

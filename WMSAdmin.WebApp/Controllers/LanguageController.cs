﻿using Microsoft.AspNetCore.Mvc;

namespace WMSAdmin.WebApp.Controllers
{
    [ApiController]
    public class LanguageController : BaseController
    {

        public LanguageController(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        [HttpGet]
        [Route(WebAppUtility.WebAppUtility.APIRoute)]
        public OkObjectResult GetGeneralString([FromQuery] string cultureCode)
        {
            var resource = new Language.ResourceManager.GeneralString(AppUtility.Configuration, new System.Globalization.CultureInfo(cultureCode));
            return Ok(new Entity.Entities.Response<Entity.Entities.LanguageStrings.GeneralString> { Data = resource.GetString() });
        }

        [HttpGet]
        [Route(WebAppUtility.WebAppUtility.APIRoute)]
        public OkObjectResult GetLoginString([FromQuery] string cultureCode)
        {
            var resource = new Language.ResourceManager.LoginString(AppUtility.Configuration, new System.Globalization.CultureInfo(cultureCode));
            return Ok(new Entity.Entities.Response<Entity.Entities.LanguageStrings.LoginString> { Data = resource.GetString() });
        }

        [HttpGet]
        [Route(WebAppUtility.WebAppUtility.APIRoute)]
        public OkObjectResult GetDefaultGeneralString()
        {
            var resource = new Language.ResourceManager.GeneralString(AppUtility.Configuration, System.Globalization.CultureInfo.CurrentCulture);
            return Ok(new Entity.Entities.Response<Entity.Entities.LanguageStrings.GeneralString> { Data = resource.GetString() });
        }

        [HttpGet]
        [Route(WebAppUtility.WebAppUtility.APIRoute)]
        public OkObjectResult GetDefaultLoginString()
        {
            var resource = new Language.ResourceManager.LoginString(AppUtility.Configuration, System.Globalization.CultureInfo.CurrentCulture);
            return Ok(new Entity.Entities.Response<Entity.Entities.LanguageStrings.LoginString> { Data = resource.GetString() });
        }

        [HttpGet]
        [Route(WebAppUtility.WebAppUtility.APIRoute)]
        public OkObjectResult GetUICulture()
        {
            var languageCultureFilter = new Entity.Filter.LanguageCulture
            {
                Pagination = new Entity.Entities.Pagination
                {
                    CurrentPage = 1,
                    RecordsPerPage = AppUtility.ConfigSetting.Pagination.MaxRecordsAllowedPerPage,
                }
            };

            var languageCultures = new List<Entity.Entities.LanguageCulture>();
            var repoService = AppUtility.GetBusinessService<BusinessService.RepoService>();
            do
            {
                languageCultures.AddRange(repoService.Get(languageCultureFilter).Data);
                languageCultureFilter.Pagination.CurrentPage++;
            }
            while (languageCultureFilter.Pagination.CurrentPage <= languageCultureFilter.Pagination.TotalPages);
            return Ok(new Entity.Entities.Response<List<Entity.Entities.LanguageCulture>> { Data = languageCultures });
        }
    }
}

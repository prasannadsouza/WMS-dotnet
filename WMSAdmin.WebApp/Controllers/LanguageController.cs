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
        [Route(WebUtility.APIRoute)]
        public JsonResult GetGeneralString([FromQuery] string cultureCode)
        {
            var generalResource = new Language.ResourceManager.GeneralString(AppUtility.Configuration, new System.Globalization.CultureInfo(cultureCode));
            return Json( generalResource.GetString());
        }

        [HttpGet]
        [Route(WebUtility.APIRoute)]
        public JsonResult GetLoginString([FromQuery] string cultureCode)
        {
            var generalResource = new Language.ResourceManager.LoginString(AppUtility.Configuration, new System.Globalization.CultureInfo(cultureCode));
            return Json(generalResource.GetString());
        }

        [HttpGet]
        [Route(WebUtility.APIRoute)]
        public JsonResult GetDefaultGeneralString()
        {
            var generalResource = new Language.ResourceManager.GeneralString(AppUtility.Configuration, System.Globalization.CultureInfo.CurrentCulture);
            return Json(generalResource.GetString());
        }

        [HttpGet]
        [Route(WebUtility.APIRoute)]
        public JsonResult GetDefaultLoginString()
        {
            var generalResource = new Language.ResourceManager.LoginString(AppUtility.Configuration, System.Globalization.CultureInfo.CurrentCulture);
            return Json(generalResource.GetString());
        }
        
    }
}

using Microsoft.AspNetCore.Mvc;
namespace WMSAdmin.WebApp.Controllers
{
    [ApiController]
    public class LanguageController : BaseController
    {

        public LanguageController(IServiceProvider serviceProvider, ILogger<WeatherForecastController> logger) :
         base(serviceProvider, logger)
        {

        }

        [HttpGet]
        [Route("api/[controller]/[action]")]
        public JsonResult GetGeneralString([FromQuery] string cultureCode)
        {
            var generalResource = new Language.ResourceManager.GeneralString(AppUtility.Configuration, new System.Globalization.CultureInfo(cultureCode));
            return Json( generalResource.GetString());
        }

        [HttpGet]
        [Route("api/[controller]/[action]")]
        public JsonResult GetLoginString([FromQuery] string cultureCode)
        {
            var generalResource = new Language.ResourceManager.LoginString(AppUtility.Configuration, new System.Globalization.CultureInfo(cultureCode));
            return Json(generalResource.GetString());
        }

#if DEBUG
        [HttpGet]
        [Route("api/[controller]/[action]")]
        public OkResult GenerateLanguageClass([FromQuery] string wmsApplicationCode, [FromQuery] string languageGroupCode)
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

            var languageGroupFilter = new Entity.Filter.LanguageGroup
            {
                Code = languageGroupCode,
                WMSApplication = new Entity.Filter.WMSApplication { Code = wmsApplicationCode },
                Pagination = new Entity.Entities.Pagination
                {
                    CurrentPage = 1,
                    RecordsPerPage = AppUtility.ConfigSetting.Pagination.MaxRecordsAllowedPerPage,
                }
            };

            var languageGroups = new List<Entity.Entities.LanguageGroup>();
            do
            {
                languageGroups.AddRange(repoService.Get(languageGroupFilter).Data);
                languageGroupFilter.Pagination.CurrentPage++;
            }
            while (languageGroupFilter.Pagination.CurrentPage <= languageGroupFilter.Pagination.TotalPages);

            var languageTextFilter = new Entity.Filter.LanguageText
            {
                LanguageGroup = new Entity.Filter.LanguageGroup
                {
                    Code = languageGroupCode,
                    WMSApplication = new Entity.Filter.WMSApplication { Code = wmsApplicationCode },
                },
                Pagination = new Entity.Entities.Pagination
                {
                    CurrentPage = 1,
                    RecordsPerPage = AppUtility.ConfigSetting.Pagination.MaxRecordsAllowedPerPage,
                }
            };

            var languageTexts = new List<Entity.Entities.LanguageText>();
            do
            {
                languageTexts.AddRange(repoService.Get(languageTextFilter).Data);
                languageTextFilter.Pagination.CurrentPage++;
            }
            while (languageTextFilter.Pagination.CurrentPage <= languageTextFilter.Pagination.TotalPages);

            var groupedLanguageText = languageTexts.GroupBy(e => e.LanguageCultureId).Select(e => new {
                LanguageCultureId = e.Key,
                LanguageTexts = e.OrderBy(f=> f.Code).ToList(),
            }).ToList();

            var defaultLanguageTexts = languageTexts.Where(e => e.LanguageCultureId == languageCultures.First(f=> f.Code == AppUtility.ConfigSetting.Application.Locale).Id).ToList();

            var className = languageGroupCode.First().ToString().ToUpper() + languageGroupCode.ToLower().Substring(1,languageGroupCode.Length -1);

            var sb_tsx_Locale = new System.Text.StringBuilder($"export type {className}LocaleStrings = {{");

            var sb_tsx_Text = new System.Text.StringBuilder($"export type {className} = {{");

            var sb_cs_Resource = new System.Text.StringBuilder("public class " + className + "String : BaseResourceManager");
            sb_cs_Resource.AppendLine("{");
            sb_cs_Resource.AppendLine($"public {className}String(Configuration configuration, CultureInfo culture) : base(configuration, culture, Entity.Constants.Language.LANGUAGEGROUP_{className.ToUpper()})");
            sb_cs_Resource.AppendLine("{ }");

            var sb_cs_Resource_GetString = new System.Text.StringBuilder($"public Entity.Entities.LanguageStrings.{className}String GetString()");
            sb_cs_Resource_GetString.AppendLine("{");
            sb_cs_Resource_GetString.AppendLine($"return new Entity.Entities.LanguageStrings.{className}String");
            sb_cs_Resource_GetString.AppendLine("{");

            var sb_cs_Entity = new System.Text.StringBuilder("public class " + className);
            sb_cs_Entity.AppendLine("{");

            foreach (var languageText in defaultLanguageTexts)
            {
                sb_tsx_Text.AppendLine($"{languageText.Code}: string;");
                sb_cs_Resource_GetString.AppendLine($"public string {languageText.Code}=> GetResourceString(nameof({languageText.Code}));");
                sb_cs_Resource_GetString.AppendLine($"{languageText.Code}={languageText.Code}");
                sb_cs_Entity.AppendLine($"public string {languageText.Code} {{ get; set; }}");
            }



            return Ok();
        }
#endif
    }
}

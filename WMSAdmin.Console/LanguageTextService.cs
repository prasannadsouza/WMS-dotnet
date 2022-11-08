using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Console
{
    internal class LanguageTextService : ConsoleService
    {
        public LanguageTextService(IServiceProvider serviceProvider, ILogger<LanguageTextService> logger):  base(serviceProvider, logger)
        {
            // Constructor's parameters validations...
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogDebug($"LanguageTextService is starting.");

            stoppingToken.Register(() => Logger.LogDebug($" LanguageTextService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                GenerateLanguageClass("WMSAdmin", "Login");
                break;
            }

            Logger.LogDebug($"LanguageTextService background task is stopping.");
        }

        public void GenerateLanguageClass(string wmsApplicationCode, string languageGroupCode)
        {
            var languageCultureFilter = new Entity.Filter.LanguageCulture
            {
                Pagination = new Entity.Entities.Pagination
                {
                    CurrentPage = 1,
                    RecordsPerPage = Utility.ConfigSetting.Pagination.MaxRecordsAllowedPerPage,
                }
            };
            var languageCultures = new List<Entity.Entities.LanguageCulture>();
            var repoService = Utility.GetBusinessService<BusinessService.RepoService>();
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
                    RecordsPerPage = Utility.ConfigSetting.Pagination.MaxRecordsAllowedPerPage,
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
                    RecordsPerPage = Utility.ConfigSetting.Pagination.MaxRecordsAllowedPerPage,
                }
            };

            var languageTexts = new List<Entity.Entities.LanguageText>();
            do
            {
                languageTexts.AddRange(repoService.Get(languageTextFilter).Data);
                languageTextFilter.Pagination.CurrentPage++;
            }
            while (languageTextFilter.Pagination.CurrentPage <= languageTextFilter.Pagination.TotalPages);

            var defaultLanguageTexts = languageTexts.Where(e => e.LanguageCultureId == languageCultures.First(f => f.Code == Utility.ConfigSetting.Application.UILocale).Id).ToList();
            GenerateCSEntity(languageGroupCode, defaultLanguageTexts);
            GenerateCSResource(languageGroupCode, defaultLanguageTexts);
            GenerateCSTSX(languageGroupCode, defaultLanguageTexts, languageCultures);
        }

        private void GenerateCSEntity(string languageGroupCode, List<Entity.Entities.LanguageText> languageTexts)
        {
            var sb_cs = new System.Text.StringBuilder("namespace WMSAdmin.Entity.Entities.LanguageStrings");
            sb_cs.AppendLine("{");
            sb_cs.AppendLine($"public class {languageGroupCode}String");
            sb_cs.AppendLine("{");
            foreach (var languageText in languageTexts)
            {
                sb_cs.AppendLine($"public string {languageText.Code} {{ get; set; }}");
            }
            sb_cs.AppendLine("}");
            sb_cs.AppendLine("}");

            //C:\Prasanna\Personal\code\WMS\WMSAdmin.Entity\Entities\LanguageStrings\GeneralString.cs
            var cdInfo = new System.IO.DirectoryInfo(Environment.CurrentDirectory);
            var savePath = Path.Combine(cdInfo.Parent.Parent.Parent.Parent.FullName, "WMSAdmin.Entity", "Entities", "LanguageStrings",$"{languageGroupCode}String.cs");
            File.WriteAllText(savePath, sb_cs.ToString());
        }

        private void GenerateCSResource(string languageGroupCode, List<Entity.Entities.LanguageText> languageTexts)
        {
            var sb_cs = new System.Text.StringBuilder("namespace WMSAdmin.Language.ResourceManager");
            sb_cs.AppendLine("{");
            sb_cs.AppendLine("public class " + languageGroupCode + "String : BaseResourceManager");
            sb_cs.AppendLine("{");
            sb_cs.AppendLine($"public {languageGroupCode}String(Configuration configuration, System.Globalization.CultureInfo culture) : base(configuration, culture, Entity.Constants.Language.LANGUAGEGROUP_{languageGroupCode.ToUpper()})");
            sb_cs.AppendLine("{");
            sb_cs.AppendLine("}");

            var sb_fn_GetString = new System.Text.StringBuilder($"public Entity.Entities.LanguageStrings.{languageGroupCode}String GetString()");
            sb_fn_GetString.AppendLine("{");
            sb_fn_GetString.AppendLine($"return new Entity.Entities.LanguageStrings.{languageGroupCode}String");
            sb_fn_GetString.AppendLine("{");

            foreach (var languageText in languageTexts)
            {
                sb_cs.AppendLine($"public string {languageText.Code}=> GetResourceString(nameof({languageText.Code}));");
                sb_fn_GetString.AppendLine($"{languageText.Code}={languageText.Code},");
            }

            sb_fn_GetString.AppendLine("};");
            sb_fn_GetString.AppendLine("}");
            sb_cs.AppendLine();
            sb_cs.Append(sb_fn_GetString);
            sb_cs.AppendLine("}");
            sb_cs.AppendLine("}");

            //C:\Prasanna\Personal\code\WMS\WMSAdmin.Language\ResourceManager\GeneralString.cs
            var cdInfo = new System.IO.DirectoryInfo(Environment.CurrentDirectory);
            var savePath = Path.Combine(cdInfo.Parent.Parent.Parent.Parent.FullName, "WMSAdmin.Language", "ResourceManager", $"{languageGroupCode}String.cs");
            File.WriteAllText(savePath, sb_cs.ToString());
        }

        private void GenerateCSTSX(string languageGroupCode, List<Entity.Entities.LanguageText> languageTexts, List<Entity.Entities.LanguageCulture> cultures)
        {
            

            var sb_cs = new System.Text.StringBuilder($"export type {languageGroupCode}LocaleString = {{");
            foreach (var culture in cultures)
            {
                sb_cs.AppendLine($"{culture.Code.Replace("-","_")}: {languageGroupCode}String;");
            }
            sb_cs.AppendLine("}");
            sb_cs.AppendLine();


            sb_cs.AppendLine($"export type {languageGroupCode}String = {{");
            foreach (var languageText in languageTexts)
            {
                sb_cs.AppendLine($"{languageText.Code}: string;");
            }
            sb_cs.AppendLine("}");

            var cdInfo = new System.IO.DirectoryInfo(Environment.CurrentDirectory);
            var savePath = Path.Combine(cdInfo.FullName, "WMSAdmin.Language.TSX", $"{languageGroupCode}String.tsx");
            var fileInfo = new FileInfo(savePath);
            if (fileInfo.Directory.Exists == false) fileInfo.Directory.Create();
            fileInfo.Refresh();
            File.WriteAllText(savePath, sb_cs.ToString());
        }
    }
}

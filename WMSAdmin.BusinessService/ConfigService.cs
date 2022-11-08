using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.BusinessService
{
    public class ConfigService : BaseService
    {
        RepoService _repoService;
        public ConfigService(Configuration configuration) : base(configuration)
        {
            _repoService = new RepoService(configuration);
        }

        private string _className;
        private string ClassName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_className) == false) return _className;
                _className = this.GetType().FullName;
                return _className;
            }
        }

        public Entity.Entities.Config.ConfigSetting GetConfigSetting()
        {
            var setting = new Entity.Entities.Config.ConfigSetting();
            setting.Application = GetApplication();
            setting.Pagination = GetPagination();
            setting.DebugTest = GetDebugTest();
            setting.Email = GetEmail();
            setting.Timestamp = GetTimestamp();
            return setting;
        }

        private Entity.Entities.AppConfigGroup GetAppConfigGroup(string code)
        {
            return _repoService.Get(new Entity.Filter.AppConfigGroup { Code = code }).Data.FirstOrDefault();
        }

        private Entity.Entities.Config.DebugTest GetDebugTest()
        {
            var key = Entity.Constants.Cache.CONFIGSETTING_DEBUGTEST;
            var to = GetFromCache<Entity.Entities.Config.DebugTest>(key, out _);
            if (to != null) return to;
            to = new Entity.Entities.Config.DebugTest();

            var appConfigGroup = GetAppConfigGroup(Entity.Constants.Config.GROUP_DEBUGTEST);

            var filter = new Entity.Filter.AppConfig
            {
                AppConfigGroup = new Entity.Filter.AppConfigGroup
                {
                    Id = appConfigGroup.Id,
                },
            };

            var repo = GetRepository<Repository.AppConfig>();

            do
            {
                var items = repo.Get(filter).Data;
                foreach (var from in items)
                {
                    switch (from.Code)
                    {
                        case Entity.Constants.Config.DEBUGTEST_IS_TESTMODE:
                            {
                                to.IsTestMode = from.Value == Entity.Constants.Config.TRUE_VALUE;
                                break;
                            }
                        case Entity.Constants.Config.DEBUGTEST_CUSTOMERNUMBER:
                            {
                                to.CustomerNumber = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.DEBUGTEST_USERNAME:
                            {
                                to.UserName = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.DEBUGTEST_IMPERSONATING_USERNAME:
                            {
                                to.ImpersonatingUserName = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.DEBUGTEST_DEV_AUTO_LOGIN:
                            {
                                to.DevAutoLogin = from.Value == Entity.Constants.Config.TRUE_VALUE;
                                break;
                            }
                        default:
                            {
                                var loginfo = new
                                {
                                    SesssionId = Configuration.Setting.Application.SessionId,
                                    Class = ClassName,
                                    Method = "SetDebugTestConfig",
                                    AppConfigGroup = appConfigGroup.Code,
                                    AppConfig = from.Code,
                                };

                                Logger.LogError($"AppConfig {from.Code} is not handled", new { LogInfo = loginfo });
                                break;
                            }
                    }
                }
                filter.Pagination.CurrentPage++;
            }
            while (filter.Pagination.CurrentPage <= filter.Pagination.TotalPages);
            Configuration.Setting.DebugTest = to;
            SaveToCache(key, to, true);
            return to;
        }
        private Entity.Entities.Config.Email GetEmail()
        {
            var key = Entity.Constants.Cache.CONFIGSETTING_EMAIL;
            var to = GetFromCache<Entity.Entities.Config.Email>(key, out _);
            if (to != null) return to;
            to = new Entity.Entities.Config.Email();
            var appConfigGroup = GetAppConfigGroup(Entity.Constants.Config.GROUP_EMAIL);

            var filter = new Entity.Filter.AppConfig
            {
                AppConfigGroup = new Entity.Filter.AppConfigGroup
                {
                    Id = appConfigGroup.Id,
                },
            };

            var repo = GetRepository<Repository.AppConfig>();

            do
            {
                var items = repo.Get(filter).Data;
                foreach (var from in items)
                {
                    switch (from.Code)
                    {
                        case Entity.Constants.Config.EMAIL_FROM_EMAIL_ADDRESS:
                            {
                                to.FromEmailAdddress = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.EMAIL_FROM_EMAIL_DISPLAYNAME:
                            {
                                to.FromEmailDisplayName = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.EMAIL_CONTACT_EMAIL_ADDRESS:
                            {
                                to.ContactEmailAdddress = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.EMAIL_CONTACT_EMAIL_DISPLAYNAME:
                            {
                                to.ContactEmailDisplayName = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.EMAIL_SERVER:
                            {
                                to.Server = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.EMAIL_PORT:
                            {
                                to.Port = int.Parse(from.Value);
                                break;
                            }
                        case Entity.Constants.Config.EMAIL_USERNAME:
                            {
                                to.UserName = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.EMAIL_USERNAME_EMAILADDRESS:
                            {
                                to.UserNameEmailAddress = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.EMAIL_PASSWORD:
                            {
                                to.Password = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.EMAIL_BCC_EMAILS_FOR_IMPORTANT_INFORMATION:
                            {
                                to.BccEmailsImportantInformation = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.EMAIL_BCC_EMAILS_FOR_SUPPORT_INFORMATION:
                            {
                                to.BccEmailsSupportInformation = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.EMAIL_ENABLE_SSL:
                            {
                                to.EnableSSL = from.Value == Entity.Constants.Config.TRUE_VALUE;
                                break;
                            }
                        case Entity.Constants.Config.EMAIL_LIST_SEPERATOR:
                            {
                                to.EmailListSeperator = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.EMAIL_EMAIL_DOMAIN_TO_TRUST:
                            {
                                to.EmailDomainToTrust = from.Value;
                                break;
                            }
                        default:
                            {
                                var loginfo = new
                                {
                                    SesssionId = Configuration.Setting.Application.SessionId,
                                    Class = ClassName,
                                    Method = "SetEmailConfig",
                                    AppConfigGroup = appConfigGroup.Code,
                                    AppConfig = from.Code,
                                };

                                Logger.LogError($"AppConfig {from.Code} is not handled", new { LogInfo = loginfo });
                                break;
                            }
                    }
                }
                filter.Pagination.CurrentPage++;
            }
            while (filter.Pagination.CurrentPage <= filter.Pagination.TotalPages);
            Configuration.Setting.Email = to;
            SaveToCache(key, to, true);
            return to;
        }
        private Entity.Entities.Config.Application GetApplication()
        {
            var key = Entity.Constants.Cache.CONFIGSETTING_APPLICATION;
            var to = GetFromCache<Entity.Entities.Config.Application>(key, out _);
            if (to != null) return to;
            to = new Entity.Entities.Config.Application(); 
            var appConfigGroup = GetAppConfigGroup(Entity.Constants.Config.GROUP_APPLICATION);

            var filter = new Entity.Filter.AppConfig
            {
                AppConfigGroup = new Entity.Filter.AppConfigGroup
                {
                    Id = appConfigGroup.Id,
                },
            };

            var repo = GetRepository<Repository.AppConfig>();

            do
            {
                var items = repo.Get(filter).Data;
                foreach (var from in items)
                {
                    switch (from.Code)
                    {
                        case Entity.Constants.Config.APPLICATION_LOCALE:
                            {
                                to.Locale = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.APPLICATION_UI_LOCALE:
                            {
                                to.UILocale = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.APPLICATION_TITLE:
                            {
                                to.ApplicationTitle = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.APPLICATION_LOG_DATABASEQUERIES:
                            {
                                to.LogDatabaseQueries = from.Value == Entity.Constants.Config.TRUE_VALUE;
                                break;
                            }
                        case Entity.Constants.Config.APPLICATION_CURRENT_VERSION:
                            {
                                to.CurrentVersion = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.APPLICATION_APPCODE:
                            {
                                to.AppCode = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.APPLICATION_LOCALFILES_BASEPATH:
                            {
                                to.LocalFilesBasePath = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.APPLICATION_WEBFOLDER_DOWNLOADPATH:
                            {
                                to.WebfolderDownloadPath = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.APPLICATION_PATH_TEMPLATEFILES:
                            {
                                to.TemplateFilesPath = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.APPLICATION_APP_APIKEY:
                            {
                                to.AppAPIKey = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.APPLICATION_CONFIG_TIMESTAMP:
                            {
                                to.ConfigTimeStamp = DateTime.Parse(from.Value, System.Globalization.CultureInfo.InvariantCulture);
                                break;
                            }
                        case Entity.Constants.Config.APPLICATION_CONFIG_BASEURL:
                            {
                                to.BaseUrl = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.APPLICATION_BASEURL_INTERNAL:
                            {
                                to.BaseUrl = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.APPLICATION_CACHEEXPIRY_INMINUTES:
                            {
                                to.CacheExpiryInMinutes = int.Parse(from.Value);
                                break;
                            }
                        default:
                            {
                                var loginfo = new
                                {
                                    SesssionId = Configuration.Setting.Application.SessionId,
                                    Class = ClassName,
                                    Method = "SetSystemConfig",
                                    AppConfigGroup = appConfigGroup.Code,
                                    AppConfig = from.Code,
                                };

                                Logger.LogError($"AppConfig {from.Code} is not handled", new { LogInfo = loginfo });
                                break;
                            }
                    }
                }
                filter.Pagination.CurrentPage++;
            }
            while (filter.Pagination.CurrentPage <= filter.Pagination.TotalPages);
            Configuration.Setting.Application = to;
            SaveToCache(key, to, true);
            return to;
        }
        private Entity.Entities.Config.Timestamp GetTimestamp()
        {
            var key = Entity.Constants.Cache.CONFIGSETTING_TIMESTAMP;
            var to = GetFromCache<Entity.Entities.Config.Timestamp>(key, out _);
            if (to != null) return to;
            to = new Entity.Entities.Config.Timestamp();

            var appConfigGroup = GetAppConfigGroup(Entity.Constants.Config.GROUP_CONFIGTIMESTAMP);

            var filter = new Entity.Filter.AppConfig
            {
                AppConfigGroup = new Entity.Filter.AppConfigGroup
                {
                    Id = appConfigGroup.Id,
                },
            };

            var repo = GetRepository<Repository.AppConfig>();

            do
            {
                var items = repo.Get(filter).Data;
                foreach (var from in items)
                {
                    switch (from.Code)
            {
                case Entity.Constants.Config.CONFIGTIMESTAMP_APPLICATION:
                    {
                        to.Application = DateTime.Parse(from.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    }
                case Entity.Constants.Config.CONFIGTIMESTAMP_EMAIL:
                    {
                        to.Email = DateTime.Parse(from.Value,System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    }
                case Entity.Constants.Config.CONFIGTIMESTAMP_DEBUGTEST:
                    {
                        to.DebugTest = DateTime.Parse(from.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    }
                case Entity.Constants.Config.CONFIGTIMESTAMP_PAGINATION:
                    {
                        to.Pagination = DateTime.Parse(from.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    }
                default:
                    {
                        var loginfo = new
                        {
                            SesssionId = Configuration.Setting.Application.SessionId,
                            Class = ClassName,
                            Method = "SetConfigTimestamp",
                            AppConfigGroup = appConfigGroup.Code,
                            AppConfig = from.Code,
                        };

                        Logger.LogError($"AppConfig is not handled", new { LogInfo = loginfo });
                        break;
                    }
            }
                }
                filter.Pagination.CurrentPage++;
            }
            while (filter.Pagination.CurrentPage <= filter.Pagination.TotalPages);
            Configuration.Setting.Timestamp = to;
            SaveToCache(key, to, true);
            return to;
        }
        private Entity.Entities.Config.Pagination GetPagination()
        {
            var key = Entity.Constants.Cache.CONFIGSETTING_PAGINATION;
            var to = GetFromCache<Entity.Entities.Config.Pagination>(key, out _);
            if (to != null) return to;
            to = new Entity.Entities.Config.Pagination();
            var appConfigGroup = GetAppConfigGroup(Entity.Constants.Config.GROUP_PAGINATION);

            var filter = new Entity.Filter.AppConfig
            {
                AppConfigGroup = new Entity.Filter.AppConfigGroup
                {
                    Id = appConfigGroup.Id,
                },
            };

            var repo = GetRepository<Repository.AppConfig>();

            do
            {
                var items = repo.Get(filter).Data;
                foreach (var from in items)
                {
                    switch (from.Code)
                    {
                        case Entity.Constants.Config.PAGINATION_TOTALPAGESTOJUMP:
                            {
                                to.TotalPagesToJump = int.Parse(from.Value, System.Globalization.CultureInfo.InvariantCulture);
                                break;
                            }
                        case Entity.Constants.Config.PAGINATION_MAXIMUM_RECORDSPERPAGE:
                            {
                                to.MaxRecordsPerPage = int.Parse(from.Value, System.Globalization.CultureInfo.InvariantCulture);
                                break;
                            }
                        case Entity.Constants.Config.PAGINATION_RECORDSPERPAGE:
                            {
                                to.RecordsPerPage = int.Parse(from.Value, System.Globalization.CultureInfo.InvariantCulture);
                                break;
                            }
                        case Entity.Constants.Config.PAGINATION_MAXIMUM_RECORDSALLOWEDPERPAGE:
                            {
                                to.MaxRecordsAllowedPerPage = int.Parse(from.Value, System.Globalization.CultureInfo.InvariantCulture);
                                break;
                            }
                        default:
                            {
                                var loginfo = new
                                {
                                    SesssionId = Configuration.Setting.Application.SessionId,
                                    Class = ClassName,
                                    Method = "GetPaginationConfig",
                                    AppConfigGroup = appConfigGroup.Code,
                                    AppConfig = from.Code,
                                };

                                Logger.LogError($"AppConfig {from.Code} is not handled", new { LogInfo = loginfo });
                                break;
                            }
                    }
                }
                filter.Pagination.CurrentPage++;
            }
            while (filter.Pagination.CurrentPage <= filter.Pagination.TotalPages);
            Configuration.Setting.Pagination = to;
            SaveToCache(key, to, true);
            return to;
        }
    }
}

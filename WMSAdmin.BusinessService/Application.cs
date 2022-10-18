using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.BusinessService
{
    public class Application : BaseBusinessService
    {
        public Application(BusinessServiceConfiguration configuration) : base(configuration)
        {
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

        public List<Entity.Entities.AppConfigGroup> GetAppConfigGroup()
        {
            var repoAppConfigGroup = GetRepository<Repository.AppConfigGroup>();
            return repoAppConfigGroup.GetAll();
        }

        public List<Entity.Entities.AppConfig> GetAppConfig(bool includeGroups)
        {
            var repoAppConfig = GetRepository<Repository.AppConfig>();
            var appConfigs = repoAppConfig.GetAll();

            if (includeGroups)
            {
                var appConfigGroup = GetAppConfigGroup();
                appConfigs.ForEach(e => e.AppConfigGroup = appConfigGroup.FirstOrDefault(f => f.Id == e.AppConfigGroupId));
            }
            return appConfigs;
        }

        public Entity.Entities.ConfigSetting GetConfigSetting()
        {
            var settings = GetAppConfig(true);

            var setting = new Entity.Entities.ConfigSetting();
            setting.System = new Entity.Entities.Config.Application();
            setting.DebugTest = new Entity.Entities.Config.DebugTest();
            setting.Email = new Entity.Entities.Config.Email();

            foreach (var item in settings)
            {
                switch (item.AppConfigGroup.Code)
                {
                    case Entity.Constants.Config.GROUP_APPLICATION:
                        {
                            SetApplicationConfig(item, setting.System);
                            break;
                        }
                    case Entity.Constants.Config.GROUP_EMAIL:
                        {
                            SetEmailConfig(item, setting.Email);
                            break;
                        }
                    case Entity.Constants.Config.GROUP_DEBUGTEST:
                        {
                            SetDebugTestConfig(item, setting.DebugTest);
                            break;
                        }
                    case Entity.Constants.Config.GROUP_CONFIGTIMESTAMP:
                        {
                            SetConfigTimestamp(item, setting.ConfigTimestamp);
                            break;
                        }
                    default:
                        {
                            var loginfo = new
                            {
                                SesssionId = Configuration.Setting.System.SessionId,
                                Class = ClassName,
                                Method = "GetConfigSetting",
                                AppConfigGroup = item.AppConfigGroup.Code,

                            };

                            Logger.LogError("SettingGroup is not handled", new { LogInfo = loginfo });
                            break;
                        }
                }
            }
            return setting;
        }

        private void SetDebugTestConfig(Entity.Entities.AppConfig from, Entity.Entities.Config.DebugTest to)
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
                            SesssionId = Configuration.Setting.System.SessionId,
                            Class = ClassName,
                            Method = "SetDebugTestConfig",
                            AppConfigGroup = from.AppConfigGroup.Code,
                            AppConfig = from.Code,

                        };

                        Logger.LogError($"AppConfig is not handled", new { LogInfo = loginfo });
                        break;
                    }
            }
        }

        private void SetEmailConfig(Entity.Entities.AppConfig from, Entity.Entities.Config.Email to)
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
                            SesssionId = Configuration.Setting.System.SessionId,
                            Class = ClassName,
                            Method = "SetEmailConfig",
                            AppConfigGroup = from.AppConfigGroup.Code,
                            AppConfig = from.Code,
                        };

                        Logger.LogError($"AppConfig is not handled", new { LogInfo = loginfo });
                        break;
                    }
            }
        }

        private void SetApplicationConfig(Entity.Entities.AppConfig from, Entity.Entities.Config.Application to)
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
                default:
                    {
                        var loginfo = new
                        {
                            SesssionId = Configuration.Setting.System.SessionId,
                            Class = ClassName,
                            Method = "SetSystemConfig",
                            AppConfigGroup = from.AppConfigGroup.Code,
                            AppConfig = from.Code,
                        };

                        Logger.LogError($"AppConfig is not handled", new { LogInfo = loginfo });
                        break;
                    }
            }
        }

        private void SetConfigTimestamp(Entity.Entities.AppConfig from, Entity.Entities.ConfigTimestamp to)
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
                            SesssionId = Configuration.Setting.System.SessionId,
                            Class = ClassName,
                            Method = "SetConfigTimestamp",
                            AppConfigGroup = from.AppConfigGroup.Code,
                            AppConfig = from.Code,
                        };

                        Logger.LogError($"AppConfig is not handled", new { LogInfo = loginfo });
                        break;
                    }
            }
        }

        private void SetPaginationConfig(Entity.Entities.AppConfig from, Entity.Entities.Config.Pagination to)
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
                            SesssionId = Configuration.Setting.System.SessionId,
                            Class = ClassName,
                            Method = "SetConfigTimestamp",
                            AppConfigGroup = from.AppConfigGroup.Code,
                            AppConfig = from.Code,
                        };

                        Logger.LogError($"AppConfig is not handled", new { LogInfo = loginfo });
                        break;
                    }
            }
        }
    }
}

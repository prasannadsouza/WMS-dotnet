﻿using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
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
            return RepoAppConfigGroup.GetAll();
        }

        public List<Entity.Entities.AppConfig> GetAppConfig(bool includeGroups)
        {
            var appConfigs = RepoAppConfig.GetAll();

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
            setting.System = new Entity.Entities.SystemConfig();
            setting.DebugTest = new Entity.Entities.DebugTestConfig();
            setting.Email = new Entity.Entities.EmailConfig();

            foreach (var item in settings)
            {
                switch (item.AppConfigGroup.Code)
                {
                    case Entity.Constants.Config.GROUP_APPLICATION:
                        {
                            SetSytemConfig(item, setting.System);
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

        private void SetDebugTestConfig(Entity.Entities.AppConfig from, Entity.Entities.DebugTestConfig to)
        {
            switch (from.Code)
            {
                case Entity.Constants.Config.DEBUGTEST_IS_TESTMODE:
                    {
                        to.IsTestMode = from.Value == Entity.Constants.Config.TRUE_VALUE;
                        break;
                    }
                case Entity.Constants.Config.DEBUGTEST_TEST_CUSTOMERNUMBER:
                    {
                        to.TestCustomerNumber = from.Value;
                        break;
                    }
                case Entity.Constants.Config.DEBUGTEST_TEST_USERNAME:
                    {
                        to.TestUserName = from.Value;
                        break;
                    }
                case Entity.Constants.Config.DEBUGTEST_TEST_IMPERSONATING_USERNAME:
                    {
                        to.TestImpersonatingUserName = from.Value;
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

        private void SetEmailConfig(Entity.Entities.AppConfig from, Entity.Entities.EmailConfig to)
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

        private void SetSytemConfig(Entity.Entities.AppConfig from, Entity.Entities.SystemConfig to)
        {

            switch (from.Code)
            {
                case Entity.Constants.Config.APPLICATION_UI_LOCALE:
                    {
                        to.Locale = from.Value;
                        break;
                    }
                case Entity.Constants.Config.APPLICATION_LOG_DATABASEQUERIES:
                    {
                        to.LogDatabaseQueries = from.Value == Entity.Constants.Config.TRUE_VALUE;
                        break;
                    }
                case Entity.Constants.Config.APPLICATION_PAGINATION_RECORDSPERPAGE:
                    {
                        to.Pagination_RecordsPerPage = int.Parse(from.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    }
                case Entity.Constants.Config.APPLICATION_PAGINATION_MAXIMUM_RECORDSPERPAGE:
                    {
                        to.Pagination_MaxRecordsPerPage = int.Parse(from.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    }
                case Entity.Constants.Config.APPLICATION_PAGINATION_TOTALPAGESTOJUMP:
                    {
                        to.Pagination_TotalPagesToJump = int.Parse(from.Value,System.Globalization.CultureInfo.InvariantCulture);
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
                case Entity.Constants.Config.APPLICATION_MAXSECONDSTO_RETAINDOWNLOADFILES:
                    {
                        to.MaxSecondsTo_RetainDownloadFiles = int.Parse(from.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    }
                case Entity.Constants.Config.APPLICATION_SESSIONEXPIRESINMINUTES:
                    {
                        to.SessionExpiresInMinutes = System.Convert.ToInt32(from.Value);
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
                case Entity.Constants.Config.APPLICATION_ADMINURL:
                    {
                        to.AdminURL = from.Value;
                        break;
                    }
                case Entity.Constants.Config.APPLICATION_ADMINURL_INTERNAL:
                    {
                        to.AdminURL_Internal = from.Value;
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
                case Entity.Constants.Config.CONFIGTIMESTAMP_CONFIGSETTING:
                    {
                        to.ConfigSetting = DateTime.Parse(from.Value,System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    }
                case Entity.Constants.Config.CONFIGTIMESTAMP_LANGUAGETEXT:
                    {
                        to.LanguagetText = DateTime.Parse(from.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    }
                case Entity.Constants.Config.CONFIGTIMESTAMP_APPLICATION:
                    {
                        to.Application = DateTime.Parse(from.Value, System.Globalization.CultureInfo.InvariantCulture);
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
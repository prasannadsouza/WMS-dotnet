﻿using Microsoft.Extensions.DependencyInjection;
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
        private ILogger _logger;
        private CacheService _cacheService;
        public ConfigService(Utility.Configuration configuration) : base(configuration)
        {
            _logger = configuration.ServiceProvider.GetRequiredService<ILogger<ConfigService>>();
            _repoService = GetBusinessService<RepoService>();
            _cacheService = GetBusinessService<CacheService>();
        }

        public Entity.Entities.Config.ConfigSetting GetConfigSetting()
        {
            var setting = new Entity.Entities.Config.ConfigSetting();
            setting.Application = GetApplication();
            setting.Pagination = GetPagination();
            setting.DebugTest = GetDebugTest();
            setting.Email = GetEmail();
            setting.JwtToken = GetJwtToken();
            return setting;
        }

        private Entity.Entities.AppConfigGroup GetAppConfigGroup(string code)
        {
            return _repoService.Get(new Entity.Filter.AppConfigGroup { Code = code }).Data.FirstOrDefault();
        }

        private Entity.Entities.Config.DebugTest GetDebugTest()
        {
            var key = Entity.Constants.Cache.CONFIGSETTING_DEBUGTEST;

            var to = CacheUtility.GetFromCache<Entity.Entities.Config.DebugTest>(key, out bool isCached);
            if (isCached && _cacheService.IsCacheChanged(key) == false) return to;

            to = new Entity.Entities.Config.DebugTest();

            var appConfigGroup = GetAppConfigGroup(Entity.Constants.Config.GROUP_DEBUGTEST);
            var filter = new Entity.Filter.AppConfig { AppConfigGroup = new Entity.Filter.AppConfigGroup { Id = appConfigGroup.Id, }, };

            do
            {
                var items = _repoService.Get(filter).Data;
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
                                    Method = nameof(GetDebugTest),
                                    AppConfigId= from.Id,
                                };

                                _logger.LogError($"AppConfig {from.Code} is not handled", new { LogInfo = loginfo });
                                break;
                            }
                    }
                }
                filter.Pagination.CurrentPage++;
            }
            while (filter.Pagination.CurrentPage <= filter.Pagination.TotalPages);
            Configuration.Setting.DebugTest = to;
            CacheUtility.SaveToCache(key, to, true);
            return to;
        }
        
        
        private Entity.Entities.Config.Email GetEmail()
        {
            var key = Entity.Constants.Cache.CONFIGSETTING_EMAIL;

            var to = CacheUtility.GetFromCache<Entity.Entities.Config.Email>(key, out bool isCached);
            if (isCached && _cacheService.IsCacheChanged(key) == false) return to;

            to = new Entity.Entities.Config.Email();
            var appConfigGroup = GetAppConfigGroup(Entity.Constants.Config.GROUP_EMAIL);

            var filter = new Entity.Filter.AppConfig
            {
                AppConfigGroup = new Entity.Filter.AppConfigGroup
                {
                    Id = appConfigGroup.Id,
                },
            };

            do
            {
                var items = _repoService.Get(filter).Data;
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
                                    Method = nameof(GetEmail),
                                    AppConfigId = from.Id,
                                };

                                _logger.LogError($"AppConfig {from.Code} is not handled", new { LogInfo = loginfo });
                                break;
                            }
                    }
                }
                filter.Pagination.CurrentPage++;
            }
            while (filter.Pagination.CurrentPage <= filter.Pagination.TotalPages);
            Configuration.Setting.Email = to;
            CacheUtility.SaveToCache(key, to, true);
            return to;
        }

        private Entity.Entities.Config.Application GetApplication()
        {
            var key = Entity.Constants.Cache.CONFIGSETTING_APPLICATION;

            var to = CacheUtility.GetFromCache<Entity.Entities.Config.Application>(key, out bool isCached);
            if (isCached && _cacheService.IsCacheChanged(key) == false) return to;

            to = new Entity.Entities.Config.Application();
            var appConfigGroup = GetAppConfigGroup(Entity.Constants.Config.GROUP_APPLICATION);

            var filter = new Entity.Filter.AppConfig
            {
                AppConfigGroup = new Entity.Filter.AppConfigGroup
                {
                    Id = appConfigGroup.Id,
                },
            };

            do
            {
                var items = _repoService.Get(filter).Data;
                foreach (var from in items)
                {
                    switch (from.Code)
                    {
                        case Entity.Constants.Config.APPLICATION_LOCALE:
                            {
                                to.LocaleCode = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.APPLICATION_UI_LOCALE:
                            {
                                to.UILocaleCode = from.Value;
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
                        case Entity.Constants.Config.APPLICATION_SYSTEMUSER_CODE:
                            {
                                to.SystemUserCode = from.Value;
                                break;
                            }
                        default:
                            {
                                var loginfo = new
                                {
                                    SesssionId = Configuration.Setting.Application.SessionId,
                                    Method = nameof(GetApplication),
                                    AppConfigId = from.Id,
                                };

                                _logger.LogError($"AppConfig {from.Code} is not handled", new { LogInfo = loginfo });
                                break;
                            }
                    }
                }
                filter.Pagination.CurrentPage++;
            }
            while (filter.Pagination.CurrentPage <= filter.Pagination.TotalPages);
            Configuration.Setting.Application = to;
            CacheUtility.SaveToCache(key, to, true);
            return to;
        }

        private Entity.Entities.Config.JwtToken GetJwtToken()
        {
            var key = Entity.Constants.Cache.CONFIGSETTING_JWTTOKEN;

            var to = CacheUtility.GetFromCache<Entity.Entities.Config.JwtToken>(key, out bool isCached);
            if (isCached && _cacheService.IsCacheChanged(key) == false) return to;

            to = new Entity.Entities.Config.JwtToken();
            var appConfigGroup = GetAppConfigGroup(Entity.Constants.Config.GROUP_JWTTOKEN);

            var filter = new Entity.Filter.AppConfig
            {
                AppConfigGroup = new Entity.Filter.AppConfigGroup
                {
                    Id = appConfigGroup.Id,
                },
            };


            do
            {
                var items = _repoService.Get(filter).Data;
                foreach (var from in items)
                {
                    switch (from.Code)
                    {
                        case Entity.Constants.Config.JWTTOKEN_ISSUER:
                            {
                                to.Issuer = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.JWTTOKEN_SECURITYKEY:
                            {
                                to.SecurityKey = from.Value;
                                break;
                            }
                        case Entity.Constants.Config.JWTTOKEN_VALIDITYINMINUTES:
                            {
                                to.ValiditiyInMinutes = int.Parse(from.Value);
                                break;
                            }
                        case Entity.Constants.Config.JWTTOKEN_MAXRENEWALS:
                            {
                                to.MaxRenewals = int.Parse(from.Value);
                                break;
                            }
                        case Entity.Constants.Config.JWTTOKEN_MAXIDLETIMEINMINUTES:
                            {
                                to.MaxIdleTimeInMinutes = int.Parse(from.Value);
                                break;
                            }
                        default:
                            {
                                var loginfo = new
                                {
                                    SesssionId = Configuration.Setting.Application.SessionId,
                                    Method = nameof(GetJwtToken),
                                    AppConfigId = from.Id,
                                };

                                _logger.LogError($"AppConfig {from.Code} is not handled", new { LogInfo = loginfo });
                                break;
                            }
                    }
                }
                filter.Pagination.CurrentPage++;
            }
            while (filter.Pagination.CurrentPage <= filter.Pagination.TotalPages);
            Configuration.Setting.JwtToken = to;
            CacheUtility.SaveToCache(key, to, true);
            return to;
        }


        private Entity.Entities.Config.Pagination GetPagination()
        {
            var key = Entity.Constants.Cache.CONFIGSETTING_PAGINATION;

            var to = CacheUtility.GetFromCache<Entity.Entities.Config.Pagination>(key, out bool isCached);
            if (isCached && _cacheService.IsCacheChanged(key) == false) return to;

            to = new Entity.Entities.Config.Pagination();
            var appConfigGroup = GetAppConfigGroup(Entity.Constants.Config.GROUP_PAGINATION);

            var filter = new Entity.Filter.AppConfig
            {
                AppConfigGroup = new Entity.Filter.AppConfigGroup
                {
                    Id = appConfigGroup.Id,
                },
            };

            do
            {
                var items = _repoService.Get(filter).Data;
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
                                    Method = nameof(GetPagination),
                                    AppConfigId = from.Id,
                                };

                                _logger.LogError($"AppConfig {from.Code} is not handled", new { LogInfo = loginfo });
                                break;
                            }
                    }
                }
                filter.Pagination.CurrentPage++;
            }
            while (filter.Pagination.CurrentPage <= filter.Pagination.TotalPages);
            Configuration.Setting.Pagination = to;
            CacheUtility.SaveToCache(key, to, true);
            return to;
        }
    }
}

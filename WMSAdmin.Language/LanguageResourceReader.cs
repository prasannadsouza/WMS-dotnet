using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Language
{
    public class LanguageResourceReader : IResourceReader
    {
       
        private string _cultureCode;
        private string _languageGroupCode;
        private Utility.Configuration _configuration;
        private readonly ILogger _logger;
        private Utility.Cache _cacheUtility;
        
        public LanguageResourceReader(Utility.Configuration configuration, CultureInfo culture, string languageGroupCode)
        {
            _cultureCode = culture.Name;
            _languageGroupCode = languageGroupCode;
            _configuration = configuration;
            _logger = configuration.ServiceProvider.GetRequiredService<ILogger<LanguageResourceReader>>();
            _cacheUtility = new Utility.Cache(configuration); 
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            var cultureCode = string.IsNullOrWhiteSpace(_cultureCode) ? _configuration.Setting.Application.Locale : _cultureCode;
            var key = $"{Entity.Constants.Cache.CONFIGSETTING_LANGUAGETEXT}_{_configuration.Setting.Application.AppCode}_{_languageGroupCode}_{cultureCode}";
            var cachedValue = _cacheUtility.GetFromCache<Hashtable>(key, out bool isCached);

            if (isCached && IsCacheChanged(key) == false) return cachedValue.GetEnumerator();

            var filter = new Entity.Filter.LanguageText
            {
                LanguageGroup = new Entity.Filter.LanguageGroup
                {
                    Code = _languageGroupCode,
                    WMSApplication = new Entity.Filter.WMSApplication { Code = _configuration.Setting.Application.AppCode },
                },
                LanguageCulture = new Entity.Filter.LanguageCulture { Code = cultureCode },

                Pagination = new Entity.Entities.Pagination
                {
                    CurrentPage = 1,
                    RecordsPerPage = _configuration.Setting.Pagination.MaxRecordsAllowedPerPage,
                }
            };

            var loginfo = new
            {
                SesssionId = _configuration.Setting.Application.SessionId,
                Method = "GetEnumerator",
                CultureCode = cultureCode,
                LanguageGroupCode = _languageGroupCode,
            };

            cachedValue = new Hashtable();

            var repo = new Repository.LanguageText(_configuration);

            do
            {
                var response = repo.Get(filter);

                foreach (var item in response.Data)
                {
                    if (cachedValue.ContainsKey(item.Code))
                    {
                        _logger.LogError("Duplicate Language Code Encountered", new { LogInfo = loginfo, LanguageTextCode = item.Code });
                    }
                    else
                    {
                        cachedValue.Add(item.Code, item.Value);
                    }
                }

                filter.Pagination.CurrentPage++;
            }
            while (filter.Pagination.CurrentPage <= filter.Pagination.TotalPages);

            _cacheUtility.SaveToCache(key, cachedValue);
            return cachedValue.GetEnumerator();
        }

        public void Close()
        {
        }

        public void Dispose()
        {
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    
        public DateTime GetTimeStamp(string code)
        {
            var repo = new Repository.AppConfig(_configuration);
            var data = repo.Get(new Entity.Filter.AppConfig
            {
                Code = code,
                AppConfigGroup = new Entity.Filter.AppConfigGroup
                {
                    Code = Entity.Constants.Config.GROUP_CONFIGTIMESTAMP
                },
            }).Data.FirstOrDefault();

            if (data == null) return DateTime.MinValue;
            return System.Text.Json.JsonSerializer.Deserialize<DateTime>(data.Value);
        }

        public bool IsCacheChanged(string cacheKey)
        {
            var lastCachedTime = _cacheUtility.GetCachedTime(cacheKey);
            if (lastCachedTime == null) return true;
            var lastChangedTime = GetTimeStamp(cacheKey);
            return lastChangedTime > lastCachedTime;
        }
    }
}

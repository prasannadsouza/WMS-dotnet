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
        private string? _className;
        private string _cultureCode;
        private string _languageGroupCode;
        private Utility.Configuration _configuration;
        private string ClassName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_className) == false) return _className;
                _className = this.GetType().FullName;
                return _className!;
            }
        }

        public LanguageResourceReader(Utility.Configuration configuration, CultureInfo culture, string languageGroupCode)
        {
            _cultureCode = culture.Name;
            _languageGroupCode = languageGroupCode;
            _configuration = configuration;
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            var _memoryCache = _configuration.ServiceProvider.GetRequiredService<IMemoryCache>();
            var cultureCode = string.IsNullOrWhiteSpace(_cultureCode) ? _configuration.Setting.Application.Locale : _cultureCode;
            var key = $"{Entity.Constants.Cache.CONFIGSETTING_LANGUAGETEXT}_{_languageGroupCode}_{cultureCode}_{_configuration.Setting.Application.AppCode}";
            var isCached = _memoryCache.TryGetValue(key, out Hashtable cachedValue);
            if (isCached) return cachedValue.GetEnumerator();
            
            
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
                Class = ClassName,
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
                        _configuration.Logger.LogError("Duplicate Language Code Encountered", new { LogInfo = loginfo, LanguageTextCode = item.Code });
                    }
                    else
                    {
                        cachedValue.Add(item.Code, item.Value);
                    }
                }

                filter.Pagination.CurrentPage++;
            }
            while (filter.Pagination.CurrentPage <= filter.Pagination.TotalPages);

            _memoryCache.Set(key, cachedValue);
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
    }
}

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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WMSAdmin.Language.ResourceManager
{
    public class BaseResourceManager : System.Resources.ResourceManager
    {
        private string? _className = null;
        private string? ClassName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_className) == false) return _className;
                _className = this.GetType().FullName;
                return _className;
            }
        }
        
        public CultureInfo Culture { get; set; }

        private Hashtable _resources;
        private string _languageGroupCode { get; set; }
        protected IMemoryCache MemoryCache { get; private set; }
        protected ILogger Logger { get; private set; }
        protected Utility.Configuration Configuration { get; private set; }

        internal BaseResourceManager(Utility.Configuration configuration,CultureInfo culture, string languageGroupCode)
        {
            Culture = culture;
            _languageGroupCode = languageGroupCode;
            _resources = new Hashtable();
            Configuration = configuration;
            MemoryCache = configuration.ServiceProvider.GetRequiredService<IMemoryCache>();
            Logger = configuration.Logger;
        }

        protected override ResourceSet? InternalGetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
        {
            LanguageResourceSet? resourceSet = null;

            // Check the resource set cache.
            if (this._resources.Contains(culture.Name))
            {
                resourceSet = this._resources[culture.Name] as LanguageResourceSet;
            }
            else
            {
                // Create a new resource set
                resourceSet = new LanguageResourceSet(Configuration, culture, _languageGroupCode);
                var enu = resourceSet.GetEnumerator() as IDictionaryEnumerator;

                var hasResource = enu.MoveNext();

                if (hasResource == false)
                {
                    // Try the parent culture if not already at the invariant culture.
                    if (tryParents)
                    {
                        if (culture.Equals(CultureInfo.InvariantCulture))
                        {
                            throw new MissingManifestResourceException();
                        }

                        // Do a recursive call on this method with the parent culture
                        resourceSet = this.InternalGetResourceSet(culture.Parent, createIfNotExists, tryParents) as LanguageResourceSet;
                    }
                }
                else
                {
                    // Only cache the resource if the createIfNotExists flag is set.
                    if (createIfNotExists)
                    {
                        this._resources.Add(culture.Name, resourceSet);
                    }
                }
            }
            return resourceSet;
        }

        protected string GetResourceString(string code)
        {

            var logdata = new
            {
                SesssionId = Configuration.Setting.Application.SessionId,
                Class = ClassName,
                Method = "GetResourceString",
                LanguageGroupCode = _languageGroupCode,
                Resource = code,
            };

            var languageText = string.Empty;
            try
            {
                languageText = GetString(code, Culture);
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (MissingManifestResourceException ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
            }
            catch (Exception ex)
            {
                Configuration.Logger.LogError(ex, $"Error", logdata);
            }

            if (string.IsNullOrWhiteSpace(languageText) == false) return languageText;
            return $"{_languageGroupCode}.{code}";
        }

    }
}

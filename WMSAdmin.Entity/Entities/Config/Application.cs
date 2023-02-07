using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities.Config
{
    public class Application
    {
        public string LocaleCode { get; set; }
        public string UILocaleCode { get; set; }
        public string ApplicationTitle { get; set; }
        public string SessionId { get; set; }
        public string CurrentVersion { get; set; }
        public bool LogDatabaseQueries { get; set; }
        public string AppCode { get; set; }
        public string WebfolderDownloadPath { get; set; }
        public string LocalFilesBasePath { get; set; }
        public string TemplateFilesPath { get; set; }
        public string AppAPIKey { get; set; }
        public string BaseUrl { get; set; }
        public int CacheExpiryInMinutes { get; set; }
        public string SystemUserCode { get; set; }
        public string JWTKey { get; set; }
        public string JWTIssuer { get; set; }
        public int JWTValiditiyInMinutes { get; set; }


    }
}

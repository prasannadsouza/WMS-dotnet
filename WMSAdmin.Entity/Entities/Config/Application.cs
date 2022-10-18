using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities.Config
{
    public class Application
    {
        public string Locale { get; set; }
        public string UILocale { get; set; }
        public string ApplicationTitle { get; set; }
        public string SessionId { get; set; }
        public string CurrentVersion { get; set; }
        public bool LogDatabaseQueries { get; set; }
        public string AppCode { get; set; }
        public string WebfolderDownloadPath { get; set; }
        public string LocalFilesBasePath { get; set; }
        public string TemplateFilesPath { get; set; }
        public string AppAPIKey { get; set; }
        public DateTime ConfigTimeStamp { get; set; }
    }
}

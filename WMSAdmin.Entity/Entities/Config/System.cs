using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities.Config
{
    public class System
    {
        public string Locale { get; set; }
        public string UILocale { get; set; }
        public string ApplicationTitle { get; set; }
        public string SessionId { get; set; }
        public string CurrentVersion { get; set; }
        public int Pagination_RecordsPerPage { get; set; }
        public int Pagination_MaxRecordsPerPage { get; set; }
        public int Pagination_TotalPagesToJump { get; set; }
        public bool LogDatabaseQueries { get; set; }
        public string AppCode { get; set; }
        public string WebfolderDownloadPath { get; set; }
        public string LocalFilesBasePath { get; set; }
        public string TemplateFilesPath { get; set; }
        public string AdminURL { get; set; }
        public string AdminURL_Internal { get; set; }
        public string AppAPIKey { get; set; }
        public int MaxSecondsTo_RetainDownloadFiles { get; set; }
        public int SessionExpiresInMinutes { get; set; }

    }
}

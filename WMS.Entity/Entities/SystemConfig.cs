using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.Entities
{
    public class SystemConfig
    {
        public string Locale { get; set; }
        public string UILocale { get; set; }
        public string ApplicationTitle { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string CurrentVersion { get; set; }
        public int Pagination_RecordsPerPage { get; set; }
        public int Pagination_MaxRecordsPerPage { get; set; }
        public int Pagination_TotalPagesToJump { get; set; }
    }
}

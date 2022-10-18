using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities.Config
{
    public class Pagination
    {
        public int RecordsPerPage { get; set; }
        public int MaxRecordsPerPage { get; set; }
        public int MaxRecordsAllowedPerPage { get; set; }
        public int TotalPagesToJump { get; set; }
    }
}

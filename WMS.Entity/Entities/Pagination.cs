using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.Entities
{
    public class Pagination
    {
        public int RecordsPerPage { get; set; }

        public int CurrentPage { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }

        public List<Sort> SortFields { get; set; }
    }
}

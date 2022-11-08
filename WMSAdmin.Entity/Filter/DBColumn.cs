using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Filter
{
    public class DBColumn
    {
        public string Table_Name { get; set; }
        public Entities.Pagination Pagination { get; set; }
    }
}

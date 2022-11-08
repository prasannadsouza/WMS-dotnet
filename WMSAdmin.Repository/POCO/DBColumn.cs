using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Repository.POCO
{
    public class DBColumn
    {
        public string Table_Name { get; set; }
        public string Column_Name { get; set; }
        public bool? Is_Nullable { get; set; }
    }
}

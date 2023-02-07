using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Filter
{
    public class AppUser
    {
        public long? Id { get; set; }
        public Entities.Pagination Pagination { get; set; }
    }
}

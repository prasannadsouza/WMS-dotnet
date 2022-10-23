using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Filter
{
    public class EntityFilter<T>
    {
        public List<T> AndFilters { get; set; }
        public List<T> OrFilters { get; set; }
        public Entities.Pagination Pagination { get; set; }
    }
}

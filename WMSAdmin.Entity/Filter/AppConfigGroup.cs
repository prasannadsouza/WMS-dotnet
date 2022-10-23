using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Filter
{
    public class AppConfigGroup
    {
        public long? Id { get; set; }
        public List<long> Ids { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? FromTimeStamp { get; set; }
        public DateTime? ToTimeStamp { get; set; }
        public Entities.Pagination Pagination { get; set; }
    }
}

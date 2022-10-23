using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Filter
{
    public class LanguageGroup
    {
        public long? Id { get; set; }
        public List<long> Ids { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? FromTimeStamp { get; set; }
        public DateTime? ToTimeStamp { get; set; }
        public WMSApplication WMSApplication { get; set; }
        public Entities.Pagination Pagination { get; set; }
    }
}

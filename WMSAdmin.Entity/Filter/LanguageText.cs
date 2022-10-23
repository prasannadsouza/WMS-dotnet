using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Filter
{
    public class LanguageText
    {
        public long? Id { get; set; }
        public List<long> Ids { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public DateTime? FromTimeStamp { get; set; }
        public DateTime? ToTimeStamp { get; set; }
        public WMSApplication WMSApplication { get; set; }
        public LanguageGroup LanguageGroup { get; set; }
        public LanguageCulture LanguageCulture { get; set; }
        public Entities.Pagination Pagination { get; set; }
    }
}

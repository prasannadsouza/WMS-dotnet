using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Repository.POCO
{
    public class LanguageText
    {
        public long? Id { get; set; }
        public long? LanguageGroupId { get; set; }
        public long? LanguageCultureId { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public DateTime? TimeStamp { get; set; }
    }

}

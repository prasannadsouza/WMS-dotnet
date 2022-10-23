using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Repository.POCO
{
    public class LanguageGroup
    {
        public long? Id { get; set; }
        public long? WMSApplicationId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}

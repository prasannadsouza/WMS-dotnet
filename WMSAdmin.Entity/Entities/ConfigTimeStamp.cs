using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities
{
    public class ConfigTimeStamp
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}

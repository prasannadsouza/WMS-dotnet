using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities.Config
{
    public class Timestamp
    {
        public DateTime Application { get; set; }
        public DateTime LanguagetText { get; set; }
        public DateTime Email { get; set; }
        public DateTime Pagination { get; set; }
        public DateTime DebugTest { get; set; }
    }
}

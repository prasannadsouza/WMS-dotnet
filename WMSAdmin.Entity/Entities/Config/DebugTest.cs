using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities.Config
{
    public class DebugTest
    {
        public bool IsTestMode { get; set; }
        public bool DevAutoLogin { get; set; }
        public string UserName { get; set; }
        public string ImpersonatingUserName { get; set; }
        public string CustomerNumber { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.Entities
{
    public class ConfigSetting
    {
        public SystemConfig System { get; set; }
        public EmailConfig Email { get; set; }
        public DebugTestConfig DebugTest { get; set; }
    }
}

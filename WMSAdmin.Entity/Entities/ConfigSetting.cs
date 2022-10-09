using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities
{
    public class ConfigSetting
    {
        public SystemConfig System { get; set; }
        public EmailConfig Email { get; set; }
        public DebugTestConfig DebugTest { get; set; }
        public ConfigTimestamp ConfigTimestamp { get; set; }
    }
}

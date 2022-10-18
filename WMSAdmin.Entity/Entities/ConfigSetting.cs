using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities
{
    public class ConfigSetting
    {
        public Config.Application System { get; set; }
        public Config.Email Email { get; set; }
        public Config.DebugTest DebugTest { get; set; }
        public ConfigTimestamp ConfigTimestamp { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities.Config.Client
{
    public class Application
    {
        public string LocaleCode { get; set; }
        public string UILocaleCode { get; set; }
        public string ApplicationTitle { get; set; }
        public string CurrentVersion { get; set; }
        public string BaseUrl { get; set; }
    }
}

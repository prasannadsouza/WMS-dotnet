using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSAdmin.Entity.Entities;

namespace WMSAdmin.Repository
{
    public class RepoConfiguration
    {
        public ConfigSetting Setting { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
        public ILogger Logger { get; set; }
    }
}

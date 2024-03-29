﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Utility
{
    public class Configuration
    {
        public Entity.Entities.Config.ConfigSetting Setting { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
        //public ILogger Logger { get; set; }
        public System.Globalization.CultureInfo Culture { get; set; }
    }
}

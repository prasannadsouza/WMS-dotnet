﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities.Config
{
    public class ConfigSetting
    {
        public Application System { get; set; }
        public Email Email { get; set; }
        public DebugTest DebugTest { get; set; }
        public Timestamp ConfigTimestamp { get; set; }
        public Pagination Pagination { get; set; }
    }
}
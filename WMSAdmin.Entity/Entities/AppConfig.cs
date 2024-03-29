﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities
{
    public class AppConfig
    {
        public long? Id { get; set; }
        public long? AppConfigGroupId { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}

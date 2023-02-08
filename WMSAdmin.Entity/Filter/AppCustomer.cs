﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Filter
{
    public class AppCustomer
    {
        public long? Id { get; set; }
        public List<long> Ids { get; set; }
        public string CustomerNumber { get; set; }
        public Entities.Pagination Pagination { get; set; }
    }
}

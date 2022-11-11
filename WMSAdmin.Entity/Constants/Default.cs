﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Constants
{
    public static class Default
    {
        public const int CacheExpiryInMinutes = 60;
        public const int Pagination_RecordsPerpage = 15;
        public const int Pagination_Max_RecordsPerpage = 250;
        public const int Pagination_Max_AllowedRecordsPerpage = 2500;
        public const string APPLICATION_CACHEKEYS = "APPLICATION_CACHEKEYS";
    }
}

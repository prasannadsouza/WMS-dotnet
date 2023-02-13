using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Utility
{
    public class AppHelper
    {
        public static Entity.Entities.Config.Pagination GetDefaultPaginationConfig()
        {
            return new Entity.Entities.Config.Pagination
            {
                MaxRecordsPerPage = Entity.Constants.Default.Pagination_Max_RecordsPerpage,
                RecordsPerPage = Entity.Constants.Default.Pagination_Max_RecordsPerpage,
                MaxRecordsAllowedPerPage = Entity.Constants.Default.Pagination_Max_AllowedRecordsPerpage,
            };
        }

        public static Entity.Entities.Config.Application GetDefaultApplicationConfig(string sessionId = null)
        {
            if (string.IsNullOrWhiteSpace(sessionId)) sessionId = $"{DateTime.UtcNow.Ticks}";
            return new Entity.Entities.Config.Application
            {
                SessionId = sessionId,
                CacheExpiryInMinutes = Entity.Constants.Default.CacheExpiryInMinutes,
            };
        }

        public static Entity.Entities.Config.ConfigSetting GetDefaultConfigSetting(string sessionId = null)
        {
            return new Entity.Entities.Config.ConfigSetting
            {
                Application = GetDefaultApplicationConfig(sessionId),
                Pagination = GetDefaultPaginationConfig(),
            };
        }

        public static Entity.Entities.Pagination GetDefaultPagination(Entity.Entities.Config.ConfigSetting setting, bool forMaxRecordsPerPage = false)
        {
            var recordsPerPage = forMaxRecordsPerPage ? setting.Pagination.MaxRecordsAllowedPerPage : setting.Pagination.RecordsPerPage;
            return new Entity.Entities.Pagination
            {
                CurrentPage = 1,
                RecordsPerPage = recordsPerPage,
                SortFields = new List<Entity.Entities.Sort> {
                    new Entity.Entities.Sort {
                        SortColumn = Entity.Constants.Sort.Id,
                    }
                }
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Filter
{
    public class AppUserRefreshToken
    {
        public long? Id { get; set; }
        public List<long> Ids { get; set; }
        public long? AppUserId { get; set; }
        public Guid? SessionKey { get; set; }
        public Guid? RefreshToken { get; set; }
        public DateTime? FromExpiryTime { get; set; }
        public DateTime? ToExpiryTime { get; set; }
        public Entities.Pagination Pagination { get; set; }
    }
}

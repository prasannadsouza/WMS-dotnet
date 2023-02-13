using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities
{
    public class AppUserRefreshToken
    {
        public long? Id { get; set; }
        public long? AppUserId { get; set; }
        public DateTime? IssuedTime { get; set; }
        public DateTime? ExpiryTime { get; set; }
        public Guid? SessionKey { get; set; }
        public Guid? RefreshToken { get; set; }
        public DateTime? LastAccessedTime { get; set; }
        public int? TotalRenewals { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}

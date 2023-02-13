using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities
{
    public class AppUser
    {
        public long? Id { get; set; }
        public long? AppCustomerId { get; set; }
        public long? AppUserTypeId { get; set; }
        public string AuthId { get; set; }
        public string AuthSecret { get; set; }
        public Guid? SecretKey { get; set; }
        public string DisplayName { get; set; }
        public string Locale { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}

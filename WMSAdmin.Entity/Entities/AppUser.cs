using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities
{
    public class AppLogin
    {
        public long? Id { get; set; }
        public string LoginId { get; set; }
        public string LoginSecret { get; set; }
        public Guid? SecretKey { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? ValidTill { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}

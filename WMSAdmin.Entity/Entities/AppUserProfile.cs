using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities
{
    public class AppUserProfile
    {
        public AppUser AppUser { get; set; }
        public AppCustomer AppCustomer { get; set; }
        public AppUserRefreshToken AppUserRefreshToken { get; set; }
    }
}

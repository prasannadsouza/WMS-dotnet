using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities.Config
{
    public class JwtToken
    {
        public string SecurityKey { get; set; }
        public string Issuer { get; set; }
        public int ValiditiyInMinutes { get; set; }
        public int MaxRenewals { get; set; }
        public int MaxIdleTimeInMinutes { get; set; }
    }
}

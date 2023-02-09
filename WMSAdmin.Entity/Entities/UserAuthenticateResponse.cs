using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities
{
    public class AuthenticationResponse
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string DisplayName { get; set; }
        public Guid? RefreshToken { get; set; }
        public string JwtToken { get; set; }
    }
}

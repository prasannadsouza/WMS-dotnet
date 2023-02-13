using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.BusinessService.Model
{
    public class AuthenticateAppUserRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AppUserType { get; set; }
    }

    public class AuthenticateAppUserResponse
    {
        public Entity.Entities.AppUserProfile AppUserProfile { get; set; }
        public Guid? RefreshToken { get; set; }
        public string JwtTokenValue { get; set; }
        public List<Entity.Entities.Error> Errors { get; set; }
    }
}

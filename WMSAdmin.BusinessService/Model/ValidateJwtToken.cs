using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.BusinessService.Model
{
    public class ValidateJwtTokenRequest
    {
        public string JwtTokenValue { get; set; }
        public Guid? RefreshToken { get; set; }
        public string AppAccessType { get; set; }
        public bool RefreshOnExpiry { get; set; }
    }

    public class ValidateJwtTokenResponse
    {
        public string JwtTokenValue { get; set; }
        public Guid? RefreshToken { get; set; }
        public Entity.Entities.AppUserProfile AppUserProfile { get; set; }
        public bool IsRefreshed { get; set; }
        public bool IsValid { get; set; }
        public List<Entity.Entities.Error> Errors { get; set; }
    }
}

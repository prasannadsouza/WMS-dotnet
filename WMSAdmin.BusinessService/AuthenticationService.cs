using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.BusinessService
{
    public class AuthenticationService : BaseService
    {
        RepoService _repoService;
        private ILogger _logger;
        public AuthenticationService(Utility.Configuration configuration) : base(configuration)
        {
            _logger = configuration.ServiceProvider.GetRequiredService<ILogger<AuthenticationService>>();
            _repoService = GetBusinessService<RepoService>();
        }

        public Entity.Entities.Response<Entity.Entities.UserAuthenticateResponse> AuthenticateAppUser(Entity.Entities.UserAuthenticateRequest model)
        {
            var response = new Entity.Entities.Response<Entity.Entities.UserAuthenticateResponse> { Data = new Entity.Entities.UserAuthenticateResponse() };
            response.Data.Token = generateJwtToken(new Entity.Entities.AppUser());
            return response;
        }

        private string generateJwtToken(Entity.Entities.AppUser user)
        {
            var appSetting = Configuration.Setting.Application;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSetting.JWTKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            // Here you  can fill claim information from database for the users as well
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, "atul"),
                new Claim("Id", $"{user.Id}"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(appSetting.JWTIssuer, appSetting.JWTIssuer, claims, expires: DateTime.Now.AddMinutes(appSetting.JWTValiditiyInMinutes), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

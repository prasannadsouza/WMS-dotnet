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

        public Entity.Entities.Response<Entity.Entities.UserAuthenticateResponse> AuthenticateAppCustomerUser(Entity.Entities.UserAuthenticateRequest model)
        {
            var response = new Entity.Entities.Response<Entity.Entities.UserAuthenticateResponse> { Errors = new List<Entity.Entities.Error>() };
            var appUser = _repoService.Get(new Entity.Filter.AppUser { AuthId = model.UserName}).Data.FirstOrDefault();
            var loginStrings = GetResourceManager<Language.ResourceManager.LoginString>();
            var authError = new Entity.Entities.Error {
                ErrorCode = Entity.Constants.ErrorCode.InvalidUserNameOrPassword,
                Message = loginStrings.UsernameOrPasswordIsInvalid
            };

            if (appUser == null || 
                appUser.AuthId == Configuration.Setting.Application.SystemUserCode 
                || appUser.AuthSecret != model.Password) {
                response.Errors.Add(authError);
                return response;
            }
            
            response.Data = new Entity.Entities.UserAuthenticateResponse
            {
                Token = generateJwtToken(new Entity.Entities.AppUser()),
            };

            return response;
        }

        public Entity.Entities.Response<JwtSecurityToken> ValidateJwtToken(string token)
        {
            var appSetting = Configuration.Setting.Application;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSetting.JWTKey));
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = key,
                ValidIssuer = appSetting.JWTIssuer,
                ValidAudience = appSetting.JWTIssuer,
                // set clockskew to zero so tokens expire exactly at token expiration time.
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return new Entity.Entities.Response<JwtSecurityToken>
            {
                Data = (JwtSecurityToken)validatedToken
            };
            
        }

        private string generateJwtToken(Entity.Entities.AppUser appLogin)
        {
            var appSetting = Configuration.Setting.Application;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSetting.JWTKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            // Here you  can fill claim information from database for the users as well
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, "atul"),
                new Claim("Id", $"{appLogin.Id}"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(appSetting.JWTIssuer, appSetting.JWTIssuer, claims, expires: DateTime.Now.AddMinutes(appSetting.JWTValiditiyInMinutes), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

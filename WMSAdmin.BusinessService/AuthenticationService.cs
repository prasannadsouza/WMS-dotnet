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
using WMSAdmin.Repository.Context;

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

        public Entity.Entities.Response<Entity.Entities.AuthenticationResponse> AuthenticateAppUser(Entity.Entities.AuthenticationRequest model)
        {
            var appSetting = Configuration.Setting.Application;
            var response = new Entity.Entities.Response<Entity.Entities.AuthenticationResponse> { Errors = new List<Entity.Entities.Error>() };

            var loginStrings = GetResourceManager<Language.ResourceManager.LoginString>();
            var authError = new Entity.Entities.Error {
                ErrorCode = Entity.Constants.ErrorCode.InvalidCredentials,
                Message = loginStrings.UsernameOrPasswordIsInvalid
            };

            if (string.IsNullOrWhiteSpace(model.UserName) ||
               string.IsNullOrWhiteSpace(model.Password) ||
               string.IsNullOrWhiteSpace(model.AppUserType) ||
               model.AppUserType == Entity.Constants.AppUserType.SYSTEMUSER)
            {
                response.Errors.Add(authError);
                return response;
            }

            var appUser = _repoService.Get(new Entity.Filter.AppUser { AuthId = model.UserName }).Data.FirstOrDefault();
            Entity.Entities.AppUserType appUserType = null;

            if (appUser != null)
            {
                appUserType = _repoService.Get(new Entity.Filter.AppUserType { Id = appUser.AppUserTypeId.Value }).Data.FirstOrDefault();
            }

            if (appUser == null || appUserType == null || appUser.SecretKey == null ||
                appUserType.Code != model.AppUserType ||
                appUser.AuthId == Configuration.Setting.Application.SystemUserCode || 
                string.IsNullOrWhiteSpace(appUser.AuthSecret) ||
                appUser.AuthSecret != model.Password) {
                response.Errors.Add(authError);
                return response;
            }

            var appCustomer = _repoService.Get(new Entity.Filter.AppCustomer { Id = appUser.AppCustomerId.Value }).Data.First();

            var refreshToken = new Entity.Entities.AppUserRefreshToken
            {
                AppUserId = appUser.Id,
                ExpiryTime = DateTime.Now.AddMinutes(appSetting.JWTValiditiyInMinutes),
                RefreshToken = Guid.NewGuid(),
                SessionKey = Guid.NewGuid(),
                TimeStamp = DateTime.Now,
            };

            response.Data = new Entity.Entities.AuthenticationResponse { 
                CustomerName  = appCustomer.CustomerName,
                DisplayName = appUser.DisplayName,
                JwtToken = generateJwtToken(refreshToken),
                RefreshToken = refreshToken.RefreshToken
            };

            using (var scope = new System.Transactions.TransactionScope())
            {
                RepoContext.AppUserRefreshToken.Add(Repository.AppUserRefreshToken.ConvertTo(refreshToken));
                var dbAppUSer = RepoContext.AppUser.First(e => e.Id == appUser.Id.Value);
                dbAppUSer.LoginTime = DateTime.Now;
                RepoContext.SaveChanges();
            }
            return response;
        }

        public Entity.Entities.Response<Entity.Entities.AuthenticationResponse> RefreshJWTToken(Entity.Entities.AppUserRefreshToken appUserRefreshToken)
        {
            var appSetting = Configuration.Setting.Application;
            var response = new Entity.Entities.Response<Entity.Entities.AuthenticationResponse> { Errors = new List<Entity.Entities.Error>() };
            var dbAppUserRefreshToken = RepoContext.AppUserRefreshToken.First(e => e.Id == appUserRefreshToken.Id);
            var appUser = _repoService.Get(new Entity.Filter.AppUser { Id = appUserRefreshToken.AppUserId.Value }).Data.First();
            var appCustomer = _repoService.Get(new Entity.Filter.AppCustomer { Id = appUser.AppCustomerId.Value }).Data.First();

            dbAppUserRefreshToken.SessionKey = Guid.NewGuid();
            dbAppUserRefreshToken.RefreshToken = Guid.NewGuid();
            dbAppUserRefreshToken.ExpiryTime = DateTime.Now.AddMinutes(appSetting.JWTValiditiyInMinutes);
            dbAppUserRefreshToken.TimeStamp = DateTime.Now;
            RepoContext.SaveChanges();

            appUserRefreshToken = Repository.AppUserRefreshToken.ConvertTo(dbAppUserRefreshToken);

            response.Data = new Entity.Entities.AuthenticationResponse
            {
                CustomerName = appCustomer.CustomerName,
                DisplayName = appUser.DisplayName,
                JwtToken = generateJwtToken(appUserRefreshToken),
                RefreshToken = dbAppUserRefreshToken.RefreshToken,
            };

            return response;
        }

        public Entity.Entities.Response<JwtSecurityToken> ValidateJwtToken(string token)
        {
            var appSetting = Configuration.Setting.Application;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSetting.JWTKey));
            var reply = tokenHandler.ValidateToken(token, new TokenValidationParameters
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

        private string generateJwtToken(Entity.Entities.AppUserRefreshToken appUserRefreshToken)
        {
            var appSetting = Configuration.Setting.Application;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSetting.JWTKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            // Here you  can fill claim information from database for the users as well
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, appUserRefreshToken.SessionKey.ToString()),
            };
            var token = new JwtSecurityToken(appSetting.JWTIssuer, appSetting.JWTIssuer, claims, expires: appUserRefreshToken.ExpiryTime, signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

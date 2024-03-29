﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WMSAdmin.Entity.Constants;
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

        public Model.AuthenticateAppUserResponse AuthenticateAppUser(Model.AuthenticateAppUserRequest request)
        {
            var appSetting = Configuration.Setting.JwtToken;
            var response = new Model.AuthenticateAppUserResponse { Errors = new List<Entity.Entities.Error>() };

            var loginStrings = GetResourceManager<Language.ResourceManager.LoginString>();
            var authError = new Entity.Entities.Error {
                ErrorCode = Entity.Constants.ErrorCode.InvalidCredentials,
                Message = loginStrings.UsernameOrPasswordIsInvalid
            };

            if (string.IsNullOrWhiteSpace(request.UserName) ||
               string.IsNullOrWhiteSpace(request.Password) ||
               string.IsNullOrWhiteSpace(request.AppUserType) ||
               request.AppUserType == Entity.Constants.AppUserType.SYSTEMUSER)
            {
                response.Errors.Add(authError);
                return response;
            }

            var appUser = _repoService.Get(new Entity.Filter.AppUser { AuthId = request.UserName }).Data.FirstOrDefault();
            Entity.Entities.AppUserType appUserType = null;

            if (appUser != null)
            {
                appUserType = _repoService.Get(new Entity.Filter.AppUserType { Id = appUser.AppUserTypeId.Value }).Data.FirstOrDefault();
            }

            if (appUser == null || appUserType == null || appUser.SecretKey == null ||
                appUserType.Code != request.AppUserType ||
                appUser.AuthId == Configuration.Setting.Application.SystemUserCode || 
                string.IsNullOrWhiteSpace(appUser.AuthSecret) ||
                appUser.AuthSecret != request.Password) {
                response.Errors.Add(authError);
                return response;
            }

            var appCustomer = _repoService.Get(new Entity.Filter.AppCustomer { Id = appUser.AppCustomerId.Value }).Data.First();

            var utcNow = DateTime.UtcNow;
            var refreshToken = new Entity.Entities.AppUserRefreshToken
            {
                AppUserId = appUser.Id,
                IssuedTime= utcNow,
                ExpiryTime = utcNow.AddMinutes(appSetting.ValiditiyInMinutes),
                RefreshToken = Guid.NewGuid(),
                SessionKey = Guid.NewGuid(),
                LastAccessedTime = utcNow,
                TimeStamp = DateTime.UtcNow,
            };

            response.AppUserProfile = new Entity.Entities.AppUserProfile
            {
                AppUser = appUser,
                AppCustomer = appCustomer,
                AppUserRefreshToken= refreshToken,
            };
            
            response.JwtTokenValue = generateJwtToken(refreshToken);
            response.RefreshToken = refreshToken.RefreshToken;

            using (var scope = new System.Transactions.TransactionScope())
            {
                RepoContext.AppUserRefreshToken.Add(Repository.AppUserRefreshToken.ConvertTo(refreshToken));
                var dbAppUSer = RepoContext.AppUser.First(e => e.Id == appUser.Id.Value);
                dbAppUSer.LoginTime = DateTime.UtcNow;
                RepoContext.SaveChanges();
                scope.Complete();
            }
            return response;
        }

        public Model.ValidateJwtTokenResponse ValidateJwtToken(Model.ValidateJwtTokenRequest request)
        {
            var response = new Model.ValidateJwtTokenResponse { Errors = new List<Entity.Entities.Error>() };
            var tokenHandler = new JwtSecurityTokenHandler();

            var loginStrings = GetResourceManager<Language.ResourceManager.LoginString>();
            var authRevalidationRequiredError = new Entity.Entities.Error
            {
                ErrorCode = Entity.Constants.ErrorCode.AuthRevalidationRequired,
                Message = loginStrings.AuthRevalidationRequired
            };

            var unauthorizedError = new Entity.Entities.Error
            {
                ErrorCode = Entity.Constants.ErrorCode.UnAuthorized,
                Message = loginStrings.UnAuthorized
            };

            Model.ValidateJwtTokenResponse GetErrorResponse(Entity.Entities.Error error)
            { 
                response.Errors.Add(error);
                return response;
            }

            if (string.IsNullOrWhiteSpace(request.JwtTokenValue)) return GetErrorResponse(unauthorizedError);

            if (request.AppAccessType == AppAccessType.WEB)
            {
                if (request.RefreshToken == null) return GetErrorResponse(unauthorizedError);
                if (request.RefreshToken == Guid.Empty) return GetErrorResponse(unauthorizedError);
            }

            if (!tokenHandler.CanReadToken(request.JwtTokenValue)) return GetErrorResponse(unauthorizedError);
            var readToken = tokenHandler.ReadToken(request.JwtTokenValue);

            var appSetting = Configuration.Setting.JwtToken;
            var repoService = GetBusinessService<RepoService>();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSetting.SecurityKey));

            try
            {
                var reply = tokenHandler.ValidateToken(request.JwtTokenValue, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    IssuerSigningKey = key,
                    ValidIssuer = appSetting.Issuer,
                    ValidAudience = appSetting.Issuer,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtSecurityToken = (JwtSecurityToken)validatedToken;

                var sessionKeyClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);
                if (sessionKeyClaim?.Value == null) return GetErrorResponse(unauthorizedError);
                if (Guid.TryParse(sessionKeyClaim.Value, out Guid sessionKey) == false) return GetErrorResponse(unauthorizedError);
                if (sessionKey == Guid.Empty) return GetErrorResponse(unauthorizedError);

                var appUserRefreshToken = repoService.Get(new Entity.Filter.AppUserRefreshToken { SessionKey = sessionKey, RefreshToken = request.RefreshToken }).Data.FirstOrDefault();
                if (appUserRefreshToken == null) return GetErrorResponse(unauthorizedError);

                var appUser = repoService.Get(new Entity.Filter.AppUser { Id = appUserRefreshToken!.AppUserId!.Value }).Data.FirstOrDefault();
                if (appUser == null) return GetErrorResponse(unauthorizedError);

                var appUserType = repoService.Get(new Entity.Filter.AppUserType { Id = appUser!.AppUserTypeId!.Value }).Data.First();

                if (IsAppAccessValid(appUserType.Code, request.AppAccessType) == false) return GetErrorResponse(unauthorizedError);

                var utcNow = DateTime.UtcNow;
                if (appUserRefreshToken.ExpiryTime < utcNow || jwtSecurityToken.ValidTo < utcNow)
                {
                    if ((utcNow - appUserRefreshToken.ExpiryTime).Value.TotalMinutes > appSetting.MaxIdleTimeInMinutes) return GetErrorResponse(authRevalidationRequiredError);
                    if (request.RefreshOnExpiry == false) return GetErrorResponse(authRevalidationRequiredError);
                    if (appUserRefreshToken.TotalRenewals >= appSetting.MaxRenewals) return GetErrorResponse(authRevalidationRequiredError);
                    
                    var refreshResponse = RefreshJwtToken(appUserRefreshToken);
                    response.IsRefreshed = true;
                    response.JwtTokenValue = refreshResponse.JwtTokenValue;
                    response.RefreshToken = refreshResponse.RefreshToken;
                    response.AppUserProfile = refreshResponse.AppUserProfile;
                }
                else
                {
                    UpdateLastAcessedTimeOfRefreshToken(appUserRefreshToken);
                    var appCustomer = _repoService.Get(new Entity.Filter.AppCustomer { Id = appUser.AppCustomerId.Value }).Data.First();
                    response.AppUserProfile = new Entity.Entities.AppUserProfile
                    {
                        AppUser = appUser,
                        AppCustomer = appCustomer,
                        AppUserRefreshToken = appUserRefreshToken,
                    };
                }
                
                return response;
            }
            catch (Exception ex)
            {
                var loginfo = new
                {
                    SesssionId = Configuration.Setting.Application.SessionId,
                    Method = nameof(ValidateJwtToken),
                    JwtSecurityToken = request.JwtTokenValue,
                    ErrorCode = Entity.Constants.ErrorCode.UnableToValidateToken
                };

                var loginString = GetResourceManager<Language.ResourceManager.LoginString>();
                response.Errors.Add(new Entity.Entities.Error
                {
                    ErrorCode = loginfo.ErrorCode,
                    Message = loginString.UnableToValidateToken,
                });
                _logger!.LogError(ex, $"Error Validating JwtToken", new { LogInfo = loginfo });
                return response;
            }

        }

        public void ExpireJWTToken(string jwtTokenValue, string refreshToken)
        {
            var repoService = GetBusinessService<RepoService>();
           

            Guid? GetSessionKey()
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(jwtTokenValue)) return null;
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var appSetting = Configuration.Setting.JwtToken;
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSetting.SecurityKey));
                    var reply = tokenHandler.ValidateToken(jwtTokenValue, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        IssuerSigningKey = key,
                        ValidIssuer = appSetting.Issuer,
                        ValidAudience = appSetting.Issuer,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);
                    var jwtSecurityToken = (JwtSecurityToken)validatedToken;
                    var sessionKeyClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);
                    if (sessionKeyClaim?.Value == null) return null;
                    if (Guid.TryParse(sessionKeyClaim.Value, out Guid sessionKey) == false) return null;
                    if (sessionKey == Guid.Empty) return null;
                    return sessionKey;
                }
                catch(Exception ex)
                {
                    var loginfo = new
                    {
                        SesssionId = Configuration.Setting.Application.SessionId,
                        Method = nameof(ValidateJwtToken),
                        JwtSecurityToken = jwtTokenValue,
                        ErrorCode = Entity.Constants.ErrorCode.UnableToValidateToken
                    };



                    _logger!.LogError(ex, $"Error Validating JwtToken", new { LogInfo = loginfo });
                    return null;
                }
            }

            Guid? GetRefreshToken()
            {
                if (string.IsNullOrWhiteSpace(refreshToken)) return null;
                if (Guid.TryParse(refreshToken, out Guid parsedRefreshToken) == false) return null;
                if (parsedRefreshToken == Guid.Empty) return null;
                return parsedRefreshToken;
            }
           

            var sessionKey = GetSessionKey();
            if (sessionKey != null)
            {
                var dbAppUserRefreshToken = RepoContext.AppUserRefreshToken.FirstOrDefault(e => e.SessionKey == sessionKey);
                if (dbAppUserRefreshToken != null)
                {
                    dbAppUserRefreshToken.ExpiryTime = DateTime.UtcNow;
                    dbAppUserRefreshToken.TimeStamp = DateTime.UtcNow;
                    RepoContext.SaveChanges();
                }
            }

            var refreshTokenToCheck = GetRefreshToken();
            if (refreshTokenToCheck != null)
            {
                var dbAppUserRefreshToken = RepoContext.AppUserRefreshToken.FirstOrDefault(e => e.RefreshToken == refreshTokenToCheck);
                if (dbAppUserRefreshToken != null)
                {
                    dbAppUserRefreshToken.ExpiryTime = DateTime.UtcNow;
                    dbAppUserRefreshToken.TimeStamp = DateTime.UtcNow;
                    RepoContext.SaveChanges();
                }
            }

        }

        private Model.AuthenticateAppUserResponse RefreshJwtToken(Entity.Entities.AppUserRefreshToken appUserRefreshToken)
        {
            var appSetting = Configuration.Setting.JwtToken;
            var response = new Model.AuthenticateAppUserResponse { Errors = new List<Entity.Entities.Error>() };
            var dbAppUserRefreshToken = RepoContext.AppUserRefreshToken.First(e => e.Id == appUserRefreshToken.Id);
            var appUser = _repoService.Get(new Entity.Filter.AppUser { Id = appUserRefreshToken.AppUserId.Value }).Data.First();
            var appCustomer = _repoService.Get(new Entity.Filter.AppCustomer { Id = appUser.AppCustomerId.Value }).Data.First();

            var utcNow = DateTime.UtcNow;
            dbAppUserRefreshToken.SessionKey = Guid.NewGuid();
            dbAppUserRefreshToken.RefreshToken = Guid.NewGuid();
            dbAppUserRefreshToken.ExpiryTime = utcNow.AddMinutes(appSetting.ValiditiyInMinutes);
            dbAppUserRefreshToken.TimeStamp = utcNow;
            dbAppUserRefreshToken.LastAccessedTime = utcNow;
            dbAppUserRefreshToken.TotalRenewals = dbAppUserRefreshToken.TotalRenewals.GetValueOrDefault() + 1;
            RepoContext.SaveChanges();

            appUserRefreshToken = Repository.AppUserRefreshToken.ConvertTo(dbAppUserRefreshToken);

            response.AppUserProfile = new Entity.Entities.AppUserProfile
            {
                AppUser = appUser,
                AppCustomer = appCustomer,
                AppUserRefreshToken = appUserRefreshToken,
            };
            
            response.JwtTokenValue = generateJwtToken(appUserRefreshToken);
            response.RefreshToken = dbAppUserRefreshToken.RefreshToken;

            return response;
        }

        private void UpdateLastAcessedTimeOfRefreshToken(Entity.Entities.AppUserRefreshToken appUserRefreshToken)
        {
            var dbAppUserRefreshToken = RepoContext.AppUserRefreshToken.First(e => e.Id == appUserRefreshToken.Id);

            var utcNow = DateTime.UtcNow;
            dbAppUserRefreshToken.TimeStamp = utcNow;
            dbAppUserRefreshToken.LastAccessedTime = utcNow;
            RepoContext.SaveChanges();
        }

        private bool IsAppAccessValid(string appUserType, string appAccessType)
        {
            if (string.IsNullOrWhiteSpace(appAccessType)) return false;

            switch (appAccessType)
            {
                case Entity.Constants.AppAccessType.API:
                    {
                        return IsAPIAccessValid(appUserType);

                    }
                case Entity.Constants.AppAccessType.WEB:
                    {
                        return IsWebAccessValid(appUserType);

                    }
                default: return false;
            }
        }

        private bool IsAPIAccessValid(string appUserType)
        {
            if (string.IsNullOrWhiteSpace(appUserType)) return false;
            if (appUserType == Entity.Constants.AppUserType.APIUSER) return true;
            return false;
        }

        private bool IsWebAccessValid(string appUserType)
        {
            if (string.IsNullOrWhiteSpace(appUserType)) return false;
            if (appUserType == Entity.Constants.AppUserType.APPUSER) return true;
            if (appUserType == Entity.Constants.AppUserType.REMOTEAPPADMIN) return true;
            return false;
        }

        private string generateJwtToken(Entity.Entities.AppUserRefreshToken appUserRefreshToken)
        {
            var appSetting = Configuration.Setting.JwtToken;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSetting.SecurityKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, appUserRefreshToken.SessionKey.ToString()),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = appSetting.Issuer,
                Audience = appSetting.Issuer,
                Expires = appUserRefreshToken.ExpiryTime.Value.ToLocalTime(),
                SigningCredentials = credentials,
                Subject = new ClaimsIdentity(claims),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

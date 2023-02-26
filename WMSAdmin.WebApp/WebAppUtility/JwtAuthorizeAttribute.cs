using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace WMSAdmin.WebApp.WebAppUtility
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JwtAuthorizeAttribute : Attribute,IFilterFactory, IAuthorizationFilter
    {
        private WebAppUtility? _appUtility;
        private IServiceProvider? _serviceProvider;
        private AuthorizationFilterContext? _context;
        private ILogger? _logger;

        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetRequiredService<ILogger<JwtAuthorizeAttribute>>();
            return this;
        }
       
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _context= context;
            _appUtility = new WebAppUtility(_context.HttpContext, _serviceProvider!);
            
            var okObjectResult = ValidateWebInvocation(out bool isWebRequest);
            if (okObjectResult != null)
            {
                context.Result = okObjectResult;
                return;
            }

            if (isWebRequest) return;

            okObjectResult = ValidateAPIInvocation(out bool isAPIRequest);
            if (okObjectResult != null)
            {
                context.Result = okObjectResult;
                return;
            }

            if (isAPIRequest) return;

            var isValidRequest = isWebRequest;
            if (isValidRequest != true) isValidRequest = isAPIRequest;

            if (isValidRequest != true)
            {
                context.Result = new OkObjectResult(new Entity.Entities.Response<object>
                {
                    Errors = new List<Entity.Entities.Error> {
                        new Entity.Entities.Error {
                            ErrorCode = Entity.Constants.ErrorCode.UnAuthorized,
                            Message = _appUtility.GetResourceManager<Language.ResourceManager.LoginString>().UnAuthorized
                        }
                    }
                });
            }

            //var user = context.HttpContext.Items[Entity.Constants.WebAppSetting.ContextItemAppUserProfile];
            //if (user == null)
            //{
            //    // user not logged in
            //    context.Result = new JsonResult(new
            //    {
            //        message = _appUtility.GetResourceManager<Language.ResourceManager.LoginString>().UnAuthorized
            //    })
            //    {
            //        StatusCode = StatusCodes.Status401Unauthorized
            //    };
            //}
        }

        private OkObjectResult? ValidateWebInvocation(out bool isWebRequest)
        {
            var _httpContext = _context!.HttpContext;
            isWebRequest = false;
            if (!_httpContext!.Request.Cookies.TryGetValue(Entity.Constants.WebAppSetting.XAccessToken, out string? jwtTokenValue)) return null;
            if (!_httpContext.Request.Cookies.TryGetValue(Entity.Constants.WebAppSetting.XRefreshToken, out string? refreshTokenValue)) return null;
            if (string.IsNullOrWhiteSpace(jwtTokenValue)) return null;
            if (string.IsNullOrWhiteSpace(refreshTokenValue)) return null;
            
            isWebRequest= true;
            if (Guid.TryParse(refreshTokenValue, out Guid refreshToken) == false) return null;
            if (refreshToken == Guid.Empty) return null;
            var authService = _appUtility!.GetBusinessService<BusinessService.AuthenticationService>();


            Entity.Entities.Response<object> GetErrorResponse(List<Entity.Entities.Error> errors)
            {
                return new Entity.Entities.Response<object> { Errors = errors };
            }

            try
            {
                var tokenResponse = authService.ValidateJwtToken(new BusinessService.Model.ValidateJwtTokenRequest
                {
                    AppAccessType = Entity.Constants.AppAccessType.WEB,
                    RefreshOnExpiry = true,
                    RefreshToken = refreshToken,
                    JwtTokenValue = jwtTokenValue,
                });

                if (tokenResponse.Errors?.Any() == true) return new OkObjectResult(GetErrorResponse(tokenResponse.Errors));

                if (tokenResponse.IsRefreshed)
                {
                    _httpContext.Response.Cookies.Append(Entity.Constants.WebAppSetting.XAccessToken, tokenResponse.JwtTokenValue, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                    _httpContext.Response.Cookies.Append(Entity.Constants.WebAppSetting.XRefreshToken, tokenResponse.RefreshToken!.Value.ToString(), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                }

                _httpContext.Items[Entity.Constants.WebAppSetting.ContextItemAppUserProfile] = tokenResponse.AppUserProfile;
                return null;
            }
            catch (Exception ex)
            {
                var loginfo = new
                {
                    SesssionId = _appUtility.Configuration.Setting.Application.SessionId,
                    Method = nameof(ValidateWebInvocation),
                    JwtSecurityToken = jwtTokenValue,
                    ErrorCode = Entity.Constants.ErrorCode.UnableToValidateToken
                };

                _logger!.LogError(ex, $"Error Validating JwtToken", new { LogInfo = loginfo });

                return new OkObjectResult(GetErrorResponse(new List<Entity.Entities.Error> { 
                    new Entity.Entities.Error{ 
                        ErrorCode = Entity.Constants.ErrorCode.UnableToValidateToken,
                        Message = _appUtility.GetResourceManager<Language.ResourceManager.LoginString>().UnableToValidateToken,
                    },
                }));
                
            }
        }

        private OkObjectResult? ValidateAPIInvocation(out bool isAPIRequest)
        {
            isAPIRequest = false;
            var _httpContext = _context!.HttpContext;
            if (!_httpContext!.Request.Headers.TryGetValue(Entity.Constants.WebAppSetting.XAccessToken, out StringValues headerAccessTokenValue)) return  null;

            var jwtTokenValue = headerAccessTokenValue.First();
            if (string.IsNullOrWhiteSpace(jwtTokenValue)) return null;

            var authService = _appUtility!.GetBusinessService<BusinessService.AuthenticationService>();

            Entity.Entities.Response<object> GetErrorResponse(List<Entity.Entities.Error> errors)
            {
                return new Entity.Entities.Response<object> { Errors = errors };
            }

            try
            {
                var tokenResponse = authService.ValidateJwtToken(new BusinessService.Model.ValidateJwtTokenRequest
                {
                    AppAccessType = Entity.Constants.AppAccessType.API,
                    RefreshOnExpiry = false,
                    JwtTokenValue = jwtTokenValue,
                });

                if (tokenResponse.Errors?.Any() == true) return new OkObjectResult(GetErrorResponse(tokenResponse.Errors));
                _httpContext.Items[Entity.Constants.WebAppSetting.ContextItemAppUserProfile] = tokenResponse.AppUserProfile;
                return null;

            }
            catch (Exception ex)
            {
                var loginfo = new
                {
                    SesssionId = _appUtility.Configuration.Setting.Application.SessionId,
                    Method = nameof(ValidateAPIInvocation),
                    JwtSecurityToken = jwtTokenValue,
                    ErrorCode = Entity.Constants.ErrorCode.UnableToValidateToken
                };

                _logger!.LogError(ex, $"Error Validating JwtToken", new { LogInfo = loginfo });

                return new OkObjectResult(GetErrorResponse(new List<Entity.Entities.Error> {
                    new Entity.Entities.Error{
                        ErrorCode = Entity.Constants.ErrorCode.UnableToValidateToken,
                        Message = _appUtility.GetResourceManager<Language.ResourceManager.LoginString>().UnableToValidateToken,
                    },
                }));
            }
        }
    }
}

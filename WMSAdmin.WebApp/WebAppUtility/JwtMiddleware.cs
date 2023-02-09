using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WMSAdmin.BusinessService;

namespace WMSAdmin.WebApp.WebAppUtility
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private WebAppUtility? _appUtility;
        private IServiceProvider? _serviceProvider;
        private ILogger? _logger;
        private HttpContext? _httpContext;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _appUtility = new WebAppUtility(context, _serviceProvider);
            _logger = _serviceProvider.GetRequiredService<ILogger<JwtMiddleware>>();
            _httpContext = context;

            try
            {
                SetAppUserToContextForAPI();
                SetAppUserToContextForWeb();
            }
            catch (Exception ex)
            {
                var loginfo = new
                {
                    SesssionId = _appUtility.Configuration.Setting.Application.SessionId,
                    Method = nameof(Invoke),
                };

                _logger.LogError(ex, $"Error Validating JwtToken", new { LogInfo = loginfo });
            }

            await _next(context);
        }

        private void SetAppUserToContextForAPI()
        {
            if (!_httpContext!.Request.Headers.TryGetValue(Entity.Constants.WebAppSetting.XAccessToken, out StringValues headerAccessTokenValue)) return;

            var jwtTokenValue = headerAccessTokenValue.First();
            if (string.IsNullOrWhiteSpace(jwtTokenValue)) return;

            var authService = _appUtility!.GetBusinessService<BusinessService.AuthenticationService>();
            var repoService = _appUtility.GetBusinessService<BusinessService.RepoService>();

            try
            {
                var tokenResponse = authService.ValidateJwtToken(jwtTokenValue);
                if (tokenResponse.Errors?.Any() == true) return;

                var sessionKeyClaim = tokenResponse.Data.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);
                if (sessionKeyClaim?.Value == null) return;
                if (Guid.TryParse(sessionKeyClaim.Value, out Guid sessionKey) == false) return;

                var appUserToken = repoService.Get(new Entity.Filter.AppUserRefreshToken { SessionKey = sessionKey }).Data.FirstOrDefault();
                if (appUserToken == null) return;

                if (appUserToken.ExpiryTime < DateTime.UtcNow) return;

                var appUser = repoService.Get(new Entity.Filter.AppUser { Id = appUserToken!.AppUserId!.Value }).Data.FirstOrDefault();
                if (appUser == null) return;

                var appUserType = repoService.Get(new Entity.Filter.AppUserType { Id = appUser!.AppUserTypeId!.Value }).Data.First();
                if (appUserType.Code != Entity.Constants.AppUserType.APPUSER) return;

                _httpContext.Items[Entity.Constants.WebAppSetting.ContextItemAppUser] = appUser;
            }
            catch (Exception ex)
            {
                var loginfo = new
                {
                    SesssionId = _appUtility.Configuration.Setting.Application.SessionId,
                    Method = nameof(SetAppUserToContextForAPI),
                    JwtSecurityToken = jwtTokenValue,
                };

                _logger!.LogError(ex, $"Error Validating JwtToken", new { LogInfo = loginfo });
            }
        }

        private void SetAppUserToContextForWeb()
        {
            if (!_httpContext!.Request.Cookies.TryGetValue(Entity.Constants.WebAppSetting.XAccessToken, out string? jwtTokenValue)) return;
            if (!_httpContext.Request.Cookies.TryGetValue(Entity.Constants.WebAppSetting.XRefreshToken, out string? refreshTokenValue)) return;
            if (string.IsNullOrWhiteSpace(jwtTokenValue)) return;
            if (string.IsNullOrWhiteSpace(refreshTokenValue)) return;
            if (Guid.TryParse(refreshTokenValue, out Guid refreshToken) == false) return;

            var authService = _appUtility!.GetBusinessService<BusinessService.AuthenticationService>();
            var repoService = _appUtility.GetBusinessService<BusinessService.RepoService>();

            try
            {
                var tokenResponse = authService.ValidateJwtToken(jwtTokenValue);

                if (tokenResponse.Errors?.Any() == true) return;

                var sessionKeyClaim = tokenResponse.Data.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);
                if (sessionKeyClaim?.Value == null) return;
                if (Guid.TryParse(sessionKeyClaim.Value, out Guid sessionKey) == false) return;

                var appUserToken = repoService.Get(new Entity.Filter.AppUserRefreshToken { SessionKey = sessionKey, RefreshToken = refreshToken }).Data.FirstOrDefault();
                if (appUserToken == null) return;

                var appUser = repoService.Get(new Entity.Filter.AppUser { Id = appUserToken!.AppUserId!.Value }).Data.FirstOrDefault();
                if (appUser == null) return;

                var appUserType = repoService.Get(new Entity.Filter.AppUserType { Id = appUser!.AppUserTypeId!.Value }).Data.First();
                if (appUserType.Code != Entity.Constants.AppUserType.APIUSER) return;

                if (appUserToken.ExpiryTime < DateTime.UtcNow)
                {
                    var refreshResponse = authService.RefreshJWTToken(appUserToken);
                    _httpContext.Response.Cookies.Append(Entity.Constants.WebAppSetting.XAccessToken, refreshResponse.Data.JwtToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                    _httpContext.Response.Cookies.Append(Entity.Constants.WebAppSetting.XRefreshToken, refreshResponse.Data.RefreshToken!.Value.ToString(), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                }

                _httpContext.Items[Entity.Constants.WebAppSetting.ContextItemAppUser] = appUser;
            }
            catch (Exception ex)
            {
                var loginfo = new
                {
                    SesssionId = _appUtility.Configuration.Setting.Application.SessionId,
                    Method = nameof(SetAppUserToContextForWeb),
                    JwtSecurityToken = jwtTokenValue,
                };

                _logger!.LogError(ex, $"Error Validating JwtToken", new { LogInfo = loginfo });
            }
        }
    }
}

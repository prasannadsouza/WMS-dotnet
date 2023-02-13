using Microsoft.Extensions.Primitives;

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
            
            try
            {
                var tokenResponse = authService.ValidateJwtToken(new BusinessService.Model.ValidateJwtTokenRequest
                {
                    AppAccessType = Entity.Constants.AppAccessType.API,
                    RefreshOnExpiry = true,
                    JwtTokenValue = jwtTokenValue,
                });

                if (tokenResponse.Errors?.Any() == true) return;
                if (tokenResponse.IsValid == false) return;
                _httpContext.Items[Entity.Constants.WebAppSetting.ContextItemAppUserProfile] = tokenResponse.AppUserProfile;

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
            if (refreshToken == Guid.Empty) return;
            var authService = _appUtility!.GetBusinessService<BusinessService.AuthenticationService>();

            try
            {
                var tokenResponse = authService.ValidateJwtToken(new BusinessService.Model.ValidateJwtTokenRequest
                {
                    AppAccessType = Entity.Constants.AppAccessType.WEB,
                    RefreshOnExpiry = true,
                    RefreshToken = refreshToken,
                    JwtTokenValue = jwtTokenValue,
                });

                if (tokenResponse.Errors?.Any() == true) return;
                if (tokenResponse.IsValid == false) return;

                if (tokenResponse.IsRefreshed)
                {
                    _httpContext.Response.Cookies.Append(Entity.Constants.WebAppSetting.XAccessToken, tokenResponse.JwtTokenValue, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                    _httpContext.Response.Cookies.Append(Entity.Constants.WebAppSetting.XRefreshToken, tokenResponse.RefreshToken!.Value.ToString(), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                }

                _httpContext.Items[Entity.Constants.WebAppSetting.ContextItemAppUserProfile] = tokenResponse.AppUserProfile;
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

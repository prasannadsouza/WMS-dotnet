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
        private ILogger _logger;


        public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _appUtility = new WebAppUtility(context, _serviceProvider);
            _logger = _serviceProvider.GetRequiredService<ILogger<JwtMiddleware>>();

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                var authService = _appUtility.GetBusinessService<BusinessService.AuthenticationService>();
                try
                {
                    var tokenResponse = authService.ValidateJwtToken(token);
                    var userId = int.Parse(tokenResponse.Data.Claims.First(x => x.Type == "id").Value);
                    // attach user to context on successful jwt validation
                    context.Items["User"] = userId;
                }
                catch (Exception ex)
                {
                    var loginfo = new
                    {
                        SesssionId = _appUtility.Configuration.Setting.Application.SessionId,
                        Method = nameof(Invoke),
                        JwtSecurityToken = token,
                    };

                    _logger.LogError(ex, $"Error Validating JwtToken", new { LogInfo = loginfo });
                }
            }
            await _next(context);
        }
    }
}

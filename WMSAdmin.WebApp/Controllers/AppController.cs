using Microsoft.AspNetCore.Mvc;

namespace WMSAdmin.WebApp.Controllers
{
    public class AppController : BaseController
    {
        public AppController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            
        }

        [HttpGet]
        [Route(WebAppUtility.WebAppUtility.APIRoute)]
        public OkObjectResult Login(Entity.Entities.AuthenticationRequest request)
        {
            var response = new Entity.Entities.Response<bool> { Errors = new List<Entity.Entities.Error>() };
            request.AppUserType = Entity.Constants.AppUserType.APPUSER;
            var authService = AppUtility.GetBusinessService<BusinessService.AuthenticationService>();
            var authResponse = authService.AuthenticateAppUser(request);

            if (authResponse.Errors?.Any() == true)
            {
                response.Errors.AddRange(authResponse.Errors);
                return Ok(response);
            }

            Response.Cookies.Append(Entity.Constants.WebAppSetting.XAccessToken, authResponse.Data.JwtToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append(Entity.Constants.WebAppSetting.XRefreshToken, authResponse.Data.RefreshToken!.Value.ToString(),new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            return Ok(response);
        }
    }
}

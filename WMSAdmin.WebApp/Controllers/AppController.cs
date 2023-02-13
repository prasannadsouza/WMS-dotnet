using Microsoft.AspNetCore.Mvc;

namespace WMSAdmin.WebApp.Controllers
{
    public class AppController : BaseController
    {
        public AppController(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        [HttpPost]
        [Route(WebAppUtility.WebAppUtility.APIRoute)]
        public OkObjectResult Login([FromBody] BusinessService.Model.AuthenticateAppUserRequest request)
        {
            var response = new Entity.Entities.Response<Entity.Model.AuthenticateAppUserResponse> { Errors = new List<Entity.Entities.Error>() };
            request.AppUserType = Entity.Constants.AppUserType.APPUSER;
            var authService = AppUtility.GetBusinessService<BusinessService.AuthenticationService>();
            var authResponse = authService.AuthenticateAppUser(request);

            if (authResponse.Errors?.Any() == true)
            {
                response.Errors.AddRange(authResponse.Errors);
                return Ok(response);
            }

            Response.Cookies.Append(Entity.Constants.WebAppSetting.XAccessToken, authResponse.JwtTokenValue, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append(Entity.Constants.WebAppSetting.XRefreshToken, authResponse.RefreshToken!.Value.ToString(), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            response.Data = new Entity.Model.AuthenticateAppUserResponse
            {
                AppCustomer = authResponse.AppUserProfile.AppCustomer,
                AppCustomerUser = new Entity.Model.AppCustomerUser
                {
                    Id = authResponse.AppUserProfile.AppUser.Id,
                    AppCustomerId = authResponse.AppUserProfile.AppUser.AppCustomerId,
                    DisplayName = authResponse.AppUserProfile.AppUser.DisplayName,
                    LocaleCode = authResponse.AppUserProfile.AppUser.Locale,
                    LoginTime = authResponse.AppUserProfile.AppUser.LoginTime
                }
            };
            return Ok(response);
        }
    }
}

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
        public OkObjectResult Login(Entity.Entities.UserAuthenticateRequest request)
        {
            var authService = AppUtility.GetBusinessService<BusinessService.AuthenticationService>();
            return Ok(authService.AuthenticateAppUser(request));
        }
    }
}

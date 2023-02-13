using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WMSAdmin.WebApp.WebAppUtility
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JwtAuthorizeAttribute : Attribute,IFilterFactory, IAuthorizationFilter
    {
        private WebAppUtility? _appUtility;
        private IServiceProvider? _serviceProvider;

        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            return this;
        }
       
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (_appUtility == null) _appUtility = new WebAppUtility(context.HttpContext, _serviceProvider!);
           
            var user = context.HttpContext.Items[Entity.Constants.WebAppSetting.ContextItemAppUserProfile];
            if (user == null)
            {
                // user not logged in
                context.Result = new JsonResult(new
                {
                    message = _appUtility.GetResourceManager<Language.ResourceManager.GeneralString>().Unauthorized
                })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }
    }
}

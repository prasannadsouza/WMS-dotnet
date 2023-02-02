using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;

namespace WMSAdmin.WebApp.Controllers
{
    public class BaseController:Controller
    {
        private WebAppUtility.WebAppUtility? _appUtility;
        
        public IServiceProvider ServiceProvider { get; private set; }

        public BaseController(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public WebAppUtility.WebAppUtility AppUtility
        {
            get
            {
                if (_appUtility == null) _appUtility = new WebAppUtility.WebAppUtility(HttpContext, ServiceProvider);
                return _appUtility;
            }
        }
    }
}

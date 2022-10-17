using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace WMSAdmin.WebApp.Controllers
{
    public class BaseController:Controller
    {
        private Utility _utility;
        public IServiceProvider ServiceProvider { get; private set; }
        protected ILogger _logger;

        public BaseController(IServiceProvider serviceProvider, ILogger logger)
        {
            ServiceProvider = serviceProvider;
            _utility = new Utility( HttpContext, serviceProvider,logger);
            _logger = logger;
        }
        public Utility Utility
        {
            get
            {
                return _utility;
            }
        }

    }
}

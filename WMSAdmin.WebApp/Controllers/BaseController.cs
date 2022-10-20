using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;

namespace WMSAdmin.WebApp.Controllers
{
    public class BaseController:Controller
    {
        private Utility? _utility;
        public IServiceProvider ServiceProvider { get; private set; }
        protected ILogger Logger { get; private set; }

        public BaseController(IServiceProvider serviceProvider, ILogger logger)
        {
            ServiceProvider = serviceProvider;
            Logger = logger;
        }



        public Utility Utility
        {
            get
            {
                if (_utility == null) _utility = new Utility(HttpContext, ServiceProvider, Logger);
                return _utility;
            }
        }
    }
}

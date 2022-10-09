using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace WMSAdmin.WebApp.Controllers
{
    public class BaseController:ControllerBase
    {
        private Utility _utility;
        public IConfiguration Configuration { get; }
        protected IMemoryCache _memoryCache;
        protected ILogger _logger;

        public BaseController(IConfiguration configuration, IMemoryCache memoryCache, ILogger logger)
        {
            Configuration = configuration;
            _memoryCache = memoryCache;
            _utility = new Utility(HttpContext, memoryCache, logger);
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

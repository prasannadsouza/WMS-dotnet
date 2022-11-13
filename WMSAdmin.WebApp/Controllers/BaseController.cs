﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;

namespace WMSAdmin.WebApp.Controllers
{
    public class BaseController:Controller
    {
        private WebUtility? _appUtility;
        
        public IServiceProvider ServiceProvider { get; private set; }

        public BaseController(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public WebUtility AppUtility
        {
            get
            {
                if (_appUtility == null) _appUtility = new WebUtility(HttpContext, ServiceProvider);
                return _appUtility;
            }
        }
    }
}

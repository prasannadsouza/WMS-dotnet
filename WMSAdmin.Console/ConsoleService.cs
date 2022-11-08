using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Console
{
    public class ConsoleService : BackgroundService
    {
        private Utility? _utility;
        public IServiceProvider ServiceProvider { get; private set; }
        protected ILogger Logger { get; private set; }
        public ConsoleService(IServiceProvider serviceProvider, ILogger logger)
        {
            ServiceProvider = serviceProvider;
            Logger = logger;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        internal Utility Utility
        {
            get
            {
                if (_utility == null) _utility = new Utility(ServiceProvider, Logger);
                return _utility;
            }
        }
    }
}

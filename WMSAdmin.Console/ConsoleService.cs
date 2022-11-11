using Microsoft.Extensions.DependencyInjection;
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
        internal AppUtility AppUtility { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }
        public ILogger Logger { get; private set; }

        protected bool RunService { get; set; }
        public ConsoleService(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            AppUtility = new AppUtility(ServiceProvider);
            Logger = serviceProvider.GetRequiredService<ILogger<ConsoleService>>();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

    }
}

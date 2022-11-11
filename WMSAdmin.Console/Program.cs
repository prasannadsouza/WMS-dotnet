using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WMSAdmin.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);

            builder.ConfigureServices((hostContext, services) =>
            {
                services.AddMemoryCache();
                services.AddHostedService<LanguageTextService>();
                services.AddHostedService<CacheTimestampService>();
            });

            var app = builder.Build();
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Starting App");
            app.Run();
            logger.LogInformation("App is Running");
        }
    }
}
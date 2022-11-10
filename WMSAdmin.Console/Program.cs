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
                //services.AddLogging(opt=> { 
                //    opt.AddConfiguration(hostContext.Configuration);
                //});
            });

            var app = builder.Build();
            
            
            IConfiguration config = app.Services.GetRequiredService<IConfiguration>();
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            var appUtility = new AppUtility(app.Services);
            var cultures = appUtility.GetSupportedCultures();
            app.Run();

        }
    }
}
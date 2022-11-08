using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

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
            });

            var app = builder.Build();
            IConfiguration config = app.Services.GetRequiredService<IConfiguration>();
            app.Run();
        }
    }
}
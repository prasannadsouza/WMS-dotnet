
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace WMSAdmin.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();
            app.Logger.LogInformation("Starting App");

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();
            app.UseSession();

            var serviceProvider = app.Services;
            app.Logger.LogInformation("Getting App Configuration");
            var configuration = GetAppConfiguration(app.Services);

            app.Logger.LogInformation("Configuring Cultures");
            var supportedCultures = GetSupportedCultures(configuration);

            app.UseRequestLocalization(opt =>
            {
                opt.DefaultRequestCulture = new RequestCulture(configuration.Culture);
                opt.SupportedCultures = supportedCultures;
                opt.SupportedUICultures = supportedCultures;
                opt.FallBackToParentCultures = true;
                opt.FallBackToParentUICultures = true;
                opt.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
            });

            app.MapFallbackToFile("index.html"); ;

            app.Run();
            app.Logger.LogInformation("App is Running");
        }


        private static Utility.Configuration GetAppConfiguration(IServiceProvider serviceProvider)
        {
            var configuration = new Utility.Configuration
            {
                Setting = Utility.AppHelper.GetDefaultConfigSetting(),
                ServiceProvider = serviceProvider,
                Culture = System.Globalization.CultureInfo.CurrentCulture,
            };

            var configService = new BusinessService.ConfigService(configuration);
            var setting = configService.GetConfigSetting();
            return new Utility.Configuration
            {
                Setting = setting,
                ServiceProvider = serviceProvider,
                Culture = new System.Globalization.CultureInfo(setting.Application.Locale),
            };
        }
        private static List<System.Globalization.CultureInfo> GetSupportedCultures(Utility.Configuration configuration)
        {
            var supportedCultures = new List<System.Globalization.CultureInfo>();
            var repoService = new BusinessService.RepoService(configuration);
            var filter = new Entity.Filter.LanguageCulture { Pagination = Utility.AppHelper.GetDefaultPagination(configuration.Setting, true) };

            do
            {
                var items = repoService.Get(filter).Data;

                foreach (var item in items)
                {
                    supportedCultures.Add(new System.Globalization.CultureInfo(item.Code));
                }

                filter.Pagination.CurrentPage++;
            }
            while (filter.Pagination.CurrentPage <= filter.Pagination.TotalPages);
            return supportedCultures;
        }
    }
}
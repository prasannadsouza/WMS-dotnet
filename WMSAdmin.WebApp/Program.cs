
using Microsoft.AspNetCore.Localization;
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

            var configuration = GetAppConfiguration(app.Services);
            
            var supportedCultures = GetSupportedCultures(configuration); 

            app.UseRequestLocalization(opt => {
                opt.DefaultRequestCulture = new RequestCulture(configuration.Culture);
                opt.SupportedCultures = supportedCultures;
                opt.SupportedUICultures = supportedCultures;
                opt.FallBackToParentCultures = true;
                opt.FallBackToParentUICultures = true;
                opt.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
            });

            app.MapFallbackToFile("index.html"); ;

            app.Run();
        }

        private static Utility.Configuration GetAppConfiguration(IServiceProvider serviceProvider)
        {
            var setting = GetConfigSetting(serviceProvider);
            var configuration = new Utility.Configuration
            {
                Setting = setting,
                ServiceProvider = serviceProvider,
                Logger = serviceProvider.GetRequiredService<ILogger>(),
                Culture = new System.Globalization.CultureInfo(setting.Application.Locale),
            };
            return configuration;
        }

        private static Entity.Entities.Config.ConfigSetting GetConfigSetting(IServiceProvider serviceProvider)
        {
            var configuration = new Utility.Configuration
            {
                Setting = Utility.AppHelper.GetDefaultConfigSetting(),
                ServiceProvider = serviceProvider,
                Logger = serviceProvider.GetRequiredService<ILogger>(),
                Culture = System.Globalization.CultureInfo.CurrentCulture,
            };

            var configService = new BusinessService.ConfigService(configuration);
            var setting = configService.GetConfigSetting();
            return setting;
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
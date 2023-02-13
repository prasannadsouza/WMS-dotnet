using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using WMSAdmin.Entity.Entities;
using WMSAdmin.WebApp.WebAppUtility;

namespace WMSAdmin.WebApp.Controllers
{
    [JwtAuthorizeAttribute]
    [ApiController]
    public class WeatherForecastController : BaseController
    {
        private static readonly string[] Summaries = new[]
         {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastController(IServiceProvider serviceProvider): base(serviceProvider)
        {
            
        }

        [HttpGet ]
        [Route("[controller]")]
        [Route("[controller]/[action]")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.UtcNow.AddDays(index).ToLocalTime(),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public Entity.Entities.Config.ConfigSetting GetConfigSetting()
        {
            var configsetting = AppUtility.ConfigSetting;
            return configsetting;
        }
    }
}
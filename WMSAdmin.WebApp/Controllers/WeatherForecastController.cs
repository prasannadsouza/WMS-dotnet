using Microsoft.AspNetCore.Mvc;
using WMSAdmin.Entity.Entities;

namespace WMSAdmin.WebApp.Controllers
{
    [ApiController]
    public class WeatherForecastController : BaseController
    {
        private static readonly string[] Summaries = new[]
         {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastController(IServiceProvider serviceProvider, ILogger<WeatherForecastController> logger): 
            base(serviceProvider, logger)
        {
            
        }

        [HttpGet ]
        [Route("[controller]")]
        [Route("[controller]/[action]")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public Entity.Entities.Config.ConfigSetting GetConfigSetting()
        {
            var configsetting = Utility.GetConfigSetting();
            return configsetting;
        }
    }
}
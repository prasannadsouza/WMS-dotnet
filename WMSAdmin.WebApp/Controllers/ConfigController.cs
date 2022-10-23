using Microsoft.AspNetCore.Mvc;

namespace WMSAdmin.WebApp.Controllers
{
    public class ConfigController : BaseController
    {
        public ConfigController(IServiceProvider serviceProvider, ILogger<WeatherForecastController> logger) :
          base(serviceProvider, logger)
        {

        }
    }
}

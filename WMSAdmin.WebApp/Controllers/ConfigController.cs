using Microsoft.AspNetCore.Mvc;

namespace WMSAdmin.WebApp.Controllers
{
    public class ConfigController : BaseController
    {
        public ConfigController(IServiceProvider serviceProvider) :
          base(serviceProvider)
        {

        }
    }
}

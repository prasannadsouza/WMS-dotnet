using Microsoft.Extensions.Logging;

namespace WMSAdmin.BusinessService
{
    public class Configuration
    {
        public Entity.Entities.Config.ConfigSetting Setting { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
        public ILogger Logger { get; set; }
    }
}

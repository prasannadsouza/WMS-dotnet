using System.Reflection.Metadata.Ecma335;

namespace WMSAdmin.BusinessService
{
    public class RepoService : BaseService
    {
        public RepoService(Configuration configuration) : base(configuration)
        {
        }

        private string _className;
        private string ClassName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_className) == false) return _className;
                _className = this.GetType().FullName;
                return _className;
            }
        }

        public Entity.Entities.Response<List<Entity.Entities.AppConfigGroup>> Get(Entity.Filter.AppConfigGroup filter)
            => GetRepository<Repository.AppConfigGroup>().Get(filter);

        public Entity.Entities.Response<List<Entity.Entities.AppConfig>> Get(Entity.Filter.AppConfig filter)
            => GetRepository<Repository.AppConfig>().Get(filter);
    }
}
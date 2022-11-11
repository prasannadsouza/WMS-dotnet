using System.Reflection.Metadata.Ecma335;

namespace WMSAdmin.BusinessService
{
    public class RepoService : BaseService
    {
        public RepoService(Utility.Configuration configuration) : base(configuration)
        {
        }

        public Entity.Entities.Response<List<Entity.Entities.AppConfigGroup>> Get(Entity.Filter.AppConfigGroup filter)
            => GetRepository<Repository.AppConfigGroup>().Get(filter);

        public Entity.Entities.Response<List<Entity.Entities.AppConfig>> Get(Entity.Filter.AppConfig filter)
            => GetRepository<Repository.AppConfig>().Get(filter);

        public Entity.Entities.Response<List<Entity.Entities.LanguageText>> Get(Entity.Filter.LanguageText filter)
            => GetRepository<Repository.LanguageText>().Get(filter);

        public Entity.Entities.Response<List<Entity.Entities.LanguageGroup>> Get(Entity.Filter.LanguageGroup filter)
            => GetRepository<Repository.LanguageGroup>().Get(filter);

        public Entity.Entities.Response<List<Entity.Entities.LanguageCulture>> Get(Entity.Filter.LanguageCulture filter)
            => GetRepository<Repository.LanguageCulture>().Get(filter);

        public void Save(Entity.Entities.AppConfig item) => GetRepository<Repository.AppConfig>().Save(item);
        public void Save(Entity.Entities.AppConfigGroup item) => GetRepository<Repository.AppConfigGroup>().Save(item);
    }
}
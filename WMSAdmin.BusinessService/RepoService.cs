
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

        public Entity.Entities.Response<List<Entity.Entities.ConfigTimeStamp>> Get(Entity.Filter.ConfigTimeStamp filter)
            => GetRepository<Repository.ConfigTimeStamp>().Get(filter);

        public Entity.Entities.Response<List<Entity.Entities.AppUser>> Get(Entity.Filter.AppUser filter)
            => GetRepository<Repository.AppUser>().Get(filter);

        public Entity.Entities.Response<List<Entity.Entities.AppUserRefreshToken>> Get(Entity.Filter.AppUserRefreshToken filter)
            => GetRepository<Repository.AppUserRefreshToken>().Get(filter);

        public Entity.Entities.Response<List<Entity.Entities.AppCustomer>> Get(Entity.Filter.AppCustomer filter)
            => GetRepository<Repository.AppCustomer>().Get(filter);

        public Entity.Entities.Response<List<Entity.Entities.AppUserType>> Get(Entity.Filter.AppUserType filter)
            => GetRepository<Repository.AppUserType>().Get(filter);

        public void Save(Entity.Entities.AppConfig item) => GetRepository<Repository.AppConfig>().Save(item);
        public void Save(Entity.Entities.AppConfigGroup item) => GetRepository<Repository.AppConfigGroup>().Save(item);
        public void Save(Entity.Entities.ConfigTimeStamp item) => GetRepository<Repository.ConfigTimeStamp>().Save(item);
        public void Save(Entity.Entities.AppUser item) => GetRepository<Repository.AppUser>().Save(item);
        public void Save(Entity.Entities.AppUserRefreshToken item) => GetRepository<Repository.AppUserRefreshToken>().Save(item);
        public void Save(Entity.Entities.AppCustomer item) => GetRepository<Repository.AppCustomer>().Save(item);
    }
}
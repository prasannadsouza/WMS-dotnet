using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Repository
{
    public class AppConfig:BaseRepository
    {
        public AppConfig(RepoConfiguration configuration) : base(configuration)
        { 
        }
        public List<Entity.Entities.AppConfig> GetAll()
        {
            using (var db = GetDbContext())
            {
                return ConvertTo(db.AppConfig);
            }
        }

        internal static List<Entity.Entities.AppConfig> ConvertTo(IEnumerable<Domain.AppConfig> fromList)
        {
            var toList = new List<Entity.Entities.AppConfig>();
            foreach (var from in fromList)
            {
                toList.Add(ConvertTo(from));
            }
            return toList;
        }

        internal static Entity.Entities.AppConfig ConvertTo(Domain.AppConfig from)
        {
            var to = new Entity.Entities.AppConfig
            {
                Id = from.Id,
                AppConfigGroupId = from.AppConfigGroupId,
                Code = from.Code,
                Value = from.Value,
                Description = from.Description,
            };
            return to;
        }

        internal static Domain.AppConfig ConvertTo(Entity.Entities.AppConfig from, Domain.AppConfig to)
        {
            if (to == null) to = new Domain.AppConfig();
            to.AppConfigGroupId = from.AppConfigGroupId.Value;
            to.Code = from.Code;
            to.Value = from.Value;
            to.Description = from.Description;
            return to;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Repository
{
    public class AppConfigGroup: BaseRepository
    {
        public AppConfigGroup(RepoConfiguration configuration) : base(configuration)
        {
        }

        public List<Entity.Entities.AppConfigGroup> GetAll()
        {
            using (var db = GetDbContext())
            {
                return ConvertTo(db.AppConfigGroup);
            }
        }

        internal static List<Entity.Entities.AppConfigGroup> ConvertTo(IEnumerable<Domain.AppConfigGroup> fromList)
        {
            var toList = new List<Entity.Entities.AppConfigGroup>();
            foreach (var from in fromList)
            {
                toList.Add(ConvertTo(from));
            }
            return toList;
        }
        internal static Entity.Entities.AppConfigGroup ConvertTo(Domain.AppConfigGroup from)
        {
            var to = new Entity.Entities.AppConfigGroup
            {
                Id = from.Id,
                Code = from.Code,
                GroupName = from.GroupName,
                Description = from.Description,
            };
            return to;
        }
        internal static Domain.AppConfigGroup ConvertTo(Entity.Entities.AppConfigGroup from, Domain.AppConfigGroup to)
        {
            if (to == null) to = new Domain.AppConfigGroup();
            to.Code = from.Code;
            to.GroupName = from.GroupName;
            to.Description = from.Description;
            return to;
        }
    }
}

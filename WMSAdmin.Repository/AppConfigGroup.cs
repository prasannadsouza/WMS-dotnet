using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WMSAdmin.Repository
{
    public class AppConfigGroup: BaseRepository
    {
        public AppConfigGroup(Utility.Configuration configuration) : base(configuration)
        {
        }

        internal IQueryable<POCO.AppConfigGroup> GetQuery(Context.RepoContext db, Entity.Filter.AppConfigGroup filter)
        {
            var query = db.AppConfigGroup.AsNoTracking();
            if (filter == null) return query;

            if (filter.Id != null) query = query.Where(p => filter.Id.Value == p.Id.Value);
            if (filter.Ids?.Any() == true) query = query.Where(p => filter.Ids.Contains(p.Id.Value));

            filter.Code = filter?.Code?.Trim();
            if (string.IsNullOrEmpty(filter?.Code) == false)
            {
                if (filter.Code.Contains("%")) query = query.Where(p => EF.Functions.Like(p.Code, filter.Code));
                else query = query.Where(e => e.Code == filter.Code);
            }

            if (string.IsNullOrEmpty(filter?.Name) == false)
            {
                if (filter.Name.Contains("%")) query = query.Where(p => EF.Functions.Like(p.Name, filter.Name));
                else query = query.Where(e => e.Name == filter.Name);
            }

            if (filter.FromTimeStamp.HasValue) query = query.Where(p => filter.FromTimeStamp >= p.TimeStamp.Value);
            if (filter.ToTimeStamp.HasValue) query = query.Where(p => filter.FromTimeStamp <= p.TimeStamp.Value);

            return query;
        }

        public List<Entity.Entities.AppConfigGroup> GetAll()
        {
            using (var db = GetDbContext())
            {
                return ConvertTo(db.AppConfigGroup);
            }
        }

        public Entity.Entities.Response<List<Entity.Entities.AppConfigGroup>> Get(Entity.Filter.AppConfigGroup filter)
        {
            var responseData = new Entity.Entities.Response<List<Entity.Entities.AppConfigGroup>>()
            {
                Errors = new List<Entity.Entities.Error>(),
            };

            using (var db = GetDbContext())
            {
                var query = GetQuery(db, filter);
                responseData.Data = ConvertTo(GetOrderedResult(null, query, filter?.Pagination, out Entity.Entities.Pagination newPagination));
                responseData.Pagination = filter.Pagination = newPagination;
                return responseData;
            }
        }

        public void Save(Entity.Entities.AppConfigGroup item)
        {
            using (var dbContext = GetDbContext())
            {
                POCO.AppConfigGroup dbItem = null;

                if (item.Id.HasValue)
                {
                    dbItem = dbContext.AppConfigGroup.First(e => e.Id == item.Id.Value);
                    ConvertTo(item, dbItem);
                    dbContext.SaveChanges();
                    return;
                }

                dbItem = ConvertTo(item, dbItem);
                dbContext.AppConfigGroup.Add(dbItem);
                dbContext.SaveChanges();
                item.Id = dbItem.Id;
                return;
            }
        }

        internal static List<Entity.Entities.AppConfigGroup> ConvertTo(IEnumerable<POCO.AppConfigGroup> fromList)
        {
            var toList = new List<Entity.Entities.AppConfigGroup>();
            foreach (var from in fromList)
            {
                toList.Add(ConvertTo(from));
            }
            return toList;
        }
        internal static Entity.Entities.AppConfigGroup ConvertTo(POCO.AppConfigGroup from)
        {
            var to = new Entity.Entities.AppConfigGroup
            {
                Id = from.Id,
                Code = from.Code,
                Name = from.Name,
                Description = from.Description,
                TimeStamp = from.TimeStamp,
                
            };
            return to;
        }
        internal static POCO.AppConfigGroup ConvertTo(Entity.Entities.AppConfigGroup from, POCO.AppConfigGroup to)
        {
            if (to == null) to = new POCO.AppConfigGroup();
            to.Code = from.Code;
            to.Name = from.Name;
            to.Description = from.Description;
            to.TimeStamp = from.TimeStamp;
            return to;
        }
    }
}

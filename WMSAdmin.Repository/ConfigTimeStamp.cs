using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WMSAdmin.Repository
{
    public class ConfigTimeStamp : BaseRepository
    {
        public ConfigTimeStamp(Utility.Configuration configuration) : base(configuration)
        {
        }

        internal IQueryable<POCO.ConfigTimeStamp> GetQuery(Context.RepoContext db, Entity.Filter.ConfigTimeStamp filter)
        {
            var query = db.ConfigTimeStamp.AsNoTracking();
            if (filter == null) return query;

            if (filter.Id != null) query = query.Where(p => filter.Id.Value == p.Id.Value);
            if (filter.Ids?.Any() == true) query = query.Where(p => filter.Ids.Contains(p.Id.Value));

            filter.Code = filter?.Code?.Trim();
            if (string.IsNullOrEmpty(filter?.Code) == false)
            {
                if (filter.Code.Contains("%")) query = query.Where(p => EF.Functions.Like(p.Code, filter.Code));
                else query = query.Where(e => e.Code == filter.Code);
            }

            if (filter.FromTimeStamp.HasValue) query = query.Where(p => filter.FromTimeStamp >= p.TimeStamp.Value);
            if (filter.ToTimeStamp.HasValue) query = query.Where(p => filter.ToTimeStamp <= p.TimeStamp.Value);
            
            return query;
        }

        public List<Entity.Entities.ConfigTimeStamp> GetAll()
        {
            using (var db = GetDbContext())
            {
                return ConvertTo(db.ConfigTimeStamp);
            }
        }

        public Entity.Entities.Response<List<Entity.Entities.ConfigTimeStamp>> Get(Entity.Filter.ConfigTimeStamp filter)
        {
            var responseData = new Entity.Entities.Response<List<Entity.Entities.ConfigTimeStamp>>()
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

        public void Save(Entity.Entities.ConfigTimeStamp item)
        {
            using (var dbContext = GetDbContext())
            {
                POCO.ConfigTimeStamp dbItem = null;

                if (item.Id.HasValue)
                {
                    dbItem = dbContext.ConfigTimeStamp.First(e => e.Id == item.Id.Value);
                    ConvertTo(item, dbItem);
                    dbContext.SaveChanges();
                    return;
                }

                dbItem = ConvertTo(item, dbItem);
                dbContext.ConfigTimeStamp.Add(dbItem);
                dbContext.SaveChanges();
                item.Id = dbItem.Id;
                return;
            }
        }

        internal static List<Entity.Entities.ConfigTimeStamp> ConvertTo(IEnumerable<POCO.ConfigTimeStamp> fromList)
        {
            var toList = new List<Entity.Entities.ConfigTimeStamp>();
            foreach (var from in fromList)
            {
                toList.Add(ConvertTo(from));
            }
            return toList;
        }
        internal static Entity.Entities.ConfigTimeStamp ConvertTo(POCO.ConfigTimeStamp from)
        {
            var to = new Entity.Entities.ConfigTimeStamp
            {
                Id = from.Id,
                Code = from.Code,
                TimeStamp = from.TimeStamp,
            };
            return to;
        }
        internal static POCO.ConfigTimeStamp ConvertTo(Entity.Entities.ConfigTimeStamp from, POCO.ConfigTimeStamp to)
        {
            if (to == null) to = new POCO.ConfigTimeStamp();
            to.Code = from.Code;
            to.TimeStamp = from.TimeStamp;
            return to;
        }
    }
}

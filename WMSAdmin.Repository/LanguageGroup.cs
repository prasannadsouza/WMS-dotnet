using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WMSAdmin.Repository
{
    public class LanguageGroup:BaseRepository
    {
        public LanguageGroup(RepoConfiguration configuration) : base(configuration)
        {

        }

        internal IQueryable<POCO.LanguageGroup> GetQuery(Context.RepoContext db, Entity.Filter.LanguageGroup filter)
        {
            var query = db.LanguageGroup.AsNoTracking();
            if (filter == null) return query;

            if (filter.Id.HasValue) query = query.Where(p => filter.Id.Value == p.Id.Value);
            if (filter.Ids?.Any() == true) query = query.Where(p => filter.Ids.Contains(p.Id.Value));
            
            filter.Code = filter?.Code?.Trim();
            if (string.IsNullOrEmpty(filter?.Code) == false)
            {
                if (filter.Code.Contains("%")) query = query.Where(p => EF.Functions.Like(p.Code, filter.Code));
                else query = query.Where(e => e.Code == filter.Code);
            }

            filter.Name = filter?.Name?.Trim();
            if (string.IsNullOrEmpty(filter?.Name) == false)
            {
                if (filter.Name.Contains("%")) query = query.Where(p => EF.Functions.Like(p.Name, filter.Name));
                else query = query.Where(e => e.Name == filter.Name);
            }

            if (filter.FromTimeStamp.HasValue) query = query.Where(p => filter.FromTimeStamp >= p.TimeStamp.Value);
            if (filter.ToTimeStamp.HasValue) query = query.Where(p => filter.FromTimeStamp <= p.TimeStamp.Value);

            return query;
        }

        public Entity.Entities.Response<List<Entity.Entities.LanguageGroup>> Get(Entity.Filter.LanguageGroup filter)
        {
            var responseData = new Entity.Entities.Response<List<Entity.Entities.LanguageGroup>>()
            {
                Errors = new List<Entity.Entities.Error>(),
            };
            
            using (var db = GetDbContext())
            {
                var lgq = GetQuery(db, filter);
                var waRepo = new WMSApplication(Configuration);
                var waq = waRepo.GetQuery(db, filter?.WMSApplication);

                var query = from lg in lgq
                            join wa in waq on lg.WMSApplicationId equals wa.Id
                            select lg;

                responseData.Data = ConvertTo(GetOrderedResult(null, query, filter?.Pagination, out Entity.Entities.Pagination newPagination));
                responseData.Pagination = filter.Pagination = newPagination;
                return responseData;
            }
        }
        public void Save(Entity.Entities.LanguageGroup item)
        {
            using (var dbContext = GetDbContext())
            {
                POCO.LanguageGroup dbItem = null;

                if (item.Id.HasValue)
                {
                    dbItem = dbContext.LanguageGroup.First(e => e.Id == item.Id.Value);
                    ConvertTo(item, dbItem);
                    dbContext.SaveChanges();
                    return;
                }

                dbItem = ConvertTo(item, dbItem);
                dbContext.LanguageGroup.Add(dbItem);
                dbContext.SaveChanges();
                item.Id = dbItem.Id;
                return;
            }
        }
        internal static List<Entity.Entities.LanguageGroup> ConvertTo(IEnumerable<POCO.LanguageGroup> fromList)
        {
            var toList = new List<Entity.Entities.LanguageGroup>();
            foreach (var from in fromList)
            {
                toList.Add(ConvertTo(from));
            }
            return toList;
        }
        internal static Entity.Entities.LanguageGroup ConvertTo(POCO.LanguageGroup from)
        {
            var to = new Entity.Entities.LanguageGroup
            {
                Id = from.Id,
                Code = from.Code,
                Name = from.Name,

            };

            return to;
        }
        internal static POCO.LanguageGroup ConvertTo(Entity.Entities.LanguageGroup from, POCO.LanguageGroup to)
        {
            if (to == null) to = new POCO.LanguageGroup();
            to.Code = from.Code;
            to.Name = from.Name;
            return to;
        }
    }
}

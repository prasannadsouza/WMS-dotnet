using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Repository
{
    public class LanguageCulture : BaseRepository
    {
        public LanguageCulture(RepoConfiguration configuration) : base(configuration)
        {

        }

        internal IQueryable<POCO.LanguageCulture> GetQuery(Context.RepoContext db, Entity.Filter.LanguageCulture filter)
        {
            var query = db.LanguageCulture.AsNoTracking();
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
            return query;
        }

        public Entity.Entities.Response<List<Entity.Entities.LanguageCulture>> Get(Entity.Filter.LanguageCulture filter)
        {
            var responseData = new Entity.Entities.Response<List<Entity.Entities.LanguageCulture>>()
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

        public void Save(Entity.Entities.LanguageCulture item)
        {
            using (var dbContext = GetDbContext())
            {
                POCO.LanguageCulture dbItem = null;

                if (item.Id.HasValue)
                {
                    dbItem = dbContext.LanguageCulture.First(e => e.Id == item.Id.Value);
                    ConvertTo(item, dbItem);
                    dbContext.SaveChanges();
                    return;
                }

                dbItem = ConvertTo(item, dbItem);
                dbContext.LanguageCulture.Add(dbItem);
                dbContext.SaveChanges();
                item.Id = dbItem.Id;
                return;
            }
        }
        internal static List<Entity.Entities.LanguageCulture> ConvertTo(IEnumerable<POCO.LanguageCulture> fromList)
        {
            var toList = new List<Entity.Entities.LanguageCulture>();
            foreach (var from in fromList)
            {
                toList.Add(ConvertTo(from));
            }
            return toList;
        }
        internal static Entity.Entities.LanguageCulture ConvertTo(POCO.LanguageCulture from)
        {
            var to = new Entity.Entities.LanguageCulture
            {
                Id = from.Id,
                Code = from.Code,
                Name = from.Name,
                Timestamp = from.Timestamp,
            };

            return to;
        }
        internal static POCO.LanguageCulture ConvertTo(Entity.Entities.LanguageCulture from, POCO.LanguageCulture to)
        {
            if (to == null) to = new POCO.LanguageCulture();
            to.Id = from.Id;
            to.Code = from.Code;
            to.Name = from.Name;
            to.Timestamp = from.Timestamp;
            return to;
        }
    }
}

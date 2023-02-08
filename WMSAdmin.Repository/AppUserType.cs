using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WMSAdmin.Repository
{
    public class AppUserType : BaseRepository
    {
        public AppUserType(Utility.Configuration configuration) : base(configuration)
        {
        }

        internal IQueryable<POCO.AppUserType> GetQuery(Context.RepoContext db, Entity.Filter.AppUserType filter)
        {
            var query = db.AppUserType.AsNoTracking();
            if (filter == null) return query;

            if (filter.Id != null) query = query.Where(p => filter.Id.Value == p.Id.Value);
            if (filter.Ids?.Any() == true) query = query.Where(p => filter.Ids.Contains(p.Id.Value));

            filter.Code = filter?.Code?.Trim();
            if (string.IsNullOrEmpty(filter?.Code) == false)
            {
                if (filter.Code.Contains("%")) query = query.Where(p => EF.Functions.Like(p.Code, filter.Code));
                else query = query.Where(e => e.Code == filter.Code);
            }
            return query;
        }

        public Entity.Entities.Response<List<Entity.Entities.AppUserType>> Get(Entity.Filter.AppUserType filter)
        {
            var responseData = new Entity.Entities.Response<List<Entity.Entities.AppUserType>>()
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

        public void Save(Entity.Entities.AppUserType item)
        {
            using (var dbContext = GetDbContext())
            {
                POCO.AppUserType dbItem = null;

                if (item.Id.HasValue)
                {
                    dbItem = dbContext.AppUserType.First(e => e.Id == item.Id.Value);
                    ConvertTo(item, dbItem);
                    dbContext.SaveChanges();
                    return;
                }

                dbItem = ConvertTo(item, dbItem);
                dbContext.AppUserType.Add(dbItem);
                dbContext.SaveChanges();
                item.Id = dbItem.Id;
                return;
            }
        }

        internal static List<Entity.Entities.AppUserType> ConvertTo(IEnumerable<POCO.AppUserType> fromList)
        {
            var toList = new List<Entity.Entities.AppUserType>();
            foreach (var from in fromList)
            {
                toList.Add(ConvertTo(from));
            }
            return toList;
        }
        internal static Entity.Entities.AppUserType ConvertTo(POCO.AppUserType from)
        {
            var to = new Entity.Entities.AppUserType
            {
                Id = from.Id,
                Code = from.Code,
                AppUserTypeName = from.AppUserTypeName,
                Description = from.Description,
                TimeStamp = from.TimeStamp,


            };
            return to;
        }
        internal static POCO.AppUserType ConvertTo(Entity.Entities.AppUserType from, POCO.AppUserType to)
        {
            if (to == null) to = new POCO.AppUserType();
            to.Code = from.Code;
            to.AppUserTypeName = from.AppUserTypeName;
            to.Description = from.Description;
            to.TimeStamp = from.TimeStamp;
            return to;
        }
    }
}

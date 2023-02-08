using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WMSAdmin.Repository
{
    public class AppCustomerUser : BaseRepository
    {
        public AppCustomerUser(Utility.Configuration configuration) : base(configuration)
        {
        }

        internal IQueryable<POCO.AppCustomerUser> GetQuery(Context.RepoContext db, Entity.Filter.AppCustomerUser filter)
        {
            var query = db.AppCustomerUser.AsNoTracking();
            if (filter == null) return query;

            if (filter.Id != null) query = query.Where(p => filter.Id.Value == p.Id.Value);
            if (filter.Ids?.Any() == true) query = query.Where(p => filter.Ids.Contains(p.Id.Value));

            return query;
        }

        public Entity.Entities.Response<List<Entity.Entities.AppCustomerUser>> Get(Entity.Filter.AppCustomerUser filter)
        {
            var responseData = new Entity.Entities.Response<List<Entity.Entities.AppCustomerUser>>()
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

        public void Save(Entity.Entities.AppCustomerUser item)
        {
            using (var dbContext = GetDbContext())
            {
                POCO.AppCustomerUser dbItem = null;

                if (item.Id.HasValue)
                {
                    dbItem = dbContext.AppCustomerUser.First(e => e.Id == item.Id.Value);
                    ConvertTo(item, dbItem);
                    dbContext.SaveChanges();
                    return;
                }

                dbItem = ConvertTo(item, dbItem);
                dbContext.AppCustomerUser.Add(dbItem);
                dbContext.SaveChanges();
                item.Id = dbItem.Id;
                return;
            }
        }

        internal static List<Entity.Entities.AppCustomerUser> ConvertTo(IEnumerable<POCO.AppCustomerUser> fromList)
        {
            var toList = new List<Entity.Entities.AppCustomerUser>();
            foreach (var from in fromList)
            {
                toList.Add(ConvertTo(from));
            }
            return toList;
        }
        internal static Entity.Entities.AppCustomerUser ConvertTo(POCO.AppCustomerUser from)
        {
            var to = new Entity.Entities.AppCustomerUser
            {
                Id = from.Id,
                AppCustomerId = from.AppCustomerId,
                FirstName = from.FirstName,
                LastName = from.LastName,
                Email = from.Email,
                Phone = from.Phone,
                AppUserId = from.AppUserId,
                TimeStamp = from.TimeStamp,
            };
            return to;
        }
        internal static POCO.AppCustomerUser ConvertTo(Entity.Entities.AppCustomerUser from, POCO.AppCustomerUser to)
        {
            if (to == null) to = new POCO.AppCustomerUser();
            to.AppCustomerId = from.AppCustomerId;
            to.FirstName = from.FirstName;
            to.LastName = from.LastName;
            to.Email = from.Email;
            to.Phone = from.Phone;
            to.AppUserId = from.AppUserId;
            to.TimeStamp = from.TimeStamp;
            return to;
        }
    }
}

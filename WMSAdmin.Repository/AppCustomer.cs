using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WMSAdmin.Repository
{
    public class AppCustomer : BaseRepository
    {
        public AppCustomer(Utility.Configuration configuration) : base(configuration)
        {
        }

        internal IQueryable<POCO.AppCustomer> GetQuery(Context.RepoContext db, Entity.Filter.AppCustomer filter)
        {
            var query = db.AppCustomer.AsNoTracking();
            if (filter == null) return query;

            if (filter.Id != null) query = query.Where(p => filter.Id.Value == p.Id.Value);
            if (filter.Ids?.Any() == true) query = query.Where(p => filter.Ids.Contains(p.Id.Value));

            return query;
        }

        public Entity.Entities.Response<List<Entity.Entities.AppCustomer>> Get(Entity.Filter.AppCustomer filter)
        {
            var responseData = new Entity.Entities.Response<List<Entity.Entities.AppCustomer>>()
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

        public void Save(Entity.Entities.AppCustomer item)
        {
            using (var dbContext = GetDbContext())
            {
                POCO.AppCustomer dbItem = null;

                if (item.Id.HasValue)
                {
                    dbItem = dbContext.AppCustomer.First(e => e.Id == item.Id.Value);
                    ConvertTo(item, dbItem);
                    dbContext.SaveChanges();
                    return;
                }

                dbItem = ConvertTo(item, dbItem);
                dbContext.AppCustomer.Add(dbItem);
                dbContext.SaveChanges();
                item.Id = dbItem.Id;
                return;
            }
        }

        internal static List<Entity.Entities.AppCustomer> ConvertTo(IEnumerable<POCO.AppCustomer> fromList)
        {
            var toList = new List<Entity.Entities.AppCustomer>();
            foreach (var from in fromList)
            {
                toList.Add(ConvertTo(from));
            }
            return toList;
        }
        internal static Entity.Entities.AppCustomer ConvertTo(POCO.AppCustomer from)
        {
            var to = new Entity.Entities.AppCustomer
            {
                Id = from.Id,
                CustomerNumber = from.CustomerNumber,
                CustomerName = from.CustomerName,
                Email = from.Email,
                Phone = from.Phone,
                LocaleCode= from.LocaleCode,
                OrganizationNumber= from.OrganizationNumber,
                TimeStamp = from.TimeStamp,
            };
            return to;
        }
        internal static POCO.AppCustomer ConvertTo(Entity.Entities.AppCustomer from, POCO.AppCustomer to)
        {
            if (to == null) to = new POCO.AppCustomer();
            to.CustomerNumber = from.CustomerNumber;
            to.CustomerName = from.CustomerName;
            to.Email = from.Email;
            to.Phone = from.Phone;
            to.LocaleCode = from.LocaleCode;
            to.OrganizationNumber = from.OrganizationNumber;
            to.TimeStamp = from.TimeStamp;

            return to;
        }
    }
}

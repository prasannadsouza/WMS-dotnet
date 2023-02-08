using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WMSAdmin.Entity.Entities.Config;

namespace WMSAdmin.Repository
{
    public class AppLogin : BaseRepository
    {
        public AppLogin(Utility.Configuration configuration) : base(configuration)
        {
        }

        internal IQueryable<POCO.AppLogin> GetQuery(Context.RepoContext db, Entity.Filter.AppLogin filter)
        {
            var query = db.AppLogin.AsNoTracking();
            if (filter == null) return query;

            if (filter.Id != null) query = query.Where(p => filter.Id.Value == p.Id.Value);
            if (filter.Ids?.Any() == true) query = query.Where(p => filter.Ids.Contains(p.Id.Value));

            filter.LoginId = filter?.LoginId?.Trim();
            if (string.IsNullOrEmpty(filter?.LoginId) == false)
            {
                if (filter.LoginId.Contains("%")) query = query.Where(p => EF.Functions.Like(p.LoginId, filter.LoginId));
                else query = query.Where(e => e.LoginId == filter.LoginId);
            }
            return query;
        }

        public List<Entity.Entities.AppLogin> GetAll()
        {
            using (var db = GetDbContext())
            {
                return ConvertTo(db.AppLogin);
            }
        }

        public Entity.Entities.Response<List<Entity.Entities.AppLogin>> Get(Entity.Filter.AppLogin filter)
        {
            var responseData = new Entity.Entities.Response<List<Entity.Entities.AppLogin>>()
            {
                Errors = new List<Entity.Entities.Error>(),
            };

            using (var db = GetDbContext())
            {
                var alq = GetQuery(db, filter);

                var query = from ac in alq select ac;
                responseData.Data = ConvertTo(GetOrderedResult(null, query, filter.Pagination, out Entity.Entities.Pagination newPagination));
                responseData.Pagination = filter.Pagination = newPagination;
                return responseData;
            }
        }

        public void Save(Entity.Entities.AppLogin item)
        {
            using (var dbContext = GetDbContext())
            {
                POCO.AppLogin dbItem = null;

                if (item.Id.HasValue)
                {
                    dbItem = dbContext.AppLogin.First(e => e.Id == item.Id.Value);
                    ConvertTo(item, dbItem);
                    dbContext.SaveChanges();
                    return;
                }

                dbItem = ConvertTo(item, dbItem);
                dbContext.AppLogin.Add(dbItem);
                dbContext.SaveChanges();
                item.Id = dbItem.Id;
                return;
            }
        }

        internal static List<Entity.Entities.AppLogin> ConvertTo(IEnumerable<POCO.AppLogin> fromList)
        {
            var toList = new List<Entity.Entities.AppLogin>();
            foreach (var from in fromList)
            {
                toList.Add(ConvertTo(from));
            }
            return toList;
        }

        internal static Entity.Entities.AppLogin ConvertTo(POCO.AppLogin from)
        {
            var to = new Entity.Entities.AppLogin
            {
                Id = from.Id,
                LoginId = from.LoginId,
                LoginSecret = from.LoginSecret,
                SecretKey = from.SecretKey,
                DisplayName = from.DisplayName,
                FirstName = from.FirstName,
                LastName = from.LastName,
                Email = from.Email,
                Phone = from.Phone,
                ValidTill = from.ValidTill,
                LastLoginTime = from.LastLoginTime,
                TimeStamp = from.TimeStamp,
            };
            return to;
        }

        internal static POCO.AppLogin ConvertTo(Entity.Entities.AppLogin from, POCO.AppLogin to)
        {
            if (to == null) to = new POCO.AppLogin();
            to.LoginId = from.LoginId;
            to.LoginSecret = from.LoginSecret;
            to.SecretKey = from.SecretKey;
            to.DisplayName = from.DisplayName;
            to.FirstName = from.FirstName;
            to.LastName = from.LastName;
            to.Email = from.Email;
            to.Phone = from.Phone;
            to.ValidTill = from.ValidTill;
            to.LastLoginTime = from.LastLoginTime;
            to.TimeStamp = from.TimeStamp;
            return to;
        }
    }
}

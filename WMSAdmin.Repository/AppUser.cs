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

        internal IQueryable<POCO.AppUser> GetQuery(Context.RepoContext db, Entity.Filter.AppUser filter)
        {
            var query = db.AppUser.AsNoTracking();
            if (filter == null) return query;

            if (filter.Id != null) query = query.Where(p => filter.Id.Value == p.Id.Value);
            if (filter.Ids?.Any() == true) query = query.Where(p => filter.Ids.Contains(p.Id.Value));

            filter.AuthId = filter?.AuthId?.Trim();
            if (string.IsNullOrEmpty(filter?.AuthId) == false)
            {
                if (filter.AuthId.Contains("%")) query = query.Where(p => EF.Functions.Like(p.AuthId, filter.AuthId));
                else query = query.Where(e => e.AuthId == filter.AuthId);
            }
            return query;
        }

        public Entity.Entities.Response<List<Entity.Entities.AppUser>> Get(Entity.Filter.AppUser filter)
        {
            var responseData = new Entity.Entities.Response<List<Entity.Entities.AppUser>>()
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

        public void Save(Entity.Entities.AppUser item)
        {
            using (var dbContext = GetDbContext())
            {
                POCO.AppUser dbItem = null;

                if (item.Id.HasValue)
                {
                    dbItem = dbContext.AppUser.First(e => e.Id == item.Id.Value);
                    ConvertTo(item, dbItem);
                    dbContext.SaveChanges();
                    return;
                }

                dbItem = ConvertTo(item, dbItem);
                dbContext.AppUser.Add(dbItem);
                dbContext.SaveChanges();
                item.Id = dbItem.Id;
                return;
            }
        }

        internal static List<Entity.Entities.AppUser> ConvertTo(IEnumerable<POCO.AppUser> fromList)
        {
            var toList = new List<Entity.Entities.AppUser>();
            foreach (var from in fromList)
            {
                toList.Add(ConvertTo(from));
            }
            return toList;
        }

        internal static Entity.Entities.AppUser ConvertTo(POCO.AppUser from)
        {
            var to = new Entity.Entities.AppUser
            {
                Id = from.Id,
                AppCustomerId = from.AppCustomerId,
                AppUserTypeId = from.AppUserTypeId,
                AuthId = from.AuthId,
                AuthSecret = from.AuthSecret,
                SecretKey = from.SecretKey,
                DisplayName = from.DisplayName,
                ValidTill = from.ValidTill,
                LastLoginTime = from.LastLoginTime,
                RefreshToken = from.RefreshToken,
                TimeStamp = from.TimeStamp,
            };
            return to;
        }

        internal static POCO.AppUser ConvertTo(Entity.Entities.AppUser from, POCO.AppUser to)
        {
            if (to == null) to = new POCO.AppUser();
            to.AppCustomerId = from.AppCustomerId;
            to.AppUserTypeId = from.AppUserTypeId;
            to.AuthId = from.AuthId;
            to.AuthSecret = from.AuthSecret;
            to.SecretKey = from.SecretKey;
            to.DisplayName = from.DisplayName;
            to.ValidTill = from.ValidTill;
            to.LastLoginTime = from.LastLoginTime;
            to.RefreshToken = from.RefreshToken;
            to.TimeStamp = from.TimeStamp;
            return to;
        }
    }
}

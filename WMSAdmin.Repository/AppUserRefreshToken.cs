using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WMSAdmin.Repository
{
    public class AppUserRefreshToken : BaseRepository
    {
        public AppUserRefreshToken(Utility.Configuration configuration) : base(configuration)
        {
        }

        internal IQueryable<POCO.AppUserRefreshToken> GetQuery(Context.RepoContext db, Entity.Filter.AppUserRefreshToken filter)
        {
            var query = db.AppUserRefreshToken.AsNoTracking();
            if (filter == null) return query;

            if (filter.Id != null) query = query.Where(p => filter.Id == p.Id);
            if (filter.Ids?.Any() == true) query = query.Where(p => filter.Ids.Contains(p.Id.Value));

            if (filter.RefreshToken.HasValue) query = query.Where(p => filter.RefreshToken == p.RefreshToken);
            if (filter.SessionKey.HasValue) query = query.Where(f => filter.SessionKey == filter.SessionKey);

            if (filter.FromExpiryTime.HasValue) query = query.Where(p => filter.FromExpiryTime >= p.ExpiryTime);
            if (filter.ToExpiryTime.HasValue) query = query.Where(p => filter.ToExpiryTime <= p.ExpiryTime);

            if (filter.AppUserId.HasValue == true) query = query.Where(p => filter.AppUserId == p.AppUserId);

            return query;
        }

        public Entity.Entities.Response<List<Entity.Entities.AppUserRefreshToken>> Get(Entity.Filter.AppUserRefreshToken filter)
        {
            var responseData = new Entity.Entities.Response<List<Entity.Entities.AppUserRefreshToken>>()
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

        public void Save(Entity.Entities.AppUserRefreshToken item)
        {
            using (var dbContext = GetDbContext())
            {
                POCO.AppUserRefreshToken dbItem = null;

                if (item.Id.HasValue)
                {
                    dbItem = dbContext.AppUserRefreshToken.First(e => e.Id == item.Id.Value);
                    ConvertTo(item, dbItem);
                    dbContext.SaveChanges();
                    return;
                }

                dbItem = ConvertTo(item, dbItem);
                dbContext.AppUserRefreshToken.Add(dbItem);
                dbContext.SaveChanges();
                item.Id = dbItem.Id;
                return;
            }
        }

        public static List<Entity.Entities.AppUserRefreshToken> ConvertTo(IEnumerable<POCO.AppUserRefreshToken> fromList)
        {
            var toList = new List<Entity.Entities.AppUserRefreshToken>();
            foreach (var from in fromList)
            {
                toList.Add(ConvertTo(from));
            }
            return toList;
        }
        public static Entity.Entities.AppUserRefreshToken ConvertTo(POCO.AppUserRefreshToken from)
        {
            var to = new Entity.Entities.AppUserRefreshToken
            {
                Id = from.Id,
                AppUserId = from.AppUserId,
                IssuedTime= from.IssuedTime,
                ExpiryTime = from.ExpiryTime,
                SessionKey = from.SessionKey,
                RefreshToken = from.RefreshToken,
                LastAccessedTime = from.LastAccessedTime,
                TotalRenewals= from.TotalRenewals,
                TimeStamp = from.TimeStamp,
            };
            return to;
        }
        public static POCO.AppUserRefreshToken ConvertTo(Entity.Entities.AppUserRefreshToken from, POCO.AppUserRefreshToken to = null)
        {
            if (to == null) to = new POCO.AppUserRefreshToken();
            to.AppUserId = from.AppUserId;
            to.IssuedTime = from.IssuedTime;
            to.ExpiryTime = from.ExpiryTime;
            to.SessionKey = from.SessionKey;
            to.RefreshToken = from.RefreshToken;
            to.LastAccessedTime = from.LastAccessedTime;
            to.TotalRenewals = from.TotalRenewals;
            to.TimeStamp = from.TimeStamp;
            return to;
        }
    }
}

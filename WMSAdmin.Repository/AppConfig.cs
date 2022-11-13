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
    public class AppConfig:BaseRepository
    {
        public AppConfig(Utility.Configuration configuration) : base(configuration)
        { 
        }

        internal IQueryable<POCO.AppConfig> GetQuery(Context.RepoContext db, Entity.Filter.AppConfig filter)
        {
            var query = db.AppConfig.AsNoTracking();
            if (filter == null) return query;

            if (filter.Id != null) query = query.Where(p => filter.Id.Value == p.Id.Value);
            if (filter.Ids?.Any() == true) query = query.Where(p => filter.Ids.Contains(p.Id.Value));

            filter.Code = filter?.Code?.Trim();
            if (string.IsNullOrEmpty(filter?.Code) == false)
            {
                if (filter.Code.Contains("%")) query = query.Where(p => EF.Functions.Like(p.Code, filter.Code));
                else query = query.Where(e => e.Code == filter.Code);
            }

            filter.Value = filter?.Value?.Trim();
            if (string.IsNullOrEmpty(filter?.Value) == false)
            {
                if (filter.Value.Contains("%")) query = query.Where(p => EF.Functions.Like(p.Value, filter.Value));
                else query = query.Where(e => e.Value == filter.Value);
            }

            if (filter.FromTimeStamp.HasValue) query = query.Where(p => filter.FromTimeStamp >= p.TimeStamp.Value);
            if (filter.ToTimeStamp.HasValue) query = query.Where(p => filter.ToTimeStamp <= p.TimeStamp.Value);

            if (filter.AppConfigGroup?.Id.HasValue == true) query = query.Where(p => filter.AppConfigGroup.Id.Value == p.AppConfigGroupId.Value);
            if (filter.AppConfigGroup?.Ids?.Any() == true) query = query.Where(p => filter.AppConfigGroup.Ids.Contains(p.AppConfigGroupId.Value));

            return query;
        }

        public List<Entity.Entities.AppConfig> GetAll()
        {
            using (var db = GetDbContext())
            {
                return ConvertTo(db.AppConfig);
            }
        }

        public Entity.Entities.Response<List<Entity.Entities.AppConfig>> Get(Entity.Filter.AppConfig filter)
        {
            var responseData = new Entity.Entities.Response<List<Entity.Entities.AppConfig>>()
            {
                Errors = new List<Entity.Entities.Error>(),
            };

            using (var db = GetDbContext())
            {
                var acq = GetQuery(db, filter);
                var acgRepo = new AppConfigGroup(Configuration);
                var acgq = acgRepo.GetQuery(db, filter.AppConfigGroup);

                var query = from ac in acq
                             join acg in acgq on ac.AppConfigGroupId equals acg.Id
                             select ac;
                
                responseData.Data = ConvertTo(GetOrderedResult(null, query, filter.Pagination, out Entity.Entities.Pagination newPagination));
                responseData.Pagination = filter.Pagination = newPagination;
                return responseData;
            }
        }

        public void Save(Entity.Entities.AppConfig item)
        {
            using (var dbContext = GetDbContext())
            {
                POCO.AppConfig dbItem = null;

                if (item.Id.HasValue)
                {
                    dbItem = dbContext.AppConfig.First(e => e.Id == item.Id.Value);
                    ConvertTo(item, dbItem);
                    dbContext.SaveChanges();
                    return;
                }

                dbItem = ConvertTo(item, dbItem);
                dbContext.AppConfig.Add(dbItem);
                dbContext.SaveChanges();
                item.Id = dbItem.Id;
                return;
            }
        }

        internal static List<Entity.Entities.AppConfig> ConvertTo(IEnumerable<POCO.AppConfig> fromList)
        {
            var toList = new List<Entity.Entities.AppConfig>();
            foreach (var from in fromList)
            {
                toList.Add(ConvertTo(from));
            }
            return toList;
        }

        internal static Entity.Entities.AppConfig ConvertTo(POCO.AppConfig from)
        {
            var to = new Entity.Entities.AppConfig
            {
                Id = from.Id,
                AppConfigGroupId = from.AppConfigGroupId,
                Code = from.Code,
                Value = from.Value,
                Description = from.Description,
                TimeStamp = from.TimeStamp,
                
            };
            return to;
        }

        internal static POCO.AppConfig ConvertTo(Entity.Entities.AppConfig from, POCO.AppConfig to)
        {
            if (to == null) to = new POCO.AppConfig();
            to.AppConfigGroupId = from.AppConfigGroupId.Value;
            to.Code = from.Code;
            to.Value = from.Value;
            to.Description = from.Description;
            to.TimeStamp = from.TimeStamp;
            return to;
        }

        
    }
}

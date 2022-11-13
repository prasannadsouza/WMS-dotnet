using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Repository
{
    public class LanguageText : BaseRepository
    {
        public LanguageText(Utility.Configuration configuration) : base(configuration)
        {

        }

        internal IQueryable<POCO.LanguageText> GetQuery(Context.RepoContext db, Entity.Filter.LanguageText filter)
        {
            var query = db.LanguageText.AsNoTracking();
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

            if (filter.LanguageGroup?.Id != null) query = query.Where(p => filter.LanguageGroup.Id.Value == p.LanguageGroupId.Value);
            if (filter.LanguageGroup?.Ids?.Any() == true) query = query.Where(p => filter.LanguageGroup.Ids.Contains(p.LanguageGroupId.Value));

            if (filter.LanguageCulture?.Id != null) query = query.Where(p => filter.LanguageCulture.Id.Value == p.LanguageCultureId.Value);
            if (filter.LanguageCulture?.Ids?.Any() == true) query = query.Where(p => filter.LanguageCulture.Ids.Contains(p.LanguageCultureId.Value));

            return query;
        }

        public Entity.Entities.Response<List<Entity.Entities.LanguageText>> Get(Entity.Filter.LanguageText filter)
        {
            var responseData = new Entity.Entities.Response<List<Entity.Entities.LanguageText>>()
            {
                Errors = new List<Entity.Entities.Error>(),
            };

            using (var db = GetDbContext())
            {
                var ltq = GetQuery(db, filter);

                var lgRepo = new LanguageGroup(Configuration);
                var lgq = lgRepo.GetQuery(db, filter?.LanguageGroup);
                
                var waRepo = new WMSApplication(Configuration);
                var waq = waRepo.GetQuery(db, filter?.LanguageGroup?.WMSApplication);

                var lcRepo = new LanguageCulture(Configuration);
                var lcq = lcRepo.GetQuery(db, filter.LanguageCulture);

                var query = from lt in ltq
                            join lg in lgq on lt.LanguageGroupId equals lg.Id
                            join lc in lcq on lt.LanguageCultureId equals lc.Id
                            join wa in waq on lg.WMSApplicationId equals wa.Id
                            select lt;

                responseData.Data = ConvertTo(GetOrderedResult(null, query, filter?.Pagination, out Entity.Entities.Pagination newPagination));
                responseData.Pagination = filter.Pagination = newPagination;
                return responseData;
            }
        }
        public void Save(Entity.Entities.LanguageText item)
        {
            using (var dbContext = GetDbContext())
            {
                POCO.LanguageText dbItem = null;

                if (item.Id.HasValue)
                {
                    dbItem = dbContext.LanguageText.First(e => e.Id == item.Id.Value);
                    ConvertTo(item, dbItem);
                    dbContext.SaveChanges();
                    return;
                }

                dbItem = ConvertTo(item, dbItem);
                dbContext.LanguageText.Add(dbItem);
                dbContext.SaveChanges();
                item.Id = dbItem.Id;
                return;
            }
        }
        internal static List<Entity.Entities.LanguageText> ConvertTo(IEnumerable<POCO.LanguageText> fromList)
        {
            var toList = new List<Entity.Entities.LanguageText>();
            foreach (var from in fromList)
            {
                toList.Add(ConvertTo(from));
            }
            return toList;
        }
        internal static Entity.Entities.LanguageText ConvertTo(POCO.LanguageText from)
        {
            var to = new Entity.Entities.LanguageText
            {
                Id = from.Id,
                LanguageGroupId = from.LanguageGroupId,
                LanguageCultureId = from.LanguageCultureId,
                Code = from.Code,
                Value = from.Value,
                Description = from.Description,
                TimeStamp = from.TimeStamp,
        };

            return to;
        }
        internal static POCO.LanguageText ConvertTo(Entity.Entities.LanguageText from, POCO.LanguageText to)
        {
            if (to == null) to = new POCO.LanguageText();
            to.LanguageGroupId = from.LanguageGroupId;
            to.LanguageCultureId = from.LanguageCultureId;
            to.Code = from.Code;
            to.Value = from.Value;
            to.Description = from.Description;
            to.TimeStamp = from.TimeStamp;
            return to;
        }
    }
}

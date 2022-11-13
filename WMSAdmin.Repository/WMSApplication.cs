using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSAdmin.Entity.Entities.Config;

namespace WMSAdmin.Repository
{
    public class WMSApplication : BaseRepository
    {
        public WMSApplication(Utility.Configuration configuration) : base(configuration)
        {
        }

        internal IQueryable<POCO.WMSApplication> GetQuery(Context.RepoContext db, Entity.Filter.WMSApplication filter)
        {
            var query = db.WMSApplication.AsNoTracking();
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

            if (filter.FromTimeStamp.HasValue) query = query.Where(p => filter.FromTimeStamp >= p.TimeStamp.Value);
            if (filter.ToTimeStamp.HasValue) query = query.Where(p => filter.ToTimeStamp <= p.TimeStamp.Value);

            return query;
        }

        public List<Entity.Entities.WMSApplication> GetAll()
        {
            using (var db = GetDbContext())
            {
                return ConvertTo(db.WMSApplication);
            }
        }

        public Entity.Entities.Response<List<Entity.Entities.WMSApplication>> Get(Entity.Filter.WMSApplication filter)
        {
            var responseData = new Entity.Entities.Response<List<Entity.Entities.WMSApplication>>()
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

        internal static List<Entity.Entities.WMSApplication> ConvertTo(IEnumerable<POCO.WMSApplication> fromList)
        {
            var toList = new List<Entity.Entities.WMSApplication>();
            foreach (var from in fromList)
            {
                toList.Add(ConvertTo(from));
            }
            return toList;
        }
        internal static Entity.Entities.WMSApplication ConvertTo(POCO.WMSApplication from)
        {
            var to = new Entity.Entities.WMSApplication
            {
                Id = from.Id,
                Code = from.Code,
                Name = from.Name,
                Description = from.Description,
                TimeStamp = from.TimeStamp,

            };
            return to;
        }
        internal static POCO.WMSApplication ConvertTo(Entity.Entities.WMSApplication from, POCO.WMSApplication to)
        {
            if (to == null) to = new POCO.WMSApplication();
            to.Code = from.Code;
            to.Name = from.Name;
            to.Description = from.Description;
            to.TimeStamp = from.TimeStamp;
            return to;
        }
    }
}

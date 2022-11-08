//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WMSAdmin.Repository
//{
//    public class DBColumn : BaseRepository
//    {
//        public DBColumn(RepoConfiguration configuration) : base(configuration)
//        {
//        }

//        internal IQueryable<POCO.DBColumn> GetQuery(Context.RepoContext db, Entity.Filter.DBColumn filter)
//        {
//            var query = db.DBColumn.AsNoTracking();
//            if (filter == null) return query;


//            filter.Table_Name = filter?.Table_Name?.Trim();
//            if (string.IsNullOrEmpty(filter?.Table_Name) == false)
//            {
//                if (filter.Table_Name.Contains("%")) query = query.Where(p => EF.Functions.Like(p.Table_Name, filter.Table_Name));
//                else query = query.Where(e => e.Table_Name == filter.Table_Name);
//            }

//            return query;
//        }

//        public List<Entity.Entities.DBColumn> GetAll()
//        {
//            using (var db = GetDbContext())
//            {
//                return ConvertTo(db.DBColumn);
//            }
//        }

//        public Entity.Entities.Response<List<Entity.Entities.DBColumn>> Get(Entity.Filter.DBColumn filter)
//        {
//            var responseData = new Entity.Entities.Response<List<Entity.Entities.DBColumn>>()
//            {
//                Errors = new List<Entity.Entities.Error>(),
//            };

//            using (var db = GetDbContext())
//            {
//                var query = GetQuery(db, filter);
//                responseData.Data = ConvertTo(GetOrderedResult(null, query, filter?.Pagination, out Entity.Entities.Pagination newPagination));
//                responseData.Pagination = filter.Pagination = newPagination;
//                return responseData;
//            }
//        }

//        internal static List<Entity.Entities.DBColumn> ConvertTo(IEnumerable<POCO.DBColumn> fromList)
//        {
//            var toList = new List<Entity.Entities.DBColumn>();
//            foreach (var from in fromList)
//            {
//                toList.Add(ConvertTo(from));
//            }
//            return toList;
//        }
//        internal static Entity.Entities.DBColumn ConvertTo(POCO.DBColumn from)
//        {
//            var to = new Entity.Entities.DBColumn
//            {
//                Table_Name = from.Table_Name,
//                Column_Name = from.Column_Name,
//                Is_Nullable = from.Is_Nullable,
//            };
//            return to;
//        }
//    }
//}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSAdmin.Utility;

namespace WMSAdmin.Repository
{
    public class BaseRepository
    {
        internal Configuration Configuration { get; private set; }

        internal BaseRepository(Utility.Configuration configuration)
        {
            Configuration = configuration;
        }

        protected Context.RepoContext GetDbContext()
        {
            var configuration = Configuration.ServiceProvider.GetService<IConfiguration>();
            var connectionString = configuration.GetConnectionString(Entity.Constants.AppSetting.ConnectionStrings_BaseConnection);
            var contextOptions = new DbContextOptionsBuilder<Context.RepoContext>()
                .UseSqlServer(connectionString)
                .Options;
            return new Context.RepoContext(contextOptions);
        }

        protected IQueryable<T> GetOrderedResult<T>(Dictionary<string, string> sortFieldMapper, IQueryable<T> result, Entity.Entities.Pagination pagination, out Entity.Entities.Pagination newPagination)
        {
            newPagination = SetPagination(pagination, result.Count());
            var dbPagination = GetDBPagination(sortFieldMapper, newPagination);
            var orderedResult = result as IOrderedQueryable<T>;
            return orderedResult.SortAndPage(dbPagination);
        }

        private Entity.Entities.Pagination SetPagination(Entity.Entities.Pagination pagination, int totalRecords)
        {
            var defaultPagination = GetDefaultPagination();
            if (pagination == null) pagination = defaultPagination;
            if (pagination.SortFields?.Any() != true) pagination.SortFields = defaultPagination.SortFields;

            pagination.TotalRecords = totalRecords;
            if (pagination.RecordsPerPage < 1) pagination.RecordsPerPage = defaultPagination.RecordsPerPage;
            if (pagination.CurrentPage < 1) pagination.CurrentPage = 1;
            if (pagination.RecordsPerPage > Configuration.Setting.Pagination.MaxRecordsAllowedPerPage) pagination.RecordsPerPage = Configuration.Setting.Pagination.MaxRecordsAllowedPerPage;

            var totalPages = Convert.ToDecimal(pagination.TotalRecords) / Convert.ToDecimal(pagination.RecordsPerPage);
            pagination.TotalPages = Convert.ToInt32(Math.Floor(totalPages));
            if (pagination.TotalPages != totalPages) pagination.TotalPages = Convert.ToInt32(Math.Floor(totalPages)) + 1;
            if (pagination.TotalPages <= 0) pagination.TotalPages = 1;

            if (pagination.CurrentPage <= 0) pagination.CurrentPage = 1;
            if (pagination.CurrentPage > pagination.TotalPages) pagination.CurrentPage = pagination.TotalPages;

            return pagination;
        }

        public Entity.Entities.Pagination GetDefaultPagination(bool forMaxRecordsPerPage = false)
        {
            var recordsPerPage = forMaxRecordsPerPage ? Configuration.Setting.Pagination.MaxRecordsAllowedPerPage : Configuration.Setting.Pagination.RecordsPerPage;
            return new Entity.Entities.Pagination
            {
                CurrentPage = 1,
                RecordsPerPage = recordsPerPage,
                SortFields = new List<Entity.Entities.Sort> {
                    new Entity.Entities.Sort {
                        SortColumn = Entity.Constants.Sort.Id,
                    }
                }
            };
        }

        private Entity.Entities.Pagination GetDBPagination(Dictionary<string, string> sortFieldMapper, Entity.Entities.Pagination pagination)
        {
            var dbDefaultSortField = new Entity.Entities.Sort { SortColumn = Entity.Constants.Sort.Id };
            var dbPagniation = new Entity.Entities.Pagination
            {
                RecordsPerPage = pagination.RecordsPerPage,
                CurrentPage = pagination.CurrentPage,
                SortFields = new List<Entity.Entities.Sort>() { dbDefaultSortField },
            };

            if (sortFieldMapper?.Any() == true == false) return dbPagniation;
            if (pagination.SortFields?.Any() == true == false) return dbPagniation;

            dbPagniation.SortFields.Clear();
            foreach (var sortField in pagination.SortFields)
            {
                var dbSortField = new Entity.Entities.Sort { SortColumn = sortFieldMapper[sortField.SortColumn], SortDescending = sortField.SortDescending };
                if (dbPagniation.SortFields.Any(e => e.SortColumn == sortField.SortColumn) == false) dbPagniation.SortFields.Add(dbSortField);
            }

            return dbPagniation;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Repository
{
    public class BaseRepository
    {
        internal BaseRepository(RepoConfiguration configuration)
        {
            this.Configuration = configuration;
            _sortFieldMapper = GetSortFieldMapper();
        }

        internal RepoConfiguration Configuration{ get; private set; }
        private Dictionary<string, string> _sortFieldMapper;

        private Dictionary<string, string> GetSortFieldMapper()
        {
            var sortFieldMapper = new Dictionary<string, string>();
            sortFieldMapper.Add(Entity.Constants.Sort.ID, "Id");
            sortFieldMapper.Add(Entity.Constants.Sort.ACTIVE, "Active");
            sortFieldMapper.Add(Entity.Constants.Sort.CODE, "Code");
            sortFieldMapper.Add(Entity.Constants.Sort.VALUE, "Value");
            sortFieldMapper.Add(Entity.Constants.Sort.DESCRIPTION, "Description");
            return sortFieldMapper;
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
    }
}

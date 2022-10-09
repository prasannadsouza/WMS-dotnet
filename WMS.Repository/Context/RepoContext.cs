using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Repository.Context
{
    public class RepoContext : DbContext
    {
        public RepoContext(DbContextOptions<RepoContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.AppConfig>().ToTable("AppConfig");
            modelBuilder.Entity<Domain.AppConfigGroup>().ToTable("AppConfigGroup");
        }
        public DbSet<Domain.AppConfig>? AppConfig {get; set;}
        public DbSet<Domain.AppConfigGroup>? AppConfigGroup { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Repository.Context
{
    public class RepoContext : DbContext
    {
        public RepoContext(DbContextOptions<RepoContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<POCO.AppConfig>().ToTable(nameof(POCO.AppConfig));
            modelBuilder.Entity<POCO.AppConfigGroup>().ToTable(nameof(POCO.AppConfigGroup));
            modelBuilder.Entity<POCO.LanguageGroup>().ToTable(nameof(POCO.LanguageGroup));
            modelBuilder.Entity<POCO.LanguageCulture>().ToTable(nameof(POCO.LanguageCulture));
            modelBuilder.Entity<POCO.LanguageText>().ToTable(nameof(POCO.LanguageText));
            modelBuilder.Entity<POCO.WMSApplication>().ToTable(nameof(POCO.WMSApplication));
            modelBuilder.Entity<POCO.ConfigTimeStamp>().ToTable(nameof(POCO.ConfigTimeStamp));
            modelBuilder.Entity<POCO.AppLogin>().ToTable(nameof(POCO.AppLogin));
            //modelBuilder.Entity<POCO.DBColumn>().ToView("INFORMATION_SCHEMA.COLUMNS));
        }
        public DbSet<POCO.AppConfig> AppConfig {get; set;}
        public DbSet<POCO.AppConfigGroup> AppConfigGroup { get; set; }
        public DbSet<POCO.LanguageGroup> LanguageGroup { get; set; }
        public DbSet<POCO.LanguageCulture> LanguageCulture { get; set; }
        public DbSet<POCO.LanguageText> LanguageText { get; set; }
        public DbSet<POCO.WMSApplication> WMSApplication { get; set; }
        public DbSet<POCO.ConfigTimeStamp> ConfigTimeStamp { get; set; }
        public DbSet<POCO.AppLogin> AppLogin { get; set; }
        //public DbSet<POCO.DBColumn> DBColumn { get; set; }
    }
}

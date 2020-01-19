using Microsoft.EntityFrameworkCore;
using System.IO;

namespace RepositoryNetCore
{
    public abstract class MultiTableNetCoreDBContext : DbContext
    {
        public const string dbFileName = "db.sqlite";
        public virtual DbSet<Entity1> Table1 { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //        .UseSqlite($"Data Source={_dbFilePath};");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entity1>().ToTable("Table1");
        }
    }
}

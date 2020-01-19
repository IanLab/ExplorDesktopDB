using Entitiy;
using Microsoft.EntityFrameworkCore;
using System;

namespace RepostoryNetStandard
{
    public class MyDBContext:DbContext
    {
        public virtual DbSet<Entity1> Entity1s { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite(@"Data Source=c:\\DB\\MultiTablesDb.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entity1>().ToTable("Entity1s");
        }
    }
}

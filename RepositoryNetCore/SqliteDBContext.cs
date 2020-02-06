using DBCommon;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace NetCoreSqliteDB
{   
    public class SqliteDBContext : DbContext
    {
        public static string DBTemplateFileName { get; } = $"testdb{DBFileExtensionName}";
        public const string DBFileExtensionName = ".sqlite";
        public static void CopyTempateDBFile(string destFilePath)
        {
            var srcFilePath = GetTemplateFilePath();
            File.Copy(srcFilePath, destFilePath, true);
        }

        private readonly string _filePath;

        public virtual DbSet<Entity1> Table1 { get; set; }
        public virtual DbSet<Entity2> Table2 { get; set; }
        public virtual DbSet<Entity3> Table3 { get; set; }
        public virtual DbSet<Entity4> Table4 { get; set; }
        public virtual DbSet<Entity5> Table5 { get; set; }

        public SqliteDBContext()
        {
            _filePath = GetTemplateFilePath();
        }

        private static string GetTemplateFilePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DBTemplateFileName);
        }

        public SqliteDBContext(string filePath)
        {
            _filePath = filePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite($"Data Source={_filePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entity1>().ToTable("Table1");
            modelBuilder.Entity<Entity2>().ToTable("Table2");
            modelBuilder.Entity<Entity3>().ToTable("Table3");
            modelBuilder.Entity<Entity4>().ToTable("Table4");
            modelBuilder.Entity<Entity5>().ToTable("Table5");
        }
    }
}

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
        }
    }
}

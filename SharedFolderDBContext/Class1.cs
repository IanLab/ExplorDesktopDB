using Microsoft.EntityFrameworkCore;
using RepositoryNetCore;
using System;
using System.IO;

namespace SharedFolderDBContext
{
    public class SharedFolderMultiTableNetCoreDBContext : MultiTableNetCoreDBContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbFilePath = Path.Combine(RepositoryCommon.Properties.Resources.SharedFolder,
                dbFileName);
            optionsBuilder
                .UseSqlite($"Data Source={dbFilePath};");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using RepositoryNetCore;
using System.IO;

namespace RepositoryNetCoreLocal
{
    public class LocalFolderMultiTableNetCoreDBContext: MultiTableNetCoreDBContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbFilePath = Path.Combine(RepositoryCommon.Properties.Resources.LocalFolder, 
                dbFileName);
            optionsBuilder
                .UseSqlite($"Data Source={dbFilePath};");
        }
    }
}

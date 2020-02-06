using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;
using NetCoreSqliteDB;

namespace DBCommon.Tests
{
    [TestClass]
    public class SaveChangesForEveryDataRow_5Tables_LocalFolder_Tests
    {
        private static SqliteDBContext _dbContext;

        [ClassInitialize]
        public static void Init(TestContext _)
        {
            var filePath = Path.Combine(NetCorDBTests.Properties.Resources.Local,
                     $"{nameof(SaveChangesForEveryDataRow_5Tables_LocalFolder_Tests)}.sqlite");
            SqliteDBContext.CopyTempateDBFile(
                filePath);
            _dbContext = new SqliteDBContext(filePath);
        }

        [TestMethod]
        public  async Task AddDataRowsTestAsync()
        {
            for(int batchId = 0; batchId < 2; batchId++)
            {
                for(int r = 0; r < 1000; r++)
                {
                    _dbContext.Table1.Add(AddDataRowsHelper.CreateDataRow<Entity1>(batchId, r));
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        [ClassCleanup]
        public static void Clean()
        {
            _dbContext.Dispose();
        }

    }
}
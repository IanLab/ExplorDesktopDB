using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBCommon;
using System.IO;
using NetCoreSqliteDB;

namespace DBCommon.Tests
{
    


    [TestClass()]
    public class OneDBContextForAllBatch_5Tables_SharedFolder_Tests
    {
        private static SqliteDBContext[] _dbContexts;

        [ClassInitialize]
        public static void Init(TestContext _)
        {
            _dbContexts = new SqliteDBContext[2];
            for (int i = 0; i < _dbContexts.Length; i++)
            {
                var filePath = Path.Combine(NetCorDBTests.Properties.Resources.Shared,
                    $"{nameof(OneDBContextForAllBatch_5Tables_SharedFolder_Tests)}{i}.sqlite");
                SqliteDBContext.CopyTempateDBFile(
                    filePath);
                _dbContexts[i] = new SqliteDBContext(filePath);
            }
        }


        [TestMethod(),DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(7)]
        [DataRow(8)]
        [DataRow(9)]
        [DataRow(10)]
        [DataRow(11)]
        [DataRow(12)]
        [DataRow(13)]
        [DataRow(14)]
        [DataRow(15)]
        public void Add5000DataRows_OneDBContextForAllBatch_Test(int batchId)
        {
            var dbContext = _dbContexts[0];
            AddDataRowsHelper.Add1BatchDataRows(dbContext.Table1, 
                batchId, 
                5000);
            dbContext.SaveChanges();
        }


        [TestMethod(),DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(7)]
        [DataRow(8)]
        [DataRow(9)]
        [DataRow(10)]
        [DataRow(11)]
        [DataRow(12)]
        [DataRow(13)]
        [DataRow(14)]
        [DataRow(15)]
        public void Add10000DataRows_OneDBContextForAllBatch_Test(int batchId)
        {
            var dbContext = _dbContexts[1];
            AddDataRowsHelper.Add1BatchDataRows(dbContext.Table1,
                batchId,
                10000);
            dbContext.SaveChanges();
        }


        [ClassCleanup]
        public static void Clean()
        {
            foreach(var dbContext in _dbContexts)
            {
                dbContext.Dispose();
            }
        }

    }
}
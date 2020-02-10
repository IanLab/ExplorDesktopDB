using DBCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCoreSqliteDB;
using System.IO;
using System.Linq;

namespace NetCorDBTests
{
    [TestClass]
    public class AddDataRowsToTable5ReadFromTable1SqliteDBTests
    {
        private static string[] _dbFiles = new string[]{ Path.Combine(Properties.Resources.Shared,
                $"{nameof(AddDataRowsToTable5ReadFromTable1SqliteDBTests)}10000.sqlite"),
        Path.Combine(Properties.Resources.Shared,
                $"{nameof(AddDataRowsToTable5ReadFromTable1SqliteDBTests)}50000.sqlite"),
        Path.Combine(Properties.Resources.Local,
                $"{nameof(AddDataRowsToTable5ReadFromTable1SqliteDBTests)}10000.sqlite"),
        Path.Combine(Properties.Resources.Local,
                $"{nameof(AddDataRowsToTable5ReadFromTable1SqliteDBTests)}50000.sqlite") };


        [ClassInitialize]
        public static void Init(TestContext _)
        {
            foreach (var dbFile in _dbFiles)
            {
                SqliteDBContext.CopyTempateDBFile(dbFile);
                AddDataRowsToTable1(dbFile);
            }
            AddDataRowsToTable5(_dbFiles[1]);
            AddDataRowsToTable5(_dbFiles[3]);
        }

        private static void AddDataRowsToTable5(string filePath)
        {
            using var dbCotnext = new SqliteDBContext(filePath);
            for (int i = 2; i < 10; i++)
            {
                AddDataRowsHelper.Add1BatchDataRows<Entity1>(dbCotnext.Table1, i);
                dbCotnext.SaveChanges();
            }
        }

        private static void AddDataRowsToTable1(string filePath)
        {
            using var dbContext1 = new SqliteDBContext(filePath);
            AddDataRowsHelper.Add1BatchDataRows<Entity1>(dbContext1.Table1, 0);
            dbContext1.SaveChanges();
            using var dbContext2 = new SqliteDBContext(filePath);
            AddDataRowsHelper.Add1BatchDataRows<Entity1>(dbContext2.Table1, 1);
            dbContext2.SaveChanges();
        }

        [TestMethod, DataRow(0)]
        [DataRow(1)]
        public void TestReadFromSharedFolder10000DataRows(int batchId)
        {
            using var dbContext = new SqliteDBContext(_dbFiles[0]);
            //var allRows = (from i in dbContext.Table1 where i.BatchId == batchId orderby i.RowNo select i).ToArray();
            var rows = (from i in dbContext.Table1 where i.BatchId == batchId && i.RowNo < 10 select i).ToArray();
            Assert.AreEqual(10, rows.Length);
        }

        [TestMethod, DataRow(0)]
        [DataRow(1)]
        public void TestReadFromSharedFolder50000DataRows(int batchId)
        {
            using var dbContext = new SqliteDBContext(_dbFiles[1]);
            //var allRows = (from i in dbContext.Table1 where i.BatchId == batchId orderby i.RowNo select i).ToArray();
            var rows = (from i in dbContext.Table1 where i.BatchId == batchId && i.RowNo < 10 select i).ToArray();
            Assert.AreEqual(10, rows.Length);
        }

        [TestMethod, DataRow(0)]
        [DataRow(1)]
        public void TestReadFromSharedLocal10000DataRows(int batchId)
        {
            using var dbContext = new SqliteDBContext(_dbFiles[2]);
            //var allRows = (from i in dbContext.Table1 where i.BatchId == batchId orderby i.RowNo select i).ToArray();
            var rows = (from i in dbContext.Table1 where i.BatchId == batchId && i.RowNo < 10 select i).ToArray();
            Assert.AreEqual(10, rows.Length);
        }

        [TestMethod, DataRow(0)]
        [DataRow(1)]
        public void TestReadFromSharedLocal50000DataRows(int batchId)
        {
            using var dbContext = new SqliteDBContext(_dbFiles[3]);
            //var allRows = (from i in dbContext.Table1 where i.BatchId == batchId orderby i.RowNo select i).ToArray();
            var rows = (from i in dbContext.Table1 where i.BatchId == batchId && i.RowNo < 10 select i).ToArray();
            Assert.AreEqual(10, rows.Length);
        }
    }
}

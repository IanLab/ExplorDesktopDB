using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositoryCommon;
using RepositoryNetCoreLocal;
using SharedFolderDBContext;
using System.Linq;

namespace RepositoryNetCore.Tests
{

    [TestClass()]
    public class MultiTableNetCoreDBContextInitTests_SharedFolder
    {
        [ClassInitialize]
        public static void CleanEntity1s(TestContext _)
        {
            using var dbc = new SharedFolderMultiTableNetCoreDBContext();
            const int oneBatchRowCount = 10000;
            while (true)
            {
                var dataRows = (from e in dbc.Table1 select e).Take(oneBatchRowCount);
                if (dataRows.Any() == false)
                {
                    break;
                }
                dbc.Table1.RemoveRange(dataRows);
                dbc.SaveChanges();
            }

        }

        [TestMethod()]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(7)]
        [DataRow(8)]
        [DataRow(9)]
        public void Add5000DataRowsEntity1sTest(int batchId)
        {

            using var dbc = new SharedFolderMultiTableNetCoreDBContext();
            AddDataRowsHelper.Add5000DataRows(dbc.Table1, batchId);
            dbc.SaveChanges();
            //Assert.AreEqual(5000 * 10 , (from e in dbc.Entity1s where e.BatchId == batchId select e).Count());
        }

        [TestMethod]
        public void Add1DataRowTest()
        {
            using var dbc = new SharedFolderMultiTableNetCoreDBContext();
            AddDataRowsHelper.Add1DataRow(dbc.Table1);
            dbc.SaveChanges();
        }
    }

    [TestClass()]
    public class MultiTableNetCoreDBContextInitTests_LocalFolder
    {
        [ClassInitialize]
        public static void CleanEntity1s(TestContext _)
        {
            using var dbc = new LocalFolderMultiTableNetCoreDBContext();
            const int oneBatchRowCount = 10000;
            while (true)
            {
                var dataRows = (from e in dbc.Table1 select e).Take(oneBatchRowCount).ToArray();
                if (dataRows.Any() == false)
                {
                    break;
                }
                dbc.Table1.RemoveRange(dataRows);
                dbc.SaveChanges();
            }

        }

        [TestMethod()]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(7)]
        [DataRow(8)]
        [DataRow(9)]
        public void Add5000DataRowsEntity1sTest(int batchId)
        {
            using var dbc = new LocalFolderMultiTableNetCoreDBContext();
            
            AddDataRowsHelper.Add5000DataRows(dbc.Table1, batchId);
            dbc.SaveChanges();
            //Assert.AreEqual(5000 * 10 , (from e in dbc.Entity1s where e.BatchId == batchId select e).Count());
        }

        [TestMethod]
        public void Add1DataRowTest()
        {
            using var dbc = new LocalFolderMultiTableNetCoreDBContext();
            AddDataRowsHelper.Add1DataRow(dbc.Table1);
            dbc.SaveChanges();
        }
    }
}
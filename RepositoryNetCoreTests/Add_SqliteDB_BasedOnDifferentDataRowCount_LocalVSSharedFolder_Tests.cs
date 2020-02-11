using DBCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCoreSqliteDB;
using System.IO;
using System.Linq;

namespace NetCorDBTests
{
    [TestClass]
    public class Add_SqliteDB_BasedOnDifferentDataRowCount_LocalVSSharedFolder_Tests
    {
        private static string[] _localDbFilePaths;
        private static string[] _sharedFolderDbFilePaths;


        [ClassInitialize]
        public static void Init(TestContext _)
        {
            _localDbFilePaths = new string[10];
            _sharedFolderDbFilePaths = new string[10];

            for(int i = 0;i< _localDbFilePaths.Length;i++)
            {
                _localDbFilePaths[i] = Path.Combine(Properties.Resources.Local, $"{nameof(Add_SqliteDB_BasedOnDifferentDataRowCount_LocalVSSharedFolder_Tests)}{i}{SqliteDBContext.DBFileExtensionName}");
                _sharedFolderDbFilePaths[i] = Path.Combine(Properties.Resources.Shared, $"{nameof(Add_SqliteDB_BasedOnDifferentDataRowCount_LocalVSSharedFolder_Tests)}{i}{SqliteDBContext.DBFileExtensionName}");
            }

            for(int i = 0; i < _localDbFilePaths.Length; i++)
            {
                SqliteDBContext.CopyTempateDBFile(_localDbFilePaths[i]);
                using var localDbContext = new SqliteDBContext(_localDbFilePaths[i]);
                for(int j = 0; j < i+ 1;j++)
                {
                    AddDataRowsHelper.Add1BatchDataRows<Entity1>(localDbContext.Table1, j);
                }
                localDbContext.SaveChanges();
                File.Copy(_localDbFilePaths[i], _sharedFolderDbFilePaths[i], true);
            }
        }


        

        [TestMethod, DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(7)]
        [DataRow(8)]
        [DataRow(9)]

        public void Add100DataRowsToLocalDb(int dbFilePathIndex)
        {
            using var dbContext = new SqliteDBContext(_localDbFilePaths[dbFilePathIndex]);
            var dataRows = new Entity1[100];
            for (int i = 0; i < 100; i++)
            {
                dataRows[i] = AddDataRowsHelper.CreateDataRow<Entity1>(dbFilePathIndex, i + 5000);
            }
            dbContext.AddRange(dataRows);
            dbContext.SaveChanges();
            Assert.AreEqual(100, dbContext.Table1.Where(i => i.BatchId == dbFilePathIndex && i.RowNo >= 5000).Count());
        }

        [TestMethod, DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(7)]
        [DataRow(8)]
        [DataRow(9)]

        public void Add100DataRowsToSharedFolderDb(int dbFilePathIndex)
        {
            using var dbContext = new SqliteDBContext(_sharedFolderDbFilePaths[dbFilePathIndex]);
            var dataRows = new Entity1[100];
            for (int i = 0; i < 100; i++)
            {
                dataRows[i] = AddDataRowsHelper.CreateDataRow<Entity1>(dbFilePathIndex, i + 5000);
            }
            dbContext.AddRange(dataRows);
            dbContext.SaveChanges();
            Assert.AreEqual(100, dbContext.Table1.Where(i => i.BatchId == dbFilePathIndex && i.RowNo >= 5000).Count());
        }
       

        [ClassCleanup]
        public static void Clean()
        {
            foreach (var f in _localDbFilePaths)
            {
                if (File.Exists(f))
                {
                    File.Delete(f);
                }
            }

            foreach(var f in _sharedFolderDbFilePaths)
            {
                if(File.Exists(f))
                {
                    File.Delete(f);
                }
            }
        }
    }
}

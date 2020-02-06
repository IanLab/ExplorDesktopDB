using DBCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using NetCoreSqliteDB;
using Microsoft.EntityFrameworkCore;
using NetCoreSqlExpressDB;

namespace NetCorDBTests
{
    [TestClass]
    public class Read_Tests
    {
        private static string[] _localDbFiles;
        private static string[] _sharedFolderDbFilePaths;

        private static void InitFilePath()
        {
            _localDbFiles = new string[10];
            _sharedFolderDbFilePaths = new string[10];
            for (int i = 0; i < _localDbFiles.Length;i++)
            {
                _localDbFiles[i] = Path.Combine(Properties.Resources.Local,
                    $"{nameof(Read_Tests)}{i}");
                _sharedFolderDbFilePaths[i] = Path.Combine(Properties.Resources.Shared,
                    $"{nameof(Read_Tests)}{i}");
            }
        }

        private static void AddFirst5000DataRowsToDb<T>(Func<string, T> dbCreator,
            Action<string> copyFromTemplate,
            string dbFileExtensionName)
            where T: DbContext, I5TablesDbContext,IDisposable
        {
            var dbFile = _localDbFiles[0] + dbFileExtensionName;
            if (!File.Exists(dbFile))
            {
                copyFromTemplate(dbFile);
                using var dbContext = dbCreator(dbFile);
                AddDataRowsHelper.Add1BatchDataRows(dbContext.Table1, 0);
                dbContext.SaveChanges();
                dbContext.Database.CloseConnection();
            }
            var sharedDbFile = _sharedFolderDbFilePaths[0] + dbFileExtensionName;
            if (!File.Exists(sharedDbFile))
            {
                File.Copy(dbFile, sharedDbFile);
            }
        }

        private static void Append5000DataRowsToDb<T>(Func<string, T> dbCreator, int batchId, string dbFileExtensionName)
            where T : DbContext, I5TablesDbContext, IDisposable
        {
            var preLocalDbFile = _localDbFiles[batchId - 1] + dbFileExtensionName;
            var localDbFile = _localDbFiles[batchId] + dbFileExtensionName;
            if(!File.Exists(localDbFile))
            {
                File.Copy(preLocalDbFile, localDbFile);
                using var dbContext = dbCreator(localDbFile);
                AddDataRowsHelper.Add1BatchDataRows(dbContext.Table1, batchId);
                dbContext.SaveChanges();
                dbContext.Database.CloseConnection();
            }

            var sharedFolderDbFile = _localDbFiles[batchId];
            if(!File.Exists(sharedFolderDbFile))
            {
                File.Copy(localDbFile, sharedFolderDbFile);
            }
        }



        [ClassInitialize]
        public static void Init(TestContext _)
        {
            InitFilePath();
            AddFirst5000DataRowsToDb((f)=> { return new SqliteDBContext(f); }, 
                (f) => { SqliteDBContext.CopyTempateDBFile(f); }, 
                SqliteDBContext.DBFileExtensionName);

            AddFirst5000DataRowsToDb((f) => { return new SqlExpressDbConext(f); },
                (f) => { SqlExpressDbConext.CopyTempateDBFile(f); },
                SqlExpressDbConext.DBFileExtensionName);


            for(int i = 0; i < _sharedFolderDbFilePaths.Length;i++)
            {
                Append5000DataRowsToDb((f) => { return new SqliteDBContext(f); },
                i,
                SqliteDBContext.DBFileExtensionName);

                Append5000DataRowsToDb((f) => { return new SqlExpressDbConext(f); },
                i,
                SqlExpressDbConext.DBFileExtensionName);
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
        public void TestRead_Sqlite_FromLocalFolder(int dbFilePathIndex)
        {
            using var dbContext = new SqliteDBContext(_localDbFiles[dbFilePathIndex] + SqliteDBContext.DBFileExtensionName);
            var p3s = new string[10];
            for (int r = 0; r < 10; r++)
            {
                p3s[r] = $"{r} RowNo.";
            }
            var rows = (from i in dbContext.Table1 where i.BatchId == 0 && p3s.Contains(i.RowNo) select i).ToArray();
            Assert.AreEqual(10, rows.Count());
        }

        [TestMethod,DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(7)]
        [DataRow(8)]
        [DataRow(9)]
        public void TestRead_Sqlite_FromSharedFolder(int dbFilePathIndex)
        {
            using var dbContext = new SqliteDBContext(_sharedFolderDbFilePaths[dbFilePathIndex] + SqliteDBContext.DBFileExtensionName);
            var p3s = new string[10];
            for (int r = 0; r < 10; r++)
            {
                p3s[r] = $"{r} RowNo.";
            }
            var rows = (from i in dbContext.Table1 where i.BatchId == 0 && p3s.Contains(i.RowNo) select i).ToArray();
            Assert.AreEqual(10, rows.Count());
        }
    }
}

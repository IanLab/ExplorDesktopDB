﻿using DBCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using NetCoreSqliteDB;
using Microsoft.EntityFrameworkCore;
using LiteDB;

namespace NetCorDBTests
{

    [TestClass]
    public class ReadFromLocalDbVSSharedFolderDbTests
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
                    $"{nameof(ReadFromLocalDbVSSharedFolderDbTests)}{i}{SqliteDBContext.DBFileExtensionName}");
                _sharedFolderDbFilePaths[i] = Path.Combine(Properties.Resources.Shared,
                    $"{nameof(ReadFromLocalDbVSSharedFolderDbTests)}{i}{SqliteDBContext.DBFileExtensionName}");
            }
        }

        private static void AddFirst5000DataRowsToDb()
        {
            var dbFile = _localDbFiles[0];
            if (!File.Exists(dbFile))
            {
                SqliteDBContext.CopyTempateDBFile(dbFile);
                using var dbContext = new SqliteDBContext(dbFile);
                AddDataRowsHelper.Add1BatchDataRows(dbContext.Table1, 0);
                dbContext.SaveChanges();
                dbContext.Database.CloseConnection();
            }
            var sharedDbFile = _sharedFolderDbFilePaths[0];
            if (!File.Exists(sharedDbFile))
            {
                File.Copy(dbFile, sharedDbFile);
            }
        }


        [ClassInitialize]
        public static void Init(TestContext _)
        {
            InitFilePath();
            AddFirst5000DataRowsToDb();

            for(int batchId = 1; batchId < _sharedFolderDbFilePaths.Length; batchId++)
            {
                var preLocalDbFile = _localDbFiles[batchId - 1];
                var localDbFile = _localDbFiles[batchId];
                if (!File.Exists(localDbFile))
                {
                    File.Copy(preLocalDbFile, localDbFile);
                    using var dbContext = new SqliteDBContext(localDbFile);
                    AddDataRowsHelper.Add1BatchDataRows(dbContext.Table1, batchId);
                    dbContext.SaveChanges();
                    dbContext.Database.CloseConnection();
                }

                var sharedFolderDbFile = _localDbFiles[batchId];
                if (!File.Exists(sharedFolderDbFile))
                {
                    File.Copy(localDbFile, sharedFolderDbFile);
                }
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
        public void TestReadFromLocalFolder(int dbFilePathIndex)
        {
            using var dbContext = new SqliteDBContext(_localDbFiles[dbFilePathIndex]);
            var p3s = new string[10];
            var rows = (from i in dbContext.Table1 where i.BatchId == 0 && i.RowNo <10 select i).ToArray();
            Assert.AreEqual(10, rows.Length);
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
        public void TestReadFromSharedFolder(int dbFilePathIndex)
        {
            using var dbContext = new SqliteDBContext(_sharedFolderDbFilePaths[dbFilePathIndex]);
            var rows = (from i in dbContext.Table1 where i.BatchId == 0 && i.RowNo < 10 select i).ToArray();
            Assert.AreEqual(10, rows.Count());
        }
    }
}

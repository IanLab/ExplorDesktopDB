﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBCommon;
using System.IO;
using NetCoreSqliteDB;

namespace DBCommon.Tests
{

    [TestClass()]
    public class NewDBContextForEveryBatch_5Tables_LocalFolder_Tests
    {
        private static string[] _dbFilePaths;

        [ClassInitialize]
        public static void Init(TestContext _)
        {
            _dbFilePaths = new string[2];
            for (int i = 0; i < _dbFilePaths.Length; i++)
            {
                _dbFilePaths[i] = Path.Combine(NetCorDBTests.Properties.Resources.Local,
                    $"{nameof(NewDBContextForEveryBatch_5Tables_LocalFolder_Tests)}{i}.sqlite");
                SqliteDBContext.CopyTempateDBFile(
                    _dbFilePaths[i]);
            }
        }

        [TestMethod, DataRow(1)]
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
        public void Add1Batch5000DataRowsTest(int batchId)
        {
            var dbFilePath = _dbFilePaths[0];
            using var dbContext = new SqliteDBContext(dbFilePath);
            AddDataRowsHelper.Add1BatchDataRows(dbContext.Table1, 
                batchId, 
                5000);
            dbContext.SaveChanges();
        }

        [TestMethod, DataRow(1)]
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
        public void Add1Batch10000DataRowsTest(int batchId)
        {
            var dbFilePath = _dbFilePaths[1];
            using var dbContext = new SqliteDBContext(dbFilePath);
            AddDataRowsHelper.Add1BatchDataRows(dbContext.Table1, 
                batchId, 
                10000);
            dbContext.SaveChanges();
        }
    }
}
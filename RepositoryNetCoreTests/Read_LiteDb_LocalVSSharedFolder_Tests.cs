using DBCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LiteDB;

namespace NetCorDBTests
{
    [TestClass]
    public class Read_LiteDb_LocalVSSharedFolder_Tests
    {
        private static string[] _localDbFiles;
        private static string[] _sharedFolderDbFilePaths;
        private const string FileExtensionName = ".LiteDb";

        [ClassInitialize]
        public static void Init(TestContext tc)
        {
            InitFilePath();

            AddFirst5000DataRowsToDb();

            for (int batchId = 1; batchId < 10; batchId++)
            {
                var preLocalDbFile = _localDbFiles[batchId - 1];
                var localDbFile = _localDbFiles[batchId];
                if (!File.Exists(localDbFile))
                {
                    File.Copy(preLocalDbFile, localDbFile);
                    Add1BatchDataRows(batchId);
                }

                var sharedFolderDbFile = _sharedFolderDbFilePaths[batchId];
                if (!File.Exists(sharedFolderDbFile))
                {
                    File.Copy(localDbFile, sharedFolderDbFile);
                }
            }
        }

        private static void InitFilePath()
        {
            _localDbFiles = new string[10];
            _sharedFolderDbFilePaths = new string[10];
            for (int i = 0; i < _localDbFiles.Length; i++)
            {
                _localDbFiles[i] = Path.Combine(Properties.Resources.Local,
                    $"{nameof(Read_LiteDb_LocalVSSharedFolder_Tests)}{i}{FileExtensionName}");
                _sharedFolderDbFilePaths[i] = Path.Combine(Properties.Resources.Shared,
                    $"{nameof(Read_LiteDb_LocalVSSharedFolder_Tests)}{i}{FileExtensionName}");
            }
            
        }

        private static void AddFirst5000DataRowsToDb()
        {
            var dbFile = _localDbFiles[0];
            if (!File.Exists(dbFile))
            {
                Add1BatchDataRows(0);
            }
            var sharedDbFile = _sharedFolderDbFilePaths[0];
            if (!File.Exists(sharedDbFile))
            {
                File.Copy(dbFile, sharedDbFile);
            }
        }

        private static void Add1BatchDataRows(int batchId)
        {
            using var db = new LiteDatabase(_localDbFiles[batchId]);
            var collection = db.GetCollection<Entity1>();
            for(int r = 0; r < 5000;r++)
            {
                var row = AddDataRowsHelper.CreateDataRow<Entity1>(batchId, r);
                collection.Insert(row);
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
            using var db = new LiteDatabase(_localDbFiles[dbFilePathIndex]);
            var collection = db.GetCollection<Entity1>();
            var rowNos = new string[10]; 
            var rows = collection.Query().Where(i => i.BatchId == 0 && i.RowNo < 10).ToArray();

            Assert.AreEqual(10, rows.Length);
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
        public void TestReadFromSharedFolder(int dbFilePathIndex)
        {
            using var db = new LiteDatabase(_sharedFolderDbFilePaths[dbFilePathIndex]);
            var collection = db.GetCollection<Entity1>();
            var p3s = new string[10];
            var rows = collection.Query().Where(i => i.BatchId == 0 && i.RowNo < 10).ToArray();

            Assert.AreEqual(10, rows.Count());
        }

    }
}

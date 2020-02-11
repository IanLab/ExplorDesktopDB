using DBCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using NetCoreSqliteDB;

namespace NetCorDBTests
{
    [TestClass]
    public class Edit_SqliteDb_CmmdToSharedFolderToLocalDbVSToSharedFolderTest
    {
        private static CommandReceiver _cmmdReceiver;
        private static string _localDbFilePath;
        private static string _sharedFolderDbFilePath;
        private static SqliteDBRepository _localRepository;
        private static SqliteDBRepository _sharedFolderRepository;
        private static Entity1[] _entitiy1000;
        private static CommandSender _cmmdSender;

        [ClassInitialize]
        public static void Init(TestContext tc)
        {
            CommandFile.SharedFolder = "A:\\Cmmd";
            _localDbFilePath = Path.Combine(Properties.Resources.Local, $"{nameof(Edit_SqliteDb_CmmdToSharedFolderToLocalDbVSToSharedFolderTest)}{SqliteDBContext.DBFileExtensionName}");
            _sharedFolderDbFilePath = Path.Combine(Properties.Resources.Shared, $"{nameof(Edit_SqliteDb_CmmdToSharedFolderToLocalDbVSToSharedFolderTest)}{SqliteDBContext.DBFileExtensionName}");

            NetCoreSqliteDB.SqliteDBContext.CopyTempateDBFile(_localDbFilePath);
            

            _localRepository = new SqliteDBRepository(_localDbFilePath);
            _sharedFolderRepository = new SqliteDBRepository(_sharedFolderDbFilePath);

            _entitiy1000 = new Entity1[1000];
            for (int i = 0; i < 1000; i++)
            {
                _entitiy1000[i] = AddDataRowsHelper.CreateDataRow<Entity1>(0, i);
            }

            var localDbContext = new NetCoreSqliteDB.SqliteDBContext(_localDbFilePath);
            localDbContext.Table1.AddRange(_entitiy1000);
            localDbContext.SaveChanges();

            File.Copy(_localDbFilePath, _sharedFolderDbFilePath,true);

            foreach(var e in _entitiy1000)
            {
                e.P3 = "Edited";
                e.BasedOnUpdatedDateTime = e.UpdatedDateTime;
                e.UpdatedDateTime = DateTime.Now;
            }

            _cmmdReceiver = new CommandReceiver(new JsonSer(), _localRepository);
            _cmmdReceiver.Start();

            _cmmdSender = new CommandSender(new JsonSer());
        }

        [TestMethod]
        public void TestUpdateDataToSharedDB()
        {
            foreach (var e in _entitiy1000)
            {
                _sharedFolderRepository.Save(e);
            }

            using var dbContext = new NetCoreSqliteDB.SqliteDBContext(_sharedFolderDbFilePath);
            var updateedRowCount = (from e in dbContext.Table1 where e.P3 == "Edited" select e).Count();
            Assert.AreEqual(1000, updateedRowCount);
        }

        [TestMethod]
        public void TestUpdateDataToLocalDbByCmmd()
        {
            foreach (var e in _entitiy1000)
            {
                _cmmdSender.Send(e);
            }
            _cmmdSender.Send(new CompleteEntity());

            for (int i = 0; i < 100; i++)
            {
                if (!_cmmdReceiver.IsRuning)
                {
                    break;
                }
                System.Threading.Thread.Sleep(100);
            }

            Assert.IsFalse(_cmmdReceiver.IsRuning);

            using var dbContext = new NetCoreSqliteDB.SqliteDBContext(_localDbFilePath);
            var updateedRowCount = (from e in dbContext.Table1 where e.P3 == "Edited" select e).Count();
            Assert.AreEqual(1000, updateedRowCount);
        }

        [ClassCleanup]
        public static void Clean()
        {
            _cmmdReceiver.Stop();
            _cmmdReceiver.Dispose();

            var cmmdFiles = Directory.EnumerateFiles(CommandFile.SharedFolder, $"*{CommandFile.FileExtension}").ToArray();
            Parallel.ForEach(cmmdFiles, (f) => File.Delete(f));

            File.Delete(_localDbFilePath);
            File.Delete(_sharedFolderDbFilePath);
        }
    }
}

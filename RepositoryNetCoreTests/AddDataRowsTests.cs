using DBCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace NetCorDBTests
{


    [TestClass]
    public class AddDataRowsTests
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
            _localDbFilePath = Path.Combine(Properties.Resources.Local, $"{nameof(AddDataRowsTests)}1.db");
            _sharedFolderDbFilePath = Path.Combine(Properties.Resources.Shared,  $"{nameof(AddDataRowsTests)}2.db");

            NetCoreSqliteDB.SqliteDBContext.CopyTempateDBFile(_localDbFilePath);
            NetCoreSqliteDB.SqliteDBContext.CopyTempateDBFile(_sharedFolderDbFilePath);

            _localRepository = new SqliteDBRepository(_localDbFilePath);
            _sharedFolderRepository = new SqliteDBRepository(_sharedFolderDbFilePath);

            _entitiy1000 = new Entity1[1000];
            for(int i = 0; i < 1000; i++)
            {
                _entitiy1000[i] = AddDataRowsHelper.CreateDataRow<Entity1>(0, i);
            }

            _cmmdReceiver = new CommandReceiver(new JsonSer(), _localRepository);
            _cmmdReceiver.Start();

            _cmmdSender = new CommandSender(new JsonSer());
        }

        [TestMethod]
        public void AddDataRowsToSharedDB()
        {
            foreach(var e in _entitiy1000)
            {
                _sharedFolderRepository.Save(e);
            }

            using var dbContext = new NetCoreSqliteDB.SqliteDBContext(_sharedFolderDbFilePath);
            var addedRowCount = dbContext.Table1.Count();
            Assert.AreEqual(1000, addedRowCount);
        }

        [TestMethod]
        public void AddDataRowsToLocalDbByCmmd()
        {
            foreach(var e in _entitiy1000)
            {
                _cmmdSender.Send(e);
            }
            _cmmdSender.Send(new CompleteEntity());

            for(int i = 0;i < 100;i++)
            {
                if(!_cmmdReceiver.IsRuning)
                {
                    break;
                }
                System.Threading.Thread.Sleep(100);
            }

            Assert.IsFalse(_cmmdReceiver.IsRuning);

            using var dbContext = new NetCoreSqliteDB.SqliteDBContext(_localDbFilePath);
            var addedRowCount = dbContext.Table1.Count();
            Assert.AreEqual(1000, addedRowCount);
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

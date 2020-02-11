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
    public class Add_SqliteDb_CmmdToSharedFolderToLocalDbVSDirectlyToLocalFolderVSToSharedFolderTest
    {
        private static CommandReceiver _cmmdReceiver;
        private static string _cmmdTolocalDbFilePath;
        private static string _directlyTolocalDbFilePath;
        private static string _sharedFolderDbFilePath;
        private static SqliteDBRepository _cmmdLocalRepository;
        private static SqliteDBRepository _directlyToLocalRepository;
        private static SqliteDBRepository _sharedFolderRepository;
        private static Entity1[] _entitiy1000;
        private static CommandSender _cmmdSender;

        [ClassInitialize]
        public static void Init(TestContext tc)
        {
            CommandFile.SharedFolder = "A:\\Cmmd";
            _cmmdTolocalDbFilePath = Path.Combine(Properties.Resources.Local, $"{nameof(Add_SqliteDb_CmmdToSharedFolderToLocalDbVSDirectlyToLocalFolderVSToSharedFolderTest)}_local{NetCoreSqliteDB.SqliteDBContext.DBFileExtensionName}");
            _directlyTolocalDbFilePath = Path.Combine(Properties.Resources.Local, $"{nameof(Add_SqliteDb_CmmdToSharedFolderToLocalDbVSDirectlyToLocalFolderVSToSharedFolderTest)}_directlyToLocal{NetCoreSqliteDB.SqliteDBContext.DBFileExtensionName}");
            _sharedFolderDbFilePath = Path.Combine(Properties.Resources.Shared,  $"{nameof(Add_SqliteDb_CmmdToSharedFolderToLocalDbVSDirectlyToLocalFolderVSToSharedFolderTest)}_sharedFolder{NetCoreSqliteDB.SqliteDBContext.DBFileExtensionName}");

            NetCoreSqliteDB.SqliteDBContext.CopyTempateDBFile(_cmmdTolocalDbFilePath);
            NetCoreSqliteDB.SqliteDBContext.CopyTempateDBFile(_directlyTolocalDbFilePath);
            NetCoreSqliteDB.SqliteDBContext.CopyTempateDBFile(_sharedFolderDbFilePath);

            _cmmdLocalRepository = new SqliteDBRepository(_cmmdTolocalDbFilePath);
            _directlyToLocalRepository = new SqliteDBRepository(_directlyTolocalDbFilePath);
            _sharedFolderRepository = new SqliteDBRepository(_sharedFolderDbFilePath);

            _entitiy1000 = new Entity1[1000];
            for(int i = 0; i < 1000; i++)
            {
                _entitiy1000[i] = AddDataRowsHelper.CreateDataRow<Entity1>(0, i);
            }

            _cmmdReceiver = new CommandReceiver(new JsonSer(), _cmmdLocalRepository);
            _cmmdReceiver.Start();

            _cmmdSender = new CommandSender(new JsonSer());
        }

        [TestMethod]
        public void TestAddDataToSharedFolderDb()
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
        public void TestAddDataToLocalDbDirectly()
        {
            foreach (var e in _entitiy1000)
            {
                _sharedFolderRepository.Save(e);
            }

            using var dbContext = new NetCoreSqliteDB.SqliteDBContext(_directlyTolocalDbFilePath);
            var addedRowCount = dbContext.Table1.Count();
            Assert.AreEqual(1000, addedRowCount);
        }

        [TestMethod]
        public void TestAddDataToLocalDbByCmmds()
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

            using var dbContext = new NetCoreSqliteDB.SqliteDBContext(_cmmdTolocalDbFilePath);
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

            File.Delete(_cmmdTolocalDbFilePath);
            File.Delete(_directlyTolocalDbFilePath);
            File.Delete(_sharedFolderDbFilePath);
        }

    }
}

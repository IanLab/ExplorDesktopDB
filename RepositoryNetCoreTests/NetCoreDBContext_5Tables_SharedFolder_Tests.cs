using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBCommon;

namespace MultiTablesNetCorDB.Tests
{
    [TestClass()]
    public class NetCoreDBContext_5Tables_SharedFolder_Tests
    {
        [ClassInitialize]
        public static void Init(TestContext _)
        {
            NetCoreDBContext_5Tables.CopyTempateDBFile(
                MultiTablesNetCorDBTests.Properties.Resources.Shared);
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
        [DataRow(11)]
        [DataRow(12)]
        [DataRow(13)]
        [DataRow(14)]
        [DataRow(15)]
        public void Add5000DataRowsTest(int batchId)
        {            
            using var dbc = new NetCoreDBContext_5Tables(MultiTablesNetCorDBTests.Properties.Resources.Shared);
            AddDataRowsHelper.Add5000DataRows(dbc.Table1, batchId);
            dbc.SaveChanges();
        }

        //[TestMethod]
        //public void Add1DataRowTest()
        //{
        //    NetCoreDBContext_5Tables.CopyTempateDBFile(MultiTablesNetCorDBTests.Properties.Resources.Shared);
        //    using var dbc = new NetCoreDBContext_5Tables(MultiTablesNetCorDBTests.Properties.Resources.Shared);
        //    AddDataRowsHelper.Add1DataRow(dbc.Table1);
        //    dbc.SaveChanges();
        //}
    }
}
using Connections;
using Orange.Business;
using Orange.Core.Enums;
using Orange.Core.Results;
using Orange.Core.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Orange.Test
{
    [TestClass]
    public class AccessTest
    {
        private readonly IDataSource _dataSource = new Database("DevOrange");

        [TestMethod]
        public void RecordAccess()
        {
            AccessDetailsResult result = new MetricsOps(_dataSource).RecordAccessDetails(new AccessDetailsTest());
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void RecordNavigation()
        {
            NavigationDetailsResult result = new MetricsOps(_dataSource).RecordPageAccess(1, 1);
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void NavigationGet()
        {
            NavigationDetailsResult result = new MetricsOps(_dataSource).NavigationRecordGet(1);
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void NavigationGetAll()
        {
            NavigationDetailsResultList result = new MetricsOps(_dataSource).NavigationRecordsGetAll();
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void AccessGet()
        {
            AccessDetailsResult result = new MetricsOps(_dataSource).RecordAccessGet(1);
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void AccessGetAll()
        {
            AccessDetailsResultList result = new MetricsOps(_dataSource).RecordAccessGetAll();
            Assert.AreEqual(Severity.Success, result.Severity);
        }
    }
}

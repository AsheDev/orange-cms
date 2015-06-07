using Orange.Business;
using Orange.Core.Enums;
using Orange.Connections;
using Orange.Core.Results;
using Orange.Core.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Orange.Test
{
    [TestClass]
    public class AccessibilitySettingsTest
    {
        private readonly IDataSource _dataSource = new Database("DevOrange");

        [TestMethod]
        public void Get()
        {

            AccessibilityResult result = new AccessibilitySettingsOps(_dataSource).Get(1);
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void Update()
        {
            AccessibilityUpdate details = new AccessibilityUpdate
            {
                UserId = 1,
                PermissionId = 3,
                ManagePosts = true,
                CreateNewUsers = false,
                AccessSettings = false,
                CanImpersonate = false,
                ViewMetrics = true,
                IsActive = true
            };
            AccessibilityResult result = new AccessibilitySettingsOps(_dataSource).Update(details);
            Assert.AreEqual(Severity.Success, result.Severity);
        }
    }
}

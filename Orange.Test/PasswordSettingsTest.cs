using Orange.Business;
using Orange.Core.Enums;
using Ripley.Connections;
using Orange.Core.Results;
using Orange.Core.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Orange.Test
{
    [TestClass]
    public class PasswordSettingsTest
    {
        private readonly IDataSource _dataSource = new Database("DevOrange");

        [TestMethod]
        public void Get()
        {   
            PasswordSettingsResult passwordSettings = new PasswordSettingsOps(_dataSource).Get();
            Assert.AreEqual(passwordSettings.Severity, Severity.Success, passwordSettings.Message);
        }

        [TestMethod]
        public void Update()
        {
            PasswordSettingsUpdate update = new PasswordSettingsUpdate
            {
                UserId = 1,
                MaxPasswordAttempts = 5,
                ExpirationInDays = 365,
                ResetExpirationInMinutes = 60
            };

            PasswordSettingsResult passwordSettings = new PasswordSettingsOps(_dataSource).Update(update);
            Assert.AreEqual(passwordSettings.Severity, Severity.Success, passwordSettings.Message);
        }
    }
}

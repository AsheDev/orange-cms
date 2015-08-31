using Connections;
using Orange.Business;
using Orange.Core.Enums;
using Orange.Core.Results;
using Orange.Core.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Orange.Test
{
    [TestClass]
    public class PasswordTest
    {
        private readonly IDataSource _dataSource = new Database("DevOrange");

        [TestMethod]
        public void PasswordCreate()
        {
            BoolResult result = new PasswordOps(_dataSource).CreateNewPassword(new PasswordAddTest());
            Assert.AreEqual(result.Severity, Severity.Success, result.Message);
        }

        //[TestMethod]
        //public void PasswordRecordAttempt()
        //{
        //    //new PasswordOps(dataSource).RecordPasswordAttempt(1);
        //    //Assert.AreEqual(result.Severity, Severity.Success, result.Message);
        //}

        [TestMethod]
        public void Reset()
        {
            int userId = 3;
            PasswordResetResult result = new PasswordOps(_dataSource).Reset(0, userId);
            Assert.AreEqual(result.Severity, Severity.Success, result.Message);
        }

        [TestMethod]
        public void GetResetDetails()
        {
            int userId = 3;
            PasswordResetResult result = new PasswordOps(_dataSource).GetResetDetails(userId);
            Assert.AreEqual(result.Severity, Severity.Success, result.Message);
        }
    }
}

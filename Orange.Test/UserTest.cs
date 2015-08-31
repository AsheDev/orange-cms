using Connections;
using Orange.Business;
using Orange.Core.Enums;
using Orange.Core.Results;
using Orange.Core.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Orange.Test
{
    [TestClass]
    public class UserTest
    {
        private readonly IDataSource _dataSource = new Database("DevOrange");

        [TestMethod]
        public void Login()
        {
            UserResult login = new UserOps(_dataSource).Login("orange@michaelovies.com", "!Orange_2015!");
            Assert.AreEqual(login.Severity, Severity.Success, login.Message);
        }

        [TestMethod]
        public void LogOutId()
        {
            UserResult login = new UserOps(_dataSource).Logout(1);
            Assert.AreEqual(login.Severity, Severity.Success, login.Message);
        }

        [TestMethod]
        public void LogOutEmail()
        {
            UserResult login = new UserOps(_dataSource).Logout("orange@michaelovies.com");
            Assert.AreEqual(login.Severity, Severity.Success, login.Message);
        }

        [TestMethod]
        public void Get()
        {
            UserResult result = new UserOps(_dataSource).Get(1);
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void GetByUsername()
        {
            UserResult result = new UserOps(_dataSource).GetByUsername("Emily Clark");
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void GetAll()
        {
            UserResultList result = new UserOps(_dataSource).GetAll();
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void Add()
        {
            UserResult result = new UserOps(_dataSource).Add(new UserAddTest());
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void Update()
        {
            UserResult result = new UserOps(_dataSource).Update(new UserUpdateTest());
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void Remove()
        {
            UserRemove remove = new UserRemove
            {
                UserId = 1,
                Id = 3
            };
            BoolResult result = new UserOps(_dataSource).Remove(remove);
            Assert.AreEqual(Severity.Success, result.Severity);
        }
    }
}

using Orange.Business;
using Orange.Core.Enums;
using Orange.Connections;
using Orange.Core.Results;
using Orange.Core.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Orange.Test
{
    [TestClass]
    public class PostTest
    {
        private readonly IDataSource _dataSource = new Database("DevOrange");

        [TestMethod]
        public void PostGet()
        {
            PostResult result = new PostOps(_dataSource).Get(1);
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void PostGetAll()
        {
            PostResultList result = new PostOps(_dataSource).GetAll();
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void PostHistoryGetAll()
        {
            PostHistoryResultList result = new PostOps(_dataSource).GetPostHistoy(-1);
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void PostAdd()
        {
            PostResult result = new PostOps(_dataSource).Add(new PostAddTest(), 1);
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void PostUpdate()
        {
            PostResult result = new PostOps(_dataSource).Update(new PostUpdateTest(), 1);
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void PostRemove()
        {
            PostRemove remove = new PostRemove
            {
                UserId = 1,
                Id = 1
            };

            BoolResult result = new PostOps(_dataSource).Remove(remove);
            Assert.AreEqual(Severity.Success, result.Severity);
        }
    }
}

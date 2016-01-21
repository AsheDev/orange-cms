//using Connections;
//using Orange.Business;
//using Orange.Core.Enums;
//using Orange.Core.Results;
//using Orange.Core.Entities;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Orange.Test
//{
//    [TestClass]
//    public class PostTest
//    {
//        private readonly IDataSource _dataSource = new Database("DevOrange");

//        [TestMethod]
//        public void Get()
//        {
//            PostResult result = new PostOps(_dataSource).Get(3);
//            Assert.AreEqual(Severity.Success, result.Severity);
//        }

//        [TestMethod]
//        public void GetAll()
//        {
//            PostResultList result = new PostOps(_dataSource).GetAll();
//            Assert.AreEqual(Severity.Success, result.Severity);
//        }

//        [TestMethod]
//        public void GetAllHistory()
//        {
//            PostHistoryResultList result = new PostOps(_dataSource).GetPostHistoy(-1);
//            Assert.AreEqual(Severity.Success, result.Severity);
//        }

//        [TestMethod]
//        public void Add()
//        {
//            PostResult result = new PostOps(_dataSource).Add(new PostAddTest());
//            Assert.AreEqual(Severity.Success, result.Severity);
//        }

//        [TestMethod]
//        public void Update()
//        {
//            PostResult result = new PostOps(_dataSource).Update(new PostUpdateTest());
//            Assert.AreEqual(Severity.Success, result.Severity);
//        }

//        [TestMethod]
//        public void Remove()
//        {
//            PostRemove remove = new PostRemove
//            {
//                UserId = 1,
//                Id = 1
//            };

//            BoolResult result = new PostOps(_dataSource).Remove(remove);
//            Assert.AreEqual(Severity.Success, result.Severity);
//        }
//    }
//}

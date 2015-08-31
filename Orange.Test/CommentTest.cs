using Connections;
using Orange.Business;
using Orange.Core.Enums;
using Orange.Core.Results;
using Orange.Core.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Orange.Test
{
    [TestClass]
    public class CommentTest
    {
        private IDataSource _dataSource = new Database("DevOrange");

        [TestMethod]
        public void Get()
        {
            CommentResult result = new CommentOps(_dataSource).Get(6);
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void GetAll()
        {
            int postId = 1;
            CommentResultList result = new CommentOps(_dataSource).GetAll(postId);
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        //[TestMethod]
        //public void GetAllHistory()
        //{
        //    PostHistoryResultList result = new CommentOps(_dataSource).GetPostHistoy(-1);
        //    Assert.AreEqual(Severity.Success, result.Severity);
        //}

        [TestMethod]
        public void Add()
        {
            // an anonymous user can make a comment but they won't be able to edit or remove them
            UserResult user = new UserOps(_dataSource).Get(1);
            CommentAdd add = new CommentAdd
            {
                UserId = 0,
                PostId = 1,
                //ProvidedName = user.Result.Name,
                ProvidedName = "Ripley",
                Body = "Salient point!"
            };
            CommentResult result = new CommentOps(_dataSource).Add(add);
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void Update()
        {
            UserResult user = new UserOps(_dataSource).Get(1);
            CommentUpdate update = new CommentUpdate
            {
                UserId = 1,
                Id = 1, // the commentId
                ProvidedName = user.Result.Name,
                Body = "Updated comment!"
            };
            CommentResult result = new CommentOps(_dataSource).Update(update);
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void Remove()
        {
            CommentRemove remove = new CommentRemove
            {
                UserId = 1,
                Id = 1
            };
            BoolResult result = new CommentOps(_dataSource).Remove(remove);
            Assert.AreEqual(Severity.Success, result.Severity);
        }

        [TestMethod]
        public void Approval()
        {
            CommentApproval approval = new CommentApproval
            {
                UserId = 1,
                Id = 1,
                Approval = Core.Enums.Approval.Approved
            };
            BoolResult result = new CommentOps(_dataSource).CommentApproval(approval);
            Assert.AreEqual(Severity.Success, result.Severity);
        }
    }
}

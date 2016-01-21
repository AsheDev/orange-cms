//using System;
//using Connections;
//using Orange.Business;
//using Orange.Core.Enums;
//using Orange.Core.Results;
//using Orange.Core.Entities;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Orange.Test
//{
//    [TestClass]
//    class PermissionTest
//    {
//        private readonly IDataSource _dataSource = new Database("DevOrange");

//        /// <summary>
//        /// Gets all permissions and then tests a Get operation on each one
//        /// </summary>
//        [TestMethod]
//        public void Get()
//        {
//            PermissionOps ops = new PermissionOps(_dataSource);
//            PermissionResultList allResults = ops.GetAll();

//            int permissionsCount = allResults.Results.Count;
//            bool getSuccess = true;
//            PermissionResult getResult = new PermissionResult();
//            for(int n = 0; n < permissionsCount; ++n)
//            {
//                if (!getSuccess) break;
//                getResult = ops.Get(allResults.Results[n].Id);
//                getSuccess = (getResult.Severity == Severity.Success);
//            }

//            Assert.AreEqual(getResult.Severity, Severity.Success);
//        }

//        // GetAll
//        [TestMethod]
//        public void GetAll()
//        {
//            PermissionResultList result = new PermissionOps(_dataSource).GetAll();
//            Assert.AreEqual(result.Severity, Severity.Success);
//        }

//        // Add
//        [TestMethod]
//        public void Add()
//        {
//            PermissionAdd newPermission = new PermissionAdd
//            {
//                UserId = 1,
//                Name = "Test Permission"
//            };

//            PermissionResult permissionOp = new PermissionOps(_dataSource).Add(newPermission);
//            Assert.AreEqual(permissionOp.Severity, Severity.Success);
//        }

//        // Update
//        [TestMethod]
//        public void Update()
//        {
//            PermissionUpdate updatePermission = new PermissionUpdate
//            {
//                UserId = 1,
//                Name = "Test Permission Update"
//            };


//        }

//        // Remove
//    }
//}

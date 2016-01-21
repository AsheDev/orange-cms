//using Connections;
//using System.Data;
//using System.Linq;
//using Orange.Core.Enums;
//using Orange.Core.Results;
//using Orange.Core.Utility;
//using Orange.Core.Entities;
//using System.Data.SqlClient;
//using Orange.Core.Interfaces;
//using System.Collections.Generic;

//namespace Orange.Business
//{
//    public class PermissionOps : Operations
//    {
//        public PermissionOps() { }

//        public PermissionOps(IDataSource dataSource)
//        {
//            DataSource = dataSource;
//        }

//        public PermissionResult Get(int roleId)
//        {
//            PermissionResult result = new PermissionResult();
//            IsDataSourceNull(result);
//            if (result.Severity != Severity.Success) return result;

//            DataTable returnedTable = DataSource.Crud("o.PermissionGet", RoleIdParameter(roleId));

//            result = (PermissionResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
//            if (result.Severity != Core.Enums.Severity.Success) return result;

//            Result.PopulateResult(result, returnedTable);
//            return result;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="userId">Selects permissions by UserId. Can be used with an impersonation Id.</param>
//        /// <returns></returns>
//        public PermissionResult GetByUserId(int userId)
//        {
//            PermissionResult result = new PermissionResult();
//            IsDataSourceNull(result);
//            if (result.Severity != Severity.Success) return result;

//            DataTable returnedTable = DataSource.Crud("o.PermissionGetByUser", RoleByUserIdParameter(userId));

//            result = (PermissionResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
//            if (result.Severity != Core.Enums.Severity.Success) return result;

//            Result.PopulateResult(result, returnedTable);
//            return result;
//        }

//        // TODO: this will need a GetAll

//        // If I use IAccessibility I can use the testing classes here
//        public PermissionResult Update(PermissionUpdate update)
//        {
//            PermissionResult result = new PermissionResult();
//            IsDataSourceNull(result);
//            if (result.Severity != Severity.Success) return result;
//            IsImpersonating((IImpersonation)update, result);
//            if (result.Severity != Core.Enums.Severity.Success) return result;

//            // TODO: error checking of some variety
//            // should I allow impersonating users to make a change? Or should they be using their real account?

//            DataTable returnedTable = DataSource.Crud("o.PermissionUpdate", UpdateParameters(update));

//            result = (PermissionResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
//            if (result.Severity != Core.Enums.Severity.Success) return result;

//            Result.PopulateResult(result, returnedTable);
//            return result;
//        }

//        private SqlParameter[] RoleIdParameter(int roleId)
//        {
//            SqlParameter[] parameters = new SqlParameter[1];
//            parameters[0] = new SqlParameter("@RoleId", roleId);
//            return parameters;
//        }

//        private SqlParameter[] RoleByUserIdParameter(int userId)
//        {
//            SqlParameter[] parameters = new SqlParameter[1];
//            parameters[0] = new SqlParameter("@UserId", userId);
//            return parameters;
//        }

//        private SqlParameter[] UpdateParameters(PermissionUpdate update)
//        {
//            SqlParameter[] parameters = new SqlParameter[11];
//            parameters[0] = new SqlParameter("@RoleId", update.RoleId);
//            parameters[1] = new SqlParameter("@ManagePosts", update.ManagePosts);
//            parameters[2] = new SqlParameter("@ManagePostComments", update.ManagePostComments);
//            parameters[3] = new SqlParameter("@CanComment", update.CanComment);
//            parameters[4] = new SqlParameter("@ManageUsers", update.ManageUsers);
//            parameters[5] = new SqlParameter("@AccessSettings", update.AccessSettings);
//            parameters[6] = new SqlParameter("@CanImpersonate", update.CanImpersonate);
//            parameters[7] = new SqlParameter("@ViewMetrics", update.ViewMetrics);
//            parameters[8] = new SqlParameter("@IsActive", update.IsActive);
//            parameters[9] = new SqlParameter("@UserId", update.UserId);
//            parameters[10] = new SqlParameter("@CallingUserId", update.CallingUserId);
//            return parameters;
//        }
//    }
//}

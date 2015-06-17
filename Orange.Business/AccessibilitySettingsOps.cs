using System.Data;
using System.Linq;
using Orange.Core.Enums;
using Ripley.Connections;
using Orange.Core.Results;
using Orange.Core.Utility;
using Orange.Core.Entities;
using System.Data.SqlClient;
using Orange.Core.Interfaces;
using System.Collections.Generic;

namespace Orange.Business
{
    public class AccessibilitySettingsOps : Operations
    {
        public AccessibilitySettingsOps() { }

        public AccessibilitySettingsOps(IDataSource dataSource)
        {
            DataSource = dataSource;
        }

        public AccessibilityResult Get(int permissionId)
        {
            AccessibilityResult result = new AccessibilityResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.AccessibilityGet", PermissionIdParameter(permissionId));

            result = (AccessibilityResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateSingleResult(result, returnedTable);
            return result;
        }

        // If I use IAccessibility I can use the testing classes here
        public AccessibilityResult Update(AccessibilityUpdate update)
        {
            AccessibilityResult result = new AccessibilityResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)update, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            // TODO: error checking of some variety
            // should I allow impersonating users to make a change? Or should they be using their real account?

            DataTable returnedTable = DataSource.Crud("o.AccessibilityUpdate", UpdateParameters(update));

            result = (AccessibilityResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateSingleResult(result, returnedTable);
            return result;
        }

        private SqlParameter[] PermissionIdParameter(int permissionId)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@PermissionId", permissionId);
            return parameters;
        }

        private SqlParameter[] UpdateParameters(AccessibilityUpdate update)
        {
            SqlParameter[] parameters = new SqlParameter[9];
            parameters[0] = new SqlParameter("@PermissionId", update.PermissionId);
            parameters[1] = new SqlParameter("@ManagePosts", update.ManagePosts);
            parameters[2] = new SqlParameter("@CreateNewUsers", update.CreateNewUsers);
            parameters[3] = new SqlParameter("@AccessSettings", update.AccessSettings);
            parameters[4] = new SqlParameter("@CanImpersonate", update.CanImpersonate);
            parameters[5] = new SqlParameter("@ViewMetrics", update.ViewMetrics);
            parameters[6] = new SqlParameter("@IsActive", update.IsActive);
            parameters[7] = new SqlParameter("@UserId", update.UserId);
            parameters[8] = new SqlParameter("@CallingUserId", update.CallingUserId);
            return parameters;
        }
    }
}

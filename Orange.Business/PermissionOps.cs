using Connections;
using System.Data;
using System.Linq;
using Orange.Core.Enums;
using Orange.Core.Results;
using Orange.Core.Utility;
using Orange.Core.Entities;
using System.Data.SqlClient;
using Orange.Core.Interfaces;
using System.Collections.Generic;

namespace Orange.Business
{
    public class PermissionOps : Operations
    {
        private PermissionUpdate _permissionUpdate;
        private PermissionAdd _permissionAdd;
        // IPermission?

        public PermissionOps() { }

        public PermissionOps(IDataSource dataSource)
        {
            DataSource = dataSource;
        }

        public PermissionResult Get(int permissionId)
        {
            PermissionResult result = new PermissionResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.PermissionGet", IdParameter(permissionId)); // TODO: stored procedure does not exist!

            result = (PermissionResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        public PermissionResultList GetAll()
        {
            PermissionResultList result = new PermissionResultList();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.PermissionGetAll"); // TODO: stored procedure does not exist!

            result = (PermissionResultList)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        public PermissionResult Add(PermissionAdd permissionAdd)
        {
            PermissionResult result = new PermissionResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)permissionAdd, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;
            _permissionAdd = permissionAdd;

            // TODO: error checking

            DataTable returnedTable = DataSource.Crud("o.PermissionAdd", AddParameters()); // TODO: stored procedure does not exist!

            result = (PermissionResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        public PermissionResult Update(PermissionUpdate permissionUpdate)
        {
            PermissionResult result = new PermissionResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)permissionUpdate, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;
            _permissionUpdate = permissionUpdate;

            // TODO: error checking

            DataTable returnedTable = DataSource.Crud("o.PermissionUpdate", UpdateParameters()); // TODO: stored procedure does not exist!

            result = (PermissionResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        // TODO: needs impersonation
        public PermissionResult Remove(PermissionRemove remove)
        {
            PermissionResult result = new PermissionResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)remove, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.PermissionRemove", RemoveParameters(remove)); // TODO: stored procedure does not exist!

            result = (PermissionResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        private SqlParameter[] AddParameters()
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@Name", _permissionAdd.Name);
            parameters[1] = new SqlParameter("@UserId", _permissionAdd.UserId);
            parameters[2] = new SqlParameter("@CallingUserId", _permissionAdd.CallingUserId);
            return parameters;
        }

        private SqlParameter[] UpdateParameters()
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@Name", _permissionUpdate.Name);
            parameters[1] = new SqlParameter("@UserId", _permissionUpdate.UserId);
            parameters[2] = new SqlParameter("@CallingUserId", _permissionUpdate.CallingUserId);
            return parameters;
        }

        private SqlParameter[] RemoveParameters(PermissionRemove remove)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@Id", remove.Id);
            parameters[1] = new SqlParameter("@UserId", remove.UserId);
            parameters[2] = new SqlParameter("@CallingUserId", remove.CallingUserId);
            return parameters;
        }

        private SqlParameter[] IdParameter(int peermissionId)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@PermissionId", peermissionId);
            return parameters;
        }
    }
}

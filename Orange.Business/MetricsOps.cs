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
    public class MetricsOps : Operations
    {
        public MetricsOps() { }

        public MetricsOps(IDataSource dataSource)
        {
            DataSource = dataSource;
        }

        public AccessDetailsResult RecordAccessGet(int accessId)
        {
            AccessDetailsResult result = new AccessDetailsResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.RecordAccessGet", AccessIdParameter(accessId));

            result = (AccessDetailsResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            
            Result.PopulateResult(result, returnedTable);
            return result;
        }

        public AccessDetailsResultList RecordAccessGetAll()
        {
            AccessDetailsResultList result = new AccessDetailsResultList();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.RecordAccessGetAll");

            result = (AccessDetailsResultList)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        // login/logout, successful/unsuccessful
        public AccessDetailsResult RecordAccessDetails(AccessDetails details)
        {
            AccessDetailsResult result = new AccessDetailsResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)details, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.RecordAccess", AccessParameters(details));

            result = (AccessDetailsResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        public NavigationDetailsResult NavigationRecordGet(int navigationId)
        {
            NavigationDetailsResult result = new NavigationDetailsResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.RecordNavigationGet", NavigationIdParameter(navigationId));

            result = (NavigationDetailsResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }
        
        public NavigationDetailsResultList NavigationRecordsGetAll()
        {
            NavigationDetailsResultList result = new NavigationDetailsResultList();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.RecordNavigationGetAll");

            result = (NavigationDetailsResultList)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        // TODO: NavigationDetails class already exists. Can I incorporate it here? I'm mildly confused about it...
        public NavigationDetailsResult RecordPageAccess(int userId, int pageId)
        {
            // create an impersonation class here? Pass a "CallingUserId" and then pass that to IsImpersonating?
            NavigationDetailsResult result = new NavigationDetailsResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            //IsImpersonating(update, result);
            //if (result.Severity != Core.Enums.Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.RecordNavigation", NavigationParameters(userId, pageId));

            result = (NavigationDetailsResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        private SqlParameter[] AccessParameters(AccessDetails details)
        {
            SqlParameter[] parameters = new SqlParameter[6];
            parameters[0] = new SqlParameter("@UserId", details.UserId);
            parameters[1] = new SqlParameter("@Action", details.Action);
            parameters[2] = new SqlParameter("@Success", details.Success);
            parameters[3] = new SqlParameter("@OperatingSystem", details.OperatingSystem);
            parameters[4] = new SqlParameter("@IPAddress", details.IPAddress);
            parameters[5] = new SqlParameter("@CallingUserId", details.CallingUserId);
            return parameters;
        }

        private SqlParameter[] NavigationParameters(int userId, int pageId)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@UserId", userId);
            parameters[1] = new SqlParameter("@PageId", pageId);
            parameters[2] = new SqlParameter("@CallingUserId", userId);
            return parameters;
        }

        private SqlParameter[] NavigationIdParameter(int navigationId)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@NavId", navigationId);
            return parameters;
        }

        private SqlParameter[] AccessIdParameter(int accessId)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@AccessId", accessId);
            return parameters;
        }

        private SqlParameter[] EmailParameter(string email)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@Email", email);
            return parameters;
        }

        private SqlParameter[] IdParameter(int userId)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@UserId", userId);
            return parameters;
        }
    }
}

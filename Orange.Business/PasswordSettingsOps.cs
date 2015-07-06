using System.Data;
using Orange.Core.Enums;
using Ripley.Connections;
using Orange.Core.Results;
using Orange.Core.Utility;
using Orange.Core.Entities;
using System.Data.SqlClient;
using Orange.Core.Interfaces;

namespace Orange.Business
{
    public class PasswordSettingsOps : Operations
    {
        public PasswordSettingsOps() { }

        public PasswordSettingsOps(IDataSource dataSource)
        {
            DataSource = dataSource;
        }

        public PasswordSettingsResult Get()
        {
            PasswordSettingsResult result = new PasswordSettingsResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.PasswordSettingsGet");

            result = (PasswordSettingsResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        // TODO: impersonation, also turn this into a class
        public PasswordSettingsResult Update(PasswordSettingsUpdate update)
        {
            PasswordSettingsResult result = new PasswordSettingsResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)update, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.PasswordSettingsUpdate", UpdateParameters(update));

            result = (PasswordSettingsResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success || result.Result.MaxPasswordAttempts < 0) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        private SqlParameter[] UpdateParameters(PasswordSettingsUpdate update)
        {
            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@MaxAttempts", update.MaxPasswordAttempts);
            parameters[1] = new SqlParameter("@ExpirationInDays", update.ExpirationInDays);
            parameters[2] = new SqlParameter("@ResetExpirationInMinutes", update.ResetExpirationInMinutes);
            parameters[3] = new SqlParameter("@UserId", update.UserId);
            parameters[4] = new SqlParameter("@CallingUserId", update.CallingUserId);
            return parameters;
        }
    }
}

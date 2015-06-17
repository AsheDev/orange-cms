using System.Data;
using System.Linq;
using Ripley.Security;
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
    public class UserOps : Operations
    {
        //private IUser _user;
        private PasswordSettingsResult _settings;
        private PasswordResult _passwordDetails;

        public UserOps() { }

        public UserOps(IDataSource dataSource)
        {
            DataSource = dataSource;
        }

        public UserResult Get(int userId)
        {
            UserResult result = new UserResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.UserGet", IdParameter(userId));

            result = (UserResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateSingleResult(result, returnedTable);
            return result;
        }

        // this string can be the username OR their email
        public UserResult GetByUsername(string username)
        {
            UserResult result = new UserResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.UserGetByUsername", UsernameParameter(username));

            result = (UserResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateSingleResult(result, returnedTable);
            return result;
        }

        public UserResultList GetAll()
        {
            UserResultList result = new UserResultList();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.UserGetAll");

            result = (UserResultList)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateMultipleResults(result, returnedTable);
            return result;
        }

        public UserResult Add(IUser newUser)
        {
            UserResult result = new UserResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)newUser, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            // TODO: error checking

            DataTable returnedTable = DataSource.Crud("o.UserAdd", AddParameters((UserAdd)newUser));

            result = (UserResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;


            Result.PopulateSingleResult(result, returnedTable);
            return result;
        }

        public UserResult Update(IUser updateUser)
        {
            UserResult result = new UserResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)updateUser, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            // TODO: error checking

            DataTable returnedTable = DataSource.Crud("o.UserUpdate", UpdateParameters((UserUpdate)updateUser));

            result = (UserResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateSingleResult(result, returnedTable);
            return result;
        }

        public BoolResult Remove(UserRemove remove)
        {
            BoolResult result = new BoolResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)remove, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;


            // TODO: error checking

            DataTable returnedTable = DataSource.Crud("o.UserRemove", RemoveParameters(remove));

            result = (BoolResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;
            // TODO: are we returning successful here or not?
            return result;
        }

        // TODO: move this to a more suitable class
        // impersonation recording is handled by MetricsOps' RecordAccessDetails method
        public UserResult Login(string username, string password)
        {
            UserOps userOperations = new UserOps(DataSource);
            UserResult userDetails = new UserResult();
            if (string.IsNullOrWhiteSpace(username)) return userDetails;
            if (string.IsNullOrWhiteSpace(password)) return userDetails;

            IsDataSourceNull(userDetails);
            if (userDetails.Severity != Severity.Success) return userDetails;

            userDetails = userOperations.GetByUsername(username);
            if (userDetails.Severity != Severity.Success) return userDetails;

            _settings = new PasswordSettingsOps(DataSource).Get();
            PasswordOps passwordOperations = new PasswordOps(DataSource);
            _passwordDetails = passwordOperations.GetPassword(username);
            if (_passwordDetails.Severity != Core.Enums.Severity.Success) return userDetails;

            if (PasswordHash.ValidatePassword(password, _passwordDetails.Result.HashedPassword))
            {
                IntResult attemptsResult = passwordOperations.UpdatePasswordAttempts(username, 0); // it could break here...
                DataTable returnedTable = DataSource.Crud("o.LogIn", UsernameParameter(username));
                userDetails = (UserResult)Result.PostDatabaseCallErrorChecking(returnedTable, userDetails);
                if (userDetails.Severity != Core.Enums.Severity.Success) return userDetails;
                Result.PopulateSingleResult(userDetails, returnedTable);
                // TODO: record successful login values (MetricOps RecordAccessDetails)
            }
            else
            {
                userDetails = (UserResult)Result.SetResultAsWarning(userDetails, Core.Enums.Password.NoMatch.GetDescription());
                IntResult attemptsResult = passwordOperations.UpdatePasswordAttempts(username, 1);
                if (attemptsResult.Result > _settings.Result.MaxPasswordAttempts)
                {
                    PasswordUpdateNonSensitive nonSensitive = new PasswordUpdateNonSensitive
                    {
                        UserId = userDetails.Result.Id,
                        Attempts = attemptsResult.Result,
                        Expires = _passwordDetails.Result.Expires,
                        Expiration = _passwordDetails.Result.Expiration,
                        IsLocked = true
                    };
                    PasswordResult updateResult = passwordOperations.UpdatePassword(nonSensitive); // it could break here...
                    PasswordResetResult resetResult = passwordOperations.Reset(0, userDetails.Result.Id); // it could break here...
                    // TODO: send an email - MessagingOps
                    userDetails = (UserResult)Result.SetResultAsWarning(userDetails, Core.Enums.Password.NoMatch.GetDescription());
                    // TODO: record failed login values (MetricOps RecordAccessDetails)
                }
            }
            return userDetails;
        }

        // TODO: impersonation recording is handled by MetricsOps' RecordAccessDetails method
        public UserResult Logout(string username)
        {
            UserResult result = new UserResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.LogOut", UsernameParameter(username));

            result = (UserResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            // TODO: record logout details (MetricOps RecordAccessDetails)

            Result.PopulateSingleResult(result, returnedTable);
            return result;
        }

        // TODO: impersonation recording is handled by MetricsOps' RecordAccessDetails method
        public UserResult Logout(int userId)
        {
            UserResult result = new UserResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.LogOut", IdParameter(userId));

            result = (UserResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            // TODO: record logout details (MetricOps RecordAccessDetails)

            Result.PopulateSingleResult(result, returnedTable);
            return result;
        }

        private SqlParameter[] UsernameParameter(string username)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@Username", username);
            return parameters;
        }

        private SqlParameter[] IdParameter(int userId)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@UserId", userId);
            return parameters;
        }

        private SqlParameter[] AddParameters(UserAdd user)
        {
            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@Name", user.Name);
            parameters[1] = new SqlParameter("@Email", user.Email);
            parameters[2] = new SqlParameter("@PermissionId", user.PermissionId);
            parameters[3] = new SqlParameter("@CallingUserId", user.CallingUserId);
            parameters[4] = new SqlParameter("@UserId", user.UserId);
            return parameters;
        }

        private SqlParameter[] UpdateParameters(UserUpdate update)
        {
            SqlParameter[] parameters = new SqlParameter[6];
            parameters[0] = new SqlParameter("@Id", update.Id);
            parameters[1] = new SqlParameter("@Name", update.Name);
            parameters[2] = new SqlParameter("@Email", update.Email);
            parameters[3] = new SqlParameter("@PermissionId", update.PermissionId);
            parameters[4] = new SqlParameter("@CallingUserId", update.CallingUserId);
            parameters[5] = new SqlParameter("@UserId", update.Id);
            return parameters;
        }

        private SqlParameter[] RemoveParameters(UserRemove remove)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@Id", remove.Id);
            parameters[1] = new SqlParameter("@UserId", remove.UserId);
            parameters[2] = new SqlParameter("@CallingUserId", remove.CallingUserId);
            return parameters;
        }
    }
}

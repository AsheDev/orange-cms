using System;
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
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Orange.Business
{
    public class PasswordOps : Operations
    {
        private PasswordSettingsResult _settings;
        private PasswordResult _passwordDetails;
        private string _hashedPassword;
        private byte[] _salt;
        
        public PasswordOps() { }

        public PasswordOps(IDataSource dataSource)
        {
            DataSource = dataSource;
        }

        public BoolResult CreateNewPassword(PasswordAdd passwordDetails)
        {
            BoolResult result = new BoolResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)passwordDetails, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            // TODO: this needs to be within an error checking method
            // I don't think this check needs to be too fancy
            PasswordResult passwordCheck = GetPassword(passwordDetails.UserId);
            if (passwordCheck.Result.UserId > 0)
            {
                result = (BoolResult)Result.SetResultAsWarning(result, Core.Enums.Password.PasswordExists.GetDescription());
                return result;
            }

            IsPasswordStrong(passwordDetails.Password, result);
            if (result.Severity != Orange.Core.Enums.Severity.Success) return result;
            
            byte[] salt = PasswordHash.CreateSalt();
            string hashedPassword = PasswordHash.CreateHash(passwordDetails.Password, salt);
            _hashedPassword = hashedPassword;
            _salt = salt;

            DataTable returnedTable = DataSource.Crud("o.PasswordAdd", AddParameters(passwordDetails));

            result = (BoolResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            return result;
        }

        internal PasswordResult GetPassword(int userId)
        {
            PasswordResult result = new PasswordResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.PasswordGet", IdParameter(userId));

            result = (PasswordResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        internal PasswordResult GetPassword(string username)
        {
            PasswordResult result = new PasswordResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.PasswordGet", UsernameParameter(username));

            result = (PasswordResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        // update the salt and hash along with the basic stuff
        public PasswordResult UpdatePassword(PasswordUpdateSensitive sensitive)
        {
            PasswordResult result = new PasswordResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)sensitive, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.PasswordUpdate", SensitiveParameters(sensitive));

            result = (PasswordResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        // update just the basic stuff
        public PasswordResult UpdatePassword(PasswordUpdateNonSensitive nonSensitive)
        {
            PasswordResult result = new PasswordResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            IsImpersonating((IImpersonation)nonSensitive, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.PasswordUpdate", NonSensitiveParameters(nonSensitive));

            result = (PasswordResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        public PasswordResetResult Reset(int callingUserId, int userId)
        {
            PasswordResetResult result = new PasswordResetResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;
            _settings = new PasswordSettingsOps(DataSource).Get();

            DateTime now = DateTime.Now;
            PasswordReset resetDetails = new PasswordReset
            {
                CallingUserId = callingUserId,
                UserId = userId,
                AuthenticationURL = GetUniqueWebSafeString((byte)32), // TODO: the fuck am I doing with this?
                Created = now,
                Expires = now.AddDays(_settings.Result.ExpirationInDays)
            };

            IsImpersonating((IImpersonation)resetDetails, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.PasswordReset", ResetParameters(resetDetails));

            result = (PasswordResetResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        public PasswordResetResult GetResetDetails(int userId)
        {
            PasswordResetResult result = new PasswordResetResult();
            IsDataSourceNull(result);
            if (result.Severity != Severity.Success) return result;

            DataTable returnedTable = DataSource.Crud("o.PasswordResetGet", IdParameter(userId));

            result = (PasswordResetResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            Result.PopulateResult(result, returnedTable);
            return result;
        }

        /// <summary>
        /// Updates the attempts value for a user's password. Greater than 0 will add 1 to the attempts, Less than or equal to 0 will zero out the field.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="attempts"></param>
        /// <returns></returns>
        internal IntResult UpdatePasswordAttempts(string username, int attempts)
        {
            IntResult result = new IntResult();
            DataTable returnedTable = DataSource.Crud("o.PasswordRecordAttempt", UsernameParameter(username));

            result = (IntResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            if (result.Severity != Core.Enums.Severity.Success) return result;

            result.Result = Convert.ToInt32(returnedTable.AsEnumerable().First().ItemArray[0]);
            return result;
        }

        private SqlParameter[] IdParameter(int userId)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@UserId", userId);
            return parameters;
        }

        private SqlParameter[] UsernameParameter(string username)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@Username", username);
            return parameters;
        }

        //private SqlParameter[] UpdateAttemptsParameters(string email, int attempts)
        //{
        //    SqlParameter[] parameters = new SqlParameter[2];
        //    parameters[0] = new SqlParameter("@Email", email);
        //    parameters[1] = new SqlParameter("@Attempts", attempts);
        //    return parameters;
        //}

        private SqlParameter[] AddParameters(PasswordAdd passwordInfo)
        {
            SqlParameter[] parameters = new SqlParameter[6];
            parameters[0] = new SqlParameter("@UserId", passwordInfo.UserId);
            parameters[1] = new SqlParameter("@HashedPassword", _hashedPassword);
            parameters[2] = new SqlParameter("@Salt", Convert.ToBase64String(_salt));
            parameters[3] = new SqlParameter("@Expires", passwordInfo.Expires);
            parameters[4] = new SqlParameter("@Expiration", passwordInfo.Expiration.ToUniversalTime());
            parameters[5] = new SqlParameter("@CallingUserId", passwordInfo.CallingUserId);
            return parameters;
        }

        private SqlParameter[] ResetParameters(PasswordReset passwordInfo)
        {
            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@UserId", passwordInfo.UserId);
            parameters[1] = new SqlParameter("@AuthenticationURL", passwordInfo.AuthenticationURL);
            parameters[2] = new SqlParameter("@Created", passwordInfo.Created.ToUniversalTime());
            parameters[3] = new SqlParameter("@Expires", passwordInfo.Expires.ToUniversalTime());
            parameters[4] = new SqlParameter("@CallingUserId", passwordInfo.CallingUserId);
            return parameters;
        }

        private SqlParameter[] SensitiveParameters(PasswordUpdateSensitive passwordInfo)
        {
            SqlParameter[] parameters = new SqlParameter[8];
            parameters[0] = new SqlParameter("@UserId", passwordInfo.UserId);
            parameters[1] = new SqlParameter("@Salt", Convert.ToBase64String(_salt));
            parameters[2] = new SqlParameter("@HashedPassword", _hashedPassword);
            parameters[3] = new SqlParameter("@Attempts", passwordInfo.Attempts);
            parameters[4] = new SqlParameter("@Expires", passwordInfo.Expires);
            parameters[5] = new SqlParameter("@Expiration", passwordInfo.Expiration.ToUniversalTime());
            parameters[6] = new SqlParameter("@IsLocked", passwordInfo.IsLocked);
            parameters[7] = new SqlParameter("@CAllingUserId", passwordInfo.CallingUserId);
            return parameters;
        }

        private SqlParameter[] NonSensitiveParameters(PasswordUpdateNonSensitive passwordInfo)
        {
            SqlParameter[] parameters = new SqlParameter[6];
            parameters[0] = new SqlParameter("@UserId", passwordInfo.UserId);
            parameters[1] = new SqlParameter("@Attempts", passwordInfo.Attempts);
            parameters[2] = new SqlParameter("@Expires", passwordInfo.Expires);
            parameters[3] = new SqlParameter("@Expiration", passwordInfo.Expiration.ToUniversalTime());
            parameters[4] = new SqlParameter("@IsLocked", passwordInfo.IsLocked);
            parameters[5] = new SqlParameter("@UserId", passwordInfo.CallingUserId);
            return parameters;
        }

        private static bool IsPasswordStrong(string password, IResult result)
        {
            // TODO: clean up all these returns. It's bad form.
            if (password.Length < 12)
            {
                result = Result.SetResultAsWarning(result, Core.Enums.Password.TooShort.GetDescription());
                return false;
            }
            if (password.Length > 166)
            {
                result = Result.SetResultAsWarning(result, Core.Enums.Password.TooLong.GetDescription());
                return false;
            }
            if (!password.Any(char.IsUpper))
            {
                result = Result.SetResultAsWarning(result, Core.Enums.Password.NoUpper.GetDescription());
                return false;
            }
            if (!password.Any(char.IsLower))
            {
                result = Result.SetResultAsWarning(result, Core.Enums.Password.NoLower.GetDescription());
                return false;
            }
            if (!password.Any(char.IsNumber))
            {
                result = Result.SetResultAsWarning(result, Core.Enums.Password.NoNumbers.GetDescription());
                return false;
            }
            if (!password.Any(char.IsLetter))
            {
                result = Result.SetResultAsWarning(result, Core.Enums.Password.NoLetters.GetDescription());
                return false;
            }
            if (!new Regex("[^a-zA-Z0-9\\s]").IsMatch(password))
            {
                result = Result.SetResultAsWarning(result, Core.Enums.Password.NoSpecialCharacters.GetDescription());
                return false;
            }
            result = Result.SetResultAsSuccess(result);
            return true;
        }

        private bool TooManyAttempts()
        {
            return (_passwordDetails.Result.Attempts > _settings.Result.MaxPasswordAttempts);
        }

        private bool PasswordHasExpired()
        {
            return (DateTime.UtcNow > _passwordDetails.Result.Expiration);
        }

        public void PreDatabaseCallErrorChecking(ref UserResult result)
        {
            if (TooManyAttempts())
            {
                result = (UserResult)Result.SetResultAsWarning(result, Core.Enums.Password.TooManyAttempts.GetDescription());
                return;
            }
            if (PasswordHasExpired())
            {
                result = (UserResult)Result.SetResultAsWarning(result, Core.Enums.Password.Expired.GetDescription());
                return;
            }
            result = (UserResult)Result.SetResultAsSuccess(result);
        }

        /// <summary>
        /// Get a unique, websafe string of characters to be used for password reset urls
        /// </summary>
        /// <param name="byteLength"></param>
        /// <returns></returns>
        public string GetUniqueWebSafeString(int byteLength)
        {
            if (byteLength < 8) byteLength = 8;
            const string availableWebSafeCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-._~";
            using (RNGCryptoServiceProvider generator = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[byteLength];
                generator.GetBytes(bytes);
                IEnumerable<char> chars = bytes.Select(b => availableWebSafeCharacters[b % availableWebSafeCharacters.Length]);
                return new string(chars.ToArray());
            }
        }
    }
}

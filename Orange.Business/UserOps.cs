//using Security;
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
//using System.Text.RegularExpressions;
//using System.Net;
//using System.Net.Sockets;
//using System;

//namespace Orange.Business
//{
//    public class UserOps : Operations
//    {
//        //private IUser _user;
//        private PasswordSettingsResult _settings;
//        private PasswordResult _passwordDetails;

//        public UserOps() { }

//        public UserOps(IDataSource dataSource)
//        {
//            DataSource = dataSource;
//        }

//        public UserResult Get(int userId)
//        {
//            UserResult result = new UserResult();
//            IsDataSourceNull(result);
//            if (result.Severity != Severity.Success) return result;

//            DataTable returnedTable = DataSource.Crud("o.UserGet", IdParameter(userId));

//            result = (UserResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
//            if (result.Severity != Core.Enums.Severity.Success) return result;

//            Result.PopulateResult(result, returnedTable);
//            return result;
//        }

//        public UserResult GetByGuid(Guid userGuid)
//        {
//            UserResult result = new UserResult();
//            IsDataSourceNull(result);
//            if (result.Severity != Severity.Success) return result;

//            DataTable returnedTable = DataSource.Crud("o.UserGetByGuid", GuidParameter(userGuid));

//            result = (UserResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
//            if (result.Severity != Core.Enums.Severity.Success) return result;

//            Result.PopulateResult(result, returnedTable);
//            return result;
//        }

//        // this string can be the username OR their email
//        public UserResult GetByUsername(string username)
//        {
//            UserResult result = new UserResult();
//            IsDataSourceNull(result);
//            if (result.Severity != Severity.Success) return result;

//            DataTable returnedTable = DataSource.Crud("o.UserGetByUsername", UsernameParameter(username));

//            result = (UserResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
//            if (result.Severity != Core.Enums.Severity.Success) return result;

//            Result.PopulateResult(result, returnedTable);
//            return result;
//        }

//        public UserResultList GetAll()
//        {
//            UserResultList result = new UserResultList();
//            IsDataSourceNull(result);
//            if (result.Severity != Severity.Success) return result;

//            DataTable returnedTable = DataSource.Crud("o.UserGetAll");

//            result = (UserResultList)Result.PostDatabaseCallErrorChecking(returnedTable, result);
//            if (result.Severity != Core.Enums.Severity.Success) return result;

//            Result.PopulateResult(result, returnedTable);
//            return result;
//        }

//        public UserResult Add(IUser newUser)
//        {
//            UserResult result = new UserResult();
//            IsDataSourceNull(result);
//            if (result.Severity != Severity.Success) return result;
//            IsImpersonating((IImpersonation)newUser, result);
//            if (result.Severity != Core.Enums.Severity.Success) return result;

//            // TODO: error checking
//            if(UsernameAlreadyExists(newUser.Name))
//            {
//                result = (UserResult)Result.SetResultAsWarning(result, Users.UsernameExists.GetDescription());
//            }

//            if (UsernameAlreadyExists(newUser.Email))
//            {
//                result = (UserResult)Result.SetResultAsWarning(result, Users.EmailExists.GetDescription());
//            }

//            UserAdd userDetails = (UserAdd)newUser;
//            // check to see if email is legit (somehow)
//            if(!IsEmailValid(userDetails.Email))
//            {
//                result = (UserResult)Result.SetResultAsWarning(result, Users.EmailInvalid.GetDescription());
//            }

//            if(!IsHostnameValid(userDetails.Email))
//            {
//                result = (UserResult)Result.SetResultAsWarning(result, Users.EmailInvalid.GetDescription());
//            }
            
//            // check passwords match AND fit criteria
//            if(userDetails.Password != userDetails.RetypedPassword)
//            {
//                result = (UserResult)Result.SetResultAsWarning(result, Orange.Core.Enums.Password.Mismatch.GetDescription());
//            }

//            // I don't like this implementation
//            if (PasswordOps.IsPasswordStrong(userDetails.Password, result)) { };

//            if (result.Severity != Core.Enums.Severity.Success) return result;

//            DataTable returnedTable = DataSource.Crud("o.UserAdd", AddParameters((UserAdd)newUser));
//            result = (UserResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
            
//            // TODO: what if this fails?

//            // TODO: need a class to get settings to check for password expiration

//            // password stuff
//            PasswordAdd userPasswordDetails = new PasswordAdd();
//            userPasswordDetails.UserId = result.Result.Id;
//            userPasswordDetails.Expires = true; // Do I have a setting for this?
//            userPasswordDetails.Expiration = DateTime.Now.AddYears(20); // TODO: there is a setting for this in the DB
//            userPasswordDetails.Password = userDetails.Password;

//            PasswordOps passwordOps = new PasswordOps(DataSource);
//            BoolResult passwordResult = passwordOps.CreateNewPassword(userPasswordDetails);
//            // TODO: this worked but it appeared to return false

            

//            if (result.Severity != Severity.Success || passwordResult.Severity != Severity.Success) return result;


//            Result.PopulateResult(result, returnedTable);
//            return result;
//        }

//        public UserResult Update(IUser updateUser)
//        {
//            UserResult result = new UserResult();
//            IsDataSourceNull(result);
//            if (result.Severity != Severity.Success) return result;
//            IsImpersonating((IImpersonation)updateUser, result);
//            if (result.Severity != Core.Enums.Severity.Success) return result;

//            // TODO: error checking

//            DataTable returnedTable = DataSource.Crud("o.UserUpdate", UpdateParameters((UserUpdate)updateUser));

//            result = (UserResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
//            if (result.Severity != Core.Enums.Severity.Success) return result;

//            Result.PopulateResult(result, returnedTable);
//            return result;
//        }

//        public BoolResult Remove(UserRemove remove)
//        {
//            BoolResult result = new BoolResult();
//            IsDataSourceNull(result);
//            if (result.Severity != Severity.Success) return result;
//            IsImpersonating((IImpersonation)remove, result);
//            if (result.Severity != Core.Enums.Severity.Success) return result;

//            // TODO: error checking

//            DataTable returnedTable = DataSource.Crud("o.UserRemove", RemoveParameters(remove));

//            result = (BoolResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
//            if (result.Severity != Core.Enums.Severity.Success) return result;
//            // TODO: are we returning successful here or not?
//            return result;
//        }

//        // TODO: move this to a more suitable class
//        // impersonation recording is handled by MetricsOps' RecordAccessDetails method
//        public UserResult Login(string username, string password)
//        {
//            UserOps userOperations = new UserOps(DataSource);
//            UserResult userDetails = new UserResult();
//            if (string.IsNullOrWhiteSpace(username)) return userDetails;
//            if (string.IsNullOrWhiteSpace(password)) return userDetails;

//            IsDataSourceNull(userDetails);
//            if (userDetails.Severity != Severity.Success) return userDetails;

//            userDetails = userOperations.GetByUsername(username);
//            if (userDetails.Severity != Severity.Success) return userDetails;

//            _settings = new PasswordSettingsOps(DataSource).Get();
//            PasswordOps passwordOperations = new PasswordOps(DataSource);
//            _passwordDetails = passwordOperations.GetPassword(username);
//            if (_passwordDetails.Severity != Core.Enums.Severity.Success) return userDetails;

//            if (PasswordHash.ValidatePassword(password, _passwordDetails.Result.HashedPassword))
//            {
//                IntResult attemptsResult = passwordOperations.UpdatePasswordAttempts(username, 0); // it could break here...
//                DataTable returnedTable = DataSource.Crud("o.LogIn", UsernameParameter(username));
//                userDetails = (UserResult)Result.PostDatabaseCallErrorChecking(returnedTable, userDetails);
//                if (userDetails.Severity != Core.Enums.Severity.Success) return userDetails;
//                Result.PopulateResult(userDetails, returnedTable);
//                // TODO: record successful login values (MetricOps RecordAccessDetails)
//            }
//            else
//            {
//                userDetails = (UserResult)Result.SetResultAsWarning(userDetails, Core.Enums.Password.NoMatch.GetDescription());
//                IntResult attemptsResult = passwordOperations.UpdatePasswordAttempts(username, 1);
//                if (attemptsResult.Result > _settings.Result.MaxPasswordAttempts)
//                {
//                    PasswordUpdateNonSensitive nonSensitive = new PasswordUpdateNonSensitive
//                    {
//                        UserId = userDetails.Result.Id,
//                        Attempts = attemptsResult.Result,
//                        Expires = _passwordDetails.Result.Expires,
//                        Expiration = _passwordDetails.Result.Expiration,
//                        IsLocked = true
//                    };
//                    PasswordResult updateResult = passwordOperations.UpdatePassword(nonSensitive); // it could break here...
//                    PasswordResetResult resetResult = passwordOperations.Reset(0, userDetails.Result.Id); // it could break here...
//                    // TODO: send an email - MessagingOps
//                    userDetails = (UserResult)Result.SetResultAsWarning(userDetails, Core.Enums.Password.NoMatch.GetDescription());
//                    // TODO: record failed login values (MetricOps RecordAccessDetails)
//                }
//            }
//            return userDetails;
//        }

//        // TODO: impersonation recording is handled by MetricsOps' RecordAccessDetails method
//        public UserResult Logout(string username)
//        {
//            UserResult result = new UserResult();
//            IsDataSourceNull(result);
//            if (result.Severity != Severity.Success) return result;

//            DataTable returnedTable = DataSource.Crud("o.LogOut", UsernameParameter(username));

//            result = (UserResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
//            if (result.Severity != Core.Enums.Severity.Success) return result;

//            // TODO: record logout details (MetricOps RecordAccessDetails)

//            Result.PopulateResult(result, returnedTable);
//            return result;
//        }

//        // TODO: impersonation recording is handled by MetricsOps' RecordAccessDetails method
//        public UserResult Logout(int userId)
//        {
//            UserResult result = new UserResult();
//            IsDataSourceNull(result);
//            if (result.Severity != Severity.Success) return result;

//            DataTable returnedTable = DataSource.Crud("o.LogOut", IdParameter(userId));

//            result = (UserResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
//            if (result.Severity != Core.Enums.Severity.Success) return result;

//            // TODO: record logout details (MetricOps RecordAccessDetails)

//            Result.PopulateResult(result, returnedTable);
//            return result;
//        }

//        /********* IMPERSONATION ************/
//        // move to its own ops class?
//        public UserResult BeginImpersonation(int userIdToImpersonate)
//        {
//            UserResult result = new UserResult();
//            IsDataSourceNull(result);
//            if (result.Severity != Severity.Success) return result;

//            // TODO: start impersonation SQL call
//            DataTable returnedTable = DataSource.Crud("o.UserGet", IdParameter(userIdToImpersonate));

//            result = (UserResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
//            if (result.Severity != Core.Enums.Severity.Success) return result;

//            Result.PopulateResult(result, returnedTable);
//            return result;
//        }

//        public UserResult EndImpersonation(int userId)
//        {
//            UserResult result = new UserResult();
//            IsDataSourceNull(result);
//            if (result.Severity != Severity.Success) return result;

//            // TODO: end impersonation SQL call
//            DataTable returnedTable = DataSource.Crud("o.UserGet", IdParameter(userId));

//            result = (UserResult)Result.PostDatabaseCallErrorChecking(returnedTable, result);
//            if (result.Severity != Core.Enums.Severity.Success) return result;

//            Result.PopulateResult(result, returnedTable);
//            return result;
//        }


//        private SqlParameter[] UsernameParameter(string username)
//        {
//            SqlParameter[] parameters = new SqlParameter[1];
//            parameters[0] = new SqlParameter("@Username", username);
//            return parameters;
//        }

//        private SqlParameter[] IdParameter(int userId)
//        {
//            SqlParameter[] parameters = new SqlParameter[1];
//            parameters[0] = new SqlParameter("@UserId", userId);
//            return parameters;
//        }

//        private SqlParameter[] GuidParameter(Guid userGuid)
//        {
//            SqlParameter[] parameters = new SqlParameter[1];
//            parameters[0] = new SqlParameter("@ObfuscatedId", userGuid);
//            return parameters;
//        }

//        private SqlParameter[] AddParameters(UserAdd user)
//        {
//            SqlParameter[] parameters = new SqlParameter[5];
//            parameters[0] = new SqlParameter("@Name", user.Name.Trim());
//            parameters[1] = new SqlParameter("@Email", user.Email.Trim());
//            parameters[2] = new SqlParameter("@RoleId", user.RoleId);
//            parameters[3] = new SqlParameter("@CallingUserId", user.CallingUserId);
//            parameters[4] = new SqlParameter("@UserId", user.UserId);
//            return parameters;
//        }

//        private SqlParameter[] UpdateParameters(UserUpdate update)
//        {
//            SqlParameter[] parameters = new SqlParameter[6];
//            parameters[0] = new SqlParameter("@Id", update.Id);
//            parameters[1] = new SqlParameter("@Name", update.Name);
//            parameters[2] = new SqlParameter("@Email", update.Email);
//            parameters[3] = new SqlParameter("@RoleId", update.RoleId);
//            parameters[4] = new SqlParameter("@CallingUserId", update.CallingUserId);
//            parameters[5] = new SqlParameter("@UserId", update.Id);
//            return parameters;
//        }

//        private SqlParameter[] RemoveParameters(UserRemove remove)
//        {
//            SqlParameter[] parameters = new SqlParameter[3];
//            parameters[0] = new SqlParameter("@Id", remove.Id);
//            parameters[1] = new SqlParameter("@UserId", remove.UserId);
//            parameters[2] = new SqlParameter("@CallingUserId", remove.CallingUserId);
//            return parameters;
//        }

//        /// <summary>
//        /// Checks whether a username already exists. Returns True is name already exists, False if not.
//        /// </summary>
//        /// <param name="username"></param>
//        /// <returns></returns>
//        private bool UsernameAlreadyExists(string username)
//        {
//            UserResult userInfo = GetByUsername(username);
//            return (userInfo.Result.Id > 0);
//        }

//        /// <summary>
//        /// Checks if email falls within normal parameters.
//        /// </summary>
//        /// <param name="inputEmail"></param>
//        /// <returns></returns>
//        private bool IsEmailValid(string email)
//        {
//            if (string.IsNullOrEmpty(email)) return false;
//            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
//                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
//                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
//            Regex re = new Regex(strRegex);
//            if (re.IsMatch(email))
//                return true;
//            else
//                return false;
//        }

//        /// <summary>
//        /// Confirm hostname checks out.
//        /// </summary>
//        /// <param name="emailAddress"></param>
//        /// <returns></returns>
//        private bool IsHostnameValid(string emailAddress)
//        {
//            if (string.IsNullOrEmpty(emailAddress)) return false;
//            if (!emailAddress.Contains("@")) return false;
//            string[] host = (emailAddress.Split('@'));
//            string hostname = host[1];

//            IPHostEntry IPhst = Dns.Resolve(hostname); // TODO: check into this
//            IPEndPoint endPt = new IPEndPoint(IPhst.AddressList[0], 25);

//            bool connected = false;
//            using (var socket = new Socket(endPt.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
//            {
//                socket.Connect(endPt);
//                connected = socket.Connected;
//                socket.Disconnect(true);
//            }
//            return connected;
//        }
//    }
//}

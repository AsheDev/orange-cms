using System;
using System.Data;
using System.Linq;
using System.Text;
//using Orange.Core.Values;
using Orange.Core.Values;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Orange.Core.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orange.Core.Entities
{
    // TODO: I think this needs to be a full fledged richly modeled object
    public class Credentials : Entity
    {
        [Key]
        public int UserId { get; private set; }
        public string RecoveryEmail { get; private set; }
        public int Iterations { get; private set; }
        public string Salt { get; private set; }
        public string HashedPassword { get; private set; }
        public string SaltedAndHashedPassword { get; private set; }
        public int LoginAttempts { get; set; }
        public bool PasswordExpires { get; private set; }
        public DateTime PasswordExpiration { get; set; }
        public bool IsLocked { get; private set; }
        public string RecoveryKey { get; set; }
        public DateTime RecoveryKeyCreated { get; private set; }
        public string Delimiter { get; private set; }

        private int _recoveryKeyLength = 32;

        private Credentials() { }

        public Credentials(Repository repo)
        {
            IsLocked = true;
            LoginAttempts = 99;
            PasswordExpiration = DateTime.MinValue;
            RecoveryKeyCreated = DateTime.MinValue;
            _repo = repo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Credentials GetByUserId(int userId)
        {
            if (userId < 1) return null;
            Credentials entity = _repo.Credentials.FirstOrDefault(c => c.UserId == userId);
            if (ReferenceEquals(entity, null)) return null;
            SetRepo(_repo);
            return entity;
        }

        public Credentials GetByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return null;

            // trying a JOIN :|
            //Credentials derp = _repo.Credentials.Join(_repo.Users,
            //                                          c => c.UserId,
            //                                          u => u.Id,
            //                                          (c, u) => new { User = u, Credentials = c })
            //                                          .Select(c => c.Credentials).Where(c => c)

            User user = _repo.Users.FirstOrDefault(u => u.Username.ToLower() == username.Trim().ToLower());
            if (ReferenceEquals(user, null)) return null;

            Credentials entity = _repo.Credentials.FirstOrDefault(c => c.UserId == user.Id);
            if (ReferenceEquals(entity, null)) return null;
            SetRepo(_repo);
            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recoveryEmail"></param>
        /// <returns></returns>
        public Credentials GetByEmailAddress(string recoveryEmail)
        {
            if (string.IsNullOrWhiteSpace(recoveryEmail)) return null;
            Credentials entity = _repo.Credentials.FirstOrDefault(c => c.RecoveryEmail.ToLower() == recoveryEmail.ToLower().Trim());
            if (ReferenceEquals(entity, null)) return null;
            SetRepo(_repo);
            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recoveryKey"></param>
        /// <returns></returns>
        public Credentials GetByRecoveryKey(string recoveryKey)
        {
            if (string.IsNullOrWhiteSpace(recoveryKey)) return null;
            Credentials entity = _repo.Credentials.FirstOrDefault(c => c.RecoveryKey.ToLower() == recoveryKey.ToLower().Trim());
            if (ReferenceEquals(entity, null)) return null;
            SetRepo(_repo);
            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IncrementAttempts()
        {
            if (UserId < 1) return false;
            if (RepositoryIsNotValid()) return false;

            ++LoginAttempts; // TODO: leave logic regarding lockout to the service layer

            _repo.Credentials.Attach(this);
            _repo.Entry(this).State = EntityState.Modified;
            int rowsAffected = _repo.SaveChanges();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// *Needs a better name* This will reset the attempts the user has 
        /// made to login to 0, the LockOut bit to 0, and the recovery key 
        /// and date to NULL.
        /// </summary>
        public bool ResetAttempts()
        {
            if (UserId < 1) return false;
            if (RepositoryIsNotValid()) return false;

            LoginAttempts = 0;

            _repo.Credentials.Attach(this);
            _repo.Entry(this).State = EntityState.Modified;
            int rowsAffected = _repo.SaveChanges();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// This will lock the user's credentials so they cannot log into 
        /// the system.
        /// </summary>
        public bool LockOut(string recoveryKey)
        {
            if (UserId < 1) return false;
            if (RepositoryIsNotValid()) return false;
            if (string.IsNullOrWhiteSpace(recoveryKey)) return false;
            if (recoveryKey.Trim().Length != _recoveryKeyLength) return false;

            IsLocked = true;
            LoginAttempts = 99; // just to be sure
            RecoveryKey = recoveryKey.Trim();
            RecoveryKeyCreated = DateTime.Now;

            _repo.Credentials.Attach(this);
            _repo.Entry(this).State = EntityState.Modified;
            int rowsAffected = _repo.SaveChanges();
            return (rowsAffected > 0);
        }

        public bool Unlock()
        {
            if (UserId < 1) return false;
            if (RepositoryIsNotValid()) return false;

            IsLocked = false;
            LoginAttempts = 0; // just to be sure
            RecoveryKey = string.Empty;

            _repo.Credentials.Attach(this);
            _repo.Entry(this).State = EntityState.Modified;
            int rowsAffected = _repo.SaveChanges();
            return (rowsAffected > 0);
        }

        public Credentials Add(int userId, string email, HashedPassword details)
        {
            if (userId < 1) return null;
            if (string.IsNullOrWhiteSpace(email)) return null;
            if (string.IsNullOrWhiteSpace(details.Hash)) return null;
            if (details.Iterations < 1) return null;
            if (string.IsNullOrWhiteSpace(details.Salt)) return null;

            UserId = userId;
            RecoveryEmail = email.Trim();
            LoginAttempts = 0;
            PasswordExpires = false;
            PasswordExpiration = DateTime.Now.AddYears(200);
            IsLocked = false;
            RecoveryKey = null;
            RecoveryKeyCreated = DateTime.Now.AddYears(-5);
            Delimiter = ":";
            Iterations = details.Iterations;
            Salt = details.Salt;
            HashedPassword = details.Hash;

            StringBuilder combinedHash = new StringBuilder();
            combinedHash.Append(details.Iterations);
            combinedHash.Append(Delimiter);
            combinedHash.Append(details.Salt);
            combinedHash.Append(Delimiter);
            combinedHash.Append(details.Hash);
            SaltedAndHashedPassword = combinedHash.ToString();

            _repo.Credentials.Add(this);
            int rowsAffected = _repo.SaveChanges();
            if (rowsAffected < 1) return null;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="passwordDetails"></param>
        /// <returns></returns>
        public bool ChangePassword(HashedPassword details)
        {
            bool status = false;
            if (UserId < 1) return status;
            if (string.IsNullOrWhiteSpace(details.Hash)) return status;
            if (details.Iterations < 1) return status;
            if (string.IsNullOrWhiteSpace(details.Salt)) return status;

            Iterations = details.Iterations;
            Salt = details.Salt;
            HashedPassword = details.Hash;

            StringBuilder combinedHash = new StringBuilder();
            combinedHash.Append(details.Iterations);
            combinedHash.Append(Delimiter);
            combinedHash.Append(details.Salt);
            combinedHash.Append(Delimiter);
            combinedHash.Append(details.Hash);
            SaltedAndHashedPassword = combinedHash.ToString();

            _repo.Credentials.Attach(this);
            _repo.Entry(this).State = EntityState.Modified;
            int rowsAffected = _repo.SaveChanges();
            return (rowsAffected > 0);
        }
    }
}

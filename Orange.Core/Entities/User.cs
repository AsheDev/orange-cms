using System;
using System.Linq;
using System.Collections;
using System.Data.Entity;
using Orange.Core.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orange.Core.Entities
{
    public class User : Entity
    {
        public int Id { get; private set; }
        public Guid ObfuscatedId { get; private set; }
        //public int ImpersonatingId { get; private set; }
        public string Username { get; private set; }
        //public string Email { get; private set; }
        public bool IsVisible { get; private set; }
        public bool InSystem { get; private set; }
        public bool IsActive { get; private set; }
        //[ForeignKey("FK_PermissionId")]
        public virtual ICollection<Permission> Permissions { get; private set; }

        private User() { }

        public User(Repository repo)
        {
            _repo = repo;
            Permissions = new HashSet<Permission>();
        }

        public User GetById(int userId)
        {
            User entity = new User();
            if(userId < 1)
            {
                entity._errorMessage = "UserId not recognized.";
                return entity;
            }

            entity = _repo.Users.Include(u => u.Permissions).FirstOrDefault(u => u.Id == userId);
            if(ReferenceEquals(entity, null))
            {
                entity = new User();
                entity._errorMessage = "Error encountered while retrieving data from the database.";
                return entity;
            }
            SetRepo(_repo);
            return entity;
        }

        public User GetByUsername(string username)
        {
            User entity = new User();
            if (string.IsNullOrWhiteSpace(username))
            {
                entity._errorMessage = "Username not recognized.";
                return entity;
            }

            entity = _repo.Users.Include(u => u.Permissions).FirstOrDefault(u => u.Username == username);
            if (ReferenceEquals(entity, null))
            {
                entity = new User();
                entity._errorMessage = "Error encountered while retrieving data from the database.";
                return entity;
            }
            SetRepo(_repo);
            return entity;
        }

        public List<User> GetAll()
        {
            List<User> users = _repo.Users.Include(u => u.Permissions).ToList();
            if (!users.Any()) return null;
            users.ToList().ForEach(u => u.SetRepo(_repo));
            return users;
        }

        public User Add(string username, bool isVisible, ICollection<Permission> permissions)
        {
            User entity = new User();
            if(string.IsNullOrWhiteSpace(username))
            {
                entity._errorMessage = "Name provided is invalid.";
                return entity;
            }
            if(!permissions.Any())
            {
                entity._errorMessage = "At least one permissions must be provided.";
                return entity;
            }
            if(Id > 0)
            {
                entity._errorMessage = "Record conflict.";
                return entity;
            }

            User userCheck = _repo.Users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());
            if (!ReferenceEquals(userCheck, null)) return null; // user already exists

            // confirm that all permissions exist
            //List<Permission> permissionsCheck = _repo.Permissions.SelectMany(p => permissions.All(p1 => p1.Id == p.Id)).ToList();

            ObfuscatedId = Guid.NewGuid();
            //ImpersonatingId = 0;
            Username = username.Trim();
            Permissions = permissions;
            IsVisible = IsVisible;
            InSystem = false;
            IsActive = true;

            _repo.Users.Add(this);
            int rowsAffected = _repo.SaveChanges(); // returns the number of rows affected
            SetRepo(_repo);
            return this;
        }

        public User Update(string username, ICollection<Permission> permissions)
        {
            User entity = new User();
            if(Id < 1)
            {
                entity._errorMessage = "No user selected";
                return entity;
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                entity._errorMessage = "Name provided is invalid.";
                return entity;
            }
            if (!permissions.Any())
            {
                entity._errorMessage = "At least one permissions must be provided.";
                return entity;
            }

            // TODO: check for username AND Id match. User should be able to change their own name

            //Role roleCheck = _repo.Roles.FirstOrDefault(r => r.Id == role.Id);
            //if (!ReferenceEquals(roleCheck, null))
            //{
            //    entity._errorMessage = "Role not found.";
            //    return entity;
            //}

            //ImpersonatingId = 0;
            Username = username.Trim();
            Permissions = permissions;

            _repo.Users.Attach(this);
            _repo.Entry(this).State = EntityState.Modified;
            int rowsAffected = _repo.SaveChanges(); // returns the number of rows affected
            SetRepo(_repo);
            return this;
        }

        public bool Remove()
        {
            if(Id < 1)
            {
                _errorMessage = "No user selected";
                return false;
            }

            IsActive = false;

            _repo.Users.Attach(this);
            _repo.Entry(this).State = EntityState.Modified;
            int rowsAffected = _repo.SaveChanges(); // returns the number of rows affected
            return true;
        }
    }
}

using System.Linq;
using System.Data.Entity;
using Orange.Core.Entities;
using Orange.Core.Repositories;
using System.Collections.Generic;

namespace Orange.Core.Entities
{
    public class Permission : Entity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        private ICollection<User> Users { get; set; }

        private Permission() { }

        public Permission(Repository repo)
        {
            _repo = repo;
            Users = new HashSet<User>();
        }

        public Permission GetById(int permissionId)
        {
            Permission entity = new Permission();
            if (permissionId < 1)
            {
                entity._errorMessage = "Id not recognized.";
                return entity;
            }

            entity = _repo.Permissions.FirstOrDefault(u => u.Id == permissionId);
            if (ReferenceEquals(entity, null))
            {
                entity = new Permission();
                entity._errorMessage = "Error encountered while retrieving data from the database.";
                return entity;
            }
            SetRepo(_repo);
            return entity;
        }

        public Permission GetByName(string name)
        {
            Permission entity = new Permission();
            if (string.IsNullOrWhiteSpace(name))
            {
                entity._errorMessage = "Name not recognized.";
                return entity;
            }

            entity = _repo.Permissions.FirstOrDefault(u => u.Name.ToLower() == name.ToLower());
            if (ReferenceEquals(entity, null))
            {
                entity = new Permission();
                entity._errorMessage = "Error encountered while retrieving data from the database.";
                return entity;
            }
            SetRepo(_repo);
            return entity;
        }

        public List<Permission> GetAll()
        {
            List<Permission> entities = _repo.Permissions.ToList();
            if (!entities.Any()) return null;
            entities.ToList().ForEach(u => u.SetRepo(_repo));
            return entities;
        }

        public Permission Add(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            if (string.IsNullOrWhiteSpace(description)) return null;

            // TODO: fix all this
            if (Id > 0)
            {
                _errorMessage = "Record conflict.";
                return null;
            }

            Permission check = _repo.Permissions.FirstOrDefault(p => p.Name.ToLower() == name.ToLower());
            if (!ReferenceEquals(check, null)) return null;

            Name = name.Trim();
            Description = description.Trim();
            IsActive = true;

            _repo.Permissions.Add(this);
            int rowsAffected = _repo.SaveChanges();

            Permission permission = _repo.Permissions.FirstOrDefault(p => p.Name.ToLower() == name.ToLower());
            if (ReferenceEquals(permission, null)) return null;

            permission.SetRepo(_repo);
            return permission;
        }
    }
}

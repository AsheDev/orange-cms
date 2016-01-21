using System;
using System.Linq;
using System.Data.Entity;
using Orange.Core.Repositories;
using System.Collections.Generic;

namespace Orange.Core.Entities
{
    public class Tag : Entity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }

        private Tag() { }

        public Tag(Repository repo)
        {
            _repo = repo;
        }

        public Tag GetById(int tagId)
        {
            Tag entity = new Tag();
            if(tagId < 1)
            {
                entity._errorMessage = "Id not recognized.";
                return entity;
            }

            entity = _repo.Tags.FirstOrDefault(t => t.Id == tagId);
            if (ReferenceEquals(entity, null))
            {
                entity = new Tag();
                entity._errorMessage = "Error encountered while retrieving data from the database.";
                return entity;
            }
            SetRepo(_repo);
            return entity;
        }

        public Tag GetByName(string tagName)
        {
            Tag entity = new Tag();
            if (string.IsNullOrWhiteSpace(tagName))
            {
                entity._errorMessage = "Name not recognized.";
                return entity;
            }

            entity = _repo.Tags.FirstOrDefault(t => t.Name == tagName);
            if (ReferenceEquals(entity, null))
            {
                entity = new Tag();
                entity._errorMessage = "Error encountered while retrieving data from the database.";
                return entity;
            }
            SetRepo(_repo);
            return entity;
        }

        public List<Tag> GetAll()
        {
            List<Tag> entities = _repo.Tags.ToList();
            if (!entities.Any()) return null;
            entities.ToList().ForEach(u => u.SetRepo(_repo));
            return entities;
        }

        public Tag Add(string name)
        {
            Tag entity = new Tag();
            if (string.IsNullOrWhiteSpace(name))
            {
                entity._errorMessage = "Name provided is invalid.";
                return entity;
            }
            if(Id > 0)
            {
                entity._errorMessage = "Record conflict.";
                return entity;
            }

            Tag tagCheck = _repo.Tags.FirstOrDefault(t => t.Name.ToLower() == name.ToLower());
            if (!ReferenceEquals(tagCheck, null)) return null; // already exists

            Name = name.Trim();
            IsActive = true;

            _repo.Tags.Add(this);
            int rowsAffected = _repo.SaveChanges(); // returns the number of rows affected
            SetRepo(_repo);
            return this;
        }

        public Tag Update(string name)
        {
            Tag entity = new Tag();
            if (Id < 1)
            {
                entity._errorMessage = "";
                return entity;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                entity._errorMessage = "Name provided is invalid.";
                return entity;
            }

            Tag tagCheck = _repo.Tags.FirstOrDefault(t => t.Name.ToLower() == name.ToLower());
            if (!ReferenceEquals(tagCheck, null)) return null; // already exists

            Name = name.Trim();
            IsActive = true;

            _repo.Tags.Attach(this);
            _repo.Entry(this).State = EntityState.Modified;
            int rowsAffected = _repo.SaveChanges(); // returns the number of rows affected
            SetRepo(_repo);
            return this;
        }

        public bool Remove()
        {
            if (Id < 1)
            {
                _errorMessage = "";
                return false;
            }

            IsActive = false;

            _repo.Tags.Attach(this);
            _repo.Entry(this).State = EntityState.Modified;
            int rowsAffected = _repo.SaveChanges(); // returns the number of rows affected
            return true;
        }
    }
}

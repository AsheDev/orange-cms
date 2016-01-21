using System;
using System.Linq;
using System.Data.Entity;
using Orange.Core.Interfaces;
using Orange.Core.Repositories;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Orange.Core.Entities
{
    public class Post : Entity
    {
        public int Id { get; private set; }
        public User Author { get; private set; } // 
        public string Title { get; private set; } //
        public string UniqueTitle { get; private set; }
        public string Body { get; private set; } //
        public DateTime Created { get; private set; }
        public DateTime EffectiveDate { get; private set; } //
        public int CommentCount { get; private set; } // Not sure this guy really fits
        public ICollection<Tag> Tags { get; private set; } //
        public bool IsPubliclyVisible { get; private set; } //
        public bool IsPublished { get; private set; } //
        public bool IsActive { get; private set; }

        public class PostBase
        {
            public User Author { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
            public DateTime EffectiveDate { get; set; }
            public ICollection<Tag> Tags { get; set; }
            public bool IsPubliclyVisible { get; set; }
            public bool IsPublished { get; set; }
        }

        private Post() { }

        public Post(Repository repo)
        {
            _repo = repo;
        }

        public Post GetById(int postId)
        {
            Post entity = new Post();
            if (Id < 1)
            {
                entity._errorMessage = "Id not recognized.";
                return entity;
            }

            entity = _repo.Posts.Include(p => p.Author).
                Include(p => p.Tags).FirstOrDefault(p => p.Id == postId);
            if (ReferenceEquals(entity, null)) return null;
            SetRepo(_repo);
            return entity;
        }

        public Post GetByUniqueTitle(string uniqueTitle)
        {
            Post entity = new Post();
            if (string.IsNullOrWhiteSpace(uniqueTitle))
            {
                entity._errorMessage = "Title not recognized.";
                return entity;
            }

            entity = _repo.Posts.Include(p => p.Author).
                Include(p => p.Tags).FirstOrDefault(p => p.UniqueTitle == uniqueTitle);
            if (ReferenceEquals(entity, null)) return null;
            SetRepo(_repo);
            return entity;
        }

        public List<Post> GetAll()
        {
            List<Post> entities = _repo.Posts.Include(p => p.Author).
                Include(p => p.Tags).ToList();
            if (!entities.Any()) return null;
            entities.ToList().ForEach(u => u.SetRepo(_repo));
            return entities;
        }

        public Post Add(PostBase details)
        {
            if (Id > 0) return null;
            if (ReferenceEquals(details.Author, null)) return null;
            if (string.IsNullOrWhiteSpace(details.Title)) return null;
            if (string.IsNullOrWhiteSpace(details.Body)) return null;
            if (details.EffectiveDate < DateTime.Now) return null;

            string uniqueTitle = GenerateUniqueTitle(details.Title.Trim());

            List<Post> postCheck = _repo.Posts.Where(p => p.Title.ToLower() == details.Title.ToLower() || 
                p.UniqueTitle.ToLower() == uniqueTitle).ToList();
            if (postCheck.Any()) return null;

            Author = details.Author;
            Title = details.Title.Trim();
            UniqueTitle = uniqueTitle;
            Body = details.Body.Trim();
            Created = DateTime.Now;
            EffectiveDate = details.EffectiveDate;
            CommentCount = 0;
            Tags = details.Tags;
            IsPubliclyVisible = details.IsPubliclyVisible;
            IsPublished = details.IsPublished;
            IsActive = true;

            _repo.Posts.Add(this);
            int rowsAffected = _repo.SaveChanges(); // returns the number of rows affected
            SetRepo(_repo);
            return this;
        }

        public Post Update(PostBase details)
        {
            if (Id < 1) return null;
            if (ReferenceEquals(details.Author, null)) return null;
            if (string.IsNullOrWhiteSpace(details.Title)) return null;
            if (string.IsNullOrWhiteSpace(details.Body)) return null;
            if (details.EffectiveDate < DateTime.Now) return null;

            string uniqueTitle = GenerateUniqueTitle(details.Title.Trim());

            // TODO: this needs to be aware of the AUTHOR
            //List<Post> postCheck = _repo.Posts.FirstOrDefault(p => p.Title.ToLower() == details.Title.ToLower() ||
            //    p.UniqueTitle.ToLower() == uniqueTitle);
            //if (postCheck.Any()) return null;

            Author = details.Author;
            Title = details.Title.Trim();
            UniqueTitle = uniqueTitle;
            Body = details.Body.Trim();
            Created = Created;
            EffectiveDate = details.EffectiveDate;
            CommentCount = CommentCount;
            Tags = details.Tags;
            IsPubliclyVisible = details.IsPubliclyVisible;
            IsPublished = details.IsPublished;
            IsActive = IsActive;

            _repo.Posts.Attach(this);
            _repo.Entry(this).State = EntityState.Modified;
            int rowsAffected = _repo.SaveChanges(); // returns the number of rows affected
            SetRepo(_repo);
            return this;
        }

        public bool Remove()
        {
            if (Id < 1) return false;

            IsActive = false;

            _repo.Posts.Attach(this);
            _repo.Entry(this).State = EntityState.Modified;
            int rowsAffected = _repo.SaveChanges(); // returns the number of rows affected
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private string GenerateUniqueTitle(string value)
        {
            string regexPattern = @"([\W_])+";
            string substitute = "+";
            int maxLengthOfUrl = 64;

            Regex regex = new Regex(regexPattern);
            string uniqueUrl = Regex.Replace(value, @"'", ""); // clear out apostrophes first
            uniqueUrl = Regex.Replace(regex.Replace(uniqueUrl, substitute), regexPattern, substitute).ToLower(); // remove remaining special characters

            // trim the string if too long
            if (uniqueUrl.Length > maxLengthOfUrl) uniqueUrl = uniqueUrl.Substring(0, maxLengthOfUrl);

            // if the post ends with a special character we should chop it off
            if (uniqueUrl.Substring(uniqueUrl.Length - 1, 1).Equals(substitute)) uniqueUrl = uniqueUrl.TrimEnd(Convert.ToChar(substitute));

            return uniqueUrl.ToLower();
        }
    }
}

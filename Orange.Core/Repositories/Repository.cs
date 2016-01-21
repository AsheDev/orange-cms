using System;
using System.Linq;
using Orange.Core.Values;
using System.Data.Entity;
using Orange.Core.Entities;
using System.Collections.Generic;

namespace Orange.Core.Repositories
{
    public class Repository : DbContext
    {
        // if we want to use a connectionstring
        public Repository() : base("DefaultConnection") 
        {
            //Database.SetInitializer<Repository>(new DropCreateDatabaseAlways<Repository>());
            Database.SetInitializer<Repository>(new RepositoryInitializer());
        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Credentials> Credentials { get; set; }
        internal DbSet<UserPermissionMap> UserPermissionMapping { get; set; }
        public DbSet<SiteSettings> SiteSettings { get; set; }
    }

    public class RepositoryInitializer : DropCreateDatabaseAlways<Repository>
    {
        private Repository _repo;

        protected override void Seed(Repository repo)
        {
            _repo = repo;
            DefineSiteSettings();
            BuildPermissions();
            BuildUsers();
            //BuildCredentials();
            BuildTags();
            BuildPosts();
            base.Seed(repo);
        }

        private void DefineSiteSettings()
        {
            bool success = new SiteSettings(_repo).Set(3600, false);
        }

        private void BuildPermissions()
        {
            Permission permission = new Permission(_repo);
            permission.Add("Manage All Posts", "Allow user to edit/delete all posts.");
            Permission canPost = new Permission(_repo);
            canPost.Add("Can Post", "Can create a new post.");
            Permission canManageOwnPost = new Permission(_repo);
            canManageOwnPost.Add("Can Manage Own Post", "User can manage their own post.");
            Permission permissionTwo = new Permission(_repo);
            permissionTwo.Add("Manage Post Comments", "Allow user to edit/delete post comments.");
            Permission permissionThree = new Permission(_repo);
            permissionThree.Add("Can Comment", "Allow user to comment on blog posts.");
            Permission permissionFour = new Permission(_repo);
            permissionFour.Add("Manage Users", "Allow user to administer user accounts.");
            Permission permissionFive = new Permission(_repo);
            permissionFive.Add("Access Settings", "Allow user to access and manage system settings.");
            Permission permissionSix = new Permission(_repo);
            permissionSix.Add("Can Impersonate", "Allow user to impersonate another user.");
            Permission permissionSeven = new Permission(_repo);
            permissionSeven.Add("View Metrics", "Allow user to view website metrics.");
            Permission permissionEight = new Permission(_repo);
            permissionEight.Add("Basic Access", "Gives the user basic access to the site.");
        }

        private void BuildUsers()
        {
            Authentication auth = new Authentication();
            // precomupted password is Welcome1
            HashedPassword precomputed = new HashedPassword("UR4d7ytj87XHHN88EhU2y1NUW4ekb1S+", "jniAKI8ORo2OQsVx6JEuMHNRq544g2Su", 50000);

            List<Permission> allPermissions = _repo.Permissions.ToList();
            User orange = new User(_repo).Add("Orange", false, allPermissions);
            new Credentials(_repo).Add(orange.Id, "orange@michaelovies.com", precomputed);

            List<Permission> adminPermissions = _repo.Permissions.ToList();
            User admin = new User(_repo).Add("Admin", true, adminPermissions);
            new Credentials(_repo).Add(admin.Id, "admin@michaelovies.com", precomputed);

            List<Permission> basicPermissions = _repo.Permissions.Where(p => p.Name == "Basic Access").ToList();
            User basic = new User(_repo).Add("Basic", true, basicPermissions);
            new Credentials(_repo).Add(basic.Id, "basic@michaelovies.com", precomputed);

            List<Permission> ripleyPermissions = _repo.Permissions.Where(p => p.Name == "Basic Access" 
                || p.Name == "Can Post" || p.Name == "Can Manage Own Post").ToList();
            User ripley = new User(_repo).Add("Ripley", true, ripleyPermissions);
            new Credentials(_repo).Add(ripley.Id, "ripley@michaelovies.com", precomputed);

            List<Permission> anonPermissions = _repo.Permissions.Where(p => p.Name == "Can Comment").ToList();
            User anonymous = new User(_repo).Add("Anonymous", true, anonPermissions);
            new Credentials(_repo).Add(anonymous.Id, "anonymous@michaelovies.com", precomputed);
        }

        private void BuildTags()
        {
            List<string> tagNames = new List<string> { "Design", "Entity Framework", "C#", "Programming" };

            foreach(string name in tagNames)
            {
                new Tag(_repo).Add(name);
            }
        }

        private void BuildPosts()
        {
            Post.PostBase postDetails = new Post.PostBase();
            postDetails.Author = _repo.Users.First(u => u.Username == "Ripley"); // break if not found
            postDetails.Title = "The Post's Title RAHF AIDS E HEFAHFEAHF FHEALS AAK DSAK LDASDKLA DEED";
            postDetails.Body = "BODY OF THE POST!";
            postDetails.EffectiveDate = DateTime.Now.AddDays(6);
            postDetails.IsPubliclyVisible = true;
            postDetails.IsPublished = true;
            postDetails.Tags = _repo.Tags.Take(2).ToList();

            Post post = new Post(_repo).Add(postDetails);
        }
    }
}

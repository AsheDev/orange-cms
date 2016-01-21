using System;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Orange.Core.Results;
using Orange.Core.Entities;
using System.Threading.Tasks;
using Orange.Core.Repositories;
using System.Collections.Generic;

namespace TestSite
{
    public class Program
    {
        private static Repository _repo = new Repository();

        public static void Main(string[] args)
        {
            //List<Permission> permissions = new Permission(_repo).GetAll();
            User user = new User(_repo).GetById(2);
            if (user.Permissions != null) Console.WriteLine("Derp");

            List<User> users = new User(_repo).GetAll();
            List<Tag> tags = _repo.Tags.ToList();
            SiteSettings settings = _repo.SiteSettings.FirstOrDefault();
            List<Post> posts = _repo.Posts.ToList();
            List<Credentials> credentials = _repo.Credentials.ToList();

            //Authentication auth = new Authentication();
            //if(auth.Validate("Welcome1", credentials.First().SaltedAndHashedPassword))
            //{
            //    Console.WriteLine("Authenticated!");
            //}


            UserResult result = new UserResult(_repo);
            result.Result = result.Result.GetById(88);


        }
    }
}

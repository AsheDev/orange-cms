using System;
using System.Web;
using Web.Models;
using System.Linq;
using System.Web.Mvc;
using Orange.Business;
using RC = Connections;
using System.Data.Entity;
using Orange.Core.Results;
using System.Configuration;
using Orange.Core.Entities;
using System.Collections.Generic;

namespace Web.Controllers
{
    public class PagesController : Controller
    {
        //
        // GET: /Pages/

        public ActionResult Home()
        {
            HttpCookie user = new HttpCookie("ChocolateChip");
            user.Value = "false";
            user.Expires = DateTime.Now.AddDays(7);
            //user.Secure = true;
            Response.Cookies.Add(user);
            return View("Home");
        }

        public ActionResult BlogFeed()
        {
            //RC.Database orange = new RC.Database(ConfigurationManager.ConnectionStrings["DevOrange"].ConnectionString);
            RC.Database orange = new RC.Database("DevOrange");
            PostResultList results = new PostOps(orange).GetAll();

            BlogFeed model = new BlogFeed()
            {
                Posts = (results.Severity == Orange.Core.Enums.Severity.Success) ? results.Results : new List<Post>()
            };

            return View("BlogFeed", model);
        }
    }
}

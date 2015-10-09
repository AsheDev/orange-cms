using System;
using System.Web;
using Connections;
using System.Linq;
using Web.Filters;
using System.Web.Mvc;
using Orange.Business;
using Orange.Core.Enums;
using Orange.Core.Models;
using Orange.Core.Results;
using System.Configuration;
using Orange.Core.Entities;
using System.Collections.Generic;

namespace Web.Controllers
{
    public class AuthenticationController : Controller
    {
        // this should be loading an empty class I think. I'd be happier with it
        // Use Session for shit you need to trust
        //  Use cookies for stuff you don't people possibly editing/deleting
        [HttpPost]
        public ActionResult Authenticate(string username, string password)
        {
            HttpCookie cookie = Request.Cookies.Get("ChocolateChip");

            //Database orange = new Database(ConfigurationManager.AppSettings["DevOrange"]);
            Database orange = new Database("DevOrange");
            UserResult userInfo = new UserOps(orange).Login(username, password);

            Authentication model = new Authentication();

            if (userInfo.Severity == Orange.Core.Enums.Severity.Success)
            {
                Session["Authenticated"] = true;
                Session["Username"] = userInfo.Result.Name;
                cookie.Expires = DateTime.Now.AddDays(1);
                cookie.Value = "true";
                model.Status = AuthenticationStatus.Success;
                Response.Cookies.Remove("ChocolateChip");
                Response.Cookies.Set(cookie);
                return Redirect("/System/Users");
            }
            else
            {
                Session["Authenticated"] = false;
                cookie.Value = "false";
                model.Status = AuthenticationStatus.Failure;
                Response.Cookies.Remove("ChocolateChip");
                Response.Cookies.Set(cookie);
                return View("Peel", model);
            }
        }

        public ActionResult Peel()
        {
            return View("Peel", new Authentication());
        }

        // !Orange_2015!
    }
}

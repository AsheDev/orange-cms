using System;
using System.Web;
using System.Linq;
using Web.Filters;
using System.Web.Mvc;
using Orange.Business;
using Orange.Core.Enums;
using Ripley.Connections;
using Orange.Core.Models;
using Orange.Core.Results;
using Orange.Core.Entities;
using System.Collections.Generic;

namespace Web.Controllers
{
    public class AuthenticationController : Controller
    {
        //  this should be loading an empty class I think. I'd be happier with it
        [HttpPost]
        public ActionResult Authenticate(string username, string password)
        {
            HttpCookie cookie = Request.Cookies.Get("ChocolateChip");

            Database orange = new Database("DevOrange"); // TODO: move to webconfig
            UserResult result = new UserOps(orange).Login(username, password);

            Authentication model = new Authentication();

            if (result.Severity == Orange.Core.Enums.Severity.Success)
            {
                cookie.Expires = DateTime.Now.AddDays(7);
                cookie.Value = "true";
                model.Status = AuthenticationStatus.Success;
                Response.Cookies.Remove("ChocolateChip");
                Response.Cookies.Set(cookie);
                return Redirect("/System/Users");
            }
            else
            {
                // TODO: this should be a bit more robust :P
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

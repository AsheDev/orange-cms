using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
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

    }
}

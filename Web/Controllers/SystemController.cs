using System;
using System.Web;
using System.Linq;
using Web.Filters;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Web.Controllers
{
    public class SystemController : Controller
    {
        //
        // GET: /Panel/

        [SessionExpiredFilter]
        public ActionResult Landing()
        {
            return View("Landing");
        }

        [SessionExpiredFilter]
        public ActionResult Users()
        {
            return PartialView("Users");
        }

        [SessionExpiredFilter]
        public ActionResult Accessibility()
        {
            return PartialView("Accessibility");
        }

        [SessionExpiredFilter]
        public ActionResult Metrics()
        {
            return PartialView("Metrics");
        }

        [SessionExpiredFilter]
        public ActionResult Permissions()
        {
            return PartialView("Permissions");
        }

        [SessionExpiredFilter]
        public ActionResult Security()
        {
            return PartialView("Security");
        }

    }
}

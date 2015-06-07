using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web.Filters
{
    // TODO: this needs to be registered in global.asax
    public class SessionExpiredFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Cookies["ChocolateChip"] == null)
            {
                // I think this is working now... maybe not the best way it can be
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Pages", area = "Home" }));
            }
            else
            {
                // if not expired update the expiration info
                base.OnActionExecuting(filterContext);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace SoftwareCelta.Filters
{
    public class Permissions : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.HttpContext.Session["userID"] != null)
            {
                int userID = Convert.ToInt32(filterContext.HttpContext.Session["userID"]);
            }
            else {

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Users",
                    action = "Login"
                }));
            }
                
            
       //     Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
         //   Action = filterContext.ActionDescriptor.ActionName + " (Logged By        

            this.OnActionExecuting(filterContext);
        }
    }
}
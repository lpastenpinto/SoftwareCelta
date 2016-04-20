using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using SoftwareCelta.Models;
using SoftwareCelta.DAL;

namespace SoftwareCelta.Filters
{
    public class Permissions : ActionFilterAttribute, IActionFilter
    {
        public int Permission1 {get;set;}
        public int Permission2 { set; get; }
        public int Permission3 { set; get; }
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.HttpContext.Session["userID"] != null)
            {
                ContextBDCelta db = new ContextBDCelta();
                int userID = Convert.ToInt32(filterContext.HttpContext.Session["userID"]);
                user user = db.Users.Find(userID);
                List<permisoUser> permisosUser = db.PermisosUser.Where(s => s.userID == user.userID).ToList();                

                List<int> permisosUserInt = new List<int>();
                foreach(var p in permisosUser){
                    permisosUserInt.Add(p.rolesID);
                }

                List<int> listaPermisosReq = new List<int>();
                if (Permission1!=0)
                {
                    listaPermisosReq.Add(Permission1);
                }
                if (Permission2 != 0)
                {
                    listaPermisosReq.Add(Permission2);
                }
                if (Permission3 != 0)
                {
                    listaPermisosReq.Add(Permission3);
                }

                
                bool verificador = false;

                if (listaPermisosReq.Count == 0)
                {
                    verificador = true;       
                }
                else
                {
                    foreach (var perm in listaPermisosReq)
                    {
                        if (permisosUserInt.Contains(perm))
                        {
                            verificador = true;
                        }
                    }
                }
                if (!verificador) {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Users",
                        action = "sinPermisos"
                    }));
                
                }
                /*else {

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Users",
                        action = "Login"
                    }));
                }*/
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
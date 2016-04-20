using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoftwareCelta.DAL;
using SoftwareCelta.Models;
using SoftwareCelta.Filters;

namespace SoftwareCelta.Controllers
{
    public class HomeController : Controller
    {
        private ContextBDCelta db = new ContextBDCelta();

        [Permissions]
        public ActionResult Index()
        {           
            return View();
        }

        public ActionResult About()
        {
            List<int> lista = (List<int>)Session["permisosUser"];
            if (lista.Contains(1))
            {
                ViewBag.Message = "Your application description page.";
            }
            else {
                ViewBag.Message = "NO LO TIENE";
            }
            

            return View();
        }

        public ActionResult Contact()
        {
            //ViewData["user"] = db.Users.ToList();          
            ViewBag.Message = "Your contact page.";

            return View(db.Users.ToList());
        }
    }
}
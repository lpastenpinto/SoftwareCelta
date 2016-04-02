using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoftwareCelta.DAL;
namespace SoftwareCelta.Controllers
{
    public class HomeController : Controller
    {
        private ContextBDCelta db = new ContextBDCelta();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
                  
            ViewBag.Message = "Your application description page.";

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
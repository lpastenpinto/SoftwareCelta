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
      
    }
}
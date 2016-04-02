using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoftwareCelta.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection post)
        {
            Session["userID"] = "User";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Contact", "Home");
        }
    }
}
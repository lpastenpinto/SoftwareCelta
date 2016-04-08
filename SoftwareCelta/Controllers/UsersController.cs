using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoftwareCelta.DAL;
using SoftwareCelta.Models;

namespace SoftwareCelta.Controllers
{
    public class UsersController : Controller
    {
        private ContextBDCelta db = new ContextBDCelta();

        public ActionResult Index() {
            return View(db.Users.ToList());
        }
        // GET: Users
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection post)
        {
            Session["userID"] = 1;
            Session["userName"] = "lpasten";

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection form) {

            user NewUser = new user();
            NewUser.userName = (string)form["userName"];
            NewUser.nombreCompleto = (string)form["nombreCompleto"];
            NewUser.password = encriptacion.GetMD5((string)form["password"]);
            db.Users.Add(NewUser);
            db.SaveChanges();
            return RedirectToAction("Index");            
        }

   
        public ActionResult AsignarPermisos(int userID) {
            user user = db.Users.Find(userID);
            ViewData["roles"] = db.Roles.ToList();
            return View(user);
        }


        [HttpPost]
        public ActionResult AsignarPermisos(FormCollection form)
        {
            string[] rolID = Request.Form.GetValues("rolID");
            string[] tienePermiso = Request.Form.GetValues("tienePermiso");
            int userID = Convert.ToInt32((string)form["userID"]);

            var permisosUser = db.PermisosUser.Where(u => u.userID == userID);

            foreach (var permiso in permisosUser)
            {
                db.PermisosUser.Remove(permiso);
            }

            for(int i=0;i<rolID.Length ;i++){
                if(tienePermiso[i].Equals("si")){
                    permisoUser permTemp= new permisoUser();
                    permTemp.userID=userID;
                    permTemp.rolesID=Convert.ToInt32(rolID[i]);
                    db.PermisosUser.Add(permTemp);
                }                
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
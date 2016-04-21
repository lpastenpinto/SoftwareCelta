using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoftwareCelta.DAL;
using SoftwareCelta.Models;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity;
using SoftwareCelta.Filters;
namespace SoftwareCelta.Controllers
{
    public class UsersController : Controller
    {
        private ContextBDCelta db = new ContextBDCelta();

        [Permissions(Permission1 = 1)]
        public ActionResult Index() {
            return View(db.Users.ToList());
        }
        // GET: Users
        public ActionResult Login()
        {
           // Mail.send("leohm63@gmail.com","subject","body");*/            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection post)
        {
            string userName=(string)post["userName"];
            string password = encriptacion.GetMD5((string)post["password"]);
            user user=db.Users.SingleOrDefault(s=>s.userName==userName & s.password==password);
            if(user==null){
                ViewBag.mensaje = "login erroneo";
                return View();
            }else{
                List<permisoUser> permisosUser= db.PermisosUser.Where(s=>s.userID==user.userID).ToList();
                List<int> permisos = new List<int>();
                foreach (var perm in permisosUser) {
                    permisos.Add(perm.rolesID);
                }
                Session["userID"] = user.userID;
                Session["userName"] = user.userName;
                Session["permisosUser"] = permisos;
                return RedirectToAction("Index", "Home");
            }                                    
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }

        [Permissions(Permission1 = 1)]
        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permissions(Permission1 = 1)]
        public ActionResult Create(FormCollection form) {

            user NewUser = new user();
            NewUser.userName = (string)form["userName"];
            NewUser.nombreCompleto = (string)form["nombreCompleto"];
            NewUser.password = encriptacion.GetMD5((string)form["password"]);
            db.Users.Add(NewUser);
            db.SaveChanges();
            return RedirectToAction("Index");            
        }

        [Permissions(Permission1 = 1)]
        public ActionResult AsignarPermisos(int userID) {
            user user = db.Users.Find(userID);
            List<permisoUser> permisos = db.PermisosUser.Where(s => s.userID == userID).ToList();
            List<int> permisosUser = new List<int>();
            foreach (var perm in permisos) {
                permisosUser.Add(perm.rolesID);
            }
            ViewData["permisosUser"] = permisosUser;
            ViewData["roles"] = db.Roles.ToList();
            return View(user);
        }


        [HttpPost]
        [Permissions(Permission1 = 1)]
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

        public ActionResult cambiarDatos()
        {                        
            if (Session["userID"] != null)
            {
                string id = Session["userID"].ToString();
                int userID = Convert.ToInt32(id);
                user user = db.Users.Find(userID);
                return View(user);
            }
            else {
                return Redirect("sinPermisos");
            }
        }

        [HttpPost]        
        public ActionResult cambiarDatos(FormCollection form)
        {
            
            int userID = Convert.ToInt32((string)form["userID"]);
            user user = db.Users.Find(userID);
            user.userName = (string)form["userName"];
            user.nombreCompleto = (string)form["nombreCompleto"];
            string newPass = encriptacion.GetMD5((string)form["passwordNew"]);
            if (!newPass.Equals("")) {
                user.password = newPass;
            }
            //user.password = (string)form["passwordNew"];//
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Success"] = "Usuario "+user.userName+" sus datos han sido guardados exitosamente";
            return View(user);
        }

        [Permissions(Permission1 = 1)]
        public ActionResult Delete(int userID) {
            user user = db.Users.Find(userID);
            return View(user);
        }


        [HttpPost]
        [Permissions(Permission1 = 1)]
        public ActionResult Delete(FormCollection form) {
            int userID = Convert.ToInt32((string)form["userID"]);
            user user = db.Users.Find(userID);
            db.Users.Remove(user);
            db.SaveChanges();
            dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Se elimina usuario de nombre " + user.userName);            
            return RedirectToAction("Index");
        }


        public ActionResult sinPermisos() {
            ViewBag.userName=Session["userName"].ToString();
            return View();
        }

    }
}
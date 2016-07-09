using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoftwareCelta.DAL;
using SoftwareCelta.Models;
using SoftwareCelta.Filters;
using System.Data.Entity;

namespace SoftwareCelta.Controllers
{
    public class HomeController : Controller
    {
        private ContextBDCelta db = new ContextBDCelta();

        [Permissions]
        public ActionResult Index()
        {
            /*dw_envio envio1 = db.DatosEnvio.Find(410);
            envio1.valeVenta = "106394";
            db.Entry(envio1).State = EntityState.Modified;
            db.SaveChanges();

            dw_envio envio2 = db.DatosEnvio.Find(438);
            envio2.valeVenta = "106743";
            db.Entry(envio2).State = EntityState.Modified;
            db.SaveChanges();

            dw_envio envio3 = db.DatosEnvio.Find(442);
            envio3.valeVenta = "106804";
            db.Entry(envio3).State = EntityState.Modified;
            db.SaveChanges();*/
            
                
            return View();
        }
      
    }
}
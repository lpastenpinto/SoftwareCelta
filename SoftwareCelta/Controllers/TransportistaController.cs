using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SoftwareCelta.DAL;
using SoftwareCelta.Models;
using SoftwareCelta.Filters;

namespace SoftwareCelta.Controllers
{
    public class TransportistaController : Controller
    {
        private ContextBDCelta db = new ContextBDCelta();

        // GET: Transportista

        [Permissions(Permission1 = 1, Permission2 = 9)]
        public ActionResult Index()
        {
            return View(db.Transportistas.ToList());
        }

        // GET: Transportista/Details/5
        [Permissions(Permission1 = 1, Permission2 = 9)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dw_datosTransportista dw_datosTransportista = db.Transportistas.Find(id);
            if (dw_datosTransportista == null)
            {
                return HttpNotFound();
            }
            return View(dw_datosTransportista);
        }

        // GET: Transportista/Create
        [Permissions(Permission1 = 1, Permission2 = 9)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transportista/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permissions(Permission1 = 1, Permission2 = 9)]
        public ActionResult Create([Bind(Include = "dw_datosTransportistaID,nombreCompleto,rut,razonSocial,patente,direccion,telefono,mail,contacto,ciudad,giro")] dw_datosTransportista dw_datosTransportista)
        {
            if (ModelState.IsValid)
            {
                db.Transportistas.Add(dw_datosTransportista);
                db.SaveChanges();
                dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Se crea transportista nuevo. Nombre:"+dw_datosTransportista.nombreCompleto+" | patente:"+dw_datosTransportista.patente);           
                return RedirectToAction("Index");
            }

            return View(dw_datosTransportista);
        }

        // GET: Transportista/Edit/5
        [Permissions(Permission1 = 1, Permission2 = 9)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dw_datosTransportista dw_datosTransportista = db.Transportistas.Find(id);
            if (dw_datosTransportista == null)
            {
                return HttpNotFound();
            }
            return View(dw_datosTransportista);
        }

        // POST: Transportista/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permissions(Permission1 = 1, Permission2 = 9)]
        public ActionResult Edit([Bind(Include = "dw_datosTransportistaID,nombreCompleto,rut,razonSocial,patente,direccion,telefono,mail,contacto,ciudad,giro")] dw_datosTransportista dw_datosTransportista)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dw_datosTransportista).State = EntityState.Modified;
                db.SaveChanges();
                dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Se Edito informacion de transportista ahora con nombre: "+dw_datosTransportista.nombreCompleto);           
                return RedirectToAction("Index");
            }
            return View(dw_datosTransportista);
        }

        // GET: Transportista/Delete/5
        [Permissions(Permission1 = 1, Permission2 = 9)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dw_datosTransportista dw_datosTransportista = db.Transportistas.Find(id);
            if (dw_datosTransportista == null)
            {
                return HttpNotFound();
            }
            return View(dw_datosTransportista);
        }

        // POST: Transportista/Delete/5
        [HttpPost, ActionName("Delete")]
        [Permissions(Permission1 = 1, Permission2 = 9)]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dw_datosTransportista dw_datosTransportista = db.Transportistas.Find(id);
            db.Transportistas.Remove(dw_datosTransportista);
            db.SaveChanges();
            dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Se elimino transportista"+dw_datosTransportista.nombreCompleto);           
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

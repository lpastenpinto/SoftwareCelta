﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SoftwareCelta.DAL;
using SoftwareCelta.Models;

namespace SoftwareCelta.Controllers
{
    public class TransportistaController : Controller
    {
        private ContextBDCelta db = new ContextBDCelta();

        // GET: Transportista
        public ActionResult Index()
        {
            return View(db.Transportistas.ToList());
        }

        // GET: Transportista/Details/5
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transportista/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "dw_datosTransportistaID,nombreCompleto,rut,razonSocial,direccion,telefono,mail,contacto,ciudad,giro")] dw_datosTransportista dw_datosTransportista)
        {
            if (ModelState.IsValid)
            {
                db.Transportistas.Add(dw_datosTransportista);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dw_datosTransportista);
        }

        // GET: Transportista/Edit/5
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "dw_datosTransportistaID,nombreCompleto,rut,razonSocial,direccion,telefono,mail,contacto,ciudad,giro")] dw_datosTransportista dw_datosTransportista)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dw_datosTransportista).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dw_datosTransportista);
        }

        // GET: Transportista/Delete/5
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
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dw_datosTransportista dw_datosTransportista = db.Transportistas.Find(id);
            db.Transportistas.Remove(dw_datosTransportista);
            db.SaveChanges();
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

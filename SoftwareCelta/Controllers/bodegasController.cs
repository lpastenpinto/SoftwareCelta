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
using SoftwareCelta.Filters;
namespace SoftwareCelta.Controllers
{
    public class bodegasController : Controller
    {
        private ContextBDCelta db = new ContextBDCelta();

        // GET: bodegas
        [Permissions(Permission1 = 1, Permission2 = 8)]
        public ActionResult Index()
        {   //db.Bodegas.ToList()
            return View(db.Bodegas.ToList());
        }

        // GET: bodegas/Details/5
        [Permissions(Permission1 = 1, Permission2 = 8)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dw_areaInterna bodega = db.Bodegas.Find(id);
            if (bodega == null)
            {
                return HttpNotFound();
            }
            return View(bodega);
        }

        // GET: bodegas/Create
        [Permissions(Permission1 = 1, Permission2 = 8)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: bodegas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permissions(Permission1 = 1, Permission2 = 8)]
        public ActionResult Create([Bind(Include = "dw_areaInternaID,nombre")] dw_areaInterna bodega)
        {
            if (ModelState.IsValid)
            {
                db.Bodegas.Add(bodega);
                db.SaveChanges();
                dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Se crea Area Interna. Nombre:"+bodega.nombre);           
                return RedirectToAction("Index");
            }

            return View(bodega);
        }

        // GET: bodegas/Edit/5
        [Permissions(Permission1 = 1, Permission2 = 8)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dw_areaInterna bodega = db.Bodegas.Find(id);
            if (bodega == null)
            {
                return HttpNotFound();
            }
            return View(bodega);
        }

        // POST: bodegas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permissions(Permission1 = 1, Permission2 = 8)]
        public ActionResult Edit([Bind(Include = "dw_areaInternaID,nombre")] dw_areaInterna bodega)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bodega).State = EntityState.Modified;
                db.SaveChanges();
                dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Se edito area Interna ahora con nombre "+bodega.nombre);
           
                return RedirectToAction("Index");
            }
            return View(bodega);
        }

        // GET: bodegas/Delete/5
        [Permissions(Permission1 = 1, Permission2 = 8)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dw_areaInterna bodega = db.Bodegas.Find(id);
            if (bodega == null)
            {
                return HttpNotFound();
            }
            return View(bodega);
        }

        // POST: bodegas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Permissions(Permission1 = 1, Permission2 = 8)]
        public ActionResult DeleteConfirmed(int id)
        {
            dw_areaInterna bodega = db.Bodegas.Find(id);
            db.Bodegas.Remove(bodega);
            db.SaveChanges();
            dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Se elimina area interna de nombre "+bodega.nombre);           
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

        public JsonResult getAreasInternas()
        {
            List<dw_areaInterna> areasInternas = db.Bodegas.ToList();
            var result = areasInternas;
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        
    }
}

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

namespace SoftwareCelta.Controllers
{
    public class tipoDocumentoController : Controller
    {
        private ContextBDCelta db = new ContextBDCelta();

        // GET: tipoDocumento
        public ActionResult Index()
        {
            return View(db.tiposDocumentos.ToList());
        }

        // GET: tipoDocumento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dw_tipoDocumento dw_tipoDocumento = db.tiposDocumentos.Find(id);
            if (dw_tipoDocumento == null)
            {
                return HttpNotFound();
            }
            return View(dw_tipoDocumento);
        }

        // GET: tipoDocumento/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tipoDocumento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "dw_tipoDocumentoID,nombreTipoDocumento")] dw_tipoDocumento dw_tipoDocumento)
        {
            if (ModelState.IsValid)
            {
                db.tiposDocumentos.Add(dw_tipoDocumento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dw_tipoDocumento);
        }

        // GET: tipoDocumento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dw_tipoDocumento dw_tipoDocumento = db.tiposDocumentos.Find(id);
            if (dw_tipoDocumento == null)
            {
                return HttpNotFound();
            }
            return View(dw_tipoDocumento);
        }

        // POST: tipoDocumento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "dw_tipoDocumentoID,nombreTipoDocumento")] dw_tipoDocumento dw_tipoDocumento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dw_tipoDocumento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dw_tipoDocumento);
        }

        // GET: tipoDocumento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dw_tipoDocumento dw_tipoDocumento = db.tiposDocumentos.Find(id);
            if (dw_tipoDocumento == null)
            {
                return HttpNotFound();
            }
            return View(dw_tipoDocumento);
        }

        // POST: tipoDocumento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dw_tipoDocumento dw_tipoDocumento = db.tiposDocumentos.Find(id);
            db.tiposDocumentos.Remove(dw_tipoDocumento);
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

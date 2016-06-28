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
    public class EnvioController : Controller
    {
        private ContextBDCelta db = new ContextBDCelta();

        // GET: Envio
        [Permissions(Permission1 = 1, Permission2 = 7)]
        public ActionResult Index(string fechaDesde, string fechaHasta)
        {
            List<dw_envio> listEnvios = new List<dw_envio>();
            if (fechaDesde != null && fechaHasta != null) {
                DateTime fechaInicial = Formateador.fechaStringToDateTime(fechaDesde);
                DateTime fechaFinal = Formateador.fechaStringToDateTime(fechaHasta);
                listEnvios = db.DatosEnvio.Where(s => s.fechaValeVenta >= fechaInicial & s.fechaValeVenta <= fechaFinal).ToList();
                ViewBag.fechaDesde = fechaDesde;
                ViewBag.fechaHasta = fechaHasta;
            }
            else
            {
                DateTime fechaActual = DateTime.Today;
                listEnvios = db.DatosEnvio.Where(s => s.fechaValeVenta == fechaActual).ToList();
                ViewBag.fechaDesde = Formateador.fechaCompletaToString(fechaActual);
                ViewBag.fechaHasta = Formateador.fechaCompletaToString(fechaActual);
            }
            
            return View(listEnvios);
        }

        // GET: Envio/Details/5
        [Permissions(Permission1 = 1, Permission2 = 7)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dw_envio dw_envio = db.DatosEnvio.Find(id);
            if (dw_envio == null)
            {
                return HttpNotFound();
            }
            return View(dw_envio);
        }

        // GET: Envio/Create
        [Permissions(Permission1 = 1, Permission2 = 7)]
        public ActionResult Create()
        {
            ViewData["dw_ciudades"] = dw_ciudades_despacho.listaCiudades();
            return View();
        }

        // POST: Envio/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permissions(Permission1 = 1, Permission2 = 7)]
        public ActionResult Create([Bind(Include = "dw_envioID,nombreCliente,rutCliente,ciudad,telefono,direccion,observacion,fechaValeVenta")] dw_envio dw_envioNew, FormCollection form)
        {
            DateTime fecha = Formateador.fechaStringToDateTime((string)form["fechaValeVenta"]);
            dw_envioNew.fechaValeVenta = fecha;

            string[] valeVenta = Request.Form.GetValues("valeVenta");
            for (int i = 0; i < valeVenta.Length; i++)
            {
                int numVale = Convert.ToInt32(valeVenta[i]);
                dw_movin movin = db.Movins.SingleOrDefault(s => s.numeroVale == numVale);
                if (movin != null)
                {
                    int idMovin = movin.dw_movinID;
                    dw_envio dw_envio = db.DatosEnvio.SingleOrDefault(s => s.dw_movinID == idMovin);
                    if (dw_envio != null)
                    {
                        dw_envio.fechaValeVenta = fecha;
                        dw_envio.valeVenta = valeVenta[i];
                        dw_envio.nombreCliente = dw_envioNew.nombreCliente;
                        dw_envio.rutCliente = dw_envioNew.rutCliente;
                        dw_envio.direccion = dw_envioNew.direccion;
                        dw_envio.ciudad = dw_envioNew.ciudad;
                        dw_envio.telefono = dw_envioNew.telefono;
                        db.Entry(dw_envio).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                    else
                    {
                        DateTime fechaD = new DateTime(2000, 1, 1);
                        dw_envioNew.fechaDespacho = fechaD;
                        dw_envioNew.valeVenta = valeVenta[i];
                        db.DatosEnvio.Add(dw_envioNew);
                        db.SaveChanges();
                    }
                }
                else
                {
                    string vale=numVale.ToString();
                    dw_envio dw_envio = db.DatosEnvio.SingleOrDefault(s => s.valeVenta == vale);
                    if (dw_envio != null)
                    {
                        dw_envio.fechaValeVenta = fecha;
                        dw_envio.valeVenta = valeVenta[i];
                        dw_envio.nombreCliente = dw_envioNew.nombreCliente;
                        dw_envio.rutCliente = dw_envioNew.rutCliente;
                        dw_envio.direccion = dw_envioNew.direccion;
                        dw_envio.ciudad = dw_envioNew.ciudad;
                        dw_envio.telefono = dw_envioNew.telefono;
                        db.Entry(dw_envio).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                    else
                    {
                        DateTime fechaD = new DateTime(2000, 1, 1);
                        dw_envioNew.fechaDespacho = fechaD;
                        dw_envioNew.valeVenta = valeVenta[i];
                        db.DatosEnvio.Add(dw_envioNew);
                        db.SaveChanges();
                    }
                   
                }
                //dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Registro nueva venta Vale Venta:" + valeVenta[i]);
            }
            
                return RedirectToAction("Index");
            

            //return View(dw_envio);
        }

        // GET: Envio/Edit/5
        [Permissions(Permission1 = 1, Permission2 = 7)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dw_envio dw_envio = db.DatosEnvio.Find(id);
            if (dw_envio == null)
            {
                return HttpNotFound();
            }
            ViewData["dw_ciudades"] = dw_ciudades_despacho.listaCiudades();
            return View(dw_envio);
        }

        // POST: Envio/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permissions(Permission1 = 1, Permission2 = 7)]
        public ActionResult Edit([Bind(Include = "dw_envioID,valeVenta,nombreCliente,rutCliente,ciudad,telefono,direccion,observacion,fechaValeVenta")] dw_envio dw_envio, FormCollection form)
        {
            dw_envio actual_datosEnvio = db.DatosEnvio.Find(Convert.ToInt32((string)form["dw_envioID"]));
            DateTime fecha = Formateador.fechaStringToDateTime((string)form["fechaValeVenta"]);
            actual_datosEnvio.fechaValeVenta = fecha;
            actual_datosEnvio.valeVenta=(string)form["valeVenta"];
            actual_datosEnvio.nombreCliente=(string)form["nombreCliente"];
            actual_datosEnvio.rutCliente=(string)form["rutCliente"];
            actual_datosEnvio.ciudad=(string)form["ciudad"];
            actual_datosEnvio.telefono=(string)form["telefono"];
            actual_datosEnvio.direccion=(string)form["direccion"];
            actual_datosEnvio.observacion=(string)form["observacion"];
                           
                db.Entry(actual_datosEnvio).State = EntityState.Modified;
                db.SaveChanges();
                dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Edito Registro de Vale Venta:" + dw_envio.valeVenta);
                return RedirectToAction("Index");
            
            //return View(dw_envio);
        }

        // GET: Envio/Delete/5
        [Permissions(Permission1 = 1, Permission2 = 7)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dw_envio dw_envio = db.DatosEnvio.Find(id);
            if (dw_envio == null)
            {
                return HttpNotFound();
            }
            return View(dw_envio);
        }

        // POST: Envio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Permissions(Permission1 = 1)]
        public ActionResult DeleteConfirmed(int id)
        {
            dw_envio dw_envio = db.DatosEnvio.Find(id);
            db.DatosEnvio.Remove(dw_envio);
            db.SaveChanges();
            dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Elimino Registro de Vale Venta:" + dw_envio.valeVenta);
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

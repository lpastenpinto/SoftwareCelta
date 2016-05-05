using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoftwareCelta.DAL;
using SoftwareCelta.Models;
using System.Data.Entity;
using SoftwareCelta.Filters;
namespace SoftwareCelta.Controllers
{
    public class validarProductosDespachoController : Controller
    {
        private ContextBDCelta db = new ContextBDCelta();
        // GET: validarProductosDespacho
        [Permissions(Permission1 = 1, Permission3 = 5)]
        public ActionResult Index(string bodega, string fechaDesde, string fechaHasta)
        {

            int userID = Convert.ToInt32(Session["userID"].ToString());
            List<int> bodegasAutorizadas =Formateador.listToInt(db.permisosUserBodegas.Where(s => s.userID == userID).ToList());

            DateTime fDesde = new DateTime();
            DateTime fHasta = new DateTime();
            if (fechaDesde != null && fechaHasta != null)
            {
                fDesde = Formateador.fechaStringToDateTime(fechaDesde);
                fHasta = Formateador.fechaStringToDateTime(fechaHasta);
            }
            else
            {
                fDesde = DateTime.Today;
                fHasta = fDesde;//DateTime.Today.AddDays(-1);    
            }

            List<dw_movin> dw_movinList = new List<dw_movin>();
            List<int> cantidadProductosPorDespachar = new List<int>();
            List<List<dw_detalle>> listaDeListaDetalle = new List<List<dw_detalle>>();
            List<dw_envio> datosEnvio = new List<dw_envio>();
            datosEnvio = db.DatosEnvio.Where(s => s.fechaDespacho <= fHasta & s.fechaDespacho >= fDesde).ToList();

            foreach (var datoEnvio in datosEnvio)
            {
                dw_movin dw_movin = db.Movins.Find(datoEnvio.dw_movinID);
                List<dw_detalle> dw_detalleList = new List<dw_detalle>();
                if (bodega == null || bodega=="todas")
                {
                    dw_detalleList = db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID & s.estadoDespacho == 1).ToList();    
                }
                else {
                    int idBodega=Convert.ToInt32(bodega);
                    dw_detalleList = db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID & s.estadoDespacho == 1 & s.dw_areaInternaID==idBodega).ToList();
                }
                
                int cantidadProductosMovin = dw_detalleList.Count;
                List<dw_detalle> detalleTemp = new List<dw_detalle>();
                if (cantidadProductosMovin > 0)
                {
                    int count = 0;
                    foreach (var det in dw_detalleList) {
                        if (bodegasAutorizadas.Contains(det.dw_areaInternaID)) {
                            count++;
                            detalleTemp.Add(det);
                        }
                        
                    }
                    if (count > 0) {
                        cantidadProductosPorDespachar.Add(count);
                        dw_movinList.Add(dw_movin);
                        listaDeListaDetalle.Add(detalleTemp);
                    }
                    
                }
            }


            List<dw_areaInterna> dw_areaInternaListTem = db.Bodegas.ToList();
            List<dw_areaInterna> dw_areaInternaList = new List<dw_areaInterna>() ;
            foreach (var bod in dw_areaInternaListTem) {
                if (bodegasAutorizadas.Contains(bod.dw_areaInternaID)) {
                    dw_areaInternaList.Add(bod);
                }
                    
            }

            ViewData["cantidadProductosPorDespachar"] = cantidadProductosPorDespachar;
            ViewData["listaDeListaDetalle"] = listaDeListaDetalle;
            ViewData["bodegas"] = dw_areaInternaList;
                        
            ViewBag.fechaInicial = Formateador.fechaCompletaToString(fDesde);
            ViewBag.fechaFinal = Formateador.fechaCompletaToString(fHasta);
            ViewBag.bodega = bodega;
            return View(dw_movinList);
        }


        [Permissions(Permission1 = 1)]
        public ActionResult listResponsabilidad() {
            ViewData["bodegas"] = db.Bodegas.ToList();
            ViewData["users"] = db.Users.ToList();
            ViewData["permisosBodegas"] = Formateador.listToHash(db.permisosUserBodegas.ToList());
            return View();
        }


        [HttpPost]
        [Permissions(Permission1 = 1)]
        public ActionResult listResponsabilidad(FormCollection form)
        { 
            string[] idBodegas= Request.Form.GetValues("idBodega");
            string[] userID = Request.Form.GetValues("userID");
            
            List<permisosBodegas> PermisosBodega= db.permisosUserBodegas.ToList();
            foreach (var perm in PermisosBodega)
            {
                permisosBodegas permB = db.permisosUserBodegas.Find(perm.permisosBodegasID);
                db.permisosUserBodegas.Remove(permB);
                db.SaveChanges();
                
            }

            for (int i = 0; i < idBodegas.Length; i++) {                
                int idUser = Convert.ToInt32(userID[i]);
                int idBodega = Convert.ToInt32(idBodegas[i]);
                permisosBodegas perm = new permisosBodegas();
                perm.bodegaID = idBodega;
                perm.userID = idUser;
                db.permisosUserBodegas.Add(perm);
                db.SaveChanges();
            }
            dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Edita Lista de Responsabilidad");
            return RedirectToAction("Index");
        }

        [Permissions(Permission1 = 1, Permission3 = 5)]
        public ActionResult documento(int documentoID) {

            int userID = Convert.ToInt32(Session["userID"].ToString());
            List<int> bodegasAutorizadas =Formateador.listToInt(db.permisosUserBodegas.Where(s => s.userID == userID).ToList());

            dw_movin dw_movin = db.Movins.Find(documentoID);            
            dw_envio datosEnvio = db.DatosEnvio.SingleOrDefault(s => s.dw_movinID == documentoID);
            List<dw_detalle> listDetalleDocumentoTemp = db.DetalleMovin.Where(s => s.dw_movinID == documentoID).ToList();
            List<dw_detalle> listDetalleDocumento = new List<dw_detalle>();
            foreach (var detalle in listDetalleDocumentoTemp) {
                if (bodegasAutorizadas.Contains(detalle.dw_areaInternaID)) {
                    listDetalleDocumento.Add(detalle);
                }
            }
            ViewData["detalleDocumento"] = listDetalleDocumento;
            ViewData["datosEnvio"] = datosEnvio;            
            
            return View(dw_movin); 
        }

        [HttpPost]
        [Permissions(Permission1 = 1, Permission3 = 5)]
        public ActionResult documento(FormCollection form) {
            string[] prodValidados = Request.Form.GetValues("validarDespacho");
            string numDoc = (string)form["numDoc"];

            if (prodValidados != null)
            {
                for (int x = 0; x < prodValidados.Length; x++)
                {
                    int idDetalle = Convert.ToInt32(prodValidados[x]);
                    dw_detalle detalle = db.DetalleMovin.Find(idDetalle);
                    detalle.validado = 1;
                    db.Entry(detalle).State = EntityState.Modified;
                    db.SaveChanges();

                }
                TempData["Success"] = "Productos del documento " + numDoc + " validados con Exito";
                dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Valida sus productos del documento:" + numDoc);
            }
            else {
                TempData["Success"] = "No se valido ningun producto del documento " + numDoc + " ya que ninguno fue marcado";
            }
            
            return RedirectToAction("Index");
        }


    }
}
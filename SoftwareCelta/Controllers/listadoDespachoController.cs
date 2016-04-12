using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoftwareCelta.Models;
using System.Text;
using SoftwareCelta.DAL;
using System.Data.Entity;

namespace SoftwareCelta.Controllers
{
    public class listadoDespachoController : Controller
    {
        private ContextBDCelta db = new ContextBDCelta();
        // GET: listadoDespacho         
        public ActionResult Despachados(string fInicial, string fFinal, string idTransport,string idBodega, string numDoc) {

            string estadoFiltroDoc="";
            string estadoFiltroFecha = "";

            DateTime fechaInicial = new DateTime();
            DateTime fechaFinal = new DateTime();            
            if (fInicial == null || fFinal == null)
            {
                fechaInicial = DateTime.Now.AddDays(-7);
                fechaFinal = DateTime.Now;
            }
            else
            {
                fechaInicial = Formateador.formatearFechaCompleta(fInicial);
                fechaFinal = Formateador.formatearFechaCompleta(fFinal);
            }


            List<dw_movin> dw_movinList = new List<dw_movin>();            
            List<List<dw_detalle>> listaDeListaDetalle = new List<List<dw_detalle>>();
            List<dw_envio> datosEnvio = new List<dw_envio>();


            if (numDoc!=null)
            {
                int numDocInt=Convert.ToInt32(numDoc);                
                dw_movin dw_movin = db.Movins.SingleOrDefault(s => s.numeroDocumento == numDocInt);
                if (dw_movin != null) {
                    List<dw_detalle> listDetalle = db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID & s.estadoDespacho!=0).ToList();
                    dw_envio dw_envio = db.DatosEnvio.SingleOrDefault(s => s.dw_movinID == dw_movin.dw_movinID);
                    datosEnvio.Add(dw_envio);
                    dw_movinList.Add(dw_movin);
                    listaDeListaDetalle.Add(listDetalle);    
                }                
                estadoFiltroDoc = "";
                estadoFiltroFecha = "hidden";
            }
            else
            { //string idTransport,string idBodega, 
                
                if (idTransport==null)
                {
                    datosEnvio = db.DatosEnvio.Where(s => s.fechaDespacho >= fechaInicial & s.fechaDespacho <= fechaFinal & s.dw_datosTransportistaID!=0).ToList();                    
                }
                else {
                    int idTransINT = Convert.ToInt32(idTransport);
                    datosEnvio = db.DatosEnvio.Where(s => s.fechaDespacho >= fechaInicial & s.fechaDespacho <= fechaFinal & s.dw_datosTransportistaID == idTransINT).ToList();                    
                }
                foreach (var envio in datosEnvio) {
                    List<dw_detalle> dw_detalle = new List<Models.dw_detalle>();
                    dw_movin dw_movin = db.Movins.Find(envio.dw_movinID);
                    
    
                    if (idBodega==null)
                    {
                        dw_detalle = db.DetalleMovin.Where(s => s.dw_movinID == envio.dw_movinID).ToList();
                        listaDeListaDetalle.Add(dw_detalle);
                    }
                    else {
                        int idBodegaInt = Convert.ToInt32(idBodega);
                        dw_detalle = db.DetalleMovin.Where(s => s.dw_movinID == envio.dw_movinID & s.dw_areaInternaID == idBodegaInt).ToList();
                        listaDeListaDetalle.Add(dw_detalle);
                    }

                    if (dw_detalle.Count > 0) {
                        dw_movinList.Add(dw_movin);
                    }
                }

                estadoFiltroDoc = "hidden";
                estadoFiltroFecha = "";
            }

            List<dw_areaInterna> dw_areaInternaList = db.Bodegas.ToList();
            ViewData["transportistas"] = db.Transportistas.ToList();
            ViewData["bodegas"] = dw_areaInternaList; 
            ViewData["datosEnvio"] = datosEnvio;
            ViewData["listaDeListaDetalle"] = listaDeListaDetalle;
            ViewData["hashBodegas"] = Formateador.listToHash(dw_areaInternaList);
            ViewBag.fechaInicial = Formateador.fechaCompletaToString(fechaInicial);
            ViewBag.fechaFinal = Formateador.fechaCompletaToString(fechaFinal);
            ViewBag.filtroDoc = estadoFiltroDoc;
            ViewBag.filtroFecha = estadoFiltroFecha;
            return View(dw_movinList);
        }

        public ActionResult noDespachados(string numDoc,string idAreaInt) {

            List<dw_movin> dw_movinList = new List<dw_movin>();
            List<List<dw_detalle>> listaDeListaDetalle = new List<List<dw_detalle>>();
            string estadoFiltroDoc="";
            string estadoFiltroAreaInterna="";

            DateTime fechaDespacho = DateTime.Today.AddDays(1);

            if (numDoc == null && idAreaInt == null)
            {
                List<dw_envio> listDwEnvio = db.DatosEnvio.Where(s => s.fechaDespacho == fechaDespacho).ToList();
                foreach (var envio in listDwEnvio) {
                    dw_movin dw_movin = db.Movins.Find(envio.dw_movinID);
                    dw_movinList.Add(dw_movin);
                    List<dw_detalle> listDetalle = db.DetalleMovin.Where(s => s.dw_movinID == envio.dw_movinID & s.estadoDespacho==1).ToList();
                    listaDeListaDetalle.Add(listDetalle);
                }
                estadoFiltroDoc="hidden";
                estadoFiltroAreaInterna="hidden";
            }
            else if(numDoc==null){
                int idBodega = Convert.ToInt32(idAreaInt);
                List<dw_envio> listDwEnvio = db.DatosEnvio.Where(s => s.fechaDespacho == fechaDespacho).ToList();
                foreach (var envio in listDwEnvio)
                {
                    dw_movin dw_movin = db.Movins.Find(envio.dw_movinID);                    
                    dw_movinList.Add(dw_movin);
                    List<dw_detalle> listDetalle = db.DetalleMovin.Where(s => s.dw_movinID == envio.dw_movinID & s.dw_areaInternaID == idBodega & s.estadoDespacho == 1).ToList();
                    listaDeListaDetalle.Add(listDetalle);
                   
                }
                estadoFiltroDoc = "hidden";
                estadoFiltroAreaInterna = "";
                
            }
            else if (idAreaInt == null) {
                int numeroDoc = Convert.ToInt32(numDoc);
                dw_movin dw_movin = db.Movins.SingleOrDefault(s => s.numeroDocumento == numeroDoc);
                dw_movinList.Add(dw_movin);
                List<dw_detalle> detalles = db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID & s.estadoDespacho == 1).ToList();
                listaDeListaDetalle.Add(detalles);

                estadoFiltroDoc = "";
                estadoFiltroAreaInterna = "hidden";
            }
            List<dw_areaInterna> dw_areaInternaList = db.Bodegas.ToList();
            ViewData["listaDeListaDetalle"] = listaDeListaDetalle;
            ViewData["bodegas"] = dw_areaInternaList;


            ViewData["hashBodegas"] = Formateador.listToHash(dw_areaInternaList);
            ViewBag.filtroDoc=estadoFiltroDoc;
            ViewBag.filtroAreaInterna = estadoFiltroAreaInterna;

            return View(dw_movinList);                     
        }

        public ActionResult porDespachar(string arInt) {

            int areaInternaID=Convert.ToInt32(arInt);
            DateTime fechaActual = DateTime.Today;
            DateTime diaAnterior = DateTime.Today.AddDays(-1);
            List<dw_movin> dw_movinList = new List<dw_movin>();
            List<int> cantidadProductosPorDespachar = new List<int>();
            List<List<dw_detalle>> listaDeListaDetalle = new List<List<dw_detalle>>();
            List<dw_envio> datosEnvio = db.DatosEnvio.Where(s => s.fechaDespacho <= fechaActual & s.fechaDespacho >= diaAnterior & s.dw_datosTransportistaID == 0).ToList();

            foreach (var datoEnvio in datosEnvio)
            {
                dw_movin dw_movin = db.Movins.Find(datoEnvio.dw_movinID);
                List<dw_detalle> dw_detalleList = db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID &s.estadoDespacho==1).ToList();
                int cantidadProductosMovin = dw_detalleList.Count;
                cantidadProductosPorDespachar.Add(cantidadProductosMovin);
                                
                dw_movinList.Add(dw_movin);
                listaDeListaDetalle.Add(dw_detalleList);
            }


            List<dw_areaInterna> dw_areaInternaList = db.Bodegas.ToList();
            ViewData["cantidadProductosPorDespachar"] = cantidadProductosPorDespachar;
            ViewData["listaDeListaDetalle"] = listaDeListaDetalle;
            ViewData["bodegas"] = dw_areaInternaList; 
            ViewData["transportistas"]=db.Transportistas.ToList();
            ViewData["hashBodegas"] = Formateador.listToHash(dw_areaInternaList);

            return View(dw_movinList);
        }

        public ActionResult Despachar(int documentoID)
        {
            dw_movin dw_movin = db.Movins.Find(documentoID);
            List<dw_datosTransportista> datosTransportistas = db.Transportistas.ToList();
            dw_envio datosEnvio = db.DatosEnvio.SingleOrDefault(s => s.dw_movinID == documentoID);
            List<dw_detalle> listDetalleDocumento = db.DetalleMovin.Where(s => s.dw_movinID == documentoID).ToList();
            ViewData["detalleDocumento"] = listDetalleDocumento;
            ViewData["datosEnvio"] = datosEnvio;
            ViewData["bodegas"] = db.Bodegas.ToList();
            ViewData["dw_movin"] = dw_movin;
            ViewData["datosTransportistas"] = datosTransportistas;
            return View(dw_movin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Despachar(FormCollection form) {
            
            DateTime fechaDespacho = Formateador.fechaStringToDateTime((string)form["fechaDespacho"]);
            int dw_movinID= Convert.ToInt32((string)form["dw_movinID"]);
            int numeroDocumento = Convert.ToInt32((string)form["numeroDocumento"]);
                        
            dw_envio dw_envio = db.DatosEnvio.SingleOrDefault(s => s.dw_movinID == dw_movinID);
            dw_envio.fechaDespacho = fechaDespacho;
            dw_envio.cantidadVisitasDespacho = dw_envio.cantidadVisitasDespacho + 1;
            dw_envio.dw_datosTransportistaID = Convert.ToInt32((string)form["datosTransportista"]);
            db.Entry(dw_envio).State = EntityState.Modified;
            db.SaveChanges();

            string[] checkeoDespacho = Request.Form.GetValues("estadoDespacho");
            for (int i = 0; i < checkeoDespacho.Length; i++) {
                dw_detalle detalle = db.DetalleMovin.Find(Convert.ToInt32(checkeoDespacho[i]));
                detalle.estadoDespacho = 2;

                db.Entry(detalle).State = EntityState.Modified;                
                db.SaveChanges();
            }
            dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Despacho Documento:" + numeroDocumento);
            return RedirectToAction("porDespachar");
        }

        [HttpPost]
        public string despacharSinEditar(string idDetalleMovin, string idTransportista ) {
            int dw_movinID = Convert.ToInt32(idDetalleMovin);
            int dw_transportistaID=Convert.ToInt32(idTransportista);
            dw_envio dw_envio = db.DatosEnvio.SingleOrDefault(s => s.dw_movinID == dw_movinID);
            List<dw_detalle> listDetalle = db.DetalleMovin.Where(s => s.dw_movinID == dw_movinID & s.estadoDespacho == 1).ToList();
            dw_movin dw_movin = db.Movins.Find(dw_movinID);
            try
            {
                dw_envio.dw_datosTransportistaID = dw_transportistaID;
                dw_envio.cantidadVisitasDespacho = dw_envio.cantidadVisitasDespacho + 1;
                dw_envio.fechaDespacho= Formateador.fechaStringToDateTime(Formateador.fechaCompletaToString(DateTime.Now));           
                db.Entry(dw_envio).State = EntityState.Modified;
                db.SaveChanges();

                foreach (var detalle in listDetalle)
                {
                    detalle.estadoDespacho = 2;
                    db.Entry(detalle).State = EntityState.Modified;
                    db.SaveChanges();
                }
                dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Despachado documento numero: " + dw_movin.numeroDocumento);
                
                return "GUARDADO";
            }
            catch (Exception e) {
                return "ERROR:" + e;
            }       
        }


        public ActionResult despachoDevuelto(int documentoID)
        {
            dw_movin dw_movin = db.Movins.Find(documentoID);
            dw_envio datosEnvio = db.DatosEnvio.SingleOrDefault(s => s.dw_movinID == documentoID);
            List<dw_detalle> listDetalleDocumento = db.DetalleMovin.Where(s => s.dw_movinID == documentoID).ToList();
            ViewData["detalleDocumento"] = listDetalleDocumento;
            ViewData["datosEnvio"] = datosEnvio;            
            ViewData["dw_movin"] = dw_movin;
            return View(dw_movin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult despachoDevuelto(FormCollection form)
        {
            int dw_movinID = Convert.ToInt32((string)form["dw_movinID"]);
            string numeroBoleta= (string)form["numeroBoleta"];
            DateTime fechaDespacho = Formateador.fechaStringToDateTime((string)form["fechaDespacho"]);

            dw_envio datosEnvio = db.DatosEnvio.SingleOrDefault(s => s.dw_movinID == dw_movinID);
            datosEnvio.dw_datosTransportistaID = 0;
            datosEnvio.fechaDespacho=fechaDespacho;
            db.Entry(datosEnvio).State = EntityState.Modified;
            db.SaveChanges();
            List<dw_detalle> listDetalleDocumento = db.DetalleMovin.Where(s => s.dw_movinID == dw_movinID && s.estadoDespacho == 1 || s.estadoDespacho == 2).ToList();
            foreach (var detalle in listDetalleDocumento) {
                detalle.estadoDespacho = 1;
                db.Entry(detalle).State = EntityState.Modified;
                db.SaveChanges();
            }
            TempData["Success"] = "Cambio guardado con Exito. Su boleta con numero " + numeroBoleta + " tiene fecha de despacho " + Formateador.fechaCompletaToString(fechaDespacho);
            dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Se cambia fecha de despacho de documento:" + numeroBoleta+" ya que fue devuelto.");
            
            return RedirectToAction("Despachados");
        }

        [HttpPost]
        public string BuscarDocumentoPorNumero(string numDoc,string tipo)
        {
            //1:despachados
            //2:no despachados
            try
            {
                int numD = Convert.ToInt32(numDoc);
                int tipoQ= Convert.ToInt32(tipo);
                dw_movin dw_movin= db.Movins.SingleOrDefault(s => s.numeroDocumento == numD);                        
                
                if (dw_movin != null)
                {
                    dw_envio dw_envio = new dw_envio();
                    if(tipoQ==1){
                        dw_envio = db.DatosEnvio.SingleOrDefault(s => s.dw_movinID==dw_movin.dw_movinID & s.dw_datosTransportistaID != 0);
                    }else{
                         DateTime fechaDespacho = DateTime.Today.AddDays(1);
                        dw_envio = db.DatosEnvio.SingleOrDefault(s => s.dw_movinID == dw_movin.dw_movinID & s.dw_datosTransportistaID == 0 & s.fechaDespacho == fechaDespacho);
                    }
                    if (dw_envio != null)
                    {
                        return "true";
                    }
                    else {
                        return "false";
                    }
                    
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception e)
            {
                return "false";
            }

        }
    }
}
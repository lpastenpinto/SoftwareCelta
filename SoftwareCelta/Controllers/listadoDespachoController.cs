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
                    List<dw_detalle> listDetalle = db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID).ToList();
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
                    datosEnvio = db.DatosEnvio.Where(s => s.fechaDespacho >= fechaInicial & s.fechaDespacho <= fechaFinal).ToList();                    
                }
                else {
                    int idTransINT = Convert.ToInt32(idTransport);
                    datosEnvio = db.DatosEnvio.Where(s => s.fechaDespacho >= fechaInicial & s.fechaDespacho <= fechaFinal & s.dw_datosTransportistaID == idTransINT).ToList();                    
                }
                foreach (var envio in datosEnvio) {                     
                    dw_movin dw_movin = db.Movins.Find(envio.dw_movinID);
                    dw_movinList.Add(dw_movin);
    
                    if (idBodega==null)
                    {
                        List<dw_detalle> dw_detalle = db.DetalleMovin.Where(s => s.dw_movinID == envio.dw_movinID).ToList();
                        listaDeListaDetalle.Add(dw_detalle);
                    }
                    else {
                        int idBodegaInt = Convert.ToInt32(idBodega);
                        List<dw_detalle> dw_detalle = db.DetalleMovin.Where(s => s.dw_movinID == envio.dw_movinID & s.dw_areaInternaID == idBodegaInt).ToList();
                        listaDeListaDetalle.Add(dw_detalle);
                    }
                }

                estadoFiltroDoc = "hidden";
                estadoFiltroFecha = "";
            }            

            ViewData["transportistas"] = db.Transportistas.ToList();
            ViewData["bodegas"] = db.Bodegas.ToList();
            ViewData["datosEnvio"] = datosEnvio;
            ViewData["listaDeListaDetalle"] = listaDeListaDetalle;
            ViewBag.fechaInicial = Formateador.fechaCompletaToString(fechaInicial);
            ViewBag.fechaFinal = Formateador.fechaCompletaToString(fechaFinal);
            ViewBag.filtroDoc = estadoFiltroDoc;
            ViewBag.filtroFecha = estadoFiltroFecha;
            return View(dw_movinList);
        }

        public ActionResult noDespachados() {
            List<StringBuilder> listText = new List<StringBuilder>();
            List<dw_movin> documentoList = new List<dw_movin>();
            dw_movin doc1 = new dw_movin();
            doc1.dw_movinID = 1;
            doc1.numeroDocumento = 100;
            doc1.numeroVale = 123;
            doc1.fechaEmision= DateTime.Now;
            documentoList.Add(doc1);
            StringBuilder textoProductos1 = new StringBuilder();
            textoProductos1.Append("Cama.");
            listText.Add(textoProductos1);

            dw_movin doc2 = new dw_movin();
            doc2.dw_movinID = 2;
            doc2.numeroDocumento = 1002;
            doc2.numeroVale = 12234233;
            doc2.fechaEmision= DateTime.Now;
            documentoList.Add(doc2);

            StringBuilder textoProductos2 = new StringBuilder();
            textoProductos2.Append("Muchas camas1.|");
            textoProductos2.Append("Muchas camas2.|");
            textoProductos2.Append("Muchas camas3.|");
            textoProductos2.Append("Muchas camas4.|");
            textoProductos2.Append("Muchas camas5.|");
            listText.Add(textoProductos2);

            List<int> cantidadProductosPorDespachar = new List<int>();
            cantidadProductosPorDespachar.Add(1);
            cantidadProductosPorDespachar.Add(4);
            ViewData["cantidadProductosPorDespachar"] = cantidadProductosPorDespachar;
            ViewData["textoProductosDespachar"] = listText;
            ViewData["bodegas"] = db.Bodegas.ToList();
            return View(documentoList);                      
        }

        public ActionResult porDespachar(string arInt) {

            int areaInternaID=Convert.ToInt32(arInt);
            DateTime fechaActual = DateTime.Today;

            List<dw_movin> dw_movinList = new List<dw_movin>();
            List<int> cantidadProductosPorDespachar = new List<int>();
            List<List<dw_detalle>> listaDeListaDetalle = new List<List<dw_detalle>>();
            List<dw_envio> datosEnvio = db.DatosEnvio.Where(s=>s.fechaDespacho==fechaActual & s.dw_datosTransportistaID==0).ToList();

            foreach (var datoEnvio in datosEnvio)
            {
                dw_movin dw_movin = db.Movins.Find(datoEnvio.dw_movinID);
                List<dw_detalle> dw_detalleList = db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID &s.estadoDespacho==1).ToList();
                int cantidadProductosMovin = dw_detalleList.Count;
                cantidadProductosPorDespachar.Add(cantidadProductosMovin);
                                
                dw_movinList.Add(dw_movin);
                listaDeListaDetalle.Add(dw_detalleList);
            }  

               
            
            ViewData["cantidadProductosPorDespachar"] = cantidadProductosPorDespachar;
            ViewData["listaDeListaDetalle"] = listaDeListaDetalle;
            ViewData["bodegas"] = db.Bodegas.ToList();
            ViewData["transportistas"]=db.Transportistas.ToList();

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
            return RedirectToAction("porDespachar");
        }

        [HttpPost]
        public string despacharSinEditar(string idDetalleMovin, string idTransportista ) {
            int dw_movinID = Convert.ToInt32(idDetalleMovin);
            int dw_transportistaID=Convert.ToInt32(idTransportista);
            dw_envio dw_envio = db.DatosEnvio.SingleOrDefault(s => s.dw_movinID == dw_movinID);
            List<dw_detalle> listDetalle = db.DetalleMovin.Where(s => s.dw_movinID == dw_movinID & s.estadoDespacho == 1).ToList();

            try
            {
                dw_envio.dw_datosTransportistaID = dw_transportistaID;
                dw_envio.cantidadVisitasDespacho = dw_envio.cantidadVisitasDespacho + 1;
                db.Entry(dw_envio).State = EntityState.Modified;
                db.SaveChanges();

                foreach (var detalle in listDetalle)
                {
                    detalle.estadoDespacho = 2;
                    db.Entry(detalle).State = EntityState.Modified;
                    db.SaveChanges();
                }
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

            List<dw_detalle> listDetalleDocumento = db.DetalleMovin.Where(s => s.dw_movinID == dw_movinID && s.estadoDespacho==1).ToList();
            foreach (var detalle in listDetalleDocumento) {
                detalle.estadoDespacho = 1;
                db.Entry(detalle).State = EntityState.Modified;
                db.SaveChanges();
            }
            TempData["Success"] = "Cambio guardado con Exito. Su boleta con numero " + numeroBoleta + " tiene fecha de despacho " + Formateador.fechaCompletaToString(fechaDespacho);                         
            return RedirectToAction("Despachados");
        }
    }
}
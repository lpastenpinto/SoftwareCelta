using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoftwareCelta.Models;
using System.Text;
using SoftwareCelta.DAL;
using System.Data.Entity;
using SoftwareCelta.Filters;

namespace SoftwareCelta.Controllers
{
    public class listadoDespachoController : Controller
    {
        private ContextBDCelta db = new ContextBDCelta();
        // GET: listadoDespacho                 
        [Permissions(Permission1 = 1, Permission3 = 3)]
        public ActionResult Despachados(string fInicial, string fFinal, string idTransport,string idBodega, string numDoc) {

            string estadoFiltroDoc="";
            string estadoFiltroFecha = "";

            DateTime fechaInicial = new DateTime();
            DateTime fechaFinal = new DateTime();            
            if (fInicial == null || fFinal == null)
            {
                fechaInicial = DateTime.Now.AddDays(-1);
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
                        dw_detalle = db.DetalleMovin.Where(s => s.dw_movinID == envio.dw_movinID & s.estadoDespacho != 0).ToList();
                        listaDeListaDetalle.Add(dw_detalle);
                    }
                    else {
                        int idBodegaInt = Convert.ToInt32(idBodega);
                        dw_detalle = db.DetalleMovin.Where(s => s.dw_movinID == envio.dw_movinID & s.dw_areaInternaID == idBodegaInt & s.estadoDespacho != 0).ToList();
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
            ViewBag.idTransportista=idTransport;
            ViewBag.idBodega=idBodega;            
            return View(dw_movinList);
        }

        [Permissions(Permission1 = 1, Permission3 = 3)]
        public ActionResult noDespachados(string numDoc,string idAreaInt,string ciudad) {

            List<dw_movin> dw_movinList = new List<dw_movin>();
            List<List<dw_detalle>> listaDeListaDetalle = new List<List<dw_detalle>>();
            string estadoFiltroDoc="";
            string estadoFiltroAreaInterna="";
            string estadoFiltroCiudad = "";
            DateTime fechaDespacho = DateTime.Today.AddDays(1);

            if (numDoc == null && (idAreaInt == null||idAreaInt.Equals("0"))&& (ciudad==null))
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
                estadoFiltroCiudad="hidden";
            }
            else if (idAreaInt != null && !idAreaInt.Equals("0"))
            {
                int idBodega = Convert.ToInt32(idAreaInt);
                List<dw_envio> listDwEnvio = db.DatosEnvio.Where(s => s.fechaDespacho == fechaDespacho).ToList();
                foreach (var envio in listDwEnvio)
                {
                    dw_movin dw_movin = db.Movins.Find(envio.dw_movinID);                    
                    
                    List<dw_detalle> listDetalle = db.DetalleMovin.Where(s => s.dw_movinID == envio.dw_movinID & s.dw_areaInternaID == idBodega & s.estadoDespacho == 1).ToList();
                    if (listDetalle.Count >= 1) {
                        dw_movinList.Add(dw_movin);
                        listaDeListaDetalle.Add(listDetalle);
                    }
                                       
                }
                estadoFiltroDoc = "hidden";
                estadoFiltroCiudad="hidden";
                estadoFiltroAreaInterna = "";
                
            }
            else if (numDoc!=null) {
                int numeroDoc = Convert.ToInt32(numDoc);
                dw_movin dw_movin = db.Movins.SingleOrDefault(s => s.numeroDocumento == numeroDoc);
                dw_movinList.Add(dw_movin);
                List<dw_detalle> detalles = db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID & s.estadoDespacho == 1).ToList();
                listaDeListaDetalle.Add(detalles);

                estadoFiltroDoc = "";
                estadoFiltroAreaInterna = "hidden";
                estadoFiltroCiudad="hidden";
            }
            else if (ciudad != null) {                
                List<dw_envio> listDwEnvio = db.DatosEnvio.Where(s => s.fechaDespacho == fechaDespacho && s.ciudad==ciudad).ToList();
                foreach (var envio in listDwEnvio)
                {
                    dw_movin dw_movin = db.Movins.Find(envio.dw_movinID);

                    List<dw_detalle> listDetalle = db.DetalleMovin.Where(s => s.dw_movinID == envio.dw_movinID & s.estadoDespacho == 1).ToList();
                    if (listDetalle.Count >= 1)
                    {
                        dw_movinList.Add(dw_movin);
                        listaDeListaDetalle.Add(listDetalle);
                    }

                }
                estadoFiltroDoc = "hidden";
                estadoFiltroCiudad="";
                estadoFiltroAreaInterna = "hidden";
                
            }
            List<dw_areaInterna> dw_areaInternaList = db.Bodegas.ToList();
            ViewData["listaDeListaDetalle"] = listaDeListaDetalle;
            ViewData["bodegas"] = dw_areaInternaList;


            ViewData["hashBodegas"] = Formateador.listToHash(dw_areaInternaList);
            ViewBag.filtroDoc=estadoFiltroDoc;
            ViewBag.filtroAreaInterna = estadoFiltroAreaInterna;
            ViewBag.filtroCiudad = estadoFiltroCiudad;
            ViewBag.ciudad=ciudad;
            ViewBag.areaInterna = idAreaInt;
            return View(dw_movinList);                     
        }

        [Permissions(Permission1 = 1, Permission3 = 3)]
        public ActionResult porDespachar(string ciudad,string fechaDesde, string fechaHasta,string estado) {

            //int areaInternaID=Convert.ToInt32(arInt);
            DateTime fDesde = new DateTime();
            DateTime fHasta= new DateTime();
            if (fechaDesde != null && fechaHasta != null)
            {
                fDesde = Formateador.fechaStringToDateTime(fechaDesde);
                fHasta = Formateador.fechaStringToDateTime(fechaHasta);
            }
            else {
                fDesde = DateTime.Today;
                fHasta = fDesde;//DateTime.Today.AddDays(-1);    
            }
            
            List<dw_movin> dw_movinList = new List<dw_movin>();
            List<int> cantidadProductosPorDespachar = new List<int>();
            List<List<dw_detalle>> listaDeListaDetalle = new List<List<dw_detalle>>();
            List<dw_envio> datosEnvio = new List<dw_envio>();
            
            if (ciudad == null || ciudad.Equals("todas")) {
                ViewBag.ciudad = "";
                datosEnvio = db.DatosEnvio.Where(s => s.fechaDespacho <= fHasta & s.fechaDespacho >= fDesde).ToList();
            }else {
                ViewBag.ciudad = ciudad;
                datosEnvio = db.DatosEnvio.Where(s => s.fechaDespacho <= fHasta  & s.fechaDespacho >= fDesde & s.ciudad == ciudad).ToList();
            }

            List<dw_envio> listaFinalEnvios = new List<dw_envio>();
            foreach (var datoEnvio in datosEnvio)
            {
                dw_movin dw_movin = db.Movins.Find(datoEnvio.dw_movinID);
                List<dw_detalle> dw_detalleListTotal = new List<dw_detalle>();// db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID & s.estadoDespacho == 1 ).ToList();
                List<dw_detalle> dw_detalleList = new List<dw_detalle>();//db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID &s.estadoDespacho==1 & s.validado!=0).ToList();

                if (estado == null) {
                    estado = "1";
                }
                if (estado == "todas") {

                    dw_detalleList = db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID & s.estadoDespacho == 1).ToList();                    
                    int cantidadProductosMovin = dw_detalleList.Count;
                    if (cantidadProductosMovin > 0)
                    {
                        cantidadProductosPorDespachar.Add(cantidadProductosMovin);
                        dw_movinList.Add(dw_movin);
                        listaDeListaDetalle.Add(dw_detalleList);
                        listaFinalEnvios.Add(datoEnvio);
                    }
                }
                else  if(estado=="0"){
                    dw_detalleListTotal = db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID & s.estadoDespacho == 1).ToList();
                    dw_detalleList = db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID & s.estadoDespacho == 1 & s.validado == 0).ToList();
                    
                    int cantidadTotalProdDoc = dw_detalleListTotal.Count;
                    int cantidadProductosMovin = dw_detalleList.Count;
                    if (cantidadProductosMovin > 0)
                    {
                        cantidadProductosPorDespachar.Add(cantidadProductosMovin);
                        dw_movinList.Add(dw_movin);
                        listaDeListaDetalle.Add(dw_detalleListTotal);
                        listaFinalEnvios.Add(datoEnvio);
                    }

                }
                else if (estado =="1") {
                    dw_detalleListTotal = db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID & s.estadoDespacho == 1).ToList();
                    dw_detalleList = db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID & s.estadoDespacho == 1 & s.validado == 1).ToList();
                    int cantidadTotalProdDoc = dw_detalleListTotal.Count;
                    int cantidadProductosMovin = dw_detalleList.Count;
                    if (cantidadProductosMovin > 0 && cantidadTotalProdDoc == cantidadProductosMovin)
                    {
                        cantidadProductosPorDespachar.Add(cantidadProductosMovin);
                        dw_movinList.Add(dw_movin);
                        listaDeListaDetalle.Add(dw_detalleList);
                        listaFinalEnvios.Add(datoEnvio);
                    }
                }
                
                /*int estadoInt = Convert.ToInt32(estado);
                    dw_detalleListTotal = db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID & s.estadoDespacho == 1).ToList();
                    dw_detalleList = db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID & s.estadoDespacho == 1 & s.validado != 0).ToList();

                    int cantidadTotalProdDoc = dw_detalleListTotal.Count;
                    int cantidadProductosMovin = dw_detalleList.Count;
                    if (cantidadProductosMovin > 0 && cantidadTotalProdDoc == cantidadProductosMovin)
                    {
                        cantidadProductosPorDespachar.Add(cantidadProductosMovin);

                        dw_movinList.Add(dw_movin);
                        listaDeListaDetalle.Add(dw_detalleList);
                    }*/
               
            }


            List<dw_areaInterna> dw_areaInternaList = db.Bodegas.ToList();
            ViewData["cantidadProductosPorDespachar"] = cantidadProductosPorDespachar;
            ViewData["listaDeListaDetalle"] = listaDeListaDetalle;
            ViewData["bodegas"] = dw_areaInternaList; 
            ViewData["transportistas"]=db.Transportistas.ToList();
            ViewData["hashBodegas"] = Formateador.listToHash(dw_areaInternaList);
            ViewData["ciudades"] = dw_ciudades_despacho.listaCiudades();
            ViewData["envios"] = listaFinalEnvios;
            ViewBag.fechaInicial = Formateador.fechaCompletaToString(fDesde);
            ViewBag.fechaFinal = Formateador.fechaCompletaToString(fHasta);
            ViewBag.ciudad = ciudad;
            ViewBag.estado = estado;
            return View(dw_movinList);
        }

        [Permissions(Permission1 = 1, Permission3 = 3)]
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
        [Permissions(Permission1 = 1, Permission3 = 3)]
        public ActionResult Despachar(FormCollection form) {
            
            DateTime fechaDespacho = Formateador.fechaStringToDateTime((string)form["fechaDespacho"]);
            int dw_movinID= Convert.ToInt32((string)form["dw_movinID"]);
            int numeroDocumento = Convert.ToInt32((string)form["numeroDocumento"]);
                        
            dw_envio dw_envio = db.DatosEnvio.SingleOrDefault(s => s.dw_movinID == dw_movinID);
            dw_envio.fechaDespacho = fechaDespacho;
            dw_envio.cantidadVisitasDespacho = dw_envio.cantidadVisitasDespacho + 1;
            dw_envio.dw_datosTransportistaID = Convert.ToInt32((string)form["datosTransportista"]);
            db.Entry(dw_envio).State = EntityState.Modified;

            historicoTransportista histTrans = new historicoTransportista();
            histTrans.fechaAnteriorDespacho = fechaDespacho;
            histTrans.dw_movinID = dw_movinID;
            histTrans.idTransportistaAnterior = dw_envio.dw_datosTransportistaID;
            histTrans.ciudad = dw_envio.ciudad;
            histTrans.tipo = "despacho";
            db.historicosTransportistas.Add(histTrans);
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
        
        [Permissions(Permission1 = 1, Permission3 = 3)]
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


                historicoTransportista histTrans = new historicoTransportista();
                histTrans.fechaAnteriorDespacho =dw_envio.fechaDespacho;
                histTrans.dw_movinID = dw_movinID;
                histTrans.idTransportistaAnterior = dw_envio.dw_datosTransportistaID;
                histTrans.ciudad = dw_envio.ciudad;
                histTrans.tipo = "despacho";
                db.historicosTransportistas.Add(histTrans);

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

        [Permissions(Permission1 = 1, Permission3 = 3)]
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
        [Permissions(Permission1 = 1, Permission3 = 3)]
        public ActionResult despachoDevuelto(FormCollection form)
        {

            int dw_movinID = Convert.ToInt32((string)form["dw_movinID"]);
            string numeroBoleta= (string)form["numeroBoleta"];
            DateTime fechaDespacho = Formateador.fechaStringToDateTime((string)form["fechaDespacho"]);
            string[] despachoDevuelto = Request.Form.GetValues("despachoDevuelto");
            dw_envio datosEnvio = db.DatosEnvio.SingleOrDefault(s => s.dw_movinID == dw_movinID);
            datosEnvio.dw_datosTransportistaID = 0;
            datosEnvio.fechaDespacho=fechaDespacho;
            db.Entry(datosEnvio).State = EntityState.Modified;
            
            //datos anterior despacho
            historicoTransportista histTrans = new historicoTransportista();
            histTrans.fechaAnteriorDespacho=Formateador.fechaStringToDateTime((string)form["fechaAnteriorDespacho"]);
            histTrans.dw_movinID = dw_movinID;
            histTrans.idTransportistaAnterior=Convert.ToInt32((string)form["idTransportistaAnterior"]);
            histTrans.ciudad = datosEnvio.ciudad;
            histTrans.tipo = "devolucion";
            db.historicosTransportistas.Add(histTrans);
            db.SaveChanges();
            
            //List<dw_detalle> listDetalleDocumento = db.DetalleMovin.Where(s => s.dw_movinID == dw_movinID && s.estadoDespacho == 1 || s.estadoDespacho == 2).ToList();
            for (int x = 0; x < despachoDevuelto.Length; x++) {
                int detalleID = Convert.ToInt32(despachoDevuelto[x]);
                dw_detalle detalle = db.DetalleMovin.Find(detalleID);
                detalle.validado = 0;
                detalle.estadoDespacho = 1;
                db.Entry(detalle).State = EntityState.Modified;
                db.SaveChanges();
            }

            TempData["Success"] = "Cambio guardado con Exito. Su boleta con numero " + numeroBoleta + " tiene fecha de despacho " + Formateador.fechaCompletaToString(fechaDespacho);
            dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Se cambia fecha de despacho de documento:" + numeroBoleta+" ya que fue devuelto.");
            
            return RedirectToAction("Despachados");
        }

        [HttpPost]
        [Permissions(Permission1 = 1, Permission3 = 3)]
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
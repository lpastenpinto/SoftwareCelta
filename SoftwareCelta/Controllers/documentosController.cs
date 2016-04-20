using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SoftwareCelta.Models;
using SoftwareCelta.DAL;
namespace SoftwareCelta.Controllers
{
    public class documentosController : Controller
    {
        private ContextBDCelta db = new ContextBDCelta();
        // GET: documentos
        public ActionResult Index()
        {
            DateTime fechaActual = DateTime.Today;

            List<dw_movin> documentosRegistrados = new List<dw_movin>();
            List<List<dw_detalle>> listaDeListaDetalle = new List<List<dw_detalle>>();
            dw_movin documento = new dw_movin();
            //documento.dw_movinID = 1;
            documento.numeroDocumento = 102;
            documento.numeroVale = 12223;
            documento.fechaEmision = DateTime.Now;
            documento.nombreVendedor = "Vendedor Nuevo";

            

            List<dw_detalle> listDetalleDocumento = new List<dw_detalle>();

            dw_detalle detalleDocumento_1 = new dw_detalle();
            detalleDocumento_1.codigoProducto = "AAA2";
            detalleDocumento_1.descripcionProducto = "CAMA AMERICANA";
            detalleDocumento_1.valorProducto = 10000;
            detalleDocumento_1.cantidadProducto = 1;

            listDetalleDocumento.Add(detalleDocumento_1);

            dw_detalle detalleDocumento_2 = new dw_detalle();
            detalleDocumento_2.codigoProducto = "MR1200";
            detalleDocumento_2.descripcionProducto = "MARQUESA";
            detalleDocumento_2.cantidadProducto = 1;
            detalleDocumento_2.valorProducto = 11111;
            listDetalleDocumento.Add(detalleDocumento_2);
            
            
            listaDeListaDetalle.Add(listDetalleDocumento);

            documento.detalleMovin = listDetalleDocumento;
            documentosRegistrados.Add(documento);


            ViewData["listaDeListaDetalle"] = listaDeListaDetalle;            
            return View(documentosRegistrados);
        }


        public ActionResult despachoDomicilio() {
            DateTime fechaActual = DateTime.Today;
            List<dw_movin> documetosRegistrados = db.Movins.Where(s => s.fechaEmision == fechaActual).ToList();
            List<List<dw_detalle>> listaDeListaDetalle = new List<List<dw_detalle>>();
            foreach (var dw_movin in documetosRegistrados)
            {
                List<dw_detalle> listDetalle = db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID & s.estadoDespacho == 1).ToList();
                listaDeListaDetalle.Add(listDetalle);
            }
            ViewData["listaDeListaDetalle"] = listaDeListaDetalle;
            ViewData["bodegas"] = db.Bodegas.ToList();
            return View(documetosRegistrados);
        }


        public ActionResult datosEnvio(int documentoID) {
            dw_envio dw_envio = db.DatosEnvio.SingleOrDefault(s=>s.dw_movinID==documentoID);
            if (dw_envio == null) {
                dw_envio = new dw_envio();
            }
            ViewData["dw_movin"] = db.Movins.Find(documentoID);
            return View(dw_envio);
        
        }

        [HttpPost]
        public ActionResult datosEnvio(FormCollection form)
        {
            int idDocumento =Convert.ToInt32((string)form["idDocumento"]);
            dw_envio dw_envioForm = db.DatosEnvio.SingleOrDefault(s => s.dw_movinID == idDocumento);
            if (dw_envioForm == null)
            {
                dw_envio new_dw_envio = new dw_envio();
                new_dw_envio.dw_movinID=idDocumento;
                new_dw_envio.ciudad = (string)form["ciudad"];
                new_dw_envio.direccion = (string)form["direccion"];
                new_dw_envio.fechaDespacho = Formateador.fechaStringToDateTime((string)form["fechaDespacho"]);
                new_dw_envio.nombreCliente = (string)form["nombreCliente"];
                new_dw_envio.observacion = (string)form["observacion"];
                new_dw_envio.rutCliente = (string)form["rutCliente"];
                new_dw_envio.telefono = (string)form["telefono"];
                db.DatosEnvio.Add(new_dw_envio);
                db.SaveChanges();
            }
            else {

                dw_envioForm.ciudad = (string)form["ciudad"];
                dw_envioForm.direccion = (string)form["direccion"];
                dw_envioForm.fechaDespacho = Formateador.fechaStringToDateTime((string)form["fechaDespacho"]);
                dw_envioForm.nombreCliente = (string)form["nombreCliente"];
                dw_envioForm.observacion = (string)form["observacion"];
                dw_envioForm.rutCliente = (string)form["rutCliente"];
                dw_envioForm.telefono = (string)form["telefono"];
                db.Entry(dw_envioForm).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("despachoDomicilio");

        }


        public ActionResult Buscador(string strFechaInicial, string strFechaFinal, string numDoc) {
            DateTime fechaInicial= new DateTime();
            DateTime fechaFinal = new DateTime();
            if(strFechaFinal==null || strFechaInicial==null){
                fechaInicial=DateTime.Now.AddDays(-1);
                fechaFinal=DateTime.Now;
            }else{
                fechaInicial=Formateador.formatearFechaCompleta(strFechaInicial);
                fechaFinal=Formateador.formatearFechaCompleta(strFechaFinal);
            }

            List<dw_movin> listDocumentos = new List<dw_movin>();
            if (numDoc != null)
            {
                int nDoc= Convert.ToInt32(numDoc);
                dw_movin dw_movin = db.Movins.SingleOrDefault(s => s.numeroDocumento == nDoc);
                listDocumentos.Add(dw_movin);
            }
            else
            {
                listDocumentos = db.Movins.Where(s => s.fechaEmision >= fechaInicial & s.fechaEmision <= fechaFinal).ToList();
            }

            ViewBag.fechaInicial = Formateador.fechaCompletaToString(fechaInicial);
            ViewBag.fechaFinal = Formateador.fechaCompletaToString(fechaFinal);
            return View(listDocumentos);
        }


        public ActionResult Documento(int documentoID)
        {
            //VER SI YA SE INGRESO BOLETA
            //int numeroDocumento = Convert.ToInt32((string)form["numeroDocumento"]);
          

            dw_movin documento = new dw_movin();
            //documento.dw_movinID = 1;
            documento.numeroDocumento = 102;
            documento.numeroVale = 12223;
            documento.fechaEmision =  DateTime.Now;
            documento.nombreVendedor = "Vendedor Nuevo";

            List<dw_detalle> listDetalleDocumento = new List<dw_detalle>();
            
            dw_detalle detalleDocumento_1 = new dw_detalle();
            detalleDocumento_1.codigoProducto = "AAA2";
            detalleDocumento_1.descripcionProducto = "CAMA AMERICANA";
            detalleDocumento_1.valorProducto = 10000;
            detalleDocumento_1.cantidadProducto = 1;            

            listDetalleDocumento.Add(detalleDocumento_1);

            dw_detalle detalleDocumento_2 = new dw_detalle();
            detalleDocumento_2.codigoProducto = "MR1200";
            detalleDocumento_2.descripcionProducto = "MARQUESA";
            detalleDocumento_2.cantidadProducto = 1;
            detalleDocumento_2.valorProducto = 11111;
            listDetalleDocumento.Add(detalleDocumento_2);
         

            ViewData["detalleDocumento"] = listDetalleDocumento;            
            return View(documento);
        }

        public ActionResult paraDespacharADomicilio(int documentoID)
        {
            dw_movin dw_movin = db.Movins.Find(documentoID);
            ViewData["detalleDocumento"] = db.DetalleMovin.Where(s => s.dw_movinID == documentoID).ToList();
            ViewData["bodegas"] = db.Bodegas.ToList();
            return View(dw_movin);
        }

        [HttpPost]
        public ActionResult paraDespacharADomicilio(FormCollection form) {
            string[] detalleID = Request.Form.GetValues("detalleID");
            string[] estadoDespacho = Request.Form.GetValues("estadoDespacho");
            string[] areaInterna =Request.Form.GetValues("areaInterna");

            for (int i = 0; i < estadoDespacho.Length; i++)
            {
                if (estadoDespacho[i].Equals("1")) {
                    int detalle_id = Convert.ToInt32(detalleID[i]);
                    dw_detalle detalle = db.DetalleMovin.Find(detalle_id);
                    detalle.dw_areaInternaID = Convert.ToInt32(areaInterna[i]);
                    db.Entry(detalle).State = EntityState.Modified;
                    db.SaveChanges();
                }                
            }
            return RedirectToAction("despachoDomicilio");
        }

        public ActionResult DocumentoRegistrado(int documentoID) {

            dw_movin dw_movin = db.Movins.Find(documentoID);
            dw_envio datosEnvio = db.DatosEnvio.SingleOrDefault(s => s.dw_movinID == documentoID);
            List<dw_detalle> listDetalleDocumento = db.DetalleMovin.Where(s => s.dw_movinID == documentoID).ToList();
            ViewData["detalleDocumento"] = listDetalleDocumento;
            ViewData["datosEnvio"] = datosEnvio;
            ViewData["bodegas"] = db.Bodegas.ToList();
            ViewData["dw_movin"] = dw_movin;
            return View(dw_movin);
        }

        
        public ActionResult Editar(int documentoID)
        {

            dw_movin dw_movin = db.Movins.Find(documentoID);
            dw_envio datosEnvio = db.DatosEnvio.SingleOrDefault(s => s.dw_movinID == documentoID);
            List<dw_detalle> listDetalleDocumento = db.DetalleMovin.Where(s => s.dw_movinID == documentoID).ToList();
            ViewData["detalleDocumento"] = listDetalleDocumento;
            ViewData["datosEnvio"] = datosEnvio;
            ViewData["bodegas"] = db.Bodegas.ToList();
            ViewData["dw_movin"] = dw_movin;
            return View(dw_movin);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(FormCollection form)
        {
            int dw_movinID = Convert.ToInt32((string)form["dw_movinID"]);
            dw_movin dw_movin = db.Movins.Find(dw_movinID);
            string nombreCliente = (string)form["nombreCliente"];
            string rutCliente = (string)form["rutCliente"];
            string ciudad = (string)form["ciudad"];
            string telefono = (string)form["telefono"];
            string direccion = (string)form["direccion"];
            string observacion = (string)form["observacion"];
            DateTime fechaDespacho = Formateador.fechaStringToDateTime((string)form["fechaDespacho"]);

            dw_envio datosEnvio = db.DatosEnvio.SingleOrDefault(s => s.dw_movinID == dw_movinID);
            datosEnvio.ciudad = ciudad;
            datosEnvio.direccion = direccion;
            datosEnvio.dw_movinID = dw_movinID;
            datosEnvio.fechaDespacho = fechaDespacho;
            datosEnvio.nombreCliente = nombreCliente;
            datosEnvio.observacion = observacion;
            datosEnvio.rutCliente = rutCliente;
            datosEnvio.telefono = telefono;
            db.Entry(datosEnvio).State = EntityState.Modified;
            db.SaveChanges();
            string[] areaInterna = Request.Form.GetValues("areaInterna");
            string[] dw_detalleID = Request.Form.GetValues("dw_detalleID");

            for (int i = 0; i < areaInterna.Length; i++)
            {
                dw_detalle detalle = db.DetalleMovin.Find(Convert.ToInt32(dw_detalleID[i]));                

                string estadoDespacho = (string)form["estadoDespacho" + (i + 1)];

                detalle.estadoDespacho = Convert.ToInt32(estadoDespacho);
                if (estadoDespacho.Equals("0"))
                {
                    detalle.dw_areaInternaID = 0;
                }
                else
                {
                    detalle.dw_areaInternaID = Convert.ToInt32(areaInterna[i]);
                }

                db.Entry(detalle).State = EntityState.Modified;
                try {
                    db.SaveChanges();
                    dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Edicion de documento numero:" + dw_movin.numeroDocumento);
                }
                catch (Exception e) {
                    bool error = true; 
                }                
            }           
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult registrarNuevoDocumento(FormCollection form) {


            int numeroDocumento = Convert.ToInt32((string)form["numeroDocumento"]);
            int numeroVale = Convert.ToInt32((string)form["numeroVale"]);
            string sdfsd = (string)form["fechaEmision"];
            DateTime fechaEmision = Formateador.fechaFormatoGuardar((string)form["fechaEmision"]);
            
            dw_movin documento = new dw_movin();
            documento.fechaEmision = fechaEmision;
            documento.numeroDocumento = numeroDocumento;
            documento.numeroVale = numeroVale;
            documento.nombreVendedor = "VENDEDOR";
            db.Movins.Add(documento);
            db.SaveChanges();
            //GUARDAR DOC

            //GUARDAR DATOS ENVIO
            //GUARDAR DETALLE
                                           
            string[] codigoProducto  = Request.Form.GetValues("codigoProducto");
            string[] descripcionProducto= Request.Form.GetValues("descripcionProducto");
            string[] cantidadProducto = Request.Form.GetValues("cantidadProducto");
            string[] valorProducto = Request.Form.GetValues("valorProducto");

            bool verifDesp = false;
            for (int i = 0; i < codigoProducto.Length; i++)
            {
                dw_detalle detalle = new dw_detalle();
                detalle.cantidadProducto = Convert.ToInt32(cantidadProducto[i]);
                detalle.codigoProducto = codigoProducto[i];
                detalle.descripcionProducto = descripcionProducto[i];                
                detalle.dw_movinID = documento.dw_movinID;
                detalle.valorProducto = Convert.ToInt32(valorProducto[i]);
     
                string estadoDespacho = (string)form["estadoDespacho" + (i + 1)];

                detalle.estadoDespacho = Convert.ToInt32(estadoDespacho);
                detalle.dw_areaInternaID = 0;                    
                if (estadoDespacho.Equals("1"))
                {
                    verifDesp = true;
                }                
                db.DetalleMovin.Add(detalle);                                
            }

            db.SaveChanges();
            
            
            dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Registro nuevo documento numero:" + numeroDocumento);
            return RedirectToAction("Index");
        }
         
        [HttpPost]
        public string guardarAreaInteraMovin(List<string> idsDetalles, List<string> areasInternas,string idMovin)
        {
            try
            {                
                int i = 0;
                int idMov = Convert.ToInt32(idMovin);
                dw_movin dw_movin = db.Movins.Find(idMov);
                foreach (var idDetalle in idsDetalles)
                {
                        int idDet = Convert.ToInt32(idDetalle);
                        dw_detalle detalle = db.DetalleMovin.Find(idDet);
                        detalle.dw_areaInternaID = Convert.ToInt32(areasInternas[i]);
                        db.Entry(detalle).State = EntityState.Modified;
                        db.SaveChanges();
                        i++;
                }
                dw_log.registrarLog(Convert.ToInt32(Session["userID"]), Session["userName"].ToString(), "Se guardan areas internas de documento :" + dw_movin.numeroDocumento);
                return "GUARDADO CORRECTAMENTE";
            }
            catch (Exception e) {
                return "ERROR";
            }
        }

        [HttpPost]
        public string BuscarDocumentoPorNumero(string numDoc) {
            try
            {
                int numD = Convert.ToInt32(numDoc);
                dw_movin dw_movin = db.Movins.SingleOrDefault(s => s.numeroDocumento == numD);
                if (dw_movin != null)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception e) {
                return "false";
            }
            
        }

        [HttpPost]
        public string BuscarDocumentoPorNumeroFechaActual(string numDoc)
        {
            try
            {
                DateTime fechaActual = DateTime.Today;
                int numD = Convert.ToInt32(numDoc);
                dw_movin dw_movin = db.Movins.SingleOrDefault(s => s.numeroDocumento == numD & s.fechaEmision == fechaActual);
                if (dw_movin != null)
                {
                    return dw_movin.dw_movinID.ToString();
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
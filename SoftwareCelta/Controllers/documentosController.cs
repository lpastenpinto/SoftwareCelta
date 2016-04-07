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

        public ActionResult Buscador(string strFechaInicial, string strFechaFinal) {
            DateTime fechaInicial= new DateTime();
            DateTime fechaFinal = new DateTime();
            if(strFechaFinal==null || strFechaInicial==null){
                fechaInicial=DateTime.Now.AddDays(-7);
                fechaFinal=DateTime.Now;
            }else{
                fechaInicial=Formateador.formatearFechaCompleta(strFechaInicial);
                fechaFinal=Formateador.formatearFechaCompleta(strFechaFinal);
            }

            List<dw_movin> listDocumentos = db.Movins.Where(s => s.fechaEmision >= fechaInicial & s.fechaEmision <= fechaFinal).ToList();

            ViewBag.fechaInicial = Formateador.fechaCompletaToString(fechaInicial);
            ViewBag.fechaFinal = Formateador.fechaCompletaToString(fechaFinal);
            return View(listDocumentos);
        }

        [HttpPost]
        public ActionResult Documento(FormCollection form) {
            //VER SI YA SE INGRESO BOLETA
            //int numeroDocumento = Convert.ToInt32((string)form["numeroDocumento"]);

            List<dw_areaInterna> bodegas = db.Bodegas.ToList();

            dw_movin documento = new dw_movin();
            documento.dw_movinID = 1;
            documento.numeroDocumento = 100;
            documento.numeroVale = 123;
            documento.fechaEmision =  DateTime.Now;

            List<dw_detalle> listDetalleDocumento = new List<dw_detalle>();
            
            dw_detalle detalleDocumento_1 = new dw_detalle();
            detalleDocumento_1.codigoProducto = "AS1111";
            detalleDocumento_1.descripcionProducto = "CAMA";
            detalleDocumento_1.valorProducto = 10000;
            detalleDocumento_1.cantidadProducto = 1;            

            listDetalleDocumento.Add(detalleDocumento_1);

            dw_detalle detalleDocumento_2 = new dw_detalle();
            detalleDocumento_2.codigoProducto = "AS1122";
            detalleDocumento_2.descripcionProducto = "Colchon";
            detalleDocumento_2.cantidadProducto = 1;
            detalleDocumento_2.valorProducto = 11111;
            listDetalleDocumento.Add(detalleDocumento_2);

            dw_detalle detalleDocumento_3 = new dw_detalle();
            detalleDocumento_3.codigoProducto = "ASCCC2";
            detalleDocumento_3.descripcionProducto = "Alhomada";
            detalleDocumento_3.cantidadProducto = 2;
            detalleDocumento_3.valorProducto = 5000;
            listDetalleDocumento.Add(detalleDocumento_3);

            ViewData["detalleDocumento"] = listDetalleDocumento;
            ViewData["bodegas"] = bodegas;
            return View(documento);
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
                }
                catch (Exception e) {
                    bool error = true; 
                }                
            }           
            
            return RedirectToAction("Index");
        }

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
           
            string nombreCliente = (string)form["nombreCliente"];
            string rutCliente = (string)form["rutCliente"];
            string ciudad = (string)form["ciudad"];
            string telefono = (string)form["telefono"];
            string direccion = (string)form["direccion"];
            string observacion = (string)form["observacion"];
            DateTime fechaDespacho = Formateador.fechaStringToDateTime((string)form["fechaDespacho"]);

            dw_envio datosEnvio = new dw_envio();
            datosEnvio.ciudad = ciudad;
            datosEnvio.direccion = direccion;
            datosEnvio.dw_movinID = documento.dw_movinID;
            datosEnvio.fechaDespacho = fechaDespacho;
            datosEnvio.nombreCliente = nombreCliente;
            datosEnvio.observacion = observacion;
            datosEnvio.rutCliente = rutCliente;
            datosEnvio.telefono = telefono;
            db.DatosEnvio.Add(datosEnvio);            

            string[] areaInterna = Request.Form.GetValues("areaInterna");
            string[] codigoProducto  = Request.Form.GetValues("codigoProducto");
            string[] descripcionProducto= Request.Form.GetValues("descripcionProducto");
            string[] cantidadProducto = Request.Form.GetValues("cantidadProducto");
            string[] valorProducto = Request.Form.GetValues("valorProducto");
            
            for (int i = 0; i < areaInterna.Length; i++)
            {
                dw_detalle detalle = new dw_detalle();
                detalle.cantidadProducto = Convert.ToInt32(cantidadProducto[i]);
                detalle.codigoProducto = codigoProducto[i];
                detalle.descripcionProducto = descripcionProducto[i];                
                detalle.dw_movinID = documento.dw_movinID;
                detalle.valorProducto = Convert.ToInt32(valorProducto[i]);
     
                string estadoDespacho = (string)form["estadoDespacho" + (i + 1)];

                detalle.estadoDespacho = Convert.ToInt32(estadoDespacho);
                if (estadoDespacho.Equals("0"))
                {
                    detalle.dw_areaInternaID = 0;                    
                }
                else {
                    detalle.dw_areaInternaID = Convert.ToInt32(areaInterna[i]);
                }
                db.DetalleMovin.Add(detalle);
                
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }
         
        [HttpPost]
        public string guardarAreaInteraMovin(List<string> idsDetalles, List<string> areasInternas)
        {
            try
            {                
                int i = 0;
                foreach (var idDetalle in idsDetalles)
                {
                        int idDet = Convert.ToInt32(idDetalle);
                        dw_detalle detalle = db.DetalleMovin.Find(idDet);
                        detalle.dw_areaInternaID = Convert.ToInt32(areasInternas[i]);
                        db.Entry(detalle).State = EntityState.Modified;
                        db.SaveChanges();
                        i++;
                }
                return "GUARDADO CORRECTAMENTE";
            }
            catch (Exception e) {
                return "ERROR";
            }
        }
    }
}
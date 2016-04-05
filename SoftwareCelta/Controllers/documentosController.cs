using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            List<dw_movin> documentoList = new List<dw_movin>();
            dw_movin doc1 = new dw_movin();
            doc1.dw_movinID = 1;
            doc1.numeroDocumento = 100;
            doc1.numeroVale = 123;
            doc1.fechaEmision = DateTime.Now;
            documentoList.Add(doc1);

            dw_movin doc2 = new dw_movin();
            doc2.dw_movinID = 2;
            doc2.numeroDocumento = 1002;
            doc2.numeroVale = 12234233;
            doc2.fechaEmision = DateTime.Now;
            documentoList.Add(doc2);
            ViewData["bodegas"] = db.Bodegas.ToList();
            return View(documentoList);
        }

        public ActionResult Buscador() {
            List<dw_movin> listDocumentos = new List<dw_movin>();
            dw_movin doc = new dw_movin();
            doc.dw_movinID = 1;
            doc.fechaEmision = DateTime.Today.AddDays(-3);            
            listDocumentos.Add(doc);
            ViewBag.fechaInicial = DateTime.Today;
            ViewBag.fechaFinal = DateTime.Today.AddDays(-7);
            ViewData["bodegas"] = db.Bodegas.ToList();
            return View(listDocumentos);
        }

        [HttpPost]
        public ActionResult Documento(FormCollection form) {
            //int numeroDocumento = Convert.ToInt32((string)form["numeroDocumento"]);

            List<dw_areaInterna> bodegas = db.Bodegas.ToList();

            dw_movin documento = new dw_movin();
            documento.dw_movinID = 1;
            documento.numeroDocumento = 100;
            documento.numeroVale = 123;
            documento.fechaEmision = DateTime.Now;

            List<dw_detalle> listDetalleDocumento = new List<dw_detalle>();
            
            dw_detalle detalleDocumento_1 = new dw_detalle();
            detalleDocumento_1.dw_detalleID = 1;
            detalleDocumento_1.dw_movinID = 1;
            detalleDocumento_1.codigoProducto = "AS1111";
            detalleDocumento_1.descripcionProducto = "CAMA";
            detalleDocumento_1.cantidadProducto = 1;            

            listDetalleDocumento.Add(detalleDocumento_1);

            dw_detalle detalleDocumento_2 = new dw_detalle();
            detalleDocumento_2.dw_detalleID = 2;
            detalleDocumento_2.dw_movinID = 1;
            detalleDocumento_2.codigoProducto = "AS1122";
            detalleDocumento_2.descripcionProducto = "Colchon";
            detalleDocumento_2.cantidadProducto = 1;
            listDetalleDocumento.Add(detalleDocumento_2);
            ViewData["detalleDocumento"] = listDetalleDocumento;
            ViewData["bodegas"] = bodegas;
            return View(documento);
        }

        public ActionResult DocumentoRegistrado(int documentoID) {

            dw_movin documento = new dw_movin();
            documento.dw_movinID = 1;
            documento.numeroDocumento = 100;
            documento.numeroVale = 123;
            documento.fechaEmision = DateTime.Now;

            List<dw_detalle> listDetalleDocumento = new List<dw_detalle>();

            dw_detalle detalleDocumento_1 = new dw_detalle();
            detalleDocumento_1.dw_areaInternaID = 3;
            detalleDocumento_1.estadoDespacho = 1;
            detalleDocumento_1.dw_detalleID = 1;
            detalleDocumento_1.dw_movinID = 1;
            detalleDocumento_1.codigoProducto = "AS1111";
            detalleDocumento_1.descripcionProducto = "CAMA";
            detalleDocumento_1.cantidadProducto = 1;

            listDetalleDocumento.Add(detalleDocumento_1);

            dw_detalle detalleDocumento_2 = new dw_detalle();
            detalleDocumento_2.dw_areaInternaID = 3;
            detalleDocumento_2.estadoDespacho = 0;
            detalleDocumento_2.dw_detalleID = 2;
            detalleDocumento_2.dw_movinID = 1;
            detalleDocumento_2.codigoProducto = "AS1122";
            detalleDocumento_2.descripcionProducto = "Colchon";
            detalleDocumento_2.cantidadProducto = 1;
            listDetalleDocumento.Add(detalleDocumento_2);

            dw_envio datosEnvio = new dw_envio();
            datosEnvio.direccion = "Las Azaleas 854";
            datosEnvio.observacion = "Ir despues de las 6pm";
            ViewData["datosEnvio"] = datosEnvio;
            ViewData["detalleDocumento"] = listDetalleDocumento;

            return View(documento);
        }

        public ActionResult Editar(int documentoID) {
            return View();
        }

        public ActionResult registrarNuevoDocumento(FormCollection form) {
            string[] bodega = Request.Form.GetValues("bodega");
            for (int i = 0; i<bodega.Length; i++) {
                string despacho = (string)form["despacho" + (i + 1)];
            }
            return RedirectToAction("Index");
        }
    }
}
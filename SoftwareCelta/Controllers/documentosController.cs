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
            List<documento> documentoList = new List<documento>();
            documento doc1 = new documento();
            doc1.documentoID = 1;
            doc1.numeroDocumento = 100;
            doc1.numeroVenta = 123;
            doc1.fechaEmisionDocumento = DateTime.Now;
            documentoList.Add(doc1);

            documento doc2 = new documento();
            doc2.documentoID = 2;
            doc2.numeroDocumento = 1002;
            doc2.numeroVenta = 12234233;
            doc2.fechaEmisionDocumento = DateTime.Now;
            documentoList.Add(doc2);
            ViewData["bodegas"] = db.Bodegas.ToList();
            return View(documentoList);
        }

        public ActionResult Buscador() {
            List<documento> listDocumentos = new List<documento>();
            documento doc = new documento();
            doc.documentoID = 1;
            doc.fechaEmisionDocumento = DateTime.Today.AddDays(-3);
            doc.estadoDespacho = 2;
            listDocumentos.Add(doc);
            ViewBag.fechaInicial = DateTime.Today;
            ViewBag.fechaFinal = DateTime.Today.AddDays(-7);
            ViewData["bodegas"] = db.Bodegas.ToList();
            return View(listDocumentos);
        }

        [HttpPost]
        public ActionResult Documento(FormCollection form) {
            //int numeroDocumento = Convert.ToInt32((string)form["numeroDocumento"]);

            List<bodega> bodegas = db.Bodegas.ToList();

            documento documento = new documento();
            documento.documentoID = 1;
            documento.numeroDocumento = 100;
            documento.numeroVenta = 123;
            documento.fechaEmisionDocumento = DateTime.Now;

            List<detalleDocumento> listDetalleDocumento = new List<detalleDocumento>();
            
            detalleDocumento detalleDocumento_1 = new detalleDocumento();
            detalleDocumento_1.detalleDocumentoID = 1;
            detalleDocumento_1.documentoID = 1;
            detalleDocumento_1.codigoProducto = "AS1111";
            detalleDocumento_1.descripcionProducto = "CAMA";
            detalleDocumento_1.cantidadProducto = 1;            

            listDetalleDocumento.Add(detalleDocumento_1);

            detalleDocumento detalleDocumento_2 = new detalleDocumento();
            detalleDocumento_2.detalleDocumentoID = 2;
            detalleDocumento_2.documentoID = 1;
            detalleDocumento_2.codigoProducto = "AS1122";
            detalleDocumento_2.descripcionProducto = "Colchon";
            detalleDocumento_2.cantidadProducto = 1;
            listDetalleDocumento.Add(detalleDocumento_2);
            ViewData["detalleDocumento"] = listDetalleDocumento;
            ViewData["bodegas"] = bodegas;
            return View(documento);
        }

        public ActionResult DocumentoRegistrado(int documentoID) {

            documento documento = new documento();
            documento.documentoID = 1;
            documento.numeroDocumento = 100;
            documento.numeroVenta = 123;
            documento.fechaEmisionDocumento = DateTime.Now;

            List<detalleDocumento> listDetalleDocumento = new List<detalleDocumento>();

            detalleDocumento detalleDocumento_1 = new detalleDocumento();
            detalleDocumento_1.bodegaID = 3;
            detalleDocumento_1.despachoProducto = 1;
            detalleDocumento_1.detalleDocumentoID = 1;
            detalleDocumento_1.documentoID = 1;
            detalleDocumento_1.codigoProducto = "AS1111";
            detalleDocumento_1.descripcionProducto = "CAMA";
            detalleDocumento_1.cantidadProducto = 1;

            listDetalleDocumento.Add(detalleDocumento_1);

            detalleDocumento detalleDocumento_2 = new detalleDocumento();
            detalleDocumento_2.bodegaID = 3;
            detalleDocumento_2.despachoProducto = 0;
            detalleDocumento_2.detalleDocumentoID = 2;
            detalleDocumento_2.documentoID = 1;
            detalleDocumento_2.codigoProducto = "AS1122";
            detalleDocumento_2.descripcionProducto = "Colchon";
            detalleDocumento_2.cantidadProducto = 1;
            listDetalleDocumento.Add(detalleDocumento_2);

            datosEnvio datosEnvio = new datosEnvio();
            datosEnvio.direccion = "Las Azaleas 854";
            datosEnvio.referencia = "Ir despues de las 6pm";
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoftwareCelta.Models;
using System.Text;
using SoftwareCelta.DAL;

namespace SoftwareCelta.Controllers
{
    public class listadoDespachoController : Controller
    {
        private ContextBDCelta db = new ContextBDCelta();
        // GET: listadoDespacho         
        public ActionResult Despachados() {
            List<StringBuilder> listText = new List<StringBuilder>();
            List<documento> documentoList = new List<documento>();
            documento doc1 = new documento();
            doc1.documentoID = 1;
            doc1.numeroDocumento = 100;
            doc1.numeroVenta = 123;
            doc1.fechaEmisionDocumento = DateTime.Now;
            documentoList.Add(doc1);
            StringBuilder textoProductos1 = new StringBuilder();
            textoProductos1.Append("Cama.");
            listText.Add(textoProductos1);

            documento doc2 = new documento();
            doc2.documentoID = 2;
            doc2.numeroDocumento = 1002;
            doc2.numeroVenta = 12234233;
            doc2.fechaEmisionDocumento = DateTime.Now;
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

        public ActionResult noDespachados() {
            List<StringBuilder> listText = new List<StringBuilder>();
            List<documento> documentoList = new List<documento>();
            documento doc1 = new documento();
            doc1.documentoID = 1;
            doc1.numeroDocumento = 100;
            doc1.numeroVenta = 123;
            doc1.fechaEmisionDocumento = DateTime.Now;
            documentoList.Add(doc1);
            StringBuilder textoProductos1 = new StringBuilder();
            textoProductos1.Append("Cama.");
            listText.Add(textoProductos1);

            documento doc2 = new documento();
            doc2.documentoID = 2;
            doc2.numeroDocumento = 1002;
            doc2.numeroVenta = 12234233;
            doc2.fechaEmisionDocumento = DateTime.Now;
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

        public ActionResult porDespachar() {

            List<StringBuilder> listText = new List<StringBuilder>();           
            List<documento> documentoList = new List<documento>();
            documento doc1 = new documento();
            doc1.documentoID = 1;
            doc1.numeroDocumento = 100;
            doc1.numeroVenta = 123;
            doc1.fechaEmisionDocumento = DateTime.Now;
            documentoList.Add(doc1);
            StringBuilder textoProductos1 = new StringBuilder();
            textoProductos1.Append("Cama.");
            listText.Add(textoProductos1);

            documento doc2 = new documento();
            doc2.documentoID = 2;
            doc2.numeroDocumento = 1002;
            doc2.numeroVenta = 12234233;
            doc2.fechaEmisionDocumento = DateTime.Now;
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

        public ActionResult Despachar(int documentoID)
        {
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

        [HttpPost]
        public ActionResult Despachar(FormCollection form) {

            string[] checkeoDespacho = Request.Form.GetValues("estadoDespacho");
            int cantidadProductosCheck = checkeoDespacho.Length;
            int totalProductos = Convert.ToInt32((string)form["cantidadProductos"]);
            return RedirectToAction("porDespachar");
        }
    }
}
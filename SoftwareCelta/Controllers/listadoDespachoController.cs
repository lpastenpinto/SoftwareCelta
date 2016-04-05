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
            List<dw_movin> documentoList = new List<dw_movin>();
            dw_movin doc1 = new dw_movin();
            doc1.dw_movinID = 1;
            doc1.numeroDocumento = 100;
            doc1.numeroVale = 123;
            doc1.fechaEmision = DateTime.Now;
            documentoList.Add(doc1);
            StringBuilder textoProductos1 = new StringBuilder();
            textoProductos1.Append("Cama.");
            listText.Add(textoProductos1);

            dw_movin doc2 = new dw_movin();
            doc2.dw_movinID = 2;
            doc2.numeroDocumento = 1002;
            doc2.numeroVale = 12234233;
            doc2.fechaEmision = DateTime.Now;
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

        public ActionResult porDespachar() {

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

        public ActionResult Despachar(int documentoID)
        {
            dw_movin documento = new dw_movin();
            documento.dw_movinID = 1;
            documento.numeroDocumento = 100;
            documento.numeroVale = 123;
            documento.fechaEmision= DateTime.Now;

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

        [HttpPost]
        public ActionResult Despachar(FormCollection form) {

            string[] checkeoDespacho = Request.Form.GetValues("estadoDespacho");
            int cantidadProductosCheck = checkeoDespacho.Length;
            int totalProductos = Convert.ToInt32((string)form["cantidadProductos"]);
            return RedirectToAction("porDespachar");
        }
    }
}
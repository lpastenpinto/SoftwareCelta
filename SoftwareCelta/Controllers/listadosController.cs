using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoftwareCelta.Models;
using SoftwareCelta.DAL;
using System.Collections;


namespace SoftwareCelta.Controllers
{
   
    public class listadosController : Controller
    {
        
        private static string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
        public ActionResult Index()
        {
            return View();
        }

        public FileContentResult productosValidar(string bodega)
        {
            LocalReport reporte_local = new LocalReport();            
            List<dw_movin> listaMovi = (List<dw_movin>)Session["listadoModelValidarProductosReport"];
            List<List<dw_detalle>> listaDetalle =(List<List<dw_detalle>>)Session["listadoDetalleValidarProductosReport"];
            
            reporte_local.ReportPath = Server.MapPath("~/Report/productosValidar.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";

            reportValidarProduct ReportClass = new reportValidarProduct();
            List<reportValidarProduct> lista = ReportClass.toReport(listaMovi, listaDetalle, bodega);
            /*List<ProductoReport> datos = new List<ProductoReport>();
            List<Producto> listaProductosStockCritico = Producto.listaProductosStockCritico();
            foreach (Producto Prod in listaProductosStockCritico)
            {
                datos.Add(new ProductoReport(Prod));

            }*/

            conjunto_datos.Value = lista;
            reporte_local.DataSources.Add(conjunto_datos);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }



    }

    public class reportValidarProduct{
        
        public string numeroDocumento { set; get; }
        public string fechaEmision { set; get; }        
        public string codigoProducto { set; get; }
        public string descripcionProducto { set; get; }
        public string cantidadProducto { set; get; }
        public string bodega{ set; get; }       

        public reportValidarProduct() { }

        public List<reportValidarProduct> toReport(List<dw_movin> movin, List<List<dw_detalle>> detalle, string bodega) {

            ContextBDCelta db = new ContextBDCelta();
            Hashtable hash = new Hashtable();
            List<dw_areaInterna> bodegas = db.Bodegas.ToList();
            foreach (var bod in bodegas) {
                hash.Add(bod.dw_areaInternaID, bod.nombre);
            }

            int x=0;
            List<reportValidarProduct> lista = new List<reportValidarProduct>();            
            foreach (var mov in movin)
            {

                
                for (int i = 0; i < detalle[x].Count; i++) {
                    reportValidarProduct newClass = new reportValidarProduct();
                    newClass.bodega = hash[detalle[x][i].dw_areaInternaID].ToString();
                    newClass.cantidadProducto=detalle[x][i].cantidadProducto.ToString();
                    newClass.codigoProducto=detalle[x][i].codigoProducto;
                    newClass.descripcionProducto = detalle[x][i].descripcionProducto;
                    newClass.fechaEmision = Formateador.fechaCompletaToString(mov.fechaEmision);
                    newClass.numeroDocumento = mov.numeroDocumento.ToString();
                    lista.Add(newClass);
                }                    
                x++;
            }
            return lista;
        }

    }
}
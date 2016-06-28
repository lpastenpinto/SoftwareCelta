using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoftwareCelta.Models;
using SoftwareCelta.DAL;
using System.Collections;
using System.Data.Entity;



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

        public FileContentResult productosValidar(string fechaDesde, string fechaHasta, string bodega)
        {
            LocalReport reporte_local = new LocalReport();            
            List<dw_movin> listaMovi = (List<dw_movin>)Session["listadoModelValidarProductosReport"];
            List<List<dw_detalle>> listaDetalle =(List<List<dw_detalle>>)Session["listadoDetalleValidarProductosReport"];
            List<dw_envio> datosEnvio = (List<dw_envio>)Session["datosEnvioProductosReport"];

            reporte_local.ReportPath = Server.MapPath("~/Report/productosValidarCiudad.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";

            reportValidarProduct ReportClass = new reportValidarProduct();
            List<reportValidarProduct> lista = ReportClass.toReport(listaMovi, listaDetalle, bodega, datosEnvio);

            ReportParameter fecha1 = new ReportParameter("fechaInicial", fechaDesde);
            ReportParameter fecha2 = new ReportParameter("fechaFinal", fechaHasta);
            reporte_local.SetParameters(new ReportParameter[] { fecha1, fecha2 });
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


        public FileContentResult productosValidarAgrupado(string fechaDesde, string fechaHasta, string bodega)
        {
            LocalReport reporte_local = new LocalReport();         
            List<List<dw_detalle>> listaDetalle = (List<List<dw_detalle>>)Session["listadoDetalleValidarProductosReport"];
                                    
            reporte_local.ReportPath = Server.MapPath("~/Report/productosValidar.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";

            reportValidarProduct ReportClass = new reportValidarProduct();
            List<reportValidarProduct> lista = ReportClass.toReportGroup(listaDetalle);

            conjunto_datos.Value = lista;
            ReportParameter fecha1 = new ReportParameter("fechaInicial",fechaDesde);
            ReportParameter fecha2= new ReportParameter("fechaFinal",fechaHasta);
            reporte_local.SetParameters(new ReportParameter[] { fecha1, fecha2 });
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


        public FileContentResult productosDespachar(string ciudad, string fechaDesde, string fechaHasta) {

            LocalReport reporte_local = new LocalReport();
            
            List<dw_movin> movins = (List<dw_movin>)Session["listadoProductosDespachoReport"];
            List<List<dw_detalle>> listaDetalle = (List<List<dw_detalle>>)Session["listadoDetalleProductosDespachoReport"];
            List<dw_envio> envios = (List<dw_envio>)Session["listadoEnviosProductosDespachoReport"];

            reporte_local.ReportPath = Server.MapPath("~/Report/productosDespachar.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";


            reportDespachoProduct reportClass = new reportDespachoProduct();
            List<reportDespachoProduct> lista = reportClass.toReport(movins, listaDetalle, envios);

            conjunto_datos.Value = lista;
            ReportParameter fecha1 = new ReportParameter("fechaInicial", fechaDesde);
            ReportParameter fecha2 = new ReportParameter("fechaFinal", fechaHasta);
            if (ciudad == null) {
                ciudad = "Todas";
            }
            ReportParameter _ciudad = new ReportParameter("ciudad", ciudad);
            reporte_local.SetParameters(new ReportParameter[] { fecha1, fecha2,_ciudad });
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

        public FileContentResult productosDespacharAgrupados(string ciudad, string fechaDesde, string fechaHasta)
        {

            LocalReport reporte_local = new LocalReport();

            List<dw_movin> movins = (List<dw_movin>)Session["listadoProductosDespachoReport"];
            List<List<dw_detalle>> listaDetalle = (List<List<dw_detalle>>)Session["listadoDetalleProductosDespachoReport"];
            List<dw_envio> envios = (List<dw_envio>)Session["listadoEnviosProductosDespachoReport"];

            reporte_local.ReportPath = Server.MapPath("~/Report/productosDespacharGroup.rdlc");
            ReportDataSource conjunto_datos = new ReportDataSource();
            conjunto_datos.Name = "DataSet1";


            reportDespachoProduct reportClass = new reportDespachoProduct();
            List<reportDespachoProduct> lista = reportClass.toReportGroup(movins, listaDetalle, envios);

            conjunto_datos.Value = lista;
            ReportParameter fecha1 = new ReportParameter("fechaInicial", fechaDesde);
            ReportParameter fecha2 = new ReportParameter("fechaFinal", fechaHasta);
            if (ciudad == null)
            {
                ciudad = "Todas";
            }
            ReportParameter _ciudad = new ReportParameter("ciudad", ciudad);
            reporte_local.SetParameters(new ReportParameter[] { fecha1, fecha2, _ciudad });
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
        public string ciudad { set; get; }

        public reportValidarProduct() { }

            public List<reportValidarProduct> toReport(List<dw_movin> movin, List<List<dw_detalle>> detalle, string bodega,List<dw_envio> datosEnvio) {

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
                    newClass.ciudad = datosEnvio[x].ciudad;
                    lista.Add(newClass);
                }                    
                x++;
            }
            return lista;
        }

            public List<reportValidarProduct> toReportGroup(List<List<dw_detalle>> listaDetalle)
        {
            ContextBDCelta db = new ContextBDCelta();            
            Hashtable hashBodegas = new Hashtable();
            List<dw_areaInterna> bodegas = db.Bodegas.ToList();
            foreach (var bod in bodegas)
            {
                hashBodegas.Add(bod.dw_areaInternaID, bod.nombre);
            }
            List<reportValidarProduct> listaFinal = new List<reportValidarProduct>();
            Hashtable hashDetalle = new Hashtable();
            int x = 0;
            foreach (var detalle in listaDetalle)
            {

                foreach (var det in detalle)
                {
                    if (!hashDetalle.Contains(det.codigoProducto))
                    {
                        hashDetalle.Add(det.codigoProducto, x);
                        reportValidarProduct report = new reportValidarProduct();
                        report.bodega = hashBodegas[det.dw_areaInternaID].ToString();
                        report.cantidadProducto = det.cantidadProducto.ToString();
                        report.codigoProducto = det.codigoProducto.ToString();
                        report.descripcionProducto = det.descripcionProducto;
                        listaFinal.Add(report);
                        ++x;
                    }
                    else
                    {
                        int posicionListaFinal = Convert.ToInt32(hashDetalle[det.codigoProducto].ToString());
                        listaFinal[posicionListaFinal].cantidadProducto = (Convert.ToDouble(listaFinal[posicionListaFinal].cantidadProducto) + Convert.ToDouble(det.cantidadProducto)).ToString();

                    }

                }

            }
            return listaFinal;
        
        }
    }


    public class reportDespachoProduct {
        public string numeroDocumento { set; get; }
        public string fechaDespacho { set; get; }
        public string codigoProducto { set; get; }
        public string descripcionProducto { set; get; }
        public string cantidadProducto { set; get; }
        public string bodegaProducto { set; get; }
        public string ciudad { set; get; }
        
        public List<reportDespachoProduct> toReport(List<dw_movin> movins, List<List<dw_detalle>> listaDetalle, List<dw_envio> envios)
        {
            ContextBDCelta db = new ContextBDCelta();
            Hashtable hashBodega = new Hashtable();
            List<dw_areaInterna> bodegas = db.Bodegas.ToList();
            foreach (var bod in bodegas) {
                hashBodega.Add(bod.dw_areaInternaID, bod.nombre);
            }
            List<reportDespachoProduct> listaFinal = new List<reportDespachoProduct>();
            reportDespachoProduct report;
            int x = 0;
            foreach (var movimiento in movins) {

                foreach (var det in listaDetalle[x]) {
                    report = new reportDespachoProduct();
                    report.bodegaProducto = hashBodega[det.dw_areaInternaID].ToString();;
                    report.cantidadProducto = det.cantidadProducto.ToString();
                    report.codigoProducto = det.codigoProducto;
                    report.descripcionProducto = det.descripcionProducto;
                    report.fechaDespacho = Formateador.fechaCompletaToString(envios[x].fechaDespacho);
                    report.numeroDocumento = movimiento.numeroDocumento.ToString();
                    report.ciudad = envios[x].ciudad;
                    listaFinal.Add(report);
                }
                ++x;
            }

            return listaFinal;
        
        }

        public List<reportDespachoProduct> toReportGroup(List<dw_movin> movins, List<List<dw_detalle>> listaDetalle, List<dw_envio> envios)
        {
            ContextBDCelta db = new ContextBDCelta();
            Hashtable hashBodega = new Hashtable();
            List<dw_areaInterna> bodegas = db.Bodegas.ToList();
            foreach (var bod in bodegas) {
                hashBodega.Add(bod.dw_areaInternaID, bod.nombre);
            }
            List<reportDespachoProduct> listaFinal = new List<reportDespachoProduct>();
            Hashtable hashMovin = new Hashtable();
            reportDespachoProduct report;
            int x = 0;
            int i = 0;
            foreach (var movimiento in movins) {

                foreach (var det in listaDetalle[x]) {
                    if (!hashMovin.Contains(det.codigoProducto))
                    {
                        hashMovin.Add(det.codigoProducto, i);
                        report = new reportDespachoProduct();
                        report.bodegaProducto = hashBodega[det.dw_areaInternaID].ToString();
                        report.cantidadProducto = det.cantidadProducto.ToString();
                        report.codigoProducto = det.codigoProducto;
                        report.descripcionProducto = det.descripcionProducto;
                        //report.fechaDespacho = Formateador.fechaCompletaToString(envios[x].fechaDespacho);
                        //report.numeroDocumento = movimiento.numeroDocumento.ToString();
                        //report.ciudad = envios[x].ciudad;
                        listaFinal.Add(report);
                        ++i;
                    }
                    else {
                        int posicionListaFinal = Convert.ToInt32(hashMovin[det.codigoProducto].ToString());
                        listaFinal[posicionListaFinal].cantidadProducto = (Convert.ToDouble(listaFinal[posicionListaFinal].cantidadProducto) + Convert.ToDouble(det.cantidadProducto)).ToString();
                    }
                }
                ++x;
            }

            return listaFinal;
        
        }
    
    
    
    }
}
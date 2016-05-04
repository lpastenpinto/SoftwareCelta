using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SoftwareCelta.Models;
using SoftwareCelta.DAL;
using System.Data.SqlClient;
using System.Data;
using SoftwareCelta.Filters;

namespace SoftwareCelta.Controllers
{
    public class documentosController : Controller
    {
        private ContextBDCelta db = new ContextBDCelta();
        // GET: documentos
        [Permissions(Permission1 = 1, Permission2 = 2)]
        public ActionResult Index()
        {                        
            return View();

        }

        [Permissions(Permission1 = 1, Permission2 = 2)]
        public ActionResult despachoDomicilio() {
            DateTime fechaActual = DateTime.Today;
            List<dw_movin> documetosRegistrados = db.Movins.Where(s => s.fechaEmision == fechaActual).ToList();
            List<List<dw_detalle>> listaDeListaDetalle = new List<List<dw_detalle>>();
            List<dw_movin> documentosRegistradosParaDespacho = new List<dw_movin>();
            foreach (var dw_movin in documetosRegistrados)
            {
                List<dw_detalle> listDetalle = db.DetalleMovin.Where(s => s.dw_movinID == dw_movin.dw_movinID & s.estadoDespacho == 1).ToList();
                if (listDetalle.Count!=0) {
                    dw_movin.detalleMovin = listDetalle;
                    documentosRegistradosParaDespacho.Add(dw_movin);
                    //listaDeListaDetalle.Add(listDetalle);
                }                
            }
            //ViewData["listaDeListaDetalle"] = listaDeListaDetalle;
            ViewData["bodegas"] = db.Bodegas.ToList();
            return View(documentosRegistradosParaDespacho);
        }

        [Permissions(Permission1 = 1, Permission2 = 2)]
        public ActionResult datosEnvio(int documentoID) {
            dw_envio dw_envio = db.DatosEnvio.SingleOrDefault(s=>s.dw_movinID==documentoID);
            if (dw_envio == null) {
                dw_envio = new dw_envio();
            }
            ViewData["dw_ciudades"] = dw_ciudades_despacho.listaCiudades();
            ViewData["dw_movin"] = db.Movins.Find(documentoID);
            return View(dw_envio);
        
        }

        [HttpPost]
        [Permissions]
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
            TempData["Success"] = "Informacion Guardada con exito";
            return RedirectToAction("despachoDomicilio");

        }

        [Permissions]
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

        [Permissions]
        public ActionResult Documento(int documentoID)
        {

            DateTime fechaActual = DateTime.Now.AddDays(-1);
            SqlConnection cnx = Connection.getConection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;

            cmd.CommandText = "SELECT * from softland.celta_ventas WHERE Folio=@documentoID  AND anno=@anioActual";            
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@documentoID", SqlDbType.Int).Value = documentoID;
            cmd.Parameters.Add("@anioActual", SqlDbType.Int).Value = fechaActual.Year;
            SqlDataReader dr = cmd.ExecuteReader();
            
            dw_movin documento = new dw_movin();
            List<dw_detalle> listDetalle = new List<dw_detalle>();
            documento.numeroDocumento = documentoID;
            documento.total = 0;
            while (dr.Read())
            {
                documento.fechaEmision = (DateTime)dr["Fecha"];
                documento.tipoDocumento = (string)dr["Tipo"];
                documento.nombreVendedor = (string)dr["CodVendedor"];
                if ((string)dr["Tipo"] == "B")
                {
                    documento.tipoDocumento = "BOLETA";
                }
                else if ((string)dr["Tipo"] == "F")
                {
                    documento.tipoDocumento = "FACTURA";
                }
                string nomAux = "";
                try
                {
                    nomAux = (string)dr["NomAux"];
                }
                catch (Exception)
                {
                    nomAux = "Cliente";
                }
                documento.nombreCliente = nomAux;  

                dw_detalle detalle = new dw_detalle();
                detalle.cantidadProducto = Convert.ToDouble(dr["CantFacturada"]);
                detalle.codigoProducto = (string)dr["CodProd"];
                detalle.descripcionProducto = (string)dr["DesProd"];
                double precioProd = Math.Round(Convert.ToDouble(dr["Valor"])) - Math.Round(Convert.ToDouble(dr["Descuento"]));
                detalle.valorProducto = Convert.ToInt32(precioProd);
                detalle.dw_movinID = documento.dw_movinID;
                listDetalle.Add(detalle);
                documento.detalleMovin = listDetalle;
                documento.total = documento.total + detalle.valorProducto;

            }
            
            cnx.Close();                        
            return View(documento);
        }

        [Permissions(Permission1 = 1, Permission2 = 2)]
        public ActionResult paraDespacharADomicilio(int documentoID)
        {
            dw_movin dw_movin = db.Movins.Find(documentoID);
            ViewData["detalleDocumento"] = db.DetalleMovin.Where(s => s.dw_movinID == documentoID).ToList();
            ViewData["bodegas"] = db.Bodegas.ToList();
            return View(dw_movin);
        }

        [HttpPost]
        [Permissions]
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
            TempData["Success"] = "Informacion Guardada con exito";
            return RedirectToAction("despachoDomicilio");
        }

        [Permissions]
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

      /*
        [Permissions]
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
        [Permissions]
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
        */
        [HttpPost]
        [Permissions(Permission1 = 1, Permission2 = 2)]
        public ActionResult registrarNuevoDocumento(FormCollection form) {
            int numeroDocumento = Convert.ToInt32((string)form["numeroDocumento"]);
            int numeroVale = Convert.ToInt32((string)form["numeroVale"]);
            string tipoDoc = (string)form["Tipo"];
            string nombreVendedor = (string)form["CodVendedor"];
            DateTime fechaEmision = Formateador.fechaFormatoGuardar((string)form["fechaEmision"]);
            
            dw_movin documento = new dw_movin();
            documento.fechaEmision = fechaEmision;
            documento.numeroDocumento = numeroDocumento;
            documento.numeroVale = numeroVale;
            documento.nombreVendedor = nombreVendedor;
            documento.tipoDocumento = tipoDoc;
            documento.nombreCliente = (string)form["nombreCliente"];
            documento.total = Convert.ToDouble((string)form["total"]);
            db.Movins.Add(documento);
            db.SaveChanges();
                                           
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
            TempData["Success"] = "Se registro el documento Folio "+documento.numeroDocumento+" Exitosamente";
            return RedirectToAction("Index");
        }
         
        [HttpPost]
        [Permissions]
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
        [Permissions]
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
        [Permissions]
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

        private List<int> generateStringQuery(DateTime fecha) {
            List<int> retorno = new List<int>();
            List<dw_movin> listMovins = db.Movins.Where(s => s.fechaEmision >= fecha).ToList();
            if (listMovins.Count != 0)
            {
                foreach (var mov in listMovins)
                {
                    retorno.Add(mov.numeroDocumento);
                }
            }
            return retorno;
        }

        [HttpGet]

        public JsonResult getNewDocuments()
        {
            DateTime fecha = DateTime.Now.AddDays(-1);

            List<int> foliosGuardados = generateStringQuery(fecha);
            SqlConnection cnx = Connection.getConection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from softland.celta_ventas WHERE Fecha >@fecha AND anno=@anioActual AND CantFacturada>0";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
            cmd.Parameters.Add("@anioActual", SqlDbType.Int).Value = fecha.Year;
            SqlDataReader dr = cmd.ExecuteReader();
            
            int x = 0;
            int folio = 0;

            dw_movin mov = new dw_movin();
            List<dw_movin> documentosRegistrados = new List<dw_movin>();
            List<dw_detalle> listDetalle = new List<dw_detalle>();
            double totalDoc = 0;
            while (dr.Read())
            {
                if (!foliosGuardados.Contains(Convert.ToInt32(dr["Folio"])))
                {
                    if (folio != Convert.ToInt32(dr["Folio"]))
                    {
                        totalDoc = 0;
                        x++;
                        mov = new dw_movin();
                        listDetalle = new List<dw_detalle>();
                        mov.dw_movinID = x;
                        mov.fechaEmision = (DateTime)dr["Fecha"];
                        mov.numeroDocumento = Convert.ToInt32(dr["Folio"]);
                        mov.nombreVendedor = Formateador.fechaCompletaToString((DateTime)dr["Fecha"]);
                        mov.tipoDocumento = (string)dr["Tipo"];
                        if ((string)dr["Tipo"] == "B")
                        {
                            mov.tipoDocumento = "BOLETA";
                        }
                        else if ((string)dr["Tipo"] == "F")
                        {
                            mov.tipoDocumento = "FACTURA";
                        }
                        string nomAux = "";
                        try
                        {
                            nomAux = (string)dr["NomAux"];
                        }
                        catch (Exception)
                        {
                            nomAux = "Cliente Boleta";
                        }
                        mov.nombreCliente = nomAux;
                        folio = Convert.ToInt32(dr["Folio"]);
                        documentosRegistrados.Add(mov);
                    }
                    dw_detalle detalle = new dw_detalle();
                    detalle.cantidadProducto = Convert.ToDouble(dr["CantFacturada"]);
                    detalle.codigoProducto = (string)dr["CodProd"];
                    detalle.descripcionProducto = (string)dr["DesProd"];
                    detalle.dw_movinID = mov.dw_movinID;
                    listDetalle.Add(detalle);
                    mov.detalleMovin = listDetalle;
                    double Valor = (double)dr["Valor"];
                    double Descuento = (double)dr["Descuento"];
                    totalDoc = totalDoc + (Valor - Descuento);

                    mov.total = Math.Round(totalDoc, 0);                    
                }
            }

            cnx.Close();
            var result = documentosRegistrados; 
            return Json(result, JsonRequestBehavior.AllowGet);

        }

    }
}
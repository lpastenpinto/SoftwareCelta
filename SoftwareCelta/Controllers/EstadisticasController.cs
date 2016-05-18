using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoftwareCelta.Models;
using SoftwareCelta.Filters;
using SoftwareCelta.DAL;

namespace SoftwareCelta.Controllers
{
    public class EstadisticasController : Controller
    {
        // GET: Estadisticas
        private ContextBDCelta db = new ContextBDCelta();


        [Permissions]
        public ActionResult bodega(string strFechaInicial, string strFechaFinal, string areaInt)
        {
            DateTime fechaInicial = new DateTime();
            DateTime fechaFinal = new DateTime();
            setDateTime(ref fechaInicial,ref fechaFinal,ref strFechaInicial,ref strFechaFinal);
            List<dw_envio> ListEnvios = db.DatosEnvio.Where(s => s.fechaDespacho <= fechaFinal & s.fechaDespacho >= fechaInicial).ToList();
            List<dw_envio> ListEnviosFinal = new List<dw_envio>();
            List<dw_detalle> listaDeListaDetalle = new List<dw_detalle>();
            if (areaInt == null)
            {
                areaInt = "todas";
                foreach (var envios in ListEnvios)
                {
                    List<dw_detalle> detallesList = db.DetalleMovin.Where(s => s.dw_movinID == envios.dw_movinID & s.validado!=0 & s.estadoDespacho!=1).ToList();
                    if (detallesList.Count > 0) {
                        for (int i = 0; i < detallesList.Count; i++) {
                            ListEnviosFinal.Add(envios);
                            listaDeListaDetalle.Add(detallesList[i]);
                        }                                                    
                    }
                    
                }
            }
            else {
                int AreaInterna = Convert.ToInt32(areaInt);
                foreach (var envios in ListEnvios)
                {
                    List<dw_detalle> detallesList = db.DetalleMovin.Where(s => s.dw_movinID == envios.dw_movinID & s.dw_areaInternaID == AreaInterna & s.validado != 0 & s.estadoDespacho != 1).ToList();                    
                    if (detallesList.Count > 0)
                    {
                        for (int i = 0; i < detallesList.Count; i++)
                        {
                            ListEnviosFinal.Add(envios);
                            listaDeListaDetalle.Add(detallesList[i]);
                        }                                       
                    }
                }
            }
            ViewBag.fechaInicial = Formateador.fechaCompletaToString(fechaInicial);
            ViewBag.fechaFinal = Formateador.fechaCompletaToString(fechaFinal);
            ViewBag.bodega = areaInt;
            ViewData["bodegas"] = Formateador.listToHash(db.Bodegas.ToList());
            ViewData["listabodegas"] = db.Bodegas.ToList();
            ViewData["listaDetalles"] = listaDeListaDetalle;
            return View(ListEnviosFinal);
        }



       [Permissions]
        public ActionResult transportista(string strFechaInicial, string strFechaFinal, string tipo, string transportista,string ciudad)
        {
            int ids = 100000;
            DateTime fechaInicial = new DateTime();
            DateTime fechaFinal = new DateTime();
            setDateTime(ref fechaInicial, ref fechaFinal, ref strFechaInicial, ref strFechaFinal);
           
            if (ciudad == null || ciudad == "todas")
            {               
                ciudad = "todas";
            }           
            
            List<dw_envio> ListEnviosFinal = new List<dw_envio>();
            List<dw_movin> ListMovins = new List<dw_movin>();
            List<int> idsDevoluciones = obtenerHistorialTransportista(fechaInicial,fechaFinal);
            if ((tipo == null || tipo == "todos") && (transportista == null || transportista == "todos") )
            {
                tipo="todos";
                transportista = "todos";
                                
                List<historicoTransportista> hisTrans = db.historicosTransportistas.Where(s => s.fechaAnteriorDespacho <= fechaFinal & s.fechaAnteriorDespacho >= fechaInicial).ToList();
                setHistoricoToList(ref hisTrans, ref ids, ref ListEnviosFinal, ref ListMovins, ciudad);
            }
            else if ((tipo != null || tipo != "todos") && (transportista == null || transportista == "todos") )
            {
                transportista = "todos";

                if (tipo == "despacho") {
                    List<historicoTransportista> hisTrans = db.historicosTransportistas.Where(s => s.fechaAnteriorDespacho <= fechaFinal & s.fechaAnteriorDespacho >= fechaInicial & s.tipo=="despacho").ToList();
                    setHistoricoToList(ref hisTrans, ref ids, ref ListEnviosFinal, ref ListMovins,ciudad);                                   
                } else {
                    List<historicoTransportista> hisTrans = db.historicosTransportistas.Where(s => s.fechaAnteriorDespacho <= fechaFinal & s.fechaAnteriorDespacho >= fechaInicial & s.tipo=="devolucion").ToList();
                    setHistoricoToList(ref hisTrans, ref ids, ref ListEnviosFinal, ref ListMovins,ciudad);
                }
                
            }
            else {
                int idTrans = Convert.ToInt32(transportista);                
                if (tipo == "despacho" || tipo == "devolucion")
                {                  
                    List<historicoTransportista> hisTrans = db.historicosTransportistas.Where(s => s.fechaAnteriorDespacho <= fechaFinal & s.fechaAnteriorDespacho >= fechaInicial & s.idTransportistaAnterior == idTrans & s.tipo==tipo).ToList();
                    setHistoricoToList(ref hisTrans, ref ids, ref ListEnviosFinal, ref ListMovins,ciudad);                   
                }               
                else
                {                  
                    List<historicoTransportista> hisTrans = db.historicosTransportistas.Where(s => s.fechaAnteriorDespacho <= fechaFinal & s.fechaAnteriorDespacho >= fechaInicial & s.idTransportistaAnterior == idTrans).ToList();
                    setHistoricoToList(ref hisTrans, ref ids, ref ListEnviosFinal, ref ListMovins,ciudad);
                    
                }
                
            }
            ViewBag.fechaInicial = Formateador.fechaCompletaToString(fechaInicial);
            ViewBag.fechaFinal = Formateador.fechaCompletaToString(fechaFinal);
            ViewBag.tipo = tipo;
            ViewBag.transportistaActual = transportista;
            ViewBag.ciudad = ciudad;
            ViewData["envios"] = ListEnviosFinal;
            ViewData["transportista"] = db.Transportistas.ToList();
            ViewData["transportistaHash"] = Formateador.listToHash(db.Transportistas.ToList());
            ViewData["ciudades"] = dw_ciudades_despacho.listaCiudades();
            return View(ListMovins);
        }

      
        private List<int> obtenerHistorialTransportista(DateTime fechaInicial, DateTime fechaFinal) {
            List<historicoTransportista> hisTrans = db.historicosTransportistas.Where(s => s.fechaAnteriorDespacho >= fechaInicial & s.fechaAnteriorDespacho <= fechaFinal).ToList();
            List<int> retorno = new List<int>();
            foreach (var his in hisTrans) {
                retorno.Add(his.idTransportistaAnterior);
            }
            return retorno;
        }

        private void setMovinsToList(ref List<dw_movin> listMov, ref List<dw_envio> ListEnviosFinal, ref List<dw_movin> ListMovins, dw_envio envios)
        {            
            if (listMov.Count > 0)
            {
                for (int i = 0; i < listMov.Count; i++)
                {
                    ListEnviosFinal.Add(envios);
                    ListMovins.Add(listMov[i]);
                }
            }        
        }

        private void setHistoricoToList(ref List<historicoTransportista> hisTrans, ref int ids, ref List<dw_envio> ListEnviosFinal, ref List<dw_movin> ListMovins,string ciudad)
        {
            foreach (var his in hisTrans)
            {
                dw_movin mov = db.Movins.Find(his.dw_movinID);
                
                dw_envio envio = new dw_envio();
                envio.dw_envioID = ids;
                envio.dw_movinID = his.dw_movinID;
                envio.dw_datosTransportistaID = his.idTransportistaAnterior;
                envio.fechaDespacho = his.fechaAnteriorDespacho;
                envio.ciudad = his.ciudad;
                envio.nombreCliente = his.tipo;

                if (ciudad == "todas" || ciudad == null)
                {
                    ListEnviosFinal.Add(envio);
                    ListMovins.Add(mov);
                    ids++;
                }
                else if (envio.ciudad == ciudad)
                {                    
                    ListEnviosFinal.Add(envio);
                    ListMovins.Add(mov);
                    ids++;
                }
                
            }

        }

        private void setDateTime(ref DateTime fechaInicial, ref DateTime fechaFinal, ref string strFechaInicial, ref string strFechaFinal)
        {
            if (strFechaFinal == null || strFechaInicial == null)
            {
                fechaInicial = DateTime.Today.AddDays(-1);
                fechaFinal = DateTime.Today;
            }
            else
            {
                fechaInicial = Formateador.formatearFechaCompleta(strFechaInicial);
                fechaFinal = Formateador.formatearFechaCompleta(strFechaFinal);
            }
        
        }
    }
}
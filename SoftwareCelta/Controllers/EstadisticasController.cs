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

        [Permissions(Permission1 = 1)]
        public ActionResult bodega(string strFechaInicial, string strFechaFinal, string areaInt)
        {
            DateTime fechaInicial = new DateTime();
            DateTime fechaFinal = new DateTime();
            if (strFechaFinal == null || strFechaInicial == null)
            {
                fechaInicial = DateTime.Now.AddDays(-1);
                fechaFinal = DateTime.Now;
            }
            else
            {
                fechaInicial = Formateador.formatearFechaCompleta(strFechaInicial);
                fechaFinal = Formateador.formatearFechaCompleta(strFechaFinal);
            }
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

        [Permissions(Permission1 = 1)]
        public ActionResult ciudad(string strFechaInicial, string strFechaFinal, string tipo, string transportista)
        {
            int ids = 100000;
            DateTime fechaInicial = new DateTime();
            DateTime fechaFinal = new DateTime();
            if (strFechaFinal == null || strFechaInicial == null)
            {
                fechaInicial = DateTime.Now.AddDays(-1);
                fechaFinal = DateTime.Now;
            }
            else
            {
                fechaInicial = Formateador.formatearFechaCompleta(strFechaInicial);
                fechaFinal = Formateador.formatearFechaCompleta(strFechaFinal);
            }
            List<dw_envio> ListEnvios = db.DatosEnvio.Where(s => s.fechaDespacho <= fechaFinal & s.fechaDespacho >= fechaInicial).ToList();
            List<dw_envio> ListEnviosFinal = new List<dw_envio>();
            List<dw_movin> ListMovins = new List<dw_movin>();
            List<int> idsDevoluciones = obtenerHistorialTransportista(fechaInicial,fechaFinal);
            if ((tipo==null||tipo=="todos" ) &&  (transportista==null||transportista=="todos" ))
            {
                tipo="todos";
                transportista = "todos";
                foreach (var envios in ListEnvios)
                {
                    if ((envios.dw_datosTransportistaID != 0) || (envios.dw_datosTransportistaID == 0 && idsDevoluciones.Contains(envios.dw_datosTransportistaID)))
                    {
                        List<dw_movin> listMov = db.Movins.Where(s => s.dw_movinID == envios.dw_movinID).ToList();

                        if (listMov.Count > 0)
                        {
                            for (int i = 0; i < listMov.Count; i++)
                            {
                                ListEnviosFinal.Add(envios);
                                ListMovins.Add(listMov[i]);
                            }
                        }

                        List<historicoTransportista> hisTrans = db.historicosTransportistas.Where(s => s.fechaAnteriorDespacho <= fechaFinal & s.fechaAnteriorDespacho >= fechaInicial).ToList();
                        foreach (var his in hisTrans)
                        {
                            dw_envio envio = new dw_envio();
                            envio.dw_envioID = ids;
                            envio.dw_movinID = his.dw_movinID;
                            envio.dw_datosTransportistaID = his.idTransportistaAnterior;
                            envio.fechaDespacho = his.fechaAnteriorDespacho;
                            envio.nombreCliente = "Devolucion";
                            dw_movin mov = db.Movins.Find(his.dw_movinID);
                            ListEnviosFinal.Add(envio);
                            ListMovins.Add(mov);
                            ids++;
                        }
                    }
                }
            }
            else if ((tipo != null||tipo != "todos") && (transportista == null||transportista == "todos"))
            {
                transportista = "todos";
                foreach (var envios in ListEnvios)
                {
                    if ((envios.dw_datosTransportistaID != 0) || (envios.dw_datosTransportistaID == 0 && idsDevoluciones.Contains(envios.dw_datosTransportistaID)))
                    {
                        if (tipo == "despacho")
                        {
                            List<dw_movin> listMov = db.Movins.Where(s => s.dw_movinID == envios.dw_movinID).ToList();

                            if (listMov.Count > 0)
                            {
                                for (int i = 0; i < listMov.Count; i++)
                                {
                                    ListEnviosFinal.Add(envios);
                                    ListMovins.Add(listMov[i]);
                                }
                            }
                        }
                        else
                        {
                            List<historicoTransportista> hisTrans = db.historicosTransportistas.Where(s => s.fechaAnteriorDespacho <= fechaFinal & s.fechaAnteriorDespacho >= fechaInicial).ToList();
                            foreach (var his in hisTrans)
                            {
                                dw_movin mov = db.Movins.Find(his.dw_movinID);

                                dw_envio envio = new dw_envio();
                                envio.dw_envioID = ids;
                                envio.dw_movinID = his.dw_movinID;
                                envio.dw_datosTransportistaID = his.idTransportistaAnterior;
                                envio.fechaDespacho = his.fechaAnteriorDespacho;
                                envio.nombreCliente = "Devolucion";
                                ListEnviosFinal.Add(envio);
                                ListMovins.Add(mov);
                                ids++;
                            }
                        }
                    }
                }
            }
            else {
                int idTrans = Convert.ToInt32(transportista);
                ListEnvios = db.DatosEnvio.Where(s => s.fechaDespacho <= fechaFinal & s.fechaDespacho >= fechaInicial & s.dw_datosTransportistaID == idTrans).ToList();
                foreach (var envios in ListEnvios)
                {
                    if ((envios.dw_datosTransportistaID != 0) || (envios.dw_datosTransportistaID == 0 && idsDevoluciones.Contains(envios.dw_datosTransportistaID)))
                    {
                        if (tipo == "despacho")
                        {
                            List<dw_movin> listMov = db.Movins.Where(s => s.dw_movinID == envios.dw_movinID).ToList();

                            if (listMov.Count > 0)
                            {
                                for (int i = 0; i < listMov.Count; i++)
                                {
                                    ListEnviosFinal.Add(envios);
                                    ListMovins.Add(listMov[i]);
                                }
                            }
                        }
                        else
                        {
                            List<historicoTransportista> hisTrans = db.historicosTransportistas.Where(s => s.fechaAnteriorDespacho <= fechaFinal & s.fechaAnteriorDespacho >= fechaInicial).ToList();
                            foreach (var his in hisTrans)
                            {
                                dw_movin mov = db.Movins.Find(his.dw_movinID);

                                dw_envio envio = new dw_envio();
                                envio.dw_envioID = ids;
                                envio.dw_movinID = his.dw_movinID;
                                envio.dw_datosTransportistaID = his.idTransportistaAnterior;
                                envio.fechaDespacho = his.fechaAnteriorDespacho;
                                envio.nombreCliente = "Devolucion";
                                ListEnviosFinal.Add(envio);
                                ListMovins.Add(mov);
                                ids++;
                            }
                        }
                    }
                }
            }
            ViewBag.fechaInicial = Formateador.fechaCompletaToString(fechaInicial);
            ViewBag.fechaFinal = Formateador.fechaCompletaToString(fechaFinal);
            ViewBag.tipo = tipo;
            ViewBag.transportistaActual = transportista;
            ViewData["envios"] = ListEnviosFinal;
            ViewData["transportista"] = db.Transportistas.ToList();
            ViewData["transportistaHash"] = Formateador.listToHash(db.Transportistas.ToList());
            return View(ListMovins);
        }

        //[Permissions(Permission1 = 1)]
        public ActionResult transportista()
        {
            return View();
        }


        private List<int> obtenerHistorialTransportista(DateTime fechaInicial, DateTime fechaFinal) {
            List<historicoTransportista> hisTrans = db.historicosTransportistas.Where(s => s.fechaAnteriorDespacho >= fechaInicial & s.fechaAnteriorDespacho <= fechaFinal).ToList();
            List<int> retorno = new List<int>();
            foreach (var his in hisTrans) {
                retorno.Add(his.idTransportistaAnterior);
            }
            return retorno;
        }
    }
}
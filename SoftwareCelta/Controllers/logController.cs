using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SoftwareCelta.DAL;
using SoftwareCelta.Models;

namespace SoftwareCelta.Controllers
{
    public class logController : Controller
    {
        private ContextBDCelta db = new ContextBDCelta();

        // GET: log
        public ActionResult Index(string fInicial, string fFinal, string user)
        {

            DateTime fechaInicial = new DateTime();
            DateTime fechaFinal = new DateTime();
            if (fInicial == null || fFinal == null)
            {
                fechaInicial = DateTime.Now.AddDays(-1);
                fechaFinal = DateTime.Now;
            }
            else
            {
                fechaInicial = Formateador.formatearFechaCompleta(fInicial);
                fechaFinal = Formateador.formatearFechaCompleta(fFinal);
            }
            List<dw_log> listLog= new List<dw_log>();

            if(user!=null){
                int userID= Convert.ToInt32(user);
                listLog = db.Logs.Where(s => s.userID == userID & s.fecha<=fechaFinal & s.fecha>=fechaInicial).ToList();
            }else{
                listLog = db.Logs.Where(s => s.fecha <= fechaFinal & s.fecha >= fechaInicial).ToList();
            }
            
            ViewBag.fechaInicial = Formateador.fechaCompletaToString(fechaInicial);
            ViewBag.fechaFinal = Formateador.fechaCompletaToString(fechaFinal);
            return View(listLog);
        }
    }
}

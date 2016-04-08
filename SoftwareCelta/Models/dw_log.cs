using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SoftwareCelta.DAL;
namespace SoftwareCelta.Models
{
    public class dw_log
    {
        public int dw_logID { set; get; }
        public int userID { set; get; }
        public string nameUser { set; get; }
        public string accion { set;get;}
        public DateTime fecha { set; get; }

        public static void registrarLog(int userID,string userName, string accion) {
            ContextBDCelta dbLog = new ContextBDCelta();
            dw_log log = new dw_log();
            log.userID = userID;
            log.nameUser = userName;
            log.fecha = DateTime.Now;
            log.accion = accion;
            dbLog.Logs.Add(log);
            dbLog.SaveChanges();  
        }
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SoftwareCelta.DAL;

namespace SoftwareCelta.Models
{
    public class dw_ciudades_despacho
    {
        public int dw_ciudades_despachoID { set; get; }
        public string nombre { set; get; }
        public static List<dw_ciudades_despacho> listaCiudades() {

            ContextBDCelta db = new ContextBDCelta();
            List<dw_ciudades_despacho> list = db.Ciudades.ToList();
            
            return list;

        } 
    }
}
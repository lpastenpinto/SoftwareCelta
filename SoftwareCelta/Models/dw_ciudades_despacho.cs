using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoftwareCelta.Models
{
    public class dw_ciudades_despacho
    {
        public int dw_ciudades_despachoID { set; get; }
        public string nombre { set; get; }
        public static List<dw_ciudades_despacho> listaCiudades() {
            List<dw_ciudades_despacho> list = new List<dw_ciudades_despacho>();
            dw_ciudades_despacho ciudad1 = new dw_ciudades_despacho();
            ciudad1.dw_ciudades_despachoID = 1;
            ciudad1.nombre = "LA SERENA";
            list.Add(ciudad1);
            
            dw_ciudades_despacho ciudad2 = new dw_ciudades_despacho();
            ciudad2.dw_ciudades_despachoID = 2;
            ciudad2.nombre = "COQUIMBO";
            list.Add(ciudad2);

            dw_ciudades_despacho ciudad3 = new dw_ciudades_despacho();
            ciudad3.dw_ciudades_despachoID = 3;
            ciudad3.nombre = "PAN DE AZUCAR";
            list.Add(ciudad3);
            return list;

        } 
    }
}
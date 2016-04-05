using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoftwareCelta.Models
{
    public class dw_envio
    {
        public int dw_envioID { set; get; }
        public int dw_movinID { set; get; }
        public int userID { set; get; }
        public string nombreCliente { set; get; }
        public string rutCliente { set; get; }
        public string ciudad { set; get; }
        public string telefono { set; get; }
        public string direccion { set; get; }
        public string perimetroDespacho { set; get; }
        public string observacion { set; get; }
        public int cantidadVisitasDespacho { set; get; }
        public DateTime fechaDespacho { set; get; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoftwareCelta.Models
{
    public class dw_datosTransportista
    {
        public int dw_datosTransportistaID { set; get; }
        public string codigo { set; get; }
        public string nombreCompleto { set; get; }
        public string rut { set; get; }
        public string razonSocial { set; get; }
        public string direccion { set; get; }
        public string telefono { set; get; }
        public string mail { set; get; }
        public string contacto { set; get; }
        public string ciudad { set; get; }
        public string giro { set; get; }
    }
}
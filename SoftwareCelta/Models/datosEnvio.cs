using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoftwareCelta.Models
{
    public class datosEnvio
    {
        public int datosEnvioID { set; get; }
        public int documentoID { set; get; }
        public string direccion { set; get; }
        public string referencia { set; get; }
    }
}
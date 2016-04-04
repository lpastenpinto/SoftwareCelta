using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoftwareCelta.Models
{
    public class documento
    {
        public int documentoID { set; get; }
        public int numeroDocumento { set; get; }
        public int numeroVenta { set; get; }
        public DateTime fechaEmisionDocumento { set; get; }
        public DateTime fechaDespacho { set; get; }
        public int estadoDespacho { set; get; }
        public int estadoDespachoInicial { set; get; }
        public string perimetroDespacho { set; get; }
        public string cantidadVisitasDespacho { set; get; }
    }
}
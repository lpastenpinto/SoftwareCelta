using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoftwareCelta.Models
{
    public class historicoTransportista
    {
        public int historicoTransportistaID { set; get; }
        public DateTime fechaAnteriorDespacho { set; get;}
        public int idTransportistaAnterior { set; get;}
        public int dw_movinID { set; get; }
        public string ciudad { set; get; }
        public string tipo { set; get; }
    }
}
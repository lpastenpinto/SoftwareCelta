using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoftwareCelta.Models
{
    public class detalleDocumento
    {
        public int detalleDocumentoID { set; get; }
        public int documentoID { set; get; }
        public int bodegaID { set; get; }
        public string codigoProducto { set; get; }
        public string descripcionProducto { set; get; }
        public int precioProducto { set; get; }
        public int cantidadProducto { set; get; }
        public int despachoProducto { set; get; }
    }
}
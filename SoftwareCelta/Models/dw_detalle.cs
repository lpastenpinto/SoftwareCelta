﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoftwareCelta.Models
{
    public class dw_detalle
    {
        public int dw_detalleID { set; get; }
        public int dw_movinID { set; get; }
        public int dw_areaInternaID { set; get; }
        public string codigoProducto { set; get; }
        public string descripcionProducto { set; get; }
        public int valorProducto { set; get; }
        public int cantidadProducto { set; get; }
        public int estadoDespacho { set; get; }
    }
}
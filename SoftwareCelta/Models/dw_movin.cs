﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoftwareCelta.Models
{
    public class dw_movin
    {
        public int dw_movinID { set; get; }
        public int numeroDocumento { set; get; }
        public int numeroVale { set; get; }
        public DateTime fechaEmision { set; get; }
        public string tipoDocumento { set; get; }
        public string nombreVendedor { set; get; }
        public string nombreCliente { set; get; }
        public double total { set; get; }
        public string notaGeneralDespacho { set; get; }
        public string notaGeneralValidacionProductos { set; get; }
        public List<dw_detalle> detalleMovin { set; get; }
    }
}
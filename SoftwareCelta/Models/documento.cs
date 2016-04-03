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
    }
}
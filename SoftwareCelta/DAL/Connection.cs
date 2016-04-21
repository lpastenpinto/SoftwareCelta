using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
namespace SoftwareCelta.DAL
{
    public class Connection
    {
        public static SqlConnection getConection()
        {
            //string conexion sql server            
            //AZURE
            //var rutaAcceso = "Server=tcp:ekgiet2alo.database.windows.net,1433;Database=softwareCelta;User ID=softwareCelta@ekgiet2alo;Password=celta2016*;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            
            //DESPACHO
            //var rutaAcceso = "Server=192.168.3.221;Database=Despacho;User ID=dys;Password=dys2016;";
            
            //VISTA SOFTLAND
            var rutaAcceso = "Server=192.168.3.221;Database=COMERCIAL;User ID=consulta;Password=consulta2011;";
            SqlConnection conn = new SqlConnection(rutaAcceso);

            conn.Open();
            return conn;
        }
    }
}
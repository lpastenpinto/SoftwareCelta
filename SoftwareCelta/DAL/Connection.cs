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
            var rutaAcceso = "Server=tcp:ekgiet2alo.database.windows.net,1433;Database=softwareCelta;User ID=softwareCelta@ekgiet2alo;Password=celta2016*;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

            SqlConnection conn = new SqlConnection(rutaAcceso);

            conn.Open();
            return conn;
        }
    }
}
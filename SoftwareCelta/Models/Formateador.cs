using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace SoftwareCelta.Models
{
    public class Formateador
    {
        public static DateTime formatearFechaCompleta(DateTime fechaCompleta)
        {

            //string fecha = fechaCompleta.ToString().Split(' ')[0];
            //string[] fecha_1 = fecha.Split('-');
            int dia1 = fechaCompleta.Day;//Convert.ToInt32(fecha_1[0]);
            int mes1 = fechaCompleta.Month;//Convert.ToInt32(fecha_1[1]);
            int ano1 = fechaCompleta.Year;//Convert.ToInt32(fecha_1[2]);

            DateTime fechaNueva = new DateTime(ano1, mes1, dia1);
            return fechaNueva;
        }
        public static DateTime fechaStringToDateTime(string fecha)
        {
            fecha = fecha.Replace("/", "-");
            string[] fecha1 = fecha.Split(' ')[0].Split('-');
            int dia1 = Convert.ToInt32(fecha1[0]);
            int mes1 = Convert.ToInt32(fecha1[1]);
            int ano1 = Convert.ToInt32(fecha1[2]);

            DateTime fechaNueva = new DateTime(ano1, mes1, dia1);
            //DateTime fechaNueva = new DateTime(ano1, dia1, mes1); //inglés
            return fechaNueva;
        }
        public static DateTime fechaStringToDateConHora(string fecha, string hora)
        {
            fecha = fecha.Replace("/", "-");
            string[] fecha1 = fecha.Split(' ')[0].Split('-');
            int dia1 = Convert.ToInt32(fecha1[0]);
            int mes1 = Convert.ToInt32(fecha1[1]);
            int ano1 = Convert.ToInt32(fecha1[2]);

            string[] horaCompleta = hora.Split(' ')[1].Split(':');
            int _hora = Convert.ToInt32(horaCompleta[0]);
            int _minuto = Convert.ToInt32(horaCompleta[1]);
            int _segundo = Convert.ToInt32(horaCompleta[2]);
            DateTime fechaNueva = new DateTime(ano1, mes1, dia1, _hora, _minuto, _segundo);
            return fechaNueva;
        }
        public static DateTime formatearFechaCompletaSlash(DateTime fechaCompleta)
        {

            string fecha = fechaCompleta.ToString().Split(' ')[0];
            string[] fecha_1 = fecha.Split('/');
            int dia1 = Convert.ToInt32(fecha_1[0]);
            int mes1 = Convert.ToInt32(fecha_1[1]);
            int ano1 = Convert.ToInt32(fecha_1[2]);

            DateTime fechaNueva = new DateTime(ano1, mes1, dia1);
            return fechaNueva;
        }

        //STRING
        public static string fechaCompletaToString(DateTime fechaCompleta)
        {
            int dia = 0;
            int mes = 0;
            int ano = 0;            
            dia = fechaCompleta.Day;
            mes = fechaCompleta.Month;
            ano = fechaCompleta.Year;

            string dia_ = dia+"";
            string mes_ = mes+"";
            if (dia < 10) {
                dia_ = "0" + dia;
            }
            if (mes < 10) {
                mes_ = "0" + mes;
            }
            return dia_ + "/" + mes_ + "/" + ano;
        }

        public static string fechaCompletaToStringS0(DateTime fechaCompleta)
        {                       
            return fechaCompleta.Day + "/" + fechaCompleta.Month + "/" + fechaCompleta.Year;
        }
        public static DateTime formatearFechaCompleta(string fechaCompleta)
        {
            fechaCompleta = fechaCompleta.Replace("-", "/");
            string[] fecha_1 = fechaCompleta.Split('/');
            int dia1 = Convert.ToInt32(fecha_1[0]);
            int mes1 = Convert.ToInt32(fecha_1[1]);
            int ano1 = Convert.ToInt32(fecha_1[2]);

            DateTime fechaNueva = new DateTime(ano1, mes1, dia1);
            return fechaNueva;
        }

        public static string valoresPesos(int entrada)
        {

            String valor = entrada.ToString();
            string retorno = "";

            char[] caracteres = valor.ToCharArray();

            for (int i = caracteres.Length - 1; i >= 0; i--)
            {
                if (i == caracteres.Length - 3)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == caracteres.Length - 6)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == caracteres.Length - 9)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == caracteres.Length - 12)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == caracteres.Length - 15)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == caracteres.Length - 18)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == caracteres.Length - 21)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else
                {
                    retorno = caracteres[i] + retorno;
                }
            }

            if (retorno.StartsWith(".")) retorno = retorno.TrimStart('.');
            else if (retorno.StartsWith("-."))
            {
                retorno = retorno.Replace("-.", "-");
            }


            return "$" + retorno;
        }

        public static DateTime fechaFormatoGuardar(string fechaString)
        {
            fechaString = fechaString.Replace("/", "-");
            DateTime fecha = new DateTime(int.Parse(fechaString.Split('-')[2]),
           int.Parse(fechaString.Split('-')[1]), int.Parse(fechaString.Split('-')[0]));
            return fecha;
        }

        public static Hashtable listToHash(List<dw_areaInterna> listaBodegas) {
            Hashtable hash = new Hashtable();
            foreach (var areaInt in listaBodegas) {
                hash.Add(areaInt.dw_areaInternaID, areaInt.nombre);
            }
            return hash;
        }

        public static Hashtable listToHash(List<dw_envio> listaEnvio)
        {
            Hashtable hash = new Hashtable();
            foreach (var env in listaEnvio)
            {
                hash.Add(env.valeVenta, env.direccion);
            }
            return hash;
        }
        public static Hashtable listToHash(List<permisosBodegas> permisosBodegas)
        {
            Hashtable hash = new Hashtable();
            foreach (var perm in permisosBodegas)
            {
                hash.Add(perm.bodegaID, perm.userID);
            }
            return hash;
        }
        public static Hashtable listToHash(List<dw_datosTransportista> Transportista)
        {
            Hashtable hash = new Hashtable();
            foreach (var perm in Transportista)
            {
                hash.Add(perm.dw_datosTransportistaID, perm.nombreCompleto);
            }
            return hash;
        }

        public static List<int> listToInt(List<permisosBodegas> permisosBodegas)
        {
            List<int> list = new List<int>();
            foreach (var perm in permisosBodegas)
            {
                list.Add(perm.bodegaID);
            }
            return list;
        }
    }
}
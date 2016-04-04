using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoftwareCelta.Models
{
    public class user
    {
        public int userID { set; get; }
        public string userName { set; get; }
        public string password { set;get; }
        public string nombreCompleto { set; get; }        
        List<permisoUser> listaPermisosUser { set; get; }
        List<int> permisosUser { set; get; }
        
    }
    public class permisoUser {
        public int permisoUserID { set; get; }
        public int userID { set; get; }
        public int rolesID { set; get; }        
    }
}
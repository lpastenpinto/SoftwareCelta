using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SoftwareCelta.Models;

namespace SoftwareCelta.DAL
{
    public class ContextBDCelta : DbContext
    {
        public ContextBDCelta()
            : base("ContextBDCelta")
        {
        }

        public DbSet<user> Users { set; get; }
        public DbSet<permisoUser> PermisosUser { set; get; }
        public DbSet<roles> Roles { set; get; }
        public DbSet<dw_areaInterna> Bodegas { set; get; }
        public DbSet<dw_datosTransportista> Transportistas { set; get; }
        public DbSet<dw_envio> DatosEnvio { set; get; }
        public DbSet<dw_detalle> DetalleMovin { set; get; }
        public DbSet<dw_movin> Movins { set; get; }
        public DbSet<dw_log> Logs { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}
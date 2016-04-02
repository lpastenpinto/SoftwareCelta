using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SoftwareCelta.Models;

namespace SoftwareCelta.DAL
{
    public class ContextBDSoftland : DbContext
    {
        public ContextBDSoftland()
            : base("ContextBDSoftland")
        {
        }

        public DbSet<user> Users { set; get; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WcfService2
{
    public class LPGContext:DbContext
    {
        public LPGContext() : base("LPGDS")
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Cylinder> Cylinders { get; set; }
        public DbSet<Stove> Stoves { get; set; }
        public DbSet<Regulator> Regulators { get; set; }

    }
}
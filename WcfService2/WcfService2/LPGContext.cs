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
        public DbSet<DistributorUser> DistributorUsers { get; set; }
        public DbSet<TxCylinder> txCylinders { get; set; }
        public DbSet<TxStoveRegulator> txStoves { get; set; }
        public DbSet<TxRegulator> txRegulators { get; set; }
        public DbSet<GSTRates> GSTRates { get; set; }
        public DbSet<CylCustMapping> CylCustMappings { get; set; }
    }
}
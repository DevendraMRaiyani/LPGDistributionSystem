using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WcfService2
{
    public class CustomerContext:DbContext
    {
        public CustomerContext() : base("LPGDS")
        {
        }
        public DbSet<Customer> Customers { get; set; }
    }
}
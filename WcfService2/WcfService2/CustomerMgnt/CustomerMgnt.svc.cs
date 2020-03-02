using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : ICustomerMgnt
    {
        LPGContext cc = new LPGContext();
        public Customer AddCustomer(Customer customer)
        {
            Customer c = cc.Customers.Add(customer);
            cc.SaveChanges();
            return c;
        }

        public string DeleteCustomer(int cid)
        {
            var result = cc.Customers.Where(x => x.CustomerId == cid).FirstOrDefault();
            cc.Customers.Remove(result);
            cc.SaveChanges();
            return "OK";
        }

        public Customer GetCustomer(string name)
        {
            return cc.Customers.Where(x => x.CustomerName.Equals(name)).FirstOrDefault();
        }

        public List<Customer> GetCustomers(string name)
        {
            if (name.Length > 0)
                return cc.Customers.Where(x => x.CustomerType.Equals(name)).ToList();
            else
                return cc.Customers.ToList();
        }

        public string[] GetCustomersName()
        {
            return cc.Customers.Select(x => x.CustomerName).ToArray();
        }

        public string[] GetCustomersTypes()
        {
            return cc.CylCustMappings.Select(x=>x.CustomerType).ToArray();
        }

        public string UpdateCustomer(Customer customer)
        {
            Customer c = cc.Customers.Where(x => x.AadharNo.Equals(customer.AadharNo)).FirstOrDefault();
            c.CustomerName = customer.CustomerName;
            c.Gender = customer.Gender;
            c.Address = customer.Address;
            c.City = customer.City;
            c.PinNo = customer.PinNo;
            c.Taluka = customer.Taluka;
            c.District = customer.District;
            c.State = customer.State;
            c.ContactNo = customer.ContactNo;
            c.Email = customer.Email;
            cc.SaveChanges();
            return "OK";
        }
    }
}

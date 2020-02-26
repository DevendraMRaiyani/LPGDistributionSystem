using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ICustomerMgnt
    {
        [OperationContract]
        Customer AddCustomer(Customer customer);

        [OperationContract]
        Customer GetCustomer(string name);

        [OperationContract]
        string[] GetCustomersName();

        [OperationContract]
        string[] GetCustomersTypes();

        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    
        [DataContract]
    public class Customer
    {
        [DataMember]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        [DataMember]
        public int DistributorCode { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public string CustomerType { get; set; }
        [DataMember]
        public string AadharNo { get; set; }
        [DataMember]
        public string RashanCardNo { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public int PinNo { get; set; }
        [DataMember]
        public string Taluka { get; set; }
        [DataMember]
        public string District { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string ContactNo { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string BankIFSC { get; set; }
        [DataMember]
        public string BankAccountNo { get; set; }
    }
}

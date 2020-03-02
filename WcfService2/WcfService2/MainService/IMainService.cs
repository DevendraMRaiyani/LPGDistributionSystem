using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMainService" in both code and config file together.
    [ServiceContract]
    public interface IMainService
    {
        [OperationContract]
        DistributorUser GetDistributor();
    }

    [DataContract]
    public class DistributorUser
    {
        [DataMember]
        [Key]
        public int DisId { get; set; }

        [DataMember]
        public int DistributorCode { get; set; }

        [DataMember]
        public string AgencyName { get; set; }

        [DataMember]
        public string DistributorName { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string PANnumber { get; set; }

        [DataMember]
        public string GSTIN { get; set; }

        [DataMember]
        public DateTime LicenceIssueDate { get; set; }

        [DataMember]
        public DateTime LicenceExpiryDate { get; set; }

        [DataMember]
        public string Address { get; set; }
    }
}

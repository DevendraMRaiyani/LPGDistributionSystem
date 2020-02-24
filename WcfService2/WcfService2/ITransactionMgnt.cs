using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITransactionMgnt" in both code and config file together.
    [ServiceContract]
    public interface ITransactionMgnt
    {
        [OperationContract]
        string AddRegulatorTx(int qty);

        [OperationContract]
        string AddStoveTx(string details,int qty);

        [OperationContract]
        string AddCylenderTx(string details, int qty);
    }

    [DataContract]
    public class TxCylinder
    {
        [DataMember]
        [Key]
        public int TxId { get; set; }
        [DataMember]
        public int CustomerId { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public DateTime TxDate { get; set; }
        [DataMember]
        public string CylinderDetails { get; set; }
        [DataMember]
        public int Quentity { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public double CGST { get; set; }
        [DataMember]
        public double SGST { get; set; }
        [DataMember]
        public double Total { get; set; }
    }

    [DataContract]
    public class TxStove
    {
        [DataMember]
        [Key]
        public int TxId { get; set; }
        [DataMember]
        public int CustomerId { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public DateTime TxDate { get; set; }
        [DataMember]
        public string Details { get; set; }
        [DataMember]
        public int Quentity { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public double CGST { get; set; }
        [DataMember]
        public double SGST { get; set; }
        [DataMember]
        public double Total { get; set; }
    }

    [DataContract]
    public class TxRegulator
    {
        [DataMember]
        [Key]
        public int TxId { get; set; }
        [DataMember]
        public int CustomerId { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public DateTime TxDate { get; set; }
        [DataMember]
        public string Details { get; set; }
        [DataMember]
        public int Quentity { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public double CGST { get; set; }
        [DataMember]
        public double SGST { get; set; }
        [DataMember]
        public double Total { get; set; }
    }

    [DataContract]
    public class GSTRates
    {
        [DataMember]
        [Key]
        public string Comodity { get; set; }
        [DataMember]
        public double CGST { get; set; }
        [DataMember]
        public double SGST { get; set; }
    }
}

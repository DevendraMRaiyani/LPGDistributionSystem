using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IStockMgnt" in both code and config file together.
    [ServiceContract]
    public interface IStockMgnt
    {
        [OperationContract]
        List<Cylinder> GetCylinders();

        [OperationContract]
        List<Stove> GetStoves();

        [OperationContract]
        List<Regulator> GetRegulators();

        [OperationContract]
        string SetCylinders(List<Cylinder> cylinders);

        
    }


    [DataContract]
    public class Cylinder
    {
        [DataMember(Name = "Cylinder Type")]
        [Key]
        public string type { get; set; }
        [DataMember]
        public int Quentity { get; set; }
        [DataMember]
        public double Price { get; set; }
    }

    [DataContract]
    public class Stove
    {
        [DataMember(Name = "Product")]
        [Key]
        public string type { get; set; }
        [DataMember]
        public int Quentity { get; set; }
        [DataMember]
        public double Price { get; set; }
    }

    [DataContract]
    public class Regulator
    {
        [DataMember]
        [Key]
        public int Quentity { get; set; }
        [DataMember]
        public double Price { get; set; }
    }
}

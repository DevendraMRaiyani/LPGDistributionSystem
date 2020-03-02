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
        Stove GetRegulators();

        [OperationContract]
        string SetECylinders(string a, int c);

        [OperationContract]
        string SetFCylinders(string a, int c);

        [OperationContract]
        string SetStoves(string a, int c);

        [OperationContract]
        string SetRegulators(int a);

        [OperationContract]
        string AddStove(Stove s);

        [OperationContract]
        string RemoveStove(string s);

        [OperationContract]
        string SetRegPrice(double a);

        [OperationContract]
        string SetCylPrice(string s,double a);

        [OperationContract]
        string SetStovePrice(string s,double a);

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
}

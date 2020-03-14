using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService2.Synchronization
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISynchronization" in both code and config file together.
    [ServiceContract]
    public interface ISynchronization
    {
        [OperationContract]
        void AddRecord(string tnm,string op,int rid);

        [OperationContract]
        void DeleteRecord(int Id);

        [OperationContract]
        List<Synchro> GetRecords();

        [OperationContract]
        string SynchroToCentralDB();
    }

    [DataContract]
    public class Synchro
    {
        [DataMember]
        [Key]
        public int Id { get; set; }
        [DataMember]
        public string TableName { get; set; }
        [DataMember]
        public string Operation { get; set; }
        [DataMember]
        public int RecordId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService2.Accounting
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICylinderReport" in both code and config file together.
    [ServiceContract]
    public interface IAccounting
    {
        [OperationContract]
        List<TxCylinder> GetTxCylinders(DateTime from, DateTime to, string cyType);

        [OperationContract]
        List<TxStoveRegulator> GetTxStoveRegulators(DateTime from, DateTime to, string cyType);
    }
}

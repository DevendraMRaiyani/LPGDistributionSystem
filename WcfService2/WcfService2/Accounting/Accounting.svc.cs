using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService2.Accounting
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CylinderReport" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CylinderReport.svc or CylinderReport.svc.cs at the Solution Explorer and start debugging.
    public class Accounting : IAccounting
    {
        public List<TxCylinder> GetTxCylinders(DateTime from, DateTime to, string cyType)
        {
            using (LPGContext sc = new LPGContext())
            {
                if (cyType.Length > 0)
                    return sc.txCylinders.Where(x => x.TxDate >= from && x.TxDate <= to && x.CylinderDetails.Equals(cyType)).ToList();
                else
                    return sc.txCylinders.Where(x => x.TxDate >= from && x.TxDate <= to).ToList();
            }
        }

        public List<TxStoveRegulator> GetTxStoveRegulators(DateTime from, DateTime to, string cyType)
        {
            using (LPGContext sc = new LPGContext())
            {
                if (cyType.Length > 0)
                    return sc.txStoves.Where(x => x.TxDate >= from && x.TxDate <= to && x.Details.Equals(cyType)).ToList();
                else
                    return sc.txStoves.Where(x => x.TxDate >= from && x.TxDate <= to).ToList();
            }
        }
    }
}

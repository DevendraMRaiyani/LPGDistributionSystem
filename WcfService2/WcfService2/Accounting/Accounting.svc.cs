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
                List<TxCylinder> res = null;
                if (from == to)
                {
                    res = sc.txCylinders.Where(x => x.TxDate >= from.Date).ToList();
                }
                else
                {
                    res = sc.txCylinders.Where(x => x.TxDate >= from.Date && x.TxDate<=to.Date).ToList();
                }
                if (cyType.Length > 0)
                    return res.Where(x => x.CylinderDetails.Equals(cyType)).ToList();
                else
                    return res;
            }
        }

        public List<TxStoveRegulator> GetTxStoveRegulators(DateTime from, DateTime to, string cyType)
        {
            using (LPGContext sc = new LPGContext())
            {
                List<TxStoveRegulator> res = null;
                if (from == to)
                {
                    res = sc.txStoves.Where(x => x.TxDate >= from.Date).ToList();
                }
                else
                {
                    res = sc.txStoves.Where(x => x.TxDate >= from.Date && x.TxDate <= to.Date).ToList();
                }
                if (cyType.Length > 0)
                    return res.Where(x => x.Details.Equals(cyType)).ToList();
                else
                    return res;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TransactionMgnt" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TransactionMgnt.svc or TransactionMgnt.svc.cs at the Solution Explorer and start debugging.
    public class TransactionMgnt : ITransactionMgnt
    {
        LPGContext db = new LPGContext();

        public string AddCylenderTx(string details, int qty)
        {
            DistributorUser du = db.DistributorUsers.FirstOrDefault();
            TxCylinder tc = new TxCylinder();
            tc.CustomerId = du.DistributorCode;
            tc.CustomerName = du.DistributorName;
            tc.TxDate = DateTime.Now;
            tc.CylinderDetails = details;
            tc.Quentity = qty;
            tc.Price = db.Cylinders.Where(x => x.type == details).FirstOrDefault().Price;
            tc.Amount = qty * tc.Price;
            tc.CGST = 0;
            tc.SGST = 0;
            tc.Total = tc.Amount;
            db.txCylinders.Add(tc);
            db.SaveChanges();
            return "OK";
        }

        public string AddRegulatorTx(int qty)
        {
            DistributorUser du = db.DistributorUsers.FirstOrDefault();
            TxRegulator tr = new TxRegulator();
            tr.CustomerId = du.DistributorCode;
            tr.CustomerName = du.DistributorName;
            tr.TxDate = DateTime.Now;
            tr.Details = "Regulator";
            tr.Quentity = qty;
            tr.Price = db.Stoves.Where(x => x.type == "Regulator").FirstOrDefault().Price;
            tr.Amount = qty * tr.Price;
            tr.CGST = 0;
            tr.SGST = 0;
            tr.Total = tr.Amount;
            db.txRegulators.Add(tr);
            db.SaveChanges();
            return "OK";
        }

        public string AddStoveTx(string details, int qty)
        {
            DistributorUser du = db.DistributorUsers.FirstOrDefault();
            TxStove ts = new TxStove();
            ts.CustomerId = du.DistributorCode;
            ts.CustomerName = du.DistributorName;
            ts.TxDate = DateTime.Now;
            ts.Details = details;
            ts.Quentity = qty;
            ts.Price = db.Stoves.Where(x => x.type == details).FirstOrDefault().Price;
            ts.Amount = qty * ts.Price;
            ts.CGST = 0;
            ts.SGST = 0;
            ts.Total = ts.Amount;
            db.txStoves.Add(ts);
            db.SaveChanges();
            return "OK";
        }

        public string BookingCylinderTx(int cid, int qty)
        {
            Customer c = db.Customers.Where(x => x.CustomerId == cid).FirstOrDefault();
            DistributorUser du = db.DistributorUsers.FirstOrDefault();
            TxCylinder tc = new TxCylinder();
            tc.CustomerId = c.CustomerId;
            tc.CustomerName = c.CustomerName;
            tc.TxDate = DateTime.Now;
            string details = null;
            int q = 0;
            if (c.CustomerType.Equals("Industrial"))
            {
                q = qty;
                details = db.CylCustMappings.Where(x => x.CustomerType.Equals("Industrial")).Select(s => s.CylenderType).FirstOrDefault();
                tc.CylinderDetails = details;
                tc.Quentity = qty;
                tc.Price = db.Cylinders.Where(x => x.type == details).FirstOrDefault().Price;
                tc.Amount = qty * tc.Price;
                var gst = db.GSTRates.Where(x => x.Comodity.Equals("Industrial Cylender")).FirstOrDefault();
                tc.CGST = (tc.Amount * gst.CGST) / 100;
                tc.SGST = (tc.Amount * gst.SGST) / 100;
            }
            else
            {
                CylCustMapping ccm = null;
                
                if (c.BankAccountNo == "")
                {
                    ccm = db.CylCustMappings.Where(x => x.CustomerType.Equals(c.CustomerType) && x.CylenderType.Equals("14.2 KG Without Subsidy (F)")).FirstOrDefault();
                    details = ccm.CylenderType;
                    q = ccm.NoCylender;
                }
                else
                {
                    ccm = db.CylCustMappings.Where(x => x.CustomerType.Equals(c.CustomerType) && x.CylenderType.Equals("14.2 KG With Subsidy (F)")).FirstOrDefault();
                    details = ccm.CylenderType;
                    q = ccm.NoCylender;
                }
                tc.CylinderDetails = details;
                tc.Quentity = q;
                tc.Price = db.Cylinders.Where(x => x.type == details).FirstOrDefault().Price;
                tc.Amount = q * tc.Price;
                var gst = db.GSTRates.Where(x => x.Comodity.Equals("Domestic Cylender")).FirstOrDefault();
                tc.CGST = (tc.Amount * gst.CGST) / 100;
                tc.SGST = (tc.Amount * gst.SGST) / 100;
            }
           
            tc.Total = tc.Amount + tc.CGST + tc.SGST;
            db.txCylinders.Add(tc);
            db.SaveChanges();

            using (LPGContext dbo = new LPGContext())
            {
                var result = dbo.Cylinders.Where(x => x.type.Equals(details)).FirstOrDefault();
                result.Quentity = result.Quentity - q;
                dbo.SaveChanges();
            }
            using (LPGContext dbo1 = new LPGContext())
            {
                details = details.Substring(0, details.Length - 2);
                details = details + "E)";
                var result = dbo1.Cylinders.Where(x => x.type.Equals(details)).FirstOrDefault();
                result.Quentity = result.Quentity + q;
                dbo1.SaveChanges();
            }

            return "OK";
        }
    }
}

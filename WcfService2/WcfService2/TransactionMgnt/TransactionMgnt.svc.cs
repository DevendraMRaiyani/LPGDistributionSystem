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
            tc.Amount = Math.Round(qty * tc.Price,2);
            tc.CGST = 0;
            tc.SGST = 0;
            tc.Total = tc.Amount;
            tc.CashMemoNo = 0;
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
            tr.Amount = Math.Round(qty * tr.Price,2);
            tr.CGST = 0;
            tr.SGST = 0;
            tr.Total = tr.Amount;
            tr.CashMemoNo = 0;
            db.txRegulators.Add(tr);
            db.SaveChanges();
            return "OK";
        }

        public string AddStoveTx(string details, int qty)
        {
            DistributorUser du = db.DistributorUsers.FirstOrDefault();
            TxStoveRegulator ts = new TxStoveRegulator();
            ts.CustomerId = du.DistributorCode;
            ts.CustomerName = du.DistributorName;
            ts.TxDate = DateTime.Now;
            ts.Details = details;
            ts.Quentity = qty;
            ts.Price = db.Stoves.Where(x => x.type == details).FirstOrDefault().Price;
            ts.Amount = Math.Round(qty * ts.Price,2);
            ts.CGST = 0;
            ts.SGST = 0;
            ts.Total = ts.Amount;
            ts.CashMemoNo = 0;
            db.txStoves.Add(ts);
            db.SaveChanges();
            return "OK";
        }

        public int BookingCylinderTx(int cid, int qty)
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
                tc.Amount = Math.Round(qty * tc.Price,2);
                var gst = db.GSTRates.Where(x => x.Comodity.Equals("Industrial Cylender")).FirstOrDefault();
                tc.CGST = Math.Round((tc.Amount * gst.CGST) / 100,2);
                tc.SGST = Math.Round((tc.Amount * gst.SGST) / 100, 2);
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
                tc.Amount = Math.Round(q * tc.Price,2);
                var gst = db.GSTRates.Where(x => x.Comodity.Equals("Domestic Cylender")).FirstOrDefault();
                tc.CGST = Math.Round((tc.Amount * gst.CGST) / 100, 2);
                tc.SGST = Math.Round((tc.Amount * gst.SGST) / 100, 2);
            }
            int cmn = db.txCylinders.Select(x => x.CashMemoNo).Max(m => m) + 1 ;
            tc.CashMemoNo = cmn;
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

            return cmn;
        }

        public List<GSTRates> GetGSTRates()
        {
            return db.GSTRates.Select(x => x).ToList();
        }

        public int RegulatorTx(string cname, int qty)
        {
            TxStoveRegulator tx = new TxStoveRegulator();
            Customer c = null;
            c = db.Customers.Where(x => x.CustomerName.Equals(cname)).FirstOrDefault();
            if (c != null)
            {
                tx.CustomerId = c.CustomerId;
                tx.CustomerName = c.CustomerName;
            }
            else
            {
                tx.CustomerId = 0;
                tx.CustomerName = cname;
            }
            tx.TxDate = DateTime.Now;
            tx.Details = "Regulator";
            tx.Quentity = qty;
            var reg = db.Stoves.Where(x => x.type.Equals("Regulator")).FirstOrDefault();
            tx.Price = reg.Price;
            tx.Amount = Math.Round(qty * tx.Price,2);
            var gst = db.GSTRates.Where(x => x.Comodity.Equals("Regulator")).FirstOrDefault();
            tx.CGST = Math.Round(tx.Amount * gst.CGST / 100,2);
            tx.SGST = Math.Round(tx.Amount * gst.SGST / 100, 2);
            tx.Total = tx.Amount + tx.CGST + tx.SGST;
            int cmn = db.txStoves.Select(x => x.CashMemoNo).Max(m => m) + 1;
            tx.CashMemoNo = cmn;
            db.txStoves.Add(tx);
            db.SaveChanges();
            using (LPGContext dbo = new LPGContext())
            {
                var result = dbo.Stoves.Where(x => x.type.Equals("Regulator")).FirstOrDefault();
                result.Quentity = result.Quentity - qty;
                dbo.SaveChanges();
            }
            return cmn;
        }

        public int StoveTx(string cname,string prod, int qty, int cmno)
        {
            TxStoveRegulator tx = new TxStoveRegulator();
            Customer c = null;
            c = db.Customers.Where(x => x.CustomerName.Equals(cname)).FirstOrDefault();
            if (c != null)
            {
                tx.CustomerId = c.CustomerId;
                tx.CustomerName = c.CustomerName;
            }
            else
            {
                tx.CustomerId = 0;
                tx.CustomerName = cname;
            }
            tx.TxDate = DateTime.Now;
            tx.Details = prod;
            tx.Quentity = qty;
            var reg = db.Stoves.Where(x => x.type.Equals(prod)).FirstOrDefault();
            tx.Price = reg.Price;
            tx.Amount = Math.Round(qty * tx.Price,2);
            var gst = db.GSTRates.Where(x => x.Comodity.Equals("Stove")).FirstOrDefault();
            tx.CGST = Math.Round(tx.Amount * gst.CGST / 100,2);
            tx.SGST = Math.Round(tx.Amount * gst.SGST / 100,2);
            tx.Total = tx.Amount + tx.CGST + tx.SGST;
            int cmn = 0;
            if (cmno == 0)
                cmn = db.txStoves.Select(x => x.CashMemoNo).Max(m => m) + 1;
            else
                cmn = cmno;
            tx.CashMemoNo = cmn;
            db.txStoves.Add(tx);
            db.SaveChanges();
            using (LPGContext dbo = new LPGContext())
            {
                var result = dbo.Stoves.Where(x => x.type.Equals(prod)).FirstOrDefault();
                result.Quentity = result.Quentity - qty;
                dbo.SaveChanges();
            }
            return cmn;
        }
    }
}

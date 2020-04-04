using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using WcfService2.AzureModels;

namespace WcfService2.Synchronization
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Synchronization" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Synchronization.svc or Synchronization.svc.cs at the Solution Explorer and start debugging.
    public class Synchronization : ISynchronization
    {
        public void AddRecord(string tnm, string op, int rid)
        {
            using (LPGContext sync = new LPGContext())
            {
                Synchro s = new Synchro();
                s.TableName = tnm;
                s.Operation = op;
                s.RecordId = rid;
                sync.Synchros.Add(s);
                sync.SaveChanges();
            }
        }

        public void DeleteRecord(int Id)
        {
            using (LPGContext sync = new LPGContext())
            {
                var s = sync.Synchros.Where(x => x.Id == Id).FirstOrDefault();
                sync.Synchros.Remove(s);
                sync.SaveChanges();
            }
        }

        public List<Synchro> GetRecords()
        {
            using (LPGContext sync = new LPGContext())
            {
                var result = sync.Synchros.ToList();
                return result;
            }
        }

        public string SynchroToCentralDB()
        {
            //Thread.Sleep(15000);
            /*string connectionString = @"Data Source=smitlpg.database.windows.net;Initial Catalog=lpg;User ID=smit;Password=Ss@12345;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            */
            LPGContext db = new LPGContext();
            LPGDSCentralDBEntities1 le = new LPGDSCentralDBEntities1();
            List<Synchro> cs = db.Synchros.Where(x=>x.TableName.Equals("Customers")).ToList();
            foreach (var ts in cs)
            {
                if (ts.Operation.Equals("Update"))
                {
                    Customer tc = db.Customers.Where(x => x.CustomerId == ts.RecordId).FirstOrDefault();
                    WcfService2.AzureModels.Customer ac = le.Customers.Where(x => x.CustomerId == tc.CustomerId && x.DistributorCode == tc.DistributorCode).FirstOrDefault();
                    ac.AadharNo = tc.AadharNo;
                    ac.Address = tc.Address;
                    ac.BankAccountNo = tc.BankAccountNo;
                    ac.BankIFSC = tc.BankIFSC;
                    ac.City = tc.City;
                    ac.ContactNo = tc.ContactNo;
                    ac.CustomerId = tc.CustomerId;
                    ac.CustomerName = tc.CustomerName;
                    ac.CustomerType = tc.CustomerType;
                    ac.DistributorCode = tc.DistributorCode;
                    ac.District = tc.District;
                    ac.Email = tc.Email;
                    ac.Gender = tc.Gender;
                    ac.PinNo = tc.PinNo;
                    ac.RashanCardNo = tc.RashanCardNo;
                    ac.State = tc.State;
                    ac.Taluka = tc.Taluka;
                    le.SaveChanges();
                }
                else if (ts.Operation.Equals("Add"))
                {
                    Customer tc = db.Customers.Where(x => x.CustomerId == ts.RecordId).FirstOrDefault();
                    WcfService2.AzureModels.Customer ac = new AzureModels.Customer();
                    ac.AadharNo = tc.AadharNo;
                    ac.Address = tc.Address;
                    ac.BankAccountNo = tc.BankAccountNo;
                    ac.BankIFSC = tc.BankIFSC;
                    ac.City = tc.City;
                    ac.ContactNo = tc.ContactNo;
                    ac.CustomerId = tc.CustomerId;
                    ac.CustomerName = tc.CustomerName;
                    ac.CustomerType = tc.CustomerType;
                    ac.DistributorCode = tc.DistributorCode;
                    ac.District = tc.District;
                    ac.Email = tc.Email;
                    ac.Gender = tc.Gender;
                    ac.PinNo = tc.PinNo;
                    ac.RashanCardNo = tc.RashanCardNo;
                    ac.State = tc.State;
                    ac.Taluka = tc.Taluka;
                    le.Customers.Add(ac);
                    le.SaveChanges();
                }
                else if (ts.Operation.Substring(0,6).Equals("Delete"))
                {
                    int dcode = int.Parse(ts.Operation.Substring(6, 6));
                    WcfService2.AzureModels.Customer ac = le.Customers.Where(x => x.CustomerId == ts.RecordId && x.DistributorCode == dcode).FirstOrDefault();
                    le.Customers.Remove(ac);
                    le.SaveChanges();
                }
                DeleteRecord(ts.Id);
            }

            List<Synchro> tcs = db.Synchros.Where(x => x.TableName.Equals("TxCylinders")).ToList();
            foreach (var ts in tcs)
            {
                if (ts.Operation.Equals("Add"))
                {
                    TxCylinder tc = db.txCylinders.Where(x => x.TxId == ts.RecordId).FirstOrDefault();
                    WcfService2.AzureModels.TxCylinder atc = new AzureModels.TxCylinder();
                    atc.TxId = tc.TxId;
                    atc.CustomerId = tc.CustomerId;
                    atc.DistributorCode = db.DistributorUsers.FirstOrDefault().DistributorCode;
                    atc.CustomerName = tc.CustomerName;
                    atc.TxDate = tc.TxDate;
                    atc.CylinderDetails = tc.CylinderDetails;
                    atc.Quentity = tc.Quentity;
                    atc.Price = tc.Price;
                    atc.Amount = tc.Amount;
                    atc.CGST = tc.CGST;
                    atc.SGST = tc.SGST;
                    atc.Total = tc.Total;
                    atc.CashMemoNo = tc.CashMemoNo;
                    le.TxCylinders.Add(atc);
                    le.SaveChanges();
                }
                DeleteRecord(ts.Id);
            }

            List<Cylinder> stock = db.Cylinders.ToList();
            int discode = db.DistributorUsers.FirstOrDefault().DistributorCode;
            WcfService2.AzureModels.Stock ast = le.Stocks.Where(x => x.DistributorCode == discode).FirstOrDefault();
            ast.DistributorCode = db.DistributorUsers.FirstOrDefault().DistributorCode;
            ast.C14_2_KG_With_Subsidy__E_ = stock.Where(x => x.type.Equals("14.2 KG With Subsidy (E)")).FirstOrDefault().Quentity;
            ast.C14_2_KG_With_Subsidy__F_ = stock.Where(x => x.type.Equals("14.2 KG With Subsidy (F)")).FirstOrDefault().Quentity;
            ast.C14_2_KG_Without_Subsidy__E_ = stock.Where(x => x.type.Equals("14.2 KG Without Subsidy (E)")).FirstOrDefault().Quentity; 
            ast.C14_2_KG_Without_Subsidy__F_ = stock.Where(x => x.type.Equals("14.2 KG Without Subsidy (F)")).FirstOrDefault().Quentity; 
            ast.C19_KG_Industrial__E_ = stock.Where(x => x.type.Equals("19 KG Industrial (E)")).FirstOrDefault().Quentity;
            ast.C19_KG_Industrial__F_ = stock.Where(x => x.type.Equals("19 KG Industrial (F)")).FirstOrDefault().Quentity;
            ast.C5_KG__E_ = stock.Where(x => x.type.Equals("5 KG (E)")).FirstOrDefault().Quentity;
            ast.C5_KG__F_ = stock.Where(x => x.type.Equals("5 KG (F)")).FirstOrDefault().Quentity;
            le.SaveChanges();

            var res=db.Synchros.Where(x => x.TableName.Equals("Synchroes")).FirstOrDefault();
            res.Operation = DateTime.Now.ToString();
            db.SaveChanges();
            return "OK";
        }
    }
}

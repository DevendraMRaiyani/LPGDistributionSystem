using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "StockMgnt" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select StockMgnt.svc or StockMgnt.svc.cs at the Solution Explorer and start debugging.
    public class StockMgnt : IStockMgnt
    {
        public List<Cylinder> GetCylinders()
        {
            using (LPGContext db = new LPGContext())
            {
                return db.Cylinders.Select(x => x).ToList();
            }
        }

        public Stove GetRegulators()
        {
            using (LPGContext db = new LPGContext())
            {
                return db.Stoves.Where(x => x.type=="Regulator").FirstOrDefault();
            }
        }

        public List<Stove> GetStoves()
        {
            using (LPGContext db = new LPGContext())
            {
                return db.Stoves.Where(x=>x.type!="Regulator").Select(x => x).ToList();
            }
        }

        public string SetFCylinders(string a, int c)
        {
            using (LPGContext db = new LPGContext())
            {
                var r = db.Cylinders.ToList();
                //ra.Quentity += c;
                r.Where(x => x.type == a).ToList().ForEach(s => s.Quentity = s.Quentity + c);
                db.SaveChanges();
            }
            return "OKF";
        }
        public string SetECylinders(string a, int c)
        {
            using (LPGContext db = new LPGContext())
            {
                var r = db.Cylinders.ToList();
                r.Where(x => x.type == a).ToList().ForEach(s => s.Quentity = s.Quentity - c);
                db.SaveChanges();
            }
            return "OKE";
        }

        public string SetRegulators(int a)
        {
            using (LPGContext db = new LPGContext())
            {
                var r = db.Stoves.Where(x=>x.type=="Regulator").ToList();
                r.ForEach(s => s.Quentity = a);
                db.SaveChanges();
            }
            return "OK";
        }

        public string AddStove(Stove s)
        {
            using (LPGContext db = new LPGContext())
            {
                db.Stoves.Add(s);
                db.SaveChanges();
                return "OK";
            }
        }

        public string RemoveStove(string s)
        {
            using (LPGContext db = new LPGContext())
            {
                var r = db.Stoves.Where(x=>x.type==s).FirstOrDefault();
                db.Stoves.Remove(r);
                db.SaveChanges();
                return "OK";
            }
        }

        public string SetStoves(string a, int c)
        {
            using (LPGContext db = new LPGContext())
            {
                var r = db.Stoves.Where(x => x.type == a).FirstOrDefault();
                r.Quentity = c;
                db.SaveChanges();
                return "OK";
            }
        }

        public string SetRegPrice(double a)
        {
            using (LPGContext db = new LPGContext())
            {
                var r = db.Stoves.Where(x => x.type == "Regulator").ToList();
                r.ForEach(s => s.Price = Math.Round(a,2));
                db.SaveChanges();
            }
            return "OK";
        }

        public string SetCylPrice(string s, double a)
        {
            using (LPGContext db = new LPGContext())
            {
                var r = db.Cylinders.ToList();
                r.Where(x => x.type == s).ToList().ForEach(x => x.Price = Math.Round(a, 2));
                db.SaveChanges();
            }
            return "OK";
        }

        public string SetStovePrice(string s, double a)
        {
            using (LPGContext db = new LPGContext())
            {
                var r = db.Stoves.Where(x => x.type == s).FirstOrDefault();
                r.Price = Math.Round(a, 2);
                db.SaveChanges();
                return "OK";
            }
        }
    }
}

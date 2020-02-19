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
        LPGContext db = new LPGContext();
        
        public List<Cylinder> GetCylinders()
        {
             return db.Cylinders.Select(x => x).ToList();
        }

        public List<Regulator> GetRegulators()
        {
            return db.Regulators.Select(x => x).ToList();
        }

        public List<Stove> GetStoves()
        {
            return db.Stoves.Select(x => x).ToList();
        }

        public string SetCylinders(List<Cylinder> cylinders)
        {
            var result = db.Cylinders.Select(x => x).ToList();
             result = cylinders;
            db.SaveChanges();
            
            return "Ok Done";
        }
    }
}

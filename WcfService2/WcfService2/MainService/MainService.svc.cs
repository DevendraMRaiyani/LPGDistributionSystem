using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MainService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MainService.svc or MainService.svc.cs at the Solution Explorer and start debugging.
    public class MainService : IMainService
    {
        LPGContext db = new LPGContext();
        public DistributorUser GetDistributor()
        {
            return db.DistributorUsers.FirstOrDefault();
        }

        public string GetPassword(int dcode)
        {
            return db.DistributorUsers.Where(x => x.DistributorCode == dcode).Select(s => s.Password).FirstOrDefault();
        }

        public DistributorUser Login(string unm, string pass)
        {
            DistributorUser duser = null;
            duser = db.DistributorUsers.Where(x => x.Username.Equals(unm) && x.Password.Equals(pass)).FirstOrDefault();
            return duser;
        }

        public string SetPassword(string newpass,int dcode)
        {
            var result = db.DistributorUsers.Where(x => x.DistributorCode == dcode).FirstOrDefault();
            result.Password = newpass;
            db.SaveChanges();
            return "OK";
        }
    }
}

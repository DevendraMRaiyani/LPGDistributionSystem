using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPG_Distribution_System
{
    public class Distributor
    {
        static MainService.MainServiceClient client = new MainService.MainServiceClient();
        static MainService.DistributorUser dist = client.GetDistributor();
        public int code = dist.DistributorCode;
        public string AgencyName = dist.AgencyName;
        public string DistributorName = dist.DistributorName;
        public string PANnumber = dist.PANnumber;
        public string GSTIN = dist.GSTIN;
        public string Address = dist.Address;
        public string City = dist.City;
        public string Taluka = dist.Taluka;
        public string District = dist.District;
        public string State = dist.State;
        public string ContectNo = dist.ContectNo;
        public string Email = dist.Email;
    }
}

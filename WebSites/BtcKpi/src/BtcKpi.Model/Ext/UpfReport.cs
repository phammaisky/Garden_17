using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Model
{
    public partial class UpfReport
    {
        public Nullable<int> UserID { get; set; }
        public string FullName { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string CompanyName { get; set; }
        public Nullable<int> AdministratorshipID { get; set; }
        public string AdministratorshipName { get; set; }
        public Nullable<byte> ScheduleType { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<byte> ScheduleID { get; set; }
        public string ScheduleName { get; set; }
        public Nullable<int> SelfWeight { get; set; }
        public Nullable<decimal> SelfScore { get; set; }
        public Nullable<int> DependWeight { get; set; }
        public Nullable<decimal> DependScore { get; set; }
        public List<DepartmentCharge> DepartmentCharges { get; set; }

    }
}

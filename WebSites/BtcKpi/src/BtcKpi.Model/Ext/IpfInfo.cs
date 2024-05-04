using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Model
{
    public partial class IpfInfo 
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string CompanyName { get; set; }
        public Nullable<int> AdministratorshipID { get; set; }
        public string AdministratorshipName { get; set; }

        public int ID { get; set; }
        public Nullable<byte> ScheduleType { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> ScheduleID { get; set; }
        public string ScheduleName { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<decimal> WorkScore { get; set; }
        public Nullable<decimal> CompetencyScore { get; set; }
        public Nullable<decimal> TotalScore { get; set; }
        public Nullable<System.DateTime> Approved { get; set; }
        public Nullable<int> ApproveBy { get; set; }
        public Nullable<int> BodId { get; set; }
        public Nullable<decimal> BodScore { get; set; }
    }
}

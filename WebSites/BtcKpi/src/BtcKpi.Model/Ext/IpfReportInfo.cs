using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Model
{
    public partial class IpfReportInfo
    {
        [Key]
        public int ID { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<decimal> BodScore { get; set; }
        public string BodComment { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<byte> DeleteFlg { get; set; }
        public Nullable<System.DateTime> Deleted { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public List<IpfScore> IpfScores { get; set; }
        public string FullName { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string CompanyName { get; set; }
        public Nullable<int> AdministratorshipID { get; set; }
        public string AdministratorshipName { get; set; }
        public Nullable<bool> IsDeparmentRow { get; set; }

        public Nullable<decimal> AverageScore { get; set; }
        public string AverageRank { get; set; }
        public string UserCode { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtcKpi.Model
{
    public partial class DepartmentInfo
    {
        public int UserID { get; set; }
        public string PersonInCharge { get; set; }
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
        public Nullable<int> StatusID { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        [NotMapped]
        public string StatusName
        {
            get
            {
                string statusName = "";
                if (StatusID == 0)
                {
                    statusName = "Lưu nháp";
                }
                else if (StatusID == 1)
                {
                    statusName = "Chưa duyệt";
                } else if (StatusID == 2)
                {
                    statusName = "QL Đã duyệt";
                } else if(StatusID == 3)
                {
                    statusName = "BOD Đã duyệt";
                } else if(StatusID == 4)
                {
                    statusName = "Từ chối";
                }

                return statusName;
            }
            set { StatusName = value; }
        }
        public Nullable<decimal> SelfAssessScore { get; set; }
        public Nullable<decimal> AssessedScore { get; set; }
        public Nullable<int> ApproveBy { get; set; }
        public Nullable<int> BodApproved { get; set; }
        public Nullable<System.DateTime> Approved { get; set; }
        public Nullable<decimal> TotalBODPoint { get; set; }
        public Nullable<decimal> January { get; set; }
        public Nullable<decimal> February { get; set; }
        public Nullable<decimal> March { get; set; }
        public Nullable<decimal> April { get; set; }
        public Nullable<decimal> May { get; set; }
        public Nullable<decimal> June { get; set; }
        public Nullable<decimal> July { get; set; }
        public Nullable<decimal> August { get; set; }
        public Nullable<decimal> September { get; set; }
        public Nullable<decimal> October { get; set; }
        public Nullable<decimal> November { get; set; }
        public Nullable<decimal> December { get; set; }
        public Nullable<decimal> EndOfYear { get; set; }
        public Nullable<decimal> AveragePoint { get; set; }
        public Nullable<decimal> BODSumPoint { get; set; }
        public string BODComment { get; set; }

    }
}

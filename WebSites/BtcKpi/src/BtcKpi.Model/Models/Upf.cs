using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtcKpi.Model
{
    public partial class Upf
    {
        [Key]
        public int ID { get; set; }
        public Nullable<byte> ScheduleType { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> ScheduleID { get; set; }
        public Nullable<int> PersChargID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<byte> DeleteFlg { get; set; }
        public Nullable<System.DateTime> Deleted { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public string OutsAchiev { get; set; }
        public Nullable<int> SelfRating { get; set; }
        public Nullable<decimal> TotalPoint { get; set; }
        public Nullable<decimal> TotalManagePoint { get; set; }
        [NotMapped]
        public bool CheckExistsDepartment { get; set; }
        [NotMapped]
        public string SelfRatingName { get; set; }
        public Nullable<System.DateTime> Approved { get; set; }
        public Nullable<int> ApproveBy { get; set; }
        public Nullable<int> DepartmentID { get; set; }

        [NotMapped]
        public int WeightTotal { get; set; }
        public Nullable<int> BodApproved { get; set; }
        public Nullable<decimal> TotalBODPoint { get; set; }
        [NotMapped]
        public string ScheduleName { get; set; }
        [NotMapped]
        public UpfSummary UpfSummary { get; set; }
    }
}
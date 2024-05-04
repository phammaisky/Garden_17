using System;
using System.ComponentModel.DataAnnotations;

namespace BtcKpi.Model
{
    public partial class UpfHis
    {
        [Key]
        public int ID { get; set; }
        public int upfID { get; set; }
        public Nullable<byte> Action { get; set; }
        public string Descriptions { get; set; }
        public Nullable<byte> ScheduleType { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> ScheduleID { get; set; }
        public Nullable<int> PersChargID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<System.DateTime> Updated { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string OutsAchiev { get; set; }
        public Nullable<int> SelfRating { get; set; }
        public Nullable<decimal> TotalPoint { get; set; }
        public Nullable<decimal> TotalManagePoint { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtcKpi.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class UpfNameDetail
    {
        [Key]
        public int ID { get; set; }
        public Nullable<int> WorkCompleteID { get; set; }
        public int UpfID { get; set; }
        public string NameKPI { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<byte> DeleteFlg { get; set; }
        public Nullable<System.DateTime> Deleted { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<byte> Order { get; set; }
        [NotMapped]
        public List<UpfJobDetail> JobDetails { get; set; }
        [NotMapped]
        public string OutsAchiev { get; set; }
        [NotMapped]
        public Nullable<int> SelfRating { get; set; }
    }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace BtcKpi.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class UpfPersRewProposal
    {
        [Key]
        public int ID { get; set; }
        public int UpfID { get; set; }
        public string EmployeeName { get; set; }
        public string PersOutsAchiev { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<byte> DeleteFlg { get; set; }
        public Nullable<System.DateTime> Deleted { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<byte> Order { get; set; }
        [NotMapped]
        public string OutsAchiev { get; set; }
        [NotMapped]
        public Nullable<int> SelfRating { get; set; }
    }
}
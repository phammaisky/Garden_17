//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BtcKpi.Model
{
    using System;
	using System.ComponentModel.DataAnnotations;
    
    public partial class zPersonalPlan
    {
		[Key]
        public int HistoryID { get; set; }
        public int ID { get; set; }
        public Nullable<int> IpfID { get; set; }
        public Nullable<byte> Type { get; set; }
        public Nullable<byte> Seq { get; set; }
        public string Activity { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public string Remark { get; set; }
        public Nullable<byte> Term { get; set; }
        public string WishesOfStaff { get; set; }
        public string RequestOfManager { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<byte> DeleteFlg { get; set; }
        public Nullable<System.DateTime> Deleted { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<System.DateTime> Updated { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}

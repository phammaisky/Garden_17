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
    
    public partial class zIpfDetail
    {
		[Key]
        public int HistoryID { get; set; }
        public int ID { get; set; }
        public int IpfID { get; set; }
        public Nullable<byte> WorkType { get; set; }
        public Nullable<int> WorkCompleteID { get; set; }
        public string Objective { get; set; }
        public Nullable<byte> Seq { get; set; }
        public string Target { get; set; }
        public Nullable<byte> Weight { get; set; }
        public string Result { get; set; }
        public Nullable<decimal> SelfScore { get; set; }
        public Nullable<decimal> ManagerScore { get; set; }
        public Nullable<decimal> TotalScore { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<byte> DeleteFlg { get; set; }
        public Nullable<System.DateTime> Deleted { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<byte> NextYearFlg { get; set; }
        public Nullable<System.DateTime> Updated { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GardenLover.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class ckJobDetail
    {
        public long Id { get; set; }
        public Nullable<long> CorporationId { get; set; }
        public Nullable<long> CompanyId { get; set; }
        public Nullable<long> BranchId { get; set; }
        public Nullable<long> ReportId { get; set; }
        public Nullable<long> ADepartmentId { get; set; }
        public Nullable<long> JobId { get; set; }
        public string JobDetailName { get; set; }
        public Nullable<long> Seq { get; set; }
        public string AWantDoneTime { get; set; }
        public string AWantResult { get; set; }
        public Nullable<long> APercent { get; set; }
        public Nullable<long> BDepartmentId { get; set; }
        public string ABDoneTime { get; set; }
        public string ABResult { get; set; }
        public Nullable<decimal> ABMark { get; set; }
        public string BPlan { get; set; }
        public string BResultDetail { get; set; }
        public string BSolution { get; set; }
        public string BDoneTime { get; set; }
        public Nullable<long> BPercent { get; set; }
        public Nullable<decimal> BMark { get; set; }
        public Nullable<decimal> CMark { get; set; }
        public Nullable<decimal> TotalMark { get; set; }
    
        public virtual ckJob ckJob { get; set; }
        public virtual ckReport ckReport { get; set; }
    }
}

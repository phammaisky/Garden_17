//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IQWebApp_Blank.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class kNextPlanDetail
    {
        public long Id { get; set; }
        public Nullable<long> CorporationId { get; set; }
        public Nullable<long> CompanyId { get; set; }
        public Nullable<long> BranchId { get; set; }
        public Nullable<long> ReportId { get; set; }
        public Nullable<long> NextPlanId { get; set; }
        public string NextPlanDetailName { get; set; }
        public Nullable<long> Seq { get; set; }
    
        public virtual kNextPlan kNextPlan { get; set; }
        public virtual kReport kReport { get; set; }
    }
}

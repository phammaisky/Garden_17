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
    
    public partial class kStep
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public kStep()
        {
            this.kReports = new HashSet<kReport>();
            this.kReport_History = new HashSet<kReport_History>();
        }
    
        public long Id { get; set; }
        public long CorporationId { get; set; }
        public long CompanyId { get; set; }
        public long BranchId { get; set; }
        public long StepRelationTypeId { get; set; }
        public string StepRelationValue { get; set; }
        public long StepForAllId { get; set; }
        public long SendToRelationTypeId { get; set; }
        public string SendToRelationValue { get; set; }
    
        public virtual kRelationTypeForAll kRelationTypeForAll { get; set; }
        public virtual kRelationTypeForAll kRelationTypeForAll1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kReport> kReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kReport_History> kReport_History { get; set; }
        public virtual kStepForAll kStepForAll { get; set; }
    }
}

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
    
    public partial class kRelationTypeForAll
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public kRelationTypeForAll()
        {
            this.kCompeForSomes = new HashSet<kCompeForSome>();
            this.kRelationTypes = new HashSet<kRelationType>();
            this.kRelationTypes1 = new HashSet<kRelationType>();
            this.kReport_History = new HashSet<kReport_History>();
            this.kReports = new HashSet<kReport>();
            this.kSteps = new HashSet<kStep>();
            this.kSteps1 = new HashSet<kStep>();
        }
    
        public long Id { get; set; }
        public string RelationTypeName { get; set; }
        public bool IsDefaultStepRelationType { get; set; }
        public bool IsDefaultCompeRelationType { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kCompeForSome> kCompeForSomes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kRelationType> kRelationTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kRelationType> kRelationTypes1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kReport_History> kReport_History { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kReport> kReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kStep> kSteps { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kStep> kSteps1 { get; set; }
    }
}

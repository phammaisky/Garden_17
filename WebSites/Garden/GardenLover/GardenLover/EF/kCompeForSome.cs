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
    
    public partial class kCompeForSome
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public kCompeForSome()
        {
            this.kCompes = new HashSet<kCompe>();
        }
    
        public long Id { get; set; }
        public long CorporationId { get; set; }
        public long CompanyId { get; set; }
        public long BranchId { get; set; }
        public long CompeRelationTypeId { get; set; }
        public string CompeRelationValue { get; set; }
        public long CompeForAllId { get; set; }
        public long Percent { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kCompe> kCompes { get; set; }
        public virtual kCompeForAll kCompeForAll { get; set; }
        public virtual kRelationTypeForAll kRelationTypeForAll { get; set; }
    }
}
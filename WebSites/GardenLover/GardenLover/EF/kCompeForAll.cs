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
    
    public partial class kCompeForAll
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public kCompeForAll()
        {
            this.kCompeForSomes = new HashSet<kCompeForSome>();
        }
    
        public long Id { get; set; }
        public Nullable<long> CorporationId { get; set; }
        public Nullable<long> CompanyId { get; set; }
        public Nullable<long> BranchId { get; set; }
        public string CompeName { get; set; }
        public long Percent { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kCompeForSome> kCompeForSomes { get; set; }
    }
}

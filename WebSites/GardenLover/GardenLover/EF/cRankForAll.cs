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
    
    public partial class cRankForAll
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cRankForAll()
        {
            this.cRanks = new HashSet<cRank>();
        }
    
        public long Id { get; set; }
        public Nullable<long> CorporationId { get; set; }
        public Nullable<long> CompanyId { get; set; }
        public Nullable<long> BranchId { get; set; }
        public string RankName { get; set; }
        public Nullable<long> Seq { get; set; }
        public decimal xKpiJob { get; set; }
        public decimal xKpiCompe { get; set; }
        public bool CanBeManager { get; set; }
        public bool AutoCreate { get; set; }
    
        public virtual cBranch cBranch { get; set; }
        public virtual cCompany cCompany { get; set; }
        public virtual cCorporation cCorporation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cRank> cRanks { get; set; }
    }
}

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
    
    public partial class aRoleForSomeAllow
    {
        public long Id { get; set; }
        public Nullable<long> RoleForSomeId { get; set; }
        public Nullable<long> RoleForAllId { get; set; }
        public Nullable<long> AllowRelationTypeId { get; set; }
        public string AllowRelationValue { get; set; }
        public Nullable<long> CorporationId { get; set; }
        public Nullable<long> CompanyId { get; set; }
        public Nullable<long> BranchId { get; set; }
    
        public virtual aRoleForAll aRoleForAll { get; set; }
        public virtual aRoleForSome aRoleForSome { get; set; }
    }
}

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
    
    public partial class aRole
    {
        public long Id { get; set; }
        public Nullable<long> CorporationId { get; set; }
        public Nullable<long> CompanyId { get; set; }
        public Nullable<long> BranchId { get; set; }
        public Nullable<long> AppId { get; set; }
        public Nullable<long> FunctionId { get; set; }
        public long RoleRelationTypeId { get; set; }
        public string RoleRelationValue { get; set; }
        public long RoleForSomeId { get; set; }
    
        public virtual aApp aApp { get; set; }
        public virtual aAppFunction aAppFunction { get; set; }
        public virtual aRelationTypeForAll aRelationTypeForAll { get; set; }
        public virtual aRoleForSome aRoleForSome { get; set; }
    }
}

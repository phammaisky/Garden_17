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
    
    public partial class vUserInfo
    {
        public System.Guid UserId { get; set; }
        public System.Guid UserIdd { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Address { get; set; }
        public string ID { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public long CountryId { get; set; }
        public long CorporationId { get; set; }
        public long CompanyId { get; set; }
        public long BranchId { get; set; }
        public long DepartmentId { get; set; }
        public long RankId { get; set; }
        public long GradeId { get; set; }
        public Nullable<System.Guid> ManagerId { get; set; }
        public Nullable<System.DateTime> StartWorkingDate { get; set; }
        public Nullable<System.DateTime> StartCurrentJobDate { get; set; }
    }
}

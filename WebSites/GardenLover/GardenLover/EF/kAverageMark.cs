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
    
    public partial class kAverageMark
    {
        public long Id { get; set; }
        public long CorporationId { get; set; }
        public long CompanyId { get; set; }
        public long BranchId { get; set; }
        public System.Guid UserId { get; set; }
        public long Year { get; set; }
        public decimal AverageValue { get; set; }
        public long RateId { get; set; }
    
        public virtual kRate kRate { get; set; }
    }
}

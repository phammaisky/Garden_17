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
    
    public partial class vBranch
    {
        public long Id { get; set; }
        public Nullable<long> ParentId { get; set; }
        public long CompanyId { get; set; }
        public string BranchName { get; set; }
        public Nullable<long> Seq { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GardenCrm.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User_Dept_Function
    {
        public int UserId { get; set; }
        public int DeptId { get; set; }
        public int FunctionId { get; set; }
        public int Extention { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual Function Function { get; set; }
        public virtual UserInfo UserInfo { get; set; }
    }
}

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
    
    public partial class aApp
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public aApp()
        {
            this.aAppFunctions = new HashSet<aAppFunction>();
            this.aRoles = new HashSet<aRole>();
            this.aRoleForAlls = new HashSet<aRoleForAll>();
        }
    
        public long Id { get; set; }
        public Nullable<long> ParentId { get; set; }
        public string AppName { get; set; }
        public Nullable<long> Seq { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<aAppFunction> aAppFunctions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<aRole> aRoles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<aRoleForAll> aRoleForAlls { get; set; }
    }
}

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
    
    public partial class sCountry
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sCountry()
        {
            this.cUserInfoes = new HashSet<cUserInfo>();
        }
    
        public long Id { get; set; }
        public string CountryName { get; set; }
        public Nullable<long> Seq { get; set; }
        public Nullable<long> LanguageId { get; set; }
        public Nullable<long> CurrencyId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cUserInfo> cUserInfoes { get; set; }
        public virtual sCurrency sCurrency { get; set; }
        public virtual sLanguage sLanguage { get; set; }
    }
}

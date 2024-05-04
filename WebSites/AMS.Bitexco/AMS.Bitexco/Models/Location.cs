namespace AMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using AMS.Resource;

    [Table("Location")]
    public partial class Location
    {
        public Location()
        {
            HistoryUses = new HashSet<HistoryUse>();
            LocationChild = new HashSet<Location>();
        }

        public int Id { get; set; }

        [Display(Name = "Thuộc địa điểm")]
        public int? ParentId { get; set; }
                
        [StringLength(50, ErrorMessageResourceName = "StringLeng", ErrorMessageResourceType = typeof(Multi))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Multi))]
        [Display(Name = "Tên viết tắt")]
        public string ShortName { get; set; }

        [StringLength(200, ErrorMessageResourceName = "StringLeng", ErrorMessageResourceType = typeof(Multi))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Multi))]
        [Display(Name = "Tên địa điểm, phòng ban")]
        public string LocationName { get; set; }

        [StringLength(500, ErrorMessageResourceName = "StringLeng", ErrorMessageResourceType = typeof(Multi))]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        public virtual ICollection<HistoryUse> HistoryUses { get; set; }

        public virtual ICollection<Location> LocationChild { get; set; }

        public virtual Location ParentLocation { get; set; }
    }
}

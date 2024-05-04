namespace AMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using AMS.Resource;

    [Table("DeviceCategory")]
    public partial class DeviceCategory
    {
        public DeviceCategory()
        {
            DeviceAndTools = new HashSet<DeviceAndTool>();
        }
        public enum TypeDevice
        {
            VP, IT
        }
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Multi))]
        [StringLength(200, ErrorMessageResourceName = "StringLeng", ErrorMessageResourceType = typeof(Multi))]
        [Display(Name = "Tên loại thiết bị")]
        public string DeviceCatName { get; set; }
        
        [StringLength(500, ErrorMessageResourceName = "StringLeng", ErrorMessageResourceType = typeof(Multi))]
        [Display(Name = "Mô tả loại thiết bị")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Loại CNTT/VP")]
        public TypeDevice Type { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<DeviceAndTool> DeviceAndTools { get; set; }
    }
}

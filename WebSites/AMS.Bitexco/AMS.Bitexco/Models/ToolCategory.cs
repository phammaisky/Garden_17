namespace AMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using AMS.Resource;

    [Table("ToolCategory")]
    public partial class ToolCategory
    {
        public ToolCategory()
        {
            DeviceAndTools = new HashSet<DeviceAndTool>();
        }

        public int Id { get; set; }

        
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Multi))]
        [StringLength(200, ErrorMessageResourceName = "StringLeng", ErrorMessageResourceType = typeof(Multi))]
        [Display(Name = "Tên loại công cụ")]
        public string ToolCatName { get; set; }

        [StringLength(500, ErrorMessageResourceName = "StringLeng", ErrorMessageResourceType = typeof(Multi))]
        [Display(Name = "Mô tả")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<DeviceAndTool> DeviceAndTools { get; set; }
    }
}

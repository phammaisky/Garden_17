namespace AMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using AMS.Resource;

    [Table("DeviceAndTool")]
    public partial class DeviceAndTool
    {
        public DeviceAndTool()
        {
            HistoryUses = new HashSet<HistoryUse>();
        }

        public int Id { get; set; }

        [Display(Name = "Tài sản thuộc CT")]
        public int CompanyId { get; set; }

        [Display(Name = "Loại thiết bị")]
        public int? DeviceCatId { get; set; }

        [Display(Name = "Loại công cụ")]
        public int? ToolCatId { get; set; }

        [Display(Name = "Mã tài sản")]
        [StringLength(50, ErrorMessageResourceName = "StringLeng", ErrorMessageResourceType = typeof(Multi))]
        public string AssetsCode { get; set; }

        [Display(Name = "Tên tài sản")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Multi))]
        [StringLength(200, ErrorMessageResourceName = "StringLeng", ErrorMessageResourceType = typeof(Multi))]
        public string DeviceName { get; set; }

        [StringLength(2000, ErrorMessageResourceName = "StringLeng", ErrorMessageResourceType = typeof(Multi))]
        [Display(Name = "Mô tả tài sản")]
        [DataType(DataType.MultilineText)]
        public string DescriptionDevice { get; set; }

        [Display(Name = "Ngày mua")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BuyDate { get; set; }

        public int? Type { get; set; }

        [Display(Name = "Người nhập")]
        public int CreateById { get; set; }

        [Display(Name = "Ngày nhập")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime CreateDate { get; set; }

        public int? EditedById { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? EditedDate { get; set; }

        public int? DestroyById { get; set; }

        [Display(Name = "Ngày kiểm kê")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? CheckedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? DestroyDate { get; set; }

        public virtual Company Company { get; set; }

        public virtual DeviceCategory DeviceCategory { get; set; }

        public virtual ToolCategory ToolCategory { get; set; }

        public virtual ICollection<HistoryUse> HistoryUses { get; set; }

    }
}

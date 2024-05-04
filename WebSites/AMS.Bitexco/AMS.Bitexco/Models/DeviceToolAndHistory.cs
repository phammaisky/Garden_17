using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using AMS.Resource;

namespace AMS.Models
{
    public class DeviceToolAndHistory
    {
        public int Id { get; set; }
        [Display(Name = "Tài sản thuộc CT")]
        public int CompanyId { get; set; }

        [Display(Name = "Loại thiết bị")]
        public int? DeviceCatId { get; set; }

        [Display(Name = "Loại công cụ")]
        public int? ToolCatId { get; set; }

        [Display(Name = "Mã tự động")]
        public bool AutoAssetsCode { get; set; }

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
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Multi))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BuyDate { get; set; }

        [Display(Name = "Người sử dụng")]
        public int? StaffId { get; set; }

        [Display(Name = "Phòng, ban")]
        public int? DeptId { get; set; }

        [Display(Name = "Địa điểm")]
        public int? LocationId { get; set; }

        [Display(Name = "Tình trạng")]
        public int StatusId { get; set; }

        [Display(Name = "Mô tả chi tiết tình trạng")]
        [DataType(DataType.MultilineText)]
        public string StatusDescription { get; set; }

        [Display(Name = "Ngày bàn giao")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Multi))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime HandedDate { get; set; }
    }
}
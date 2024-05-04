namespace AMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using AMS.Resource;

    [Table("HistoryUse")]
    public partial class HistoryUse
    {   
        public int Id { get; set; }

        [Display(Name = "Loại công cụ thiết bị")]
        public int DeviceToolId { get; set; }

        [Display(Name = "Người nhận bàn giao")]
        public int? HandedToStaffId { get; set; }

        [Display(Name = "Ngày bàn giao, thanh lý")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime HandedDate { get; set; }

        [Display(Name = "Phòng ban")]
        public int? DeptId { get; set; }

        [Display(Name = "Địa điểm")]
        public int? LocationId { get; set; }

        [Display(Name = "Tình trạng")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Multi))]
        public int StatusId { get; set; }

        [StringLength(2000)]
        [Display(Name = "Mô tả tình trạng")]
        [DataType(DataType.MultilineText)]
        public string StatusDescrition { get; set; }

        public virtual Department Department { get; set; }

        public virtual DeviceAndTool DeviceAndTool { get; set; }

        public virtual Location Location { get; set; }

        public virtual UserInfo Staff { get; set; }

        public virtual StatusCategory StatusCategory { get; set; }
      
    }
}

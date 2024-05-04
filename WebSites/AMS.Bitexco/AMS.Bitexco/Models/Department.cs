namespace AMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Department
    {
        public Department()
        {
            UserInfoes = new HashSet<UserInfo>();
            
            HistoryUses = new HashSet<HistoryUse>();
        }

        public int Id { get; set; }

         [Display(Name = "Công ty")]
        public int CompanyId { get; set; }

        [StringLength(50)]
        [Display(Name = "Mã phòng ban")]
        public string DeptCode { get; set; }

        [StringLength(50)]
        [Display(Name = "Tên phòng ban")]
        public string DeptName { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Mô tả")]
        public string DeptDesc { get; set; }

        public bool? Active { get; set; }

        public int? DeptOrder { get; set; }

        [Display(Name = "Công ty")]
        public virtual Company Company { get; set; }
        public virtual ICollection<UserInfo> UserInfoes { get; set; }
        public virtual ICollection<HistoryUse> HistoryUses { get; set; }
    }
}

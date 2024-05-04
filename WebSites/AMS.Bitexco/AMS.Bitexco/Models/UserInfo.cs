namespace AMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using AMS.Resource;

    [Table("UserInfo")]
    public partial class UserInfo
    {
        public UserInfo()
        {
            GroupUsers = new HashSet<GroupUser>();
            Companies = new HashSet<Company>();
            HistoryUses = new HashSet<HistoryUse>();
        }
        public int Id { get; set; }

        [Display(Name = "Phòng ban")]
        public int? DeptId { get; set; }

        [StringLength(200)]
        [Display(Name = "Tên tài khoản")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Multi))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Multi))]
        [StringLength(200)]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        
        [StringLength(200)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [StringLength(100)]
        public string Phone { get; set; }

        public byte[] ImageSignature { get; set; }
        [Display(Name = "Đã kích hoạt")]
        public bool Active { get; set; }
        [Display(Name = "Bị khóa")]
        public bool IsLock { get; set; }

        public virtual Department Department { get; set; }

        [Display(Name = "Nhóm người dùng")]
        public virtual ICollection<GroupUser> GroupUsers { get; set; }

        [Display(Name = "Quyền quản lý công ty")]
        public virtual ICollection<Company> Companies { get; set; }

        public virtual ICollection<HistoryUse> HistoryUses { get; set; }

    }
}

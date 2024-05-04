namespace BtcKpi.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class UserInfo
    {
        
        [Key]
        public int Id { get; set; }

        [Display(Name = "Phòng ban/ Departement")]
        public int? DeptId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Tên tài khoản/ Account")]
        public string UserName { get; set; }

        [Display(Name = "Chức danh/ Title")]
        public int? TitleId { get; set; }

        [StringLength(200)]
        [Display(Name = "Họ tên/ Fullname")]
        [Required(ErrorMessage="Họ tên là bắt buộc phải nhập/ FullName is required")]
        public string FullName { get; set; }

        [StringLength(200)]
        public string FullNameEn { get; set; }

        [StringLength(2000)]
        [Display(Name = "Email/ Email")]
        //[Required(ErrorMessage = "Email là bắt buộc phải nhập/ Email is required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email không hợp lệ/ Email incorect")]
        public string Email { get; set; }

        [StringLength(100)]
        [Display(Name = "Số đt/ Phone")]
        public string Phone { get; set; }

        [StringLength(50)]
        [Display(Name = "Số máy lẻ/ Bitexco phone extension")]
        public string BitexcoPhoneExt { get; set; }

        [Display(Name = "Chữ ký/ Signature")]
        public byte[] ImageSignature { get; set; }

         [StringLength(10)]
        public string SignImageType { get; set; }

        public bool Active { get; set; }

        public bool IsLock { get; set; }

        public bool? ConfirmCheck { get; set; }

        public int? UpdateById { get; set; }

        [StringLength(255)]
        [Display(Name = "Mật khẩu/ Password")]
        public string UserPass { get; set; }

        public Nullable<DateTime> DateOfBirth { get; set; }
        public string Extension { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BtcKpi.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }

    public class CustomSerializeModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public List<string> RoleName { get; set; }

    }

    public class RegistrationView
    {
        [Required(ErrorMessage = "User Name required")]
        [Display(Name ="User Name")]
        public string Username { get; set; }

        [Required(ErrorMessage = "First Name required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        public Guid ActivationCode { get; set; }

        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Error : Confirm password does not match with password")]
        public string ConfirmPassword { get; set; }



    }

    public class ChangePasswordViewModel {
        [Required(ErrorMessage = "Mật khẩu cũ không được để trống!")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu cũ")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới không được để trống!")]
        [StringLength(255, ErrorMessage = "Mật khẩu mới phải từ 6 ký tự trở lên.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu mới không được để trống!")]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Lỗi : Xác nhận mật khẩu mới không trùng khớp với mật khẩu mới!")]
        public string ConfirmPassword { get; set; }
    }
}
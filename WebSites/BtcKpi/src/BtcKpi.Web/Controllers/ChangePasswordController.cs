using System.Web.Mvc;
using System.Web.Security;
using BtcKpi.Service;
using BtcKpi.Web.ViewModels;

namespace BtcKpi.Web.Controllers
{
    [RoutePrefix("ChangePassword")]
    public class ChangePasswordController : BaseController
    {
        private readonly IUserService userService;
        public ChangePasswordController(IUserService userService) {
            this.userService = userService;
        }
        // GET: ChangePassword
        public ActionResult ChangePassword() {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel changePassword)
        {
            if (ModelState.IsValid) {
                var items = userService.GetUserFullInfo(CurrentUser.UserId);
                if (changePassword.OldPassword.Equals(changePassword.NewPassword)) {
                    ModelState.AddModelError("", "Mật khẩu mới không được trùng với mật khẩu cũ . Vui lòng kiểm tra lại!");
                    return View(changePassword);
                } else if (Membership.ValidateUser(items.UserName, changePassword.OldPassword)) {
                    userService.updatePassword(items, changePassword.NewPassword);
                } else {
                    ModelState.AddModelError("", "Mật khẩu cũ không đúng. Vui lòng kiểm tra lại!");
                    return View(changePassword);
                }

            }
            return RedirectToAction("Index", "Home");
        }
    }
}
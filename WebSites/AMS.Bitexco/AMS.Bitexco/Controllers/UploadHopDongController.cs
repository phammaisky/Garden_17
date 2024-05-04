using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS.Models;

namespace AMS.Controllers
{
    public class UploadHopDongController : Controller
    {
        private AMSEntities db = new AMSEntities();
        // GET: UploadData
        public ActionResult Index()
        {
            ViewBag.Message = "";
            return View();
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            GroupUser_Authorize gruAu;
            var userSession = CheckPermission.CheckControler(this, User.Identity.Name, out gruAu);
            if (userSession == null)
            {
                return RedirectToAction("UserLogin", "Error");
            }
            else
            {
                if (gruAu == null)
                    return RedirectToAction("PermitErrorPop", "Error");
            }

            if (ModelState.IsValid)
            {
                if (file == null)
                {
                    ModelState.AddModelError("File", "Chọn file để tải lên");
                }
                else if (file.ContentLength > 0)
                {
                    int MaxContentLength = 1024 * 1024 * 4; //Size = 4 MB
                    string[] AllowedFileExtensions = new string[] { ".xls", ".xlsx" };
                    if (!AllowedFileExtensions.Contains
                    (file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                    {
                        ModelState.AddModelError("File", "Chỉ chọn các file: " + string.Join(", ", AllowedFileExtensions));
                    }
                    else if (file.ContentLength > MaxContentLength)
                    {
                        ModelState.AddModelError("File", "File quá lớn, giới hạn dung lượng file là: " + MaxContentLength + " MB");
                    }
                    else
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                        file.SaveAs(path);
                        ModelState.Clear();

                        MSExcelReaderHD ExcelReaderHD = new MSExcelReaderHD(path);
                        ExcelReaderHD.UploadKiemKe();

                        ViewBag.Message = "Đã tải xong file";
                    }
                }
            }
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
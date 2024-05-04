using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AMS.Models;

namespace AMS.Controllers
{
    public class DeviceCategoriesController : Controller
    {
        private AMSEntities db = new AMSEntities();
        private const int defaultPageSize = 30;

        // GET: DeviceCategories
        public ActionResult Index(int? page)
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
                    return RedirectToAction("PermitError", "Error");
            }

            IQueryable<DeviceCategory> deviceCategories = db.DeviceCategories;

            int currentPageIndex = page ?? 1;

            int skip = (currentPageIndex - 1) * defaultPageSize;
            int totalRow = deviceCategories.Count();

            deviceCategories = deviceCategories.OrderByDescending(e => e.Id).Skip(skip).Take(defaultPageSize);

            Pagging<DeviceCategory> pagingDeviceCategory = new Pagging<DeviceCategory>(deviceCategories.ToList(), totalRow, currentPageIndex, defaultPageSize);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_DeviceCategories", pagingDeviceCategory);
            }
            else
            {
                ViewBag.PaggingDeviceCat = pagingDeviceCategory;
                return View(deviceCategories.ToList());
            }
        }

        // GET: DeviceCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceCategory deviceCategory = db.DeviceCategories.Find(id);
            if (deviceCategory == null)
            {
                return HttpNotFound();
            }
            return View(deviceCategory);
        }

        // GET: DeviceCategories/Create
        public ActionResult Create()
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
            return PartialView("_Create");
        }

        // POST: DeviceCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DeviceCatName,Description,Type,Active")] DeviceCategory deviceCategory)
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
                if (checkExitDeviceCategories(deviceCategory.DeviceCatName, null))
                {
                    ModelState.AddModelError(string.Empty, "Danh mục này đã có trong dữ liệu!");
                    return PartialView("_Create", deviceCategory);
                }
                db.DeviceCategories.Add(deviceCategory);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return PartialView("_Create", deviceCategory);

        }

        bool checkExitDeviceCategories(string name, int? exepId)
        {
            int countExit = 0;
            if (exepId != null)
            {
                countExit = db.DeviceCategories.Where(tc => (tc.DeviceCatName == name) && tc.Id
                    != exepId).Count();
            }
            else
            {
                countExit = db.DeviceCategories.Where(tc => (tc.DeviceCatName == name) && tc.Id
                   != exepId).Count();
            }

            return countExit > 0 ? true : false;
        }

        // GET: DeviceCategories/Edit/5
        public ActionResult Edit(int? id)
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

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceCategory deviceCategory = db.DeviceCategories.Find(id);
            if (deviceCategory == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Edit", deviceCategory);
        }

        // POST: DeviceCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DeviceCatName,Description,Type,Active")] DeviceCategory deviceCategory)
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
                if (checkExitDeviceCategories(deviceCategory.DeviceCatName, deviceCategory.Id))
                {
                    ModelState.AddModelError(string.Empty, "Danh mục này đã có trong dữ liệu!");
                    return PartialView("_Create", deviceCategory);
                }
                db.Entry(deviceCategory).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return PartialView("_Edit", deviceCategory);
        }

        // GET: DeviceCategories/Delete/5
        public ActionResult Delete(int? id)
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

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceCategory deviceCategory = db.DeviceCategories.Find(id);
            if (deviceCategory == null)
            {
                return HttpNotFound();
            }
            if (deviceCategory.DeviceAndTools.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa! Có các thiết bị thuộc danh mục này");
                ViewBag.UnableDelete = true;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bạn chắc chắn muốn xóa danh mục thiết bị?");
                ViewBag.UnableDelete = false;
            }
            return PartialView("_Delete", deviceCategory);
        }

        // POST: DeviceCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeviceCategory deviceCategory = db.DeviceCategories.Find(id);
            if (deviceCategory.DeviceAndTools.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa! Có các thiết bị thuộc danh mục này!");
                return PartialView("_Delete", deviceCategory);
            }
            else
            {
                db.DeviceCategories.Remove(deviceCategory);
                try
                {
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Lỗi! Không xóa được danh mục thiết bị!");
                    return PartialView("_Delete", deviceCategory);
                }
            }
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

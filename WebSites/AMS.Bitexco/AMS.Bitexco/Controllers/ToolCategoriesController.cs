using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AMS.Models;

namespace AMS.Controllers
{
    public class ToolCategoriesController : Controller
    {
        private AMSEntities db = new AMSEntities();
        private const int defaultPageSize = 30;

        // GET: ToolCategories
        public ActionResult Index(int? page)
        {
            GroupUser_Authorize gruAu;
            var userSession = CheckPermission.CheckControler(this, User.Identity.Name, out gruAu);
            if (userSession == null)
            {
                return RedirectToAction("UserLogin", "PDSHome");
            }
            else
            {
                if (gruAu == null)
                    return RedirectToAction("PermitError", "PDSHome");
            }

            IQueryable<ToolCategory> toolCategories = db.ToolCategories;

            int currentPageIndex = page ?? 1;

            int skip = (currentPageIndex - 1) * defaultPageSize;
            int totalRow = toolCategories.Count();

            toolCategories = toolCategories.OrderByDescending(e => e.Id).Skip(skip).Take(defaultPageSize);

            Pagging<ToolCategory> pagingToolCategory = new Pagging<ToolCategory>(toolCategories.ToList(), totalRow, currentPageIndex, defaultPageSize);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ToolCategories", pagingToolCategory);
            }
            else
            {
                ViewBag.PaggingToolCat = pagingToolCategory;
                return View(toolCategories.ToList());
            }
        }

        // GET: ToolCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToolCategory toolCategory = db.ToolCategories.Find(id);
            if (toolCategory == null)
            {
                return HttpNotFound();
            }
            return View(toolCategory);
        }

        // GET: ToolCategories/Create
        public ActionResult Create()
        {
            GroupUser_Authorize gruAu;
            var userSession = CheckPermission.CheckControler(this, User.Identity.Name, out gruAu);
            if (userSession == null)
            {
                return RedirectToAction("UserLogin", "PDSHome");
            }
            else
            {
                if (gruAu == null)
                    return RedirectToAction("PermitErrorPop", "PDSHome");
            }
            return PartialView("_Create");
        }

        // POST: ToolCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ToolCatName,Description,Active")] ToolCategory toolCategory)
        {
            GroupUser_Authorize gruAu;
            var userSession = CheckPermission.CheckControler(this, User.Identity.Name, out gruAu);
            if (userSession == null)
            {
                return RedirectToAction("UserLogin", "PDSHome");
            }
            else
            {
                if (gruAu == null)
                    return RedirectToAction("PermitErrorPop", "PDSHome");
            }

            if (ModelState.IsValid)
            {
                if (checkExitToolCategories(toolCategory.ToolCatName, null))
                {
                    ModelState.AddModelError(string.Empty, "Danh mục này đã có trong dữ liệu!");
                    return PartialView("_Create", toolCategory);
                }
                db.ToolCategories.Add(toolCategory);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return PartialView("_Create", toolCategory);

        }

        bool checkExitToolCategories(string name, int? exepId)
        {
            int countExit = 0;
            if (exepId != null)
            {
                countExit = db.ToolCategories.Where(tc => (tc.ToolCatName == name) && tc.Id
                    != exepId).Count();
            }
            else
            {
                countExit = db.ToolCategories.Where(tc => (tc.ToolCatName == name) && tc.Id
                   != exepId).Count();
            }

            return countExit > 0 ? true : false;
        }

        // GET: ToolCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            GroupUser_Authorize gruAu;
            var userSession = CheckPermission.CheckControler(this, User.Identity.Name, out gruAu);
            if (userSession == null)
            {
                return RedirectToAction("UserLogin", "PDSHome");
            }
            else
            {
                if (gruAu == null)
                    return RedirectToAction("PermitErrorPop", "PDSHome");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToolCategory toolCategory = db.ToolCategories.Find(id);
            if (toolCategory == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Edit", toolCategory);            
        }

        // POST: ToolCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ToolCatName,Description,Active")] ToolCategory toolCategory)
        {   
            GroupUser_Authorize gruAu;
            var userSession = CheckPermission.CheckControler(this, User.Identity.Name, out gruAu);
            if (userSession == null)
            {
                return RedirectToAction("UserLogin", "PDSHome");
            }
            else
            {
                if (gruAu == null)
                    return RedirectToAction("PermitErrorPop", "PDSHome");
            }

            if (ModelState.IsValid)
            {
                if (checkExitToolCategories(toolCategory.ToolCatName, toolCategory.Id))
                {
                    ModelState.AddModelError(string.Empty, "Danh mục này đã có trong dữ liệu!");
                    return PartialView("_Create", toolCategory);
                }
                db.Entry(toolCategory).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return PartialView("_Edit", toolCategory);
        }

        // GET: ToolCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            GroupUser_Authorize gruAu;
            var userSession = CheckPermission.CheckControler(this, User.Identity.Name, out gruAu);
            if (userSession == null)
            {
                return RedirectToAction("UserLogin", "PDSHome");
            }
            else
            {
                if (gruAu == null)
                    return RedirectToAction("PermitErrorPop", "PDSHome");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToolCategory toolCategory = db.ToolCategories.Find(id);
            if (toolCategory == null)
            {
                return HttpNotFound();
            }
            if (toolCategory.DeviceAndTools.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa! Có các công cụ thuộc danh mục này");
                ViewBag.UnableDelete = true;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bạn chắc chắn muốn xóa danh mục công cụ?");
                ViewBag.UnableDelete = false;
            }


            return PartialView("_Delete", toolCategory);
        }

        // POST: ToolCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ToolCategory toolCategory = db.ToolCategories.Find(id);
            if (toolCategory.DeviceAndTools.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa! Có các công cụ thuộc danh mục này");
                return PartialView("_Delete", toolCategory);
            }
            else
            {
                db.ToolCategories.Remove(toolCategory);
                try
                {
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Lỗi! Không xóa được danh mục công cụ!");
                    return PartialView("_Delete", toolCategory);
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

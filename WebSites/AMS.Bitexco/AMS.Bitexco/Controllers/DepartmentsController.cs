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
    public class DepartmentsController : Controller
    {
        private AMSEntities db = new AMSEntities();

        // GET: Departments
        public ActionResult Index()
        {
            var departments = db.Departments.Include(d => d.Company);
            return View(departments.ToList());
        }

        public ActionResult GetListDepartments(int? Id)
        {   
            var listDept = db.Departments.Where(dp => dp.CompanyId == Id).OrderBy(p => p.DeptOrder);
            return PartialView("_ListDept", listDept);
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Departments/Create
        public ActionResult Create(int? CompanyId)
        {
            if (CompanyId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            else
            {
                var company = db.Companies.Find(CompanyId);
                if(company == null)
                { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
                else
                {
                    ViewBag.Company = company;
                    return PartialView("_Create");
                }
            }
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyId,DeptCode,DeptName,DeptDesc,Active,DeptOrder")] Department department)
        {
            if (ModelState.IsValid)
            {
                if (checkExitDepartement(department.DeptCode, department.DeptName, department.CompanyId, null))
                {
                    ModelState.AddModelError(string.Empty, "Tên hoặc mã của phòng ban đã có trong công ty!");
                    return PartialView("_Create", department);
                }
                db.Departments.Add(department);
                db.SaveChanges();
                return Json(new { success = true });
            }
            var company = db.Companies.Find(department.CompanyId);
            if (company == null)
            { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            else
            {
                ViewBag.Company = company;
                return PartialView("_Create", department);
            }           
        }

        bool checkExitDepartement(string code, string name, int companyId, int? exepId)
        {
            int countExit = 0;
            if (exepId != null)
                countExit = db.Departments.Where(dpt => (dpt.DeptCode == code || dpt.DeptName == name) && dpt.CompanyId == companyId && dpt.Id != exepId).Count();
            else
                countExit = db.Departments.Where(dpt => (dpt.DeptCode == code || dpt.DeptName == name) && dpt.CompanyId == companyId).Count();
            return countExit > 0 ? true : false;
        }
        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Edit", department);           
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyId,DeptCode,DeptName,DeptDesc,DeptOrder")] Department department)
        {
            if (ModelState.IsValid)
            {
                if (checkExitDepartement(department.DeptCode, department.DeptName, department.CompanyId, department.Id))
                {
                    ModelState.AddModelError(string.Empty, "Mã hoặc tên của phòng, ban đã có trong hệ thống!");
                    return PartialView("_Edit", department);
                }

                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return PartialView("_Edit", department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department dept = db.Departments.Find(id);
            if (dept == null)
            {
                return HttpNotFound();
            }

            if (dept.UserInfoes.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa! Nhiều dữ liệu trong hệ thống sử dụng thông tin phòng ban này!");
                ViewBag.UnableDelete = true;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bạn chắc chắn muốn xóa phòng ban?");
                ViewBag.UnableDelete = false;
            }
            return PartialView("_Delete");
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department dept = db.Departments.Find(id);
            if (dept.UserInfoes.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa! Nhiều dữ liệu trong hệ thống sử dụng thông tin phòng ban này!");
                return PartialView("_Delete", dept);
            }
            else
            {
                db.Departments.Remove(dept);
                try
                {
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Lỗi! Không xóa được phòng, ban!");
                    return PartialView("_Delete", dept);
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

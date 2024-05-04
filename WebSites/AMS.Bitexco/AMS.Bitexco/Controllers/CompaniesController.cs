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
    public class CompaniesController : Controller
    {
        private AMSEntities db = new AMSEntities();

        // GET: Companies
        public ActionResult Index()
        {
            var companies = db.Companies;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_CompanyList", companies.ToList());
            }
            else
            {
                return View(companies.ToList());
            }
        }

        public ActionResult CompanySelector()
        {
            var userSession = db.UserInfoes.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (userSession == null)
            {
                return HttpNotFound();
            }
            return PartialView("_CompanySelector", userSession.Companies.ToList());
        }
        public ActionResult DepartementSelector()
        {
            var userSession = db.UserInfoes.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (userSession == null)
            {
                return HttpNotFound();
            }                 
            return PartialView("_DepartementSelector", userSession.Companies.ToList());
        }
        public ActionResult CompanyDeptSelector(string type)
        {           
            var userSession = db.UserInfoes.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (userSession == null)
            {
                return HttpNotFound();
            }            
            if (type == "ToSearch")
                return PartialView("_CompanyDeptSearch", userSession.Companies.ToList());
            if (type == "ToSelect")
                return PartialView("_CompanyDeptSelector", db.Companies.OrderBy(c=>c.Sort).ToList());
            return PartialView("_CompanyDeptSelector", userSession.Companies.ToList());
        }
        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
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

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ParentId,NameVn,NameEn,Contents,LogoLocation")] Company company)
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
                if (company.ParentId == 0)
                    company.ParentId = null;

                db.Companies.Add(company);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return PartialView("_Create");
        }

        // GET: Companies/Edit/5
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
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Edit", company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParentId,NameVn,NameEn,Contents,LogoLocation")] Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.ParentId == 0)
                    company.ParentId = null;
                else
                {
                    if (!checkValidParentSelected(company.ParentId, company.Id))
                    {
                        ModelState.AddModelError(string.Empty, "Chọn cây công ty không phù hợp!");
                        return PartialView("_Edit", company);
                    }
                }

                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return PartialView("_Edit", company);
        }

        public bool checkValidParentSelected(int? parentId, int companyId)
        {
            using (AMSEntities dbAm = new AMSEntities())
            {
                var company = dbAm.Companies.Where(lc => lc.Id == companyId).FirstOrDefault();
                if (company != null)
                {
                    if (company.CompanyChild.Where(lcc => lcc.Id == parentId).Count() > 0)
                        return false;
                    else
                        return true;
                }
                else
                    return false;
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

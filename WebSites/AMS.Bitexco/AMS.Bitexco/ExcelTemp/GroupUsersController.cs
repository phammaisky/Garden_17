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
    public class GroupUsersController : Controller
    {
        private AMSEntities db = new AMSEntities();
        private const int defaultPageSize = 15;
        // GET: GroupUsers
        public ActionResult Index(int? page)
        {
            IQueryable<GroupUser> grpUser = db.GroupUsers.Where(g=>g.AppName=="AMS");

            int currentPageIndex = page ?? 1;
            int skip = (currentPageIndex - 1) * defaultPageSize;
            int totalRow = grpUser.Count();
            grpUser = grpUser.OrderByDescending(e => e.Id).Skip(skip).Take(defaultPageSize);

            Pagging<GroupUser> pagingGroupUser = new Pagging<GroupUser>(grpUser.ToList(), totalRow, currentPageIndex, defaultPageSize);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListGroupUser", pagingGroupUser);
            }
            else
            {
                ViewBag.GroupUser = pagingGroupUser;
                return View(grpUser.ToList());
            }

        }

        // GET: GroupUsers/Details/5
        public ActionResult Details(int? id)
        {
            //var userSession = CheckPermission.ReturnUserSession(User.Identity.Name);
            //if (userSession == null)
            //{
            //    return RedirectToAction("Login", "User");
            //}

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupUser groupUser = db.GroupUsers.Find(id);
            if (groupUser == null)
            {
                return HttpNotFound();
            }
            return View(groupUser);
        }

        // GET: GroupUsers/Create
        public ActionResult Create()
        {
            var userSession = CheckPermission.ReturnUserSession(User.Identity.Name);
            if (userSession == null)
            {
                return RedirectToAction("Login", "User");
            }
            return PartialView("Create");
        }

        // POST: GroupUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GroupName,Description,Active")] GroupUser groupUser)
        {
            var userSession = CheckPermission.ReturnUserSession(User.Identity.Name);
            if (userSession == null)
            {
                return RedirectToAction("Login", "User");
            }
            if (ModelState.IsValid)
            {
                groupUser.AppName = "AMS";
                db.GroupUsers.Add(groupUser);
                db.SaveChanges();
                return Json(new { success = true });
            }

            return PartialView("Create", groupUser);
        }

        public ActionResult Edit(int? id)
        {
            //var userSession = CheckPermission.ReturnUserSession(User.Identity.Name);
            //if (userSession == null)
            //{
            //    return RedirectToAction("Login", "User");
            //}
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupUser groupUser = db.GroupUsers.Find(id);
            if (groupUser == null)
            {
                return HttpNotFound();
            }
            return PartialView("Edit", groupUser);
        }

        // POST: GroupUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GroupName,Description,Active")] GroupUser groupUser)
        {
            //var userSession = CheckPermission.ReturnUserSession(User.Identity.Name);
            //if (userSession == null)
            //{
            //    return RedirectToAction("Login", "User");
            //}
            if (ModelState.IsValid)
            {
                db.Entry(groupUser).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return PartialView("Edit", groupUser);
          
        }
        public ActionResult SetPermission(int? id)
        {
            GroupUser groupUser = db.GroupUsers.Find(id);
            if (groupUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.MenuFunction = db.MenuFunctions.Where(m => m.ParentId == null && m.AppName == "AMS").OrderBy(m=>m.Sort).ToList();
            return View(groupUser);
        }

        [HttpPost]
        public ActionResult SetPermission(int? id, FormCollection formCollection)
        {
            // ModelBinder will set "form" appropriately

            GroupUser groupUser = db.GroupUsers.Find(id);
            if (groupUser == null)
            {
                return HttpNotFound();
            }
            GroupUser_Authorize GA;
            //db.GroupUser_Authorize.RemoveRange(groupUser.GroupUser_Authorize.Where(ga=>ga.Authorize.MenuFunction.AppName == "AMS"));
            db.GroupUser_Authorize.RemoveRange(groupUser.GroupUser_Authorize);

            foreach (var key in formCollection.AllKeys)
            {
                int value = Convert.ToInt32(formCollection[key]);
                if(value != 0)
                {
                    GA = new GroupUser_Authorize
                    {
                        GroupUserId = groupUser.Id,
                        AuthorizeId = Convert.ToInt32(key),
                        Extend = value
                    };
                    groupUser.GroupUser_Authorize.Add(GA);
                }               
            }
            db.Entry(groupUser).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.MenuFunction = db.MenuFunctions.Where(m => m.ParentId == null && m.AppName == "AMS").ToList();
            ModelState.AddModelError(string.Empty, "Đã cập nhật!");
            return View(groupUser);
        }
        public ActionResult Delete(int? id)
        {
            var userSession = CheckPermission.ReturnUserSession(User.Identity.Name);
            if (userSession == null)
            {
                return RedirectToAction("Login", "User");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupUser groupUser = db.GroupUsers.Find(id);
            if (groupUser == null)
            {
                return HttpNotFound();
            }
            return View(groupUser);
        }

        // POST: GroupUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userSession = CheckPermission.ReturnUserSession(User.Identity.Name);
            if (userSession == null)
            {
                return RedirectToAction("Login", "User");
            }
            GroupUser groupUser = db.GroupUsers.Find(id);
            ModelState.AddModelError(string.Empty, "Hiện tại chương trình chưa cho phép xóa nhóm người dùng!");
            return View(groupUser);
            
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

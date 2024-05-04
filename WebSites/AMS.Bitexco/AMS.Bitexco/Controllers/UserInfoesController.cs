using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AMS.Models;
using System.IO;


namespace AMS.Controllers
{
    public class UserInfoesController : Controller
    {
        private AMSEntities db = new AMSEntities();
        private const int defaultPageSize = 200;

        // getUserInfo
        public static UserInfo getUserInfo(int? userId)
        {
            if (!userId.HasValue)
                return null;
            using (AMSEntities db = new AMSEntities())
            {
                var user = db.UserInfoes.Include(u => u.Department).Where(u => u.Id == userId).FirstOrDefault();
                return user;
            }

        }

        //// GET: UserInfoes
        //public ActionResult Index()
        //{          
        //    int currentPageIndex = 1;

        //    int skip = (currentPageIndex - 1) * defaultPageSize;

        //    ViewBag.phongBan = new SelectList(db.Departments, "Id", "DeptName", null);

        //    var userInfoes = db.UserInfoes.Include(u => u.Department).Include(u => u.GroupUsers).Where(u=>u.Active == true);

        //    int totalRow = userInfoes.Count();

        //    userInfoes = userInfoes.OrderBy(e => e.DeptId).Skip(skip).Take(defaultPageSize);
        //    var groupUser = from u in userInfoes
        //                    group u by u.Department.DeptName into gu
        //                    select new Group<string, UserInfo> { Key = gu.Key, Values = gu };

        //    Pagging<UserInfo> pagingUser = new Pagging<UserInfo>(userInfoes.ToList(), totalRow, currentPageIndex, defaultPageSize);
        //    if (Request.IsAjaxRequest())
        //    {
        //        ViewBag.PaggingUserInfo = pagingUser;
        //        return PartialView("_UserList", groupUser);
        //    }
        //    else
        //    {
        //        ViewBag.PaggingUserInfo = pagingUser;
        //        ViewBag.GroupUser = groupUser.ToList();
        //        return View(userInfoes);
        //    }

        //}

        //[HttpPost]
        public ActionResult Index(UserInfoFilter userFillter)
        {
            var userSession = db.UserInfoes.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (userSession == null)
            {
                return HttpNotFound();
            }

            int currentPageIndex = userFillter.page ?? 1;

            int skip = (currentPageIndex - 1) * defaultPageSize;

            var comArray = userSession.Companies.Select(c => c.Id).ToArray();

            var deptIdArray = db.Departments.Where(d => comArray.Contains(d.CompanyId)).Select(dp => dp.Id).ToArray();

            var userInfoes = db.UserInfoes.Include(u => u.Department).Include(u => u.GroupUsers).Where(u => u.Active == true);

            userInfoes = userInfoes.Where(u => deptIdArray.Contains(u.DeptId.Value));

            if (userFillter.companyId != null)
                ViewBag.CompanyId = userFillter.companyId;
            if (userFillter.deptId != null)
                ViewBag.DeptId = userFillter.deptId;

            if (userFillter.companyId != null)
            {
                var company = db.Companies.Find(userFillter.companyId);
                if (company != null)
                {
                    string sqlQuery = "WITH    q AS " +
                                        "(" +
                                        "SELECT  * " +
                                        "FROM    Company " +
                                        "WHERE  Id = " + company.Id + " " +
                                        "UNION ALL " +
                                        "SELECT  m.* " +
                                        "FROM    Company m " +
                                        "JOIN    q " +
                                        "ON      m.parentID = q.Id" +
                                        ")" +
                                    "SELECT  Dept.Id " +
                                    "FROM    q as Company Join Departments as Dept on Company.Id = Dept.CompanyId";

                    var deptsId = db.Database.SqlQuery<int>(sqlQuery).ToArray();
                    userInfoes = userInfoes.Where(u => deptsId.Contains(u.DeptId.Value));
                }
            }
            else
            {
                if (userFillter.deptId != null)
                {
                    userInfoes = userInfoes.Where(u => u.DeptId == userFillter.deptId);
                }
            }
            if (userFillter.fullName != null)
            {
                userInfoes = userInfoes.Where(u => u.FullName.Contains(userFillter.fullName) || u.UserName.Contains(userFillter.fullName));
            }

            int totalRow = userInfoes.Count();

            userInfoes = userInfoes.OrderBy(e => e.DeptId).Skip(skip).Take(defaultPageSize);
            var groupUser = from u in userInfoes
                            group u by u.Department.DeptName into gu
                            select new Group<string, UserInfo> { Key = gu.Key, Values = gu };

            Pagging<UserInfo> pagingUser = new Pagging<UserInfo>(userInfoes.ToList(), totalRow, currentPageIndex, defaultPageSize);
            if (Request.IsAjaxRequest())
            {
                ViewBag.PaggingUserInfo = pagingUser;
                return PartialView("_UserList", groupUser.ToList());
            }
            else
            {
                ViewBag.PaggingUserInfo = pagingUser;
                ViewBag.GroupUser = groupUser.ToList();
                return View(userInfoes);
            }

        }

        // GET: UserInfoes/Details/5
        public ActionResult Details(int? id)
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
            UserInfo userInfo = db.UserInfoes.Find(id);
            if (userInfo == null)
            {
                return HttpNotFound();
            }
            return View(userInfo);
        }

        public ActionResult GetImageSignature(string Id)
        {
            try
            {
                int jmId = Convert.ToInt32(Id);
                var imageWafs = db.UserInfoes.Where(jm => jm.Id == jmId).FirstOrDefault();
                if (imageWafs != null)
                    return File(imageWafs.ImageSignature, "image/jpg");
                else
                    return null;
            }
            catch
            {
                return null;
            }

        }

        public ActionResult SetUserToProject()
        {
            var userSession = CheckPermission.ReturnUserSession(User.Identity.Name);
            if (userSession == null)
            {
                return RedirectToAction("Login", "User");
            }


            List<UserInfo> userInfoes = db.Database.SqlQuery<UserInfo>("SELECT * From UserInfo Where GroupUserId in (select Id from GroupUser where Id in (select GroupUserId from GroupUser_Authorize where AuthorizeId = 59))").ToList();

            for (int i = 0; i < userInfoes.Count; i++)
            {
                Department dp = db.Departments.Find(userInfoes[i].DeptId);
                if (dp != null)
                    userInfoes[i].Department = dp;
            }

            ViewBag.UserFisrt = userInfoes.First();
            return View(userInfoes);
        }

        public ActionResult GetUser_Project(int? id)
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
            UserInfo userInfo = db.UserInfoes.Find(id);

            if (userInfo == null)
            {
                return HttpNotFound();
            }
            return PartialView("_ProjectPartial", userInfo);
        }



        // GET: UserInfoes/Create
        public ActionResult Create(int? DeptId)
        {
            var userSession = CheckPermission.ReturnUserSession(User.Identity.Name);
            if (userSession == null)
            {
                return RedirectToAction("Login", "User");
            }
            if (DeptId != null)
            {
                var dept = db.Departments.Find(DeptId);
                if (dept != null)
                    ViewBag.Department = dept;
            }
            ViewBag.GroupUserId = new SelectList(db.GroupUsers.Where(g => g.AppName == "AMS"), "Id", "GroupName");
            ViewBag.Companies = db.Companies.ToList();

            return PartialView("_Create");
        }

        // POST: UserInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DeptId,UserName,FullName,IsLock")] UserInfo userInfo, int GroupUserId)
        {
            var userSession = CheckPermission.ReturnUserSession(User.Identity.Name);
            if (userSession == null)
            {
                return RedirectToAction("Login", "User");
            }

            var userCheck = db.UserInfoes.Where(u => u.UserName == userInfo.UserName).FirstOrDefault();
            if (userCheck != null)
            {
                string exitsUser = "Tài khoản đã có trong phòng " + userCheck.Department.DeptName + ".";
                if (userCheck.Active == false)
                {
                    exitsUser += " Tài khoản hiện đang bị khóa! ";
                }
                if (userInfo.DeptId != null)
                {
                    var dept = db.Departments.Find(userInfo.DeptId);
                    if (dept != null)
                        ViewBag.Department = dept;
                }
                ViewBag.GroupUserId = new SelectList(db.GroupUsers.Where(g => g.AppName == "AMS"), "Id", "GroupName", GroupUserId);
                ModelState.AddModelError(string.Empty, exitsUser);
                return PartialView("_Create", userInfo);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var groupUser = db.GroupUsers.Find(GroupUserId);
                    if (groupUser != null)
                        userInfo.GroupUsers.Add(groupUser);
                    userInfo.Active = true;
                    db.UserInfoes.Add(userInfo);
                    db.SaveChanges();
                    return Json(new { success = true });
                }

            }

            if (userInfo.DeptId != null)
            {
                var dept = db.Departments.Find(userInfo.DeptId);
                if (dept != null)
                    ViewBag.Department = dept;
            }
            ViewBag.GroupUserId = new SelectList(db.GroupUsers.Where(g => g.AppName == "AMS"), "Id", "GroupName", GroupUserId);
            return PartialView("_Create", userInfo);
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        // GET: UserInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            var userSession = CheckPermission.ReturnUserSession(User.Identity.Name);
            if (userSession == null)
            {
                return RedirectToAction("Login", "User");
            }
            userSession = db.UserInfoes.Find(userSession.Id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInfo userInfo = db.UserInfoes.Find(id);
            if (userInfo == null)
            {
                return HttpNotFound();
            }
            if (userInfo.GroupUsers.Where(g => g.AppName == "AMS").FirstOrDefault() != null)
            {
                ViewBag.GroupUserId = new SelectList(db.GroupUsers.Where(g => g.AppName == "AMS"), "Id", "GroupName", userInfo.GroupUsers.Where(g => g.AppName == "AMS").FirstOrDefault().Id);
                ViewBag.GroupUserId = FunctionsGeneral.AddDefaultOption(ViewBag.GroupUserId, "", "0");
            }
            else
            {
                ViewBag.GroupUserId = new SelectList(db.GroupUsers.Where(g => g.AppName == "AMS"), "Id", "GroupName");
                ViewBag.GroupUserId = FunctionsGeneral.AddDefaultOption(ViewBag.GroupUserId, "", "0");
            }
            ViewBag.Companies = userSession.Companies.ToList();
            return PartialView("_Edit", userInfo);
        }

        // POST: UserInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id")] UserInfo userInfo, int GroupUserId, string[] companies)
        {
            var userSession = CheckPermission.ReturnUserSession(User.Identity.Name);
            if (userSession == null)
            {
                return RedirectToAction("Login", "User");
            }
            userSession = db.UserInfoes.Find(userSession.Id);

            var userUpdate = db.UserInfoes.Find(userInfo.Id);
            if (userUpdate != null)
            {
                List<GroupUser> gau = userUpdate.GroupUsers.Where(g => g.AppName == "AMS").ToList();
                foreach (var ga in gau)
                {
                    userUpdate.GroupUsers.Remove(ga);
                }
                var groupUser = db.GroupUsers.Find(GroupUserId);
                if (groupUser != null)
                {
                    userUpdate.GroupUsers.Add(groupUser);
                }

                if (companies != null)
                {
                    userUpdate.Companies.Clear();
                    foreach (string companyId in companies)
                    {
                        var company = db.Companies.Find(Convert.ToInt32(companyId));
                        if (company != null)
                        {
                            userUpdate.Companies.Add(company);
                        }
                    }
                }
                db.Entry(userUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });

            }
            ViewBag.GroupUserId = new SelectList(db.GroupUsers.Where(g => g.AppName == "AMS"), "Id", "GroupName", GroupUserId);
            ViewBag.GroupUserId = FunctionsGeneral.AddDefaultOption(ViewBag.GroupUserId, "", "0");
            ViewBag.Companies = userSession.Companies.ToList();
            return PartialView("_Edit", userUpdate);
        }

        // GET: UserInfoes/Delete/5
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
            UserInfo userInfo = db.UserInfoes.Find(id);
            if (userInfo == null)
            {
                return HttpNotFound();
            }
            ModelState.AddModelError(string.Empty, "Bạn chắc chắn muốn xóa tài khoản này?");
            return PartialView("_Delete", userInfo);
        }

        // POST: UserInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userSession = CheckPermission.ReturnUserSession(User.Identity.Name);
            if (userSession == null)
            {
                return RedirectToAction("Login", "User");
            }
            UserInfo userInfo = db.UserInfoes.Find(id);
            userInfo.Active = false;
            try
            {
                db.Entry(userInfo).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Lỗi! Không xóa được tài khoản!");
                return PartialView("_Delete", userInfo);
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

    public class UserInfoFilter
    {
        public int? page { get; set; }
        public string fullName { get; set; }

        public int? deptId { get; set; }
        public int? companyId { get; set; }
    }
}

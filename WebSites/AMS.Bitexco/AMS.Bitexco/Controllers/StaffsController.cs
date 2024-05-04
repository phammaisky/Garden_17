using System;
using System.IO;
using System.Drawing;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AMS.Models;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace AMS.Controllers
{
    public class StaffsController : Controller
    {
        private AMSEntities db = new AMSEntities();
        private const int defaultPageSize = 30;
        // GET: UserInfoes
        public ActionResult Index(StaffFillter staffFillter)
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
            userSession = db.UserInfoes.Where(u => u.Id == userSession.Id).FirstOrDefault();
            if (userSession == null)
            {
                return HttpNotFound();
            }

            IQueryable<UserInfo> staffs = db.UserInfoes;

            int currentPageIndex = staffFillter.page ?? 1;
            int skip = (currentPageIndex - 1) * defaultPageSize;

            //Loc user theo cong ty duoc quyen quan ly

            //var comArray = userSession.Companies.Select(c => c.Id).ToArray();
            //var deptIdArray = db.Departments.Where(d => comArray.Contains(d.CompanyId)).Select(dp => dp.Id).ToArray();

            //staffs = staffs.Where(sff => deptIdArray.Contains(sff.DeptId.Value));

            if (staffFillter.companyId != null)
                ViewBag.CompanyId = staffFillter.companyId;
            if (staffFillter.deptId != null)
                ViewBag.DeptId = staffFillter.deptId;

            if (staffFillter.companyId != null)
            {
                var company = db.Companies.Find(staffFillter.companyId);
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
                    staffs = staffs.Where(sf => deptsId.Contains(sf.DeptId.Value));
                }
            }
            else
            {
                if (staffFillter.deptId != null)
                {
                    staffs = staffs.Where(sf => sf.DeptId == staffFillter.deptId);
                }
            }
            if(staffFillter.fullName != null)
            {
                staffs = staffs.Where(sf => sf.FullName.Contains(staffFillter.fullName));
            }
            int totalRow = staffs.Count();

            staffs = staffs.OrderByDescending(e => e.Id).Skip(skip).Take(defaultPageSize);

            Pagging<UserInfo> pagingUserInfoes = new Pagging<UserInfo>(staffs.ToList(), totalRow, currentPageIndex, defaultPageSize);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListStaff", pagingUserInfoes);
            }
            else
            {
                ViewBag.PaggingStaff = pagingUserInfoes;
                return View(staffs.ToList());
            }
        }

        public List<int> GetDeptOfCompany(int companyId)
        {
            List<int> deptId = null;
            var company = db.Companies.Find(companyId);
            if (company != null)
            {

            }

            return deptId;
        }

        public ActionResult GetListStaff(UserInfo staff)
        {
            var userSession = db.UserInfoes.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (userSession == null)
            {
                return HttpNotFound();
            }
            //UserInfo

            //var comArray = userSession.Companies.Select(c => c.Id).ToArray();

            //var deptIdArray = db.Departments.Where(d => comArray.Contains(d.CompanyId)).Select(dp => dp.Id).ToArray();

            //IQueryable<UserInfo> staffs = db.UserInfoes.Where(sff => deptIdArray.Contains(sff.DeptId.Value));

            IQueryable<UserInfo> staffs = db.UserInfoes;

            if (staff != null)
            {
                if (!String.IsNullOrEmpty(staff.UserName))
                {
                    staffs = staffs.Where(stf => stf.UserName.Contains(staff.UserName) || stf.FullName.Contains(staff.UserName));
                }
            }
            else
            {
                staffs = staffs.Take(500);
            }
            return PartialView("_StaffPartial", staffs.OrderByDescending(p => p.FullName).ToList());
        }

        public ActionResult ListAllUserDomain()
        {
            List<UsersDomain> lstADUsers = new List<UsersDomain>();
            UsersDomain ud;
            using (var context = new PrincipalContext(ContextType.Domain, "bitexco"))
            {
                using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {
                    int i = 0;
                    foreach (var result in searcher.FindAll())
                    {
                        i++;
                        ud = new UsersDomain();
                        DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;

                        ud.Id = i;
                        ud.UserName = de.Properties["samAccountName"].Value.ToString();
                        if (de.Properties["mail"].Value != null)
                            ud.Email = de.Properties["mail"].Value.ToString();
                        if (de.Properties["displayname"].Value != null)
                            ud.DisplayName = de.Properties["displayname"].Value.ToString();

                        //Console.WriteLine("First Name: " + );
                        //Console.WriteLine("Last Name : " + de.Properties["sn"].Value);
                        //Console.WriteLine("SAM account name   : " + de.Properties["samAccountName"].Value);
                        //Console.WriteLine("User principal name: " + de.Properties["userPrincipalName"].Value);
                        //Console.WriteLine();

                        lstADUsers.Add(ud);
                    }
                }
            }

            return View(lstADUsers);
        }

        public JsonResult AutoCompleteStaff(string term)
        {
            var userSession = db.UserInfoes.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            IQueryable<UserInfo> staffs = db.UserInfoes;

            var result = (from r in staffs
                          where r.UserName.ToLower().Contains(term.ToLower()) || r.FullName.Contains(term.ToLower())
                          select new { r.Id, r.UserName, r.FullName, r.Department.DeptCode, r.Department.Company.NameVn }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: UserInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInfo staff = db.UserInfoes.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            IQueryable<DeviceAndTool> deviceAndTool = db.DeviceAndTools.Include(dv => dv.HistoryUses);

            deviceAndTool = deviceAndTool.Where(dvt => dvt.HistoryUses.OrderByDescending(h => h.HandedDate).FirstOrDefault().HandedToStaffId == id);
            ViewBag.DeviceAndTool = deviceAndTool.ToList();
            return PartialView("_Details", staff);
        }
        public ActionResult Using(int? id)
        {
            var userSession = db.UserInfoes.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (userSession == null)
            {
                return HttpNotFound();
            }
            //UserInfo

            var comArray = userSession.Companies.Select(c => c.Id).ToArray();

            var deptIdArray = db.Departments.Where(d => comArray.Contains(d.CompanyId)).Select(dp => dp.Id).ToArray();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInfo staff = db.UserInfoes.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            IQueryable<DeviceAndTool> deviceAndTool = db.DeviceAndTools.Include(dv => dv.HistoryUses);
            deviceAndTool = deviceAndTool.Where(dvt => comArray.Contains(dvt.CompanyId));
            deviceAndTool = deviceAndTool.Where(dvt => dvt.HistoryUses.OrderByDescending(h => h.HandedDate).FirstOrDefault().HandedToStaffId == id);
            return PartialView("_Using", deviceAndTool);
        }

        public FileContentResult ExportUsing(int? id)
        {
            UserInfo staff = db.UserInfoes.Find(id);
            if (staff == null)
            {
                return null;
            }
            IQueryable<DeviceAndTool> deviceAndTool = db.DeviceAndTools.Include(dv => dv.HistoryUses);

            deviceAndTool = deviceAndTool.Where(dvt => dvt.HistoryUses.OrderByDescending(h => h.HandedDate).FirstOrDefault().HandedToStaffId == id);

            List<DeviceAndTool> _deviceAndTool = deviceAndTool.ToList();

            FileInfo file = new FileInfo(Server.MapPath(@"~/ExcelTemp/Using.xlsx"));
            ExcelPackage pck = new ExcelPackage(file);
            var ws = pck.Workbook.Worksheets[1];


            ws.Cells[4, 2].Value = staff.FullName.ToString();
            ws.Cells[5, 2].Value = staff.Department.Company.NameVn.ToString();
            ws.Cells[5, 4].Value = staff.Department.DeptName.ToString();

            int row = 8;
            foreach (var _asert in _deviceAndTool)
            {
                //Project prj = (Project)group.Key;

                ws.Cells[row, 1].Value = _asert.AssetsCode;
                ws.Cells[row, 2].Value = _asert.DeviceName;
                ws.Cells[row, 3].Value = _asert.DeviceCategory != null? _asert.DeviceCategory.DeviceCatName:"";
                ws.Cells[row, 4].Value = _asert.ToolCategory != null?_asert.ToolCategory.ToolCatName:"";
                ws.Cells[row, 5].Value = _asert.BuyDate;
                ws.Cells[row, 6].Value = _asert.HistoryUses.OrderByDescending(hu=>hu.HandedDate).First().HandedDate;
                ws.Cells[row, 6].Value = _asert.HistoryUses.OrderByDescending(hu => hu.HandedDate).First().StatusCategory.Name;
              

                //using (var range = ws.Cells[row, 1, row, 7])
                //{
                //    range.Style.Font.Bold = true;
                //    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 153, 255));
                //    range.Style.Font.Color.SetColor(Color.FromArgb(0, 32, 96));
                //}                                
                row++;
            }
           
            using (var range = ws.Cells[8, 1, row, 7])
            {
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }
            using (var range = ws.Cells[row, 1, row, 7])
            {
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }
            FileContentResult result = new FileContentResult(pck.GetAsByteArray(), "application / vnd.openxmlformats - officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "Bien ban.xlsx"
            };
            return result;
        }
        public ActionResult HistoryUse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInfo staff = db.UserInfoes.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            IQueryable<HistoryUse> historyUse = db.HistoryUses.Where(hu => hu.HandedToStaffId == id);

            ViewBag.HistoryUse = historyUse.ToList();
            return PartialView("_HistoryUse", staff);
        }

        public ActionResult PrintBanGiao(string dvt)
        {
            return PartialView("_PrintBanGiao");
        }

        // GET: UserInfoes/Create
        public ActionResult Create(int? DeptId)
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
            if(DeptId != null)
            {
                var dept = db.Departments.Find(DeptId);
                if (dept != null)
                    ViewBag.Department = dept;
            }
            return PartialView("_Create");
        }

        // POST: UserInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DeptId,UserName,FullName,Email")] UserInfo staff)
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
                if (checkExitStaff(staff.UserName, null))
                {
                    ModelState.AddModelError(string.Empty, "Tên tài khoản đã có trong dữ liệu!");
                    if (staff.DeptId != null)
                    {
                        var dept = db.Departments.Find(staff.DeptId);
                        if (dept != null)
                            ViewBag.Department = dept;
                    }
                    return PartialView("_Create", staff);
                }
                //if (!CheckExitsUserDomainName(staff.SysName))
                //{
                //    ModelState.AddModelError(string.Empty, "Tên tài khoản không có trong hệ thống tên miền bitexco.com.vn");
                //    return PartialView("_Create", staff);
                //}

                db.UserInfoes.Add(staff);
                db.SaveChanges();
                return Json(new { success = true });
            }
            if (staff.DeptId != null)
            {
                var dept = db.Departments.Find(staff.DeptId);
                if (dept != null)
                    ViewBag.Department = dept;
            }
            return PartialView("_Create", staff);
        }
        bool checkExitStaff(string name, int? exepId)
        {
            int countExit = 0;
            if (exepId != null)
            {
                countExit = db.UserInfoes.Where(tc => (tc.UserName == name) && tc.Id
                    != exepId).Count();
            }
            else
            {
                countExit = db.UserInfoes.Where(tc => (tc.UserName == name) && tc.Id
                   != exepId).Count();
            }

            return countExit > 0 ? true : false;
        }

        bool CheckExitsUserDomainName(string userName)
        {
            using (var pc = new PrincipalContext(ContextType.Domain, "bitexco"))
            using (var p = Principal.FindByIdentity(pc, IdentityType.SamAccountName, userName))
            {
                return p != null;
            }
        }
        // GET: UserInfoes/Edit/5
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
            UserInfo staff = db.UserInfoes.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Edit", staff);
        }

        // POST: UserInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DeptId,UserName,FullName,Email")] UserInfo staff)
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
                if (checkExitStaff(staff.UserName, staff.Id))
                {
                    ModelState.AddModelError(string.Empty, "Tên tài khoản đã có trong dữ liệu!");
                    return PartialView("_Create", staff);
                }
                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return PartialView("_Edit", staff);
        }

        // GET: UserInfoes/Delete/5
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
            UserInfo staff = db.UserInfoes.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            if (staff.HistoryUses.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa! Nhân viên chưa được xóa hết các tài sản đang sử dụng");
                ViewBag.UnableDelete = true;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bạn chắc chắn muốn xóa nhân viên?");
                ViewBag.UnableDelete = false;
            }


            return PartialView("_Delete", staff);
        }

        // POST: UserInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserInfo staff = db.UserInfoes.Find(id);
            if (staff.HistoryUses.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa! Nhân viên chưa được xóa hết các tài sản đang sử dụng");
                return PartialView("_Delete", staff);
            }
            else
            {
                db.UserInfoes.Remove(staff);
                try
                {
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Lỗi! Không xóa được nhân viên!");
                    return PartialView("_Delete", staff);
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

    public class StaffFillter
    {
        public int? page { get; set; }
        public int? companyId { get; set; }
        public int? deptId { get; set; }
        public string fullName { get; set; }
    }
    public class UsersDomain
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public bool isMapped { get; set; }
    }
}

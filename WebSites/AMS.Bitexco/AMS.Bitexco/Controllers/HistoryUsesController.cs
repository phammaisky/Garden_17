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
    public class HistoryUsesController : Controller
    {
        private AMSEntities db = new AMSEntities();

        // GET: HistoryUses
        public ActionResult Index()
        {
            var historyUses = db.HistoryUses.Include(h => h.DeviceAndTool).Include(h => h.Location).Include(h => h.Staff).Include(h => h.StatusCategory);
            return View(historyUses.ToList());
        }

        public ActionResult HistoryOfAssert(int Id)
        {
            var historyUses = db.HistoryUses.Include(h => h.DeviceAndTool).Include(h => h.Location).Include(h => h.Staff).Include(h => h.StatusCategory).Where(h=>h.DeviceToolId == Id).OrderByDescending(h=>h.HandedDate);

            return PartialView("_HistoryOfAssert", historyUses);
        }

        // GET: HistoryUses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistoryUse historyUse = db.HistoryUses.Find(id);
            if (historyUse == null)
            {
                return HttpNotFound();
            }
            return View(historyUse);
        }

        // GET: HistoryUses/Create
        public ActionResult Create(int Id)
        {
            DeviceAndTool deviceAndTool = db.DeviceAndTools.Find(Id);
            if (deviceAndTool == null)
            {
                return HttpNotFound();
            }
            HistoryUse historyUse = new HistoryUse();
            historyUse.DeviceAndTool = deviceAndTool;
            historyUse.HandedDate = DateTime.Now;
            ViewBag.StatusId = new SelectList(db.StatusCategories, "Id", "Name");
            return PartialView("_Create", historyUse);
        }

        // POST: HistoryUses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DeviceToolId,HandedToStaffId,HandedDate,DeptId,LocationId,StatusId,StatusDescrition")] HistoryUse historyUse)
        {
            DeviceAndTool deviceAndTool = db.DeviceAndTools.Find(historyUse.DeviceToolId);
            if (deviceAndTool == null)
            {
                return HttpNotFound();
            }
            if(deviceAndTool.HistoryUses.Count(h=>h.StatusId ==5) > 0)
            {
                if(historyUse.StatusId == 5)
                    ModelState.AddModelError(string.Empty, "Tài sản đã thanh lý! không thể tạo một quá trình thanh lý khác");
                if(historyUse.HandedDate > deviceAndTool.HistoryUses.Where(h=>h.StatusId ==5).First().HandedDate)
                    ModelState.AddModelError(string.Empty, "Tài sản đã thanh lý! Quá trình sử dụng mới có ngày bàn giao không được sau ngày thanh lý");
            }

            if (historyUse.HandedToStaffId == null && historyUse.DeptId == null && historyUse.LocationId == null && historyUse.StatusId != 5)
            {
                ModelState.AddModelError(string.Empty, "Cần phải nhập người sử dụng tài sản hoặc phòng ban hoặc địa điểm khi bàn giao! trừ trường hợp thanh lý.");
            }

            if (ModelState.IsValid)
            {
                db.HistoryUses.Add(historyUse);
                db.SaveChanges();
                return Json(new { success = true });
            }
            
            historyUse.DeviceAndTool = deviceAndTool;
            ViewBag.StatusId = new SelectList(db.StatusCategories, "Id", "Name", historyUse.StatusId);
            if (historyUse.DeptId != null)
            {
                var dept = db.Departments.Find(historyUse.DeptId);
                if (dept != null)
                    ViewBag.Department = dept;
            }
            return PartialView("_Create", historyUse);
        }

        public ActionResult CreateToStaff(int Id)
        {
            UserInfo staff = db.UserInfoes.Find(Id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            HistoryUse historyUse = new HistoryUse();
            historyUse.Staff = staff;
            historyUse.HandedToStaffId = staff.Id;
            historyUse.HandedDate = DateTime.Now;
           
            var prj = db.StatusCategories;
            ViewBag.StatusId = new SelectList(prj, "Id", "Name", null);
            ViewBag.StatusId = FunctionsGeneral.AddDefaultOption(ViewBag.StatusId, "", "null");
            ViewBag.StatusSearchId = new SelectList(prj, "Id", "Name", null);
            ViewBag.StatusSearchId = FunctionsGeneral.AddDefaultOption(ViewBag.StatusSearchId, "", "null");
            return PartialView("_CreateToStaff", historyUse);
        }

        // POST: HistoryUses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateToStaff([Bind(Include = "Id,DeviceToolId,HandedToStaffId,HandedDate,StatusId,StatusDescrition")] HistoryUse historyUse)
        {
            DeviceAndTool deviceAndTool = db.DeviceAndTools.Find(historyUse.DeviceToolId);
            if (deviceAndTool == null)
            {
                ModelState.AddModelError("DeviceTool", "Chưa chọn tài sản để bàn giao.");
            }
            else
            {
                if (deviceAndTool.HistoryUses.Count(h => h.StatusId == 5) > 0)
                {
                    if (historyUse.StatusId == 5)
                        ModelState.AddModelError(string.Empty, "Tài sản đã thanh lý! không thể tạo một quá trình thanh lý khác");
                    if (historyUse.HandedDate > deviceAndTool.HistoryUses.Where(h => h.StatusId == 5).First().HandedDate)
                        ModelState.AddModelError(string.Empty, "Tài sản đã thanh lý! Quá trình sử dụng mới có ngày bàn giao không được sau ngày thanh lý");
                }
            }
            if (historyUse.HandedToStaffId == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy thông tin người nhận bàn giao.");
            }           
            if (historyUse.StatusId == 0)
            {
                ModelState.AddModelError("StatusId", "Chưa chọn tình trạng lúc bàn giao");
            }

            if (ModelState.IsValid)
            {
                db.HistoryUses.Add(historyUse);
                db.SaveChanges();
                return Json(new { success = true });
            }

            historyUse.DeviceAndTool = deviceAndTool;

            var prj = db.StatusCategories;
            ViewBag.StatusId = new SelectList(prj, "Id", "Name", null);
            ViewBag.StatusId = FunctionsGeneral.AddDefaultOption(ViewBag.StatusId, "", "null");
            ViewBag.StatusSearchId = new SelectList(prj, "Id", "Name", null);
            ViewBag.StatusSearchId = FunctionsGeneral.AddDefaultOption(ViewBag.StatusSearchId, "", "null");
            if (historyUse.DeptId != null)
            {
                var dept = db.Departments.Find(historyUse.DeptId);
                if (dept != null)
                    ViewBag.Department = dept;
            }
            return PartialView("_CreateToStaff", historyUse);
        }
      
        public ActionResult Edit(int? id)
        {
            HistoryUse historyUse = db.HistoryUses.Find(id);
            if (historyUse == null)
            {
                return HttpNotFound();
            }    
            historyUse.HandedDate = DateTime.Now;
            ViewBag.StatusId = new SelectList(db.StatusCategories, "Id", "Name", historyUse.StatusId);
            return PartialView("_Edit", historyUse);
        }

        // POST: HistoryUses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DeviceToolId,HandedToStaffId,HandedDate,DeptId,LocationId,StatusId,StatusDescrition")] HistoryUse historyUse)
        {
            DeviceAndTool deviceAndTool = db.DeviceAndTools.Find(historyUse.DeviceToolId);
            if (deviceAndTool == null)
            {
                return HttpNotFound();
            }
            if (deviceAndTool.HistoryUses.Count(h => h.StatusId == 5) > 0)
            {
                if (historyUse.StatusId == 5)
                    ModelState.AddModelError(string.Empty, "Tài sản đã thanh lý! không thể tạo một quá trình thanh lý khác");
                if (historyUse.HandedDate > deviceAndTool.HistoryUses.Where(h => h.StatusId == 5).First().HandedDate)
                    ModelState.AddModelError(string.Empty, "Tài sản đã thanh lý! Quá trình sử dụng mới có ngày bàn giao không được sau ngày thanh lý");
            }
            if (historyUse.HandedToStaffId == null && historyUse.DeptId == null && historyUse.LocationId == null && historyUse.StatusId != 5)
            {
                ModelState.AddModelError(string.Empty, "Cần phải nhập người sử dụng tài sản hoặc phòng ban hoặc địa điểm khi bàn giao! trừ trường hợp thanh lý.");
            }

            if (ModelState.IsValid)
            {
                var historyUseUpdate = db.HistoryUses.Find(historyUse.Id);
                historyUseUpdate.HandedToStaffId = historyUse.HandedToStaffId;
                historyUseUpdate.DeptId = historyUse.DeptId;
                historyUseUpdate.LocationId = historyUse.LocationId;
                historyUseUpdate.StatusId = historyUse.StatusId;
                historyUseUpdate.HandedDate = historyUse.HandedDate;
                historyUseUpdate.StatusDescrition = historyUse.StatusDescrition;

                db.Entry(historyUseUpdate).State = EntityState.Modified;
                db.SaveChanges();               
                return Json(new { success = true });
            }           
            historyUse.DeviceAndTool = deviceAndTool;
            ViewBag.StatusId = new SelectList(db.StatusCategories, "Id", "Name", historyUse.StatusId);
            return PartialView("_Edit", historyUse);
        }

        // GET: HistoryUses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistoryUse historyUse = db.HistoryUses.Find(id);
            if (historyUse == null)
            {
                return HttpNotFound();
            }
            DeviceAndTool deviceAndTool = db.DeviceAndTools.Find(historyUse.DeviceToolId);
            if(deviceAndTool != null)
            {
                if(deviceAndTool.HistoryUses.Count == 1)
                {
                    ModelState.AddModelError(string.Empty, "Không thể xóa! Một tài sản bắt buộc phải có ít nhất một quá trình sử dụng!");
                    ViewBag.UnableDelete = true;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Bạn chắc chắn muốn xóa quá trình sử dụng?");
                    ViewBag.UnableDelete = false;
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bạn chắc chắn muốn xóa quá trình sử dụng?");
                ViewBag.UnableDelete = false;
            }
            return PartialView("_Delete", historyUse);
        }

        // POST: HistoryUses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HistoryUse historyUse = db.HistoryUses.Find(id);
            db.HistoryUses.Remove(historyUse);
            try
            {
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Lỗi! Không xóa được!");
                return PartialView("_Delete", historyUse);
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

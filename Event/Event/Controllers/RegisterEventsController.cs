using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Event.Models;
using Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Event.Controllers
{
    public class RegisterEventsController : Controller
    {
        private GardenEventEntities db = new GardenEventEntities();
        private const int defaultPageSize = 30;

        // GET: RegisterEvents
        public ActionResult Index(int? details)
        {
            var registerEvents = db.RegisterEvent.OrderByDescending(o => o.Id).Distinct();

            int currentPageIndex = 1;
            int skip = (currentPageIndex - 1) * defaultPageSize;
            int totalRow = registerEvents.Count();

            List<RegisterEvent> lstEvent = registerEvents.OrderByDescending(p => p.Id).Skip(skip).Take(defaultPageSize).ToList();

            Pagging<RegisterEvent> pagingEvents = new Pagging<RegisterEvent>(lstEvent, totalRow, currentPageIndex, defaultPageSize);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListIndexEvent", pagingEvents);
            }
            else
            {
                if (details != null)
                {
                    var doc = db.RegisterEvent.Find(details);
                    if (doc != null)
                        ViewBag.PagingEvents = doc;
                }
                ViewBag.PagingEvents = pagingEvents;
                return View(pagingEvents.model);
            }
        }

        [HttpPost]
        public ActionResult Index(EventsFilter filter, int? details)
        {
            var registerEvents = db.RegisterEvent.OrderByDescending(o => o.Id).Distinct();

            if (!string.IsNullOrEmpty(filter.fullName))
            {
                registerEvents = registerEvents.Where(w => w.FullName.ToUpper().Contains(filter.fullName.ToUpper()));
            }
            if (!string.IsNullOrEmpty(filter.phoneNumber))
            {
                registerEvents = registerEvents.Where(w => w.Phone.ToUpper().Contains(filter.phoneNumber.ToUpper()));
            }

            int currentPageIndex = filter.page.HasValue ? filter.page.Value : 1;
            int skip = (currentPageIndex - 1) * defaultPageSize;
            int totalRow = registerEvents.Count();

            List<RegisterEvent> lstEvent = registerEvents.OrderByDescending(p => p.Id).Skip(skip).Take(defaultPageSize).ToList();

            Pagging<RegisterEvent> pagingEvents = new Pagging<RegisterEvent>(lstEvent, totalRow, currentPageIndex, defaultPageSize);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListIndexEvent", pagingEvents);
            }
            else
            {
                if (details != null)
                {
                    var doc = db.RegisterEvent.Find(details);
                    if (doc != null)
                        ViewBag.PagingEvents = doc;
                }
                ViewBag.PagingEvents = pagingEvents;
                return View(pagingEvents.model);
            }
        }

        // GET: RegisterEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisterEvent registerEvent = db.RegisterEvent.Find(id);
            if (registerEvent == null)
            {
                return HttpNotFound();
            }
            return View(registerEvent);
        }

        // GET: RegisterEvents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RegisterEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FullName,Phone,Email,RoundNumber")] RegisterEvent registerEvent)
        {
            if (ModelState.IsValid)
            {
                RegisterEvent isCheck = db.RegisterEvent.FirstOrDefault(w => w.Phone.Equals(registerEvent.Phone));
                if (isCheck != null)
                {
                    ModelState.AddModelError(String.Empty, "Số điện thoại đã được đăng ký!");
                }
                else
                {
                    registerEvent.CreateDated = DateTime.Now;
                    registerEvent.Win = "0";
                    db.RegisterEvent.Add(registerEvent);

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(registerEvent);
        }

        // GET: RegisterEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisterEvent registerEvent = db.RegisterEvent.Find(id);
            if (registerEvent == null)
            {
                return HttpNotFound();
            }
            return View(registerEvent);
        }

        // POST: RegisterEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FullName,Phone,Email,RoundNumber,Description")] RegisterEvent registerEvent)
        {
            if (ModelState.IsValid)
            {
                RegisterEvent updateEvent = db.RegisterEvent.Find(registerEvent.Id);
                if (updateEvent != null)
                {
                    updateEvent.FullName = registerEvent.FullName;
                    updateEvent.Phone = registerEvent.Phone;
                    updateEvent.Email = registerEvent.Email;
                    updateEvent.RoundNumber = registerEvent.RoundNumber;
                    updateEvent.Description = registerEvent.Description;

                    db.Entry(updateEvent).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(registerEvent);
        }

        // GET: RegisterEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisterEvent registerEvent = db.RegisterEvent.Find(id);
            if (registerEvent == null)
            {
                return HttpNotFound();
            }
            return View(registerEvent);
        }

        // POST: RegisterEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RegisterEvent registerEvent = db.RegisterEvent.Find(id);
            db.RegisterEvent.Remove(registerEvent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ExportExcel(EventsFilter filter)
        {
            var registerEvents = db.RegisterEvent.OrderByDescending(o => o.Id).Distinct();

            if (!string.IsNullOrEmpty(filter.fullName))
            {
                registerEvents = registerEvents.Where(w => w.FullName.ToUpper().Contains(filter.fullName.ToUpper()));
            }
            if (!string.IsNullOrEmpty(filter.phoneNumber))
            {
                registerEvents = registerEvents.Where(w => w.Phone.ToUpper().Contains(filter.phoneNumber.ToUpper()));
            }

            List<RegisterEvent> lstEvent = registerEvents.OrderByDescending(p => p.Id).ToList();



            //Save the workbook to disk in xlsx format
            string dateStr = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy_hhmmss");
            var fileDownloadName = "Báo cáo DS người đăng ký dự thưởng_" + dateStr + ".xlsx";
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileStream = new MemoryStream();
            //
            ExcelPackage excelEngine = FillDataTableExcel(lstEvent);
            excelEngine.SaveAs(fileStream);
            fileStream.Position = 0;
            var fsr = new FileStreamResult(fileStream, contentType);
            fsr.FileDownloadName = fileDownloadName;

            return fsr;
        }

        public ExcelPackage FillDataTableExcel(List<RegisterEvent> model)
        {
            ExcelPackage excelEngine = new ExcelPackage();
            ExcelWorksheet worksheet = excelEngine.Workbook.Worksheets.Add("DSNDKDT");

            //Enter values to the cells from A1 to E5
            worksheet.Cells["A1:G1"].Merge = true;
            worksheet.Cells["A1"].Value = "DANH SÁCH ĐĂNG KÝ NGƯỜI DỰ THƯỞNG";
            worksheet.Cells["A1"].Style.Font.Bold = true;
            worksheet.Cells["A1"].Style.Font.Size = 18;
            worksheet.Row(1).Height = 40;
            worksheet.Cells["A1:G1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["A1:G1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            worksheet.Cells["A2"].Value = "STT";
            worksheet.Cells["B2"].Value = "Họ tên";
            worksheet.Cells["C2"].Value = "Số điện thoại";
            worksheet.Cells["D2"].Value = "Email";
            worksheet.Cells["E2"].Value = "Mã vòng";
            worksheet.Cells["F2"].Value = "Ngày đăng ký";
            worksheet.Cells["G2"].Value = "Mô tả";
            worksheet.Cells["A2:G2"].Style.Font.Bold = true;
            worksheet.Cells["A2:G2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            int col = 2;
            for (int i = 0; i < model.Count; i++)
            {
                col++;
                worksheet.Cells["A" + col].Value = i + 1;
                worksheet.Cells["B" + col].Value = model[i].FullName.Trim();
                worksheet.Cells["C" + col].Value = model[i].Phone.Trim();
                worksheet.Cells["D" + col].Value = model[i].Email.Trim();
                worksheet.Cells["E" + col].Value = model[i].RoundNumber.Trim();
                worksheet.Cells["F" + col].Value = Convert.ToDateTime(model[i].CreateDated).ToString("dd/MM/yyyy");
                worksheet.Cells["G" + col].Value = model[i].Description;
                worksheet.Cells["A" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["B" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells["C" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells["D" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells["E" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells["F" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["G" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            }
            worksheet.Cells["A1:G" + col].Style.WrapText = true;
            // format cells - add borders A5:I...
            worksheet.Cells["A2:G" + col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A2:G" + col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A2:G" + col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A2:G" + col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + col + ":G" + col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //Apply row height and column width to look good
            worksheet.Column(1).Width = 5;
            worksheet.Column(2).Width = 30;
            worksheet.Column(3).Width = 25;
            worksheet.Column(4).Width = 30;
            worksheet.Column(5).Width = 20;
            worksheet.Column(6).Width = 15;
            worksheet.Column(7).Width = 30;

            return excelEngine;
        }

        [HttpPost]
        public ActionResult GenerateRandom(EventsFilter filter, int? details)
        {
            try
            {
                var random = new Random();
                var list = new List<string>();
                List<RegisterEvent> lst = db.RegisterEvent.Where(w => "0".Equals(w.Win)).ToList();
                foreach (var item in lst)
                {
                    list.Add(item.RoundNumber);
                }
                int index = random.Next(list.Count);
                var roundNumber = list[index].ToString();

                var registerEvents = db.RegisterEvent.Where(w => roundNumber.Equals(w.RoundNumber) && "0".Equals(w.Win)).OrderByDescending(u => u.Id).Distinct();

                int currentPageIndex = filter.page.HasValue ? filter.page.Value : 1;
                int skip = (currentPageIndex - 1) * defaultPageSize;
                int totalRow = registerEvents.Count();
                List<RegisterEvent> lstEvent = registerEvents.OrderByDescending(u => u.Id).Skip(skip).Take(defaultPageSize).ToList();

                Pagging<RegisterEvent> pagingEvents = new Pagging<RegisterEvent>(lstEvent, totalRow, currentPageIndex, defaultPageSize);
                if (totalRow > 0)
                {
                    if (Request.IsAjaxRequest())
                    {
                        lstEvent[0].Win = "1";
                        db.Entry(lstEvent[0]).State = EntityState.Modified;
                        db.SaveChanges();

                        return PartialView("_ListIndexEvent", pagingEvents);
                    }
                    else
                    {
                        ViewBag.PagingEvents = pagingEvents;
                        return View("Index", pagingEvents.model);
                    }
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Số trúng thưởng đã hết!");

                    return View("Index", pagingEvents.model);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
    public class EventsFilter
    {
        public int? page { get; set; }
        public string fullName { get; set; }
        public string phoneNumber { get; set; }
    }
}

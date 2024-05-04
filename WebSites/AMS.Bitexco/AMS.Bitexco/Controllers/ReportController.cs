using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AMS;
using AMS.Models;

using Spire.Barcode;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Ionic.Zip;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;

namespace AMS.Controllers
{
    public class ReportController : Controller
    {
        private AMSEntities db = new AMSEntities();
        private const int defaultPageSize = 14;
        // GET: Report
        public ActionResult Index(DeviceToolFilter dvtFilter)
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
            var companiesId = userSession.Companies.Select(c => c.Id).ToList();

            IQueryable<DeviceAndTool> deviceAndTool = db.DeviceAndTools.Include(dv => dv.HistoryUses);

            deviceAndTool = deviceAndTool.Where(dv => companiesId.Contains(dv.CompanyId));

            int currentPageIndex = dvtFilter.page ?? 1;

            int skip = (currentPageIndex - 1) * defaultPageSize;

            if (dvtFilter.maTS != null)
                deviceAndTool = deviceAndTool.Where(dvt => dvt.AssetsCode.Contains(dvtFilter.maTS));

            if (dvtFilter.staffId != null)
                deviceAndTool = deviceAndTool.Where(dvt => dvt.HistoryUses.OrderByDescending(h => h.HandedDate).FirstOrDefault().HandedToStaffId == dvtFilter.staffId);

            if (dvtFilter.statusId != null)
                deviceAndTool = deviceAndTool.Where(dvt => dvt.HistoryUses.OrderByDescending(h => h.HandedDate).FirstOrDefault().StatusId == dvtFilter.statusId);

            if (dvtFilter.deviceCatId != null && dvtFilter.toolCatId != null)
            {
                deviceAndTool = deviceAndTool.Where(dvt => dvt.DeviceCatId == dvtFilter.deviceCatId || dvt.ToolCatId == dvtFilter.toolCatId);
            }
            else
            {
                if (dvtFilter.deviceCatId != null)
                {
                    deviceAndTool = deviceAndTool.Where(dvt => dvt.DeviceCatId == dvtFilter.deviceCatId);
                }
                if (dvtFilter.toolCatId != null)
                {
                    deviceAndTool = deviceAndTool.Where(dvt => dvt.ToolCatId == dvtFilter.toolCatId);
                }
            }


            if (dvtFilter.deptId != null)
                deviceAndTool = deviceAndTool.Where(dvt => dvt.HistoryUses.OrderByDescending(h => h.HandedDate).FirstOrDefault().DeptId == dvtFilter.deptId);

            if (dvtFilter.companyId != null)
                deviceAndTool = deviceAndTool.Where(dvt => dvt.CompanyId == dvtFilter.companyId);

            if (dvtFilter.AdminOrId != null)
            {
                if(dvtFilter.AdminOrId == 0)
                {
                    var deviceCat = db.DeviceCategories.Where(dc => dc.Type == (DeviceCategory.TypeDevice)dvtFilter.AdminOrId).Select(dvc => dvc.Id).ToArray();
                    deviceAndTool = deviceAndTool.Where(dvt =>dvt.DeviceCatId == null || deviceCat.Contains(dvt.DeviceCatId.Value));
                }
                if (dvtFilter.AdminOrId == 1)
                {
                    var deviceCat = db.DeviceCategories.Where(dc => dc.Type == (DeviceCategory.TypeDevice)dvtFilter.AdminOrId).Select(dvc => dvc.Id).ToArray();
                    deviceAndTool = deviceAndTool.Where(dvt => dvt.DeviceCatId != null && deviceCat.Contains(dvt.DeviceCatId.Value));
                }               
            }
                


            if (dvtFilter.locatioId != null)
            {
                var location = db.Locations.Find(dvtFilter.locatioId);
                if (location != null)
                {
                    string sqlQuery = "WITH    q AS " +
                                        "(" +
                                        "SELECT  * " +
                                        "FROM    Location " +
                                        "WHERE  Id = " + location.Id + " " +
                                        "UNION ALL " +
                                        "SELECT  m.* " +
                                        "FROM    Location m " +
                                        "JOIN    q " +
                                        "ON      m.parentID = q.Id" +
                                        ")" +
                                    "SELECT  Id " +
                                    "FROM    q";

                    var locationIds = db.Database.SqlQuery<int>(sqlQuery).ToArray();
                    deviceAndTool = deviceAndTool.Where(dvt => locationIds.Contains(dvt.HistoryUses.OrderByDescending(h => h.HandedDate).FirstOrDefault().LocationId.Value));
                }
            }

            DateTime? startDate = new DateTime();
            DateTime? endDate = new DateTime();
            SetFromDateToDate(dvtFilter.fromDate, dvtFilter.toDate, ref startDate, ref endDate);

            if(startDate.HasValue)
            {
                if(endDate.HasValue)
                    deviceAndTool = deviceAndTool.Where(e => e.CheckedDate >= startDate && e.CheckedDate <= endDate);
                else
                    deviceAndTool = deviceAndTool.Where(e => e.CheckedDate >= startDate);
            }
            else
            {
                if(endDate.HasValue)
                    deviceAndTool = deviceAndTool.Where(e => e.CheckedDate <= endDate);
            }

            int totalRow = deviceAndTool.Count();

            deviceAndTool = deviceAndTool.OrderByDescending(e => e.Id).Skip(skip).Take(defaultPageSize);

            Pagging<DeviceAndTool> PaggingDeviceAndTool = new Pagging<DeviceAndTool>(deviceAndTool.ToList(), totalRow, currentPageIndex, defaultPageSize);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListDeviceAndTool", PaggingDeviceAndTool);
            }
            else
            {
                var prj = db.StatusCategories;
                ViewBag.StatusId = new SelectList(prj, "Id", "Name", null);
                ViewBag.StatusId = FunctionsGeneral.AddDefaultOption(ViewBag.StatusId, "", "null");
                ViewBag.DeviceCatId = new SelectList(db.DeviceCategories, "Id", "DeviceCatName", null);
                ViewBag.DeviceCatId = FunctionsGeneral.AddDefaultOption(ViewBag.DeviceCatId, "", "null");
                ViewBag.ToolCatId = new SelectList(db.ToolCategories, "Id", "ToolCatName");
                ViewBag.ToolCatId = FunctionsGeneral.AddDefaultOption(ViewBag.ToolCatId, "", "null");
                ViewBag.PaggingDeviceAndTool = PaggingDeviceAndTool;
                return View(deviceAndTool.ToList());
            }
        }

        public ActionResult UpdateCheck()
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
            ViewBag.Message = "";
            return PartialView("_UpdateCheck");
        }
        [HttpPost]
        public ActionResult UpdateCheck(HttpPostedFileBase fileUpload)
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
                if (fileUpload == null)
                {
                    ModelState.AddModelError("File", "Chọn file để tải lên");
                }
                else if (fileUpload.ContentLength > 0)
                {
                    int MaxContentLength = 1024 * 1024 * 4; //Size = 4 MB
                    string[] AllowedFileExtensions = new string[] { ".xls", ".xlsx" };
                    if (!AllowedFileExtensions.Contains
                    (fileUpload.FileName.Substring(fileUpload.FileName.LastIndexOf('.'))))
                    {
                        ModelState.AddModelError("File", "Chỉ chọn các file: " + string.Join(", ", AllowedFileExtensions));
                    }
                    else if (fileUpload.ContentLength > MaxContentLength)
                    {
                        ModelState.AddModelError("File", "File quá lớn, giới hạn dung lượng file là: " + MaxContentLength + " MB");
                    }
                    else
                    {
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        var path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                        fileUpload.SaveAs(path);
                        ModelState.Clear();
                        MSExcelReaderHD ExcelReaderHD = new MSExcelReaderHD(path);
                        ExcelReaderHD.UploadKiemKe();
                        ModelState.AddModelError("File", "Đã tải xong!");
                    }
                }
            }
            return PartialView("_UpdateCheck");
        }
        public ActionResult ExportBarcode(DeviceToolFilter dvtFilter)
        {
            GroupUser_Authorize gruAu;
            var userSession = CheckPermission.CheckControler(this, User.Identity.Name, out gruAu);
            if (userSession == null)
            {
                return null;
            }
            else
            {
                if (gruAu == null)
                    return RedirectToAction("PermitError", "Error");
            }
            userSession = db.UserInfoes.Where(u => u.Id == userSession.Id).FirstOrDefault();
            if (userSession == null)
            {
                return null;
            }
            var companiesId = userSession.Companies.Select(c => c.Id).ToList();

            IQueryable<DeviceAndTool> deviceAndTool = db.DeviceAndTools.Include(dv => dv.HistoryUses);

            deviceAndTool = deviceAndTool.Where(dv => companiesId.Contains(dv.CompanyId));

            int currentPageIndex = dvtFilter.page ?? 1;

            int skip = (currentPageIndex - 1) * defaultPageSize;

            if (dvtFilter.maTS != null)
                deviceAndTool = deviceAndTool.Where(dvt => dvt.AssetsCode.Contains(dvtFilter.maTS));

            if (dvtFilter.staffId != null)
                deviceAndTool = deviceAndTool.Where(dvt => dvt.HistoryUses.OrderByDescending(h => h.HandedDate).FirstOrDefault().HandedToStaffId == dvtFilter.staffId);

            if (dvtFilter.statusId != null)
                deviceAndTool = deviceAndTool.Where(dvt => dvt.HistoryUses.OrderByDescending(h => h.HandedDate).FirstOrDefault().StatusId == dvtFilter.statusId);

            if (dvtFilter.deviceCatId != null && dvtFilter.toolCatId != null)
            {
                deviceAndTool = deviceAndTool.Where(dvt => dvt.DeviceCatId == dvtFilter.deviceCatId || dvt.ToolCatId == dvtFilter.toolCatId);
            }
            else
            {
                if (dvtFilter.deviceCatId != null)
                {
                    deviceAndTool = deviceAndTool.Where(dvt => dvt.DeviceCatId == dvtFilter.deviceCatId);
                }
                if (dvtFilter.toolCatId != null)
                {
                    deviceAndTool = deviceAndTool.Where(dvt => dvt.ToolCatId == dvtFilter.toolCatId);
                }
            }


            if (dvtFilter.deptId != null)
                deviceAndTool = deviceAndTool.Where(dvt => dvt.HistoryUses.OrderByDescending(h => h.HandedDate).FirstOrDefault().DeptId == dvtFilter.deptId);

            if (dvtFilter.companyId != null)
                deviceAndTool = deviceAndTool.Where(dvt => dvt.CompanyId == dvtFilter.companyId);

            if (dvtFilter.AdminOrId != null)
            {
                if (dvtFilter.AdminOrId == 0)
                {
                    var deviceCat = db.DeviceCategories.Where(dc => dc.Type == (DeviceCategory.TypeDevice)dvtFilter.AdminOrId).Select(dvc => dvc.Id).ToArray();
                    deviceAndTool = deviceAndTool.Where(dvt => dvt.DeviceCatId == null || deviceCat.Contains(dvt.DeviceCatId.Value));
                }
                if (dvtFilter.AdminOrId == 1)
                {
                    var deviceCat = db.DeviceCategories.Where(dc => dc.Type == (DeviceCategory.TypeDevice)dvtFilter.AdminOrId).Select(dvc => dvc.Id).ToArray();
                    deviceAndTool = deviceAndTool.Where(dvt => dvt.DeviceCatId != null && deviceCat.Contains(dvt.DeviceCatId.Value));
                }
            }
            if (dvtFilter.locatioId != null)
            {
                var location = db.Locations.Find(dvtFilter.locatioId);
                if (location != null)
                {
                    string sqlQuery = "WITH    q AS " +
                                        "(" +
                                        "SELECT  * " +
                                        "FROM    Location " +
                                        "WHERE  Id = " + location.Id + " " +
                                        "UNION ALL " +
                                        "SELECT  m.* " +
                                        "FROM    Location m " +
                                        "JOIN    q " +
                                        "ON      m.parentID = q.Id" +
                                        ")" +
                                    "SELECT  Id " +
                                    "FROM    q";

                    var locationIds = db.Database.SqlQuery<int>(sqlQuery).ToArray();
                    deviceAndTool = deviceAndTool.Where(dvt => locationIds.Contains(dvt.HistoryUses.OrderByDescending(h => h.HandedDate).FirstOrDefault().LocationId.Value));
                }
            }

            DateTime? startDate = new DateTime();
            DateTime? endDate = new DateTime();
            SetFromDateToDate(dvtFilter.fromDate, dvtFilter.toDate, ref startDate, ref endDate);

            if (startDate.HasValue)
            {
                if (endDate.HasValue)
                    deviceAndTool = deviceAndTool.Where(e => e.CheckedDate >= startDate && e.CheckedDate <= endDate);
                else
                    deviceAndTool = deviceAndTool.Where(e => e.CheckedDate >= startDate);
            }
            else
            {
                if (endDate.HasValue)
                    deviceAndTool = deviceAndTool.Where(e => e.CheckedDate <= endDate);
            }

            int totalRow = deviceAndTool.Count();

            deviceAndTool = deviceAndTool.OrderByDescending(e => e.Id).Skip(skip).Take(defaultPageSize);

            List<ImagePrinterBarcode> listImage = new List<ImagePrinterBarcode>();

            FileInfo file = new FileInfo(Server.MapPath(@"~/ExcelTemp/ExportBarcode.xlsx"));
            ExcelPackage pck = new ExcelPackage(file);
            var ws = pck.Workbook.Worksheets[1];

            ws.Cells[1, 1].Value ="Mã";
            ws.Cells[1, 2].Value = "Tên";
            ws.Cells[1, 3].Value = "Mô tả";
            ws.Cells[1, 4].Value = "Loại";
            ws.Cells[1, 5].Value = "Loại";
            ws.Cells[1, 6].Value = "Ngày mua";
            ws.Cells[1, 7].Value = "Người dùng/Phòng ban";
            ws.Cells[1, 8].Value = "Vị trí";
            ws.Cells[1, 9].Value = "Ngày bàn giao";
            ws.Cells[1, 10].Value = "Tình trạng";

            int row = 2;
            foreach (var _asert in deviceAndTool)
            {
                //Project prj = (Project)group.Key;
                listImage.Add(new ImagePrinterBarcode(_asert.AssetsCode, GetbarCodeImage(_asert.Id)));

                ws.Cells[row, 1].Value = _asert.AssetsCode;
                ws.Cells[row, 2].Value = _asert.DeviceName;
                ws.Cells[row, 3].Value = _asert.DescriptionDevice;
                ws.Cells[row, 4].Value = _asert.DeviceCategory != null ? _asert.DeviceCategory.DeviceCatName : "";
                ws.Cells[row, 5].Value = _asert.ToolCategory != null ? _asert.ToolCategory.ToolCatName : "";

                ws.Cells[row, 6].Value = _asert.BuyDate;
                ws.Cells[row, 6].Style.Numberformat.Format = "dd-MM-yy";

                var history = _asert.HistoryUses.OrderByDescending(hu => hu.HandedDate).First();
                if (history.HandedToStaffId != null)
                    ws.Cells[row, 7].Value = history.Staff.FullName;
                else
                {
                    if (history.Department != null)
                        ws.Cells[row, 7].Value = history.Department.DeptName;
                }
                if (history.Location != null)
                    ws.Cells[row, 8].Value = history.Location.ShortName;

                ws.Cells[row, 9].Value = history.HandedDate;
                ws.Cells[row, 9].Style.Numberformat.Format = "dd-MM-yy";

                ws.Cells[row, 10].Value = history.StatusCategory.Name;

                //ws.Cells[row, 11].Value = @"D:\\Printer barcode\\" + _asert.AssetsCode + ".png";

                //using (var range = ws.Cells[row, 1, row, 7])
                //{
                //    range.Style.Font.Bold = true;
                //    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 153, 255));
                //    range.Style.Font.Color.SetColor(Color.FromArgb(0, 32, 96));
                //}                                
                row++;
            }

            //
            int Total_Column = 10;

            //
            for (int x1 = 1; x1 <= Total_Column; x1++)
            {
                ws.Column(x1).AutoFit();
            }

            //using (var range = ws.Cells[1, 1, row, 9])
            //{
            //    range.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
            //    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            //    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //}

            var outputStream = new MemoryStream();
            using (var zip = new ZipFile())
            {
                zip.AddEntry("Bien ban.xlsx", pck.GetAsByteArray());
                MemoryStream imageStream;
                foreach (var image in listImage)
                {
                    imageStream = new MemoryStream();
                    image.imageCode.Save(imageStream, System.Drawing.Imaging.ImageFormat.Png);
                    zip.AddEntry(image.code + ".png", imageStream.ToArray());
                }
                zip.Save(outputStream);
            }
            FileContentResult result = new FileContentResult(outputStream.ToArray(), "application / zip")
            {
                FileDownloadName = "Printer barcode.zip"
            };
            return result;
        }
        public ActionResult ExportBarcodeAll(DeviceToolFilter dvtFilter)
        {
            GroupUser_Authorize gruAu;
            var userSession = CheckPermission.CheckControler("Report", "ExportBarcode", User.Identity.Name, out gruAu);
            if (userSession == null)
            {
                return null;
            }
            else
            {
                if (gruAu == null)
                    return RedirectToAction("PermitError", "Error");
            }
            userSession = db.UserInfoes.Where(u => u.Id == userSession.Id).FirstOrDefault();
            if (userSession == null)
            {
                return null;
            }
            var companiesId = userSession.Companies.Select(c => c.Id).ToList();

            IQueryable<DeviceAndTool> deviceAndTool = db.DeviceAndTools.Include(dv => dv.HistoryUses);

            deviceAndTool = deviceAndTool.Where(dv => companiesId.Contains(dv.CompanyId));

            int currentPageIndex = dvtFilter.page ?? 1;

            int skip = (currentPageIndex - 1) * defaultPageSize;

            if (dvtFilter.maTS != null)
                deviceAndTool = deviceAndTool.Where(dvt => dvt.AssetsCode.Contains(dvtFilter.maTS));

            if (dvtFilter.staffId != null)
                deviceAndTool = deviceAndTool.Where(dvt => dvt.HistoryUses.OrderByDescending(h => h.HandedDate).FirstOrDefault().HandedToStaffId == dvtFilter.staffId);

            if (dvtFilter.statusId != null)
                deviceAndTool = deviceAndTool.Where(dvt => dvt.HistoryUses.OrderByDescending(h => h.HandedDate).FirstOrDefault().StatusId == dvtFilter.statusId);

            if (dvtFilter.deviceCatId != null && dvtFilter.toolCatId != null)
            {
                deviceAndTool = deviceAndTool.Where(dvt => dvt.DeviceCatId == dvtFilter.deviceCatId || dvt.ToolCatId == dvtFilter.toolCatId);
            }
            else
            {
                if (dvtFilter.deviceCatId != null)
                {
                    deviceAndTool = deviceAndTool.Where(dvt => dvt.DeviceCatId == dvtFilter.deviceCatId);
                }
                if (dvtFilter.toolCatId != null)
                {
                    deviceAndTool = deviceAndTool.Where(dvt => dvt.ToolCatId == dvtFilter.toolCatId);
                }
            }


            if (dvtFilter.deptId != null)
                deviceAndTool = deviceAndTool.Where(dvt => dvt.HistoryUses.OrderByDescending(h => h.HandedDate).FirstOrDefault().DeptId == dvtFilter.deptId);

            if (dvtFilter.companyId != null)
                deviceAndTool = deviceAndTool.Where(dvt => dvt.CompanyId == dvtFilter.companyId);

            if (dvtFilter.AdminOrId != null)
            {
                if (dvtFilter.AdminOrId == 0)
                {
                    var deviceCat = db.DeviceCategories.Where(dc => dc.Type == (DeviceCategory.TypeDevice)dvtFilter.AdminOrId).Select(dvc => dvc.Id).ToArray();
                    deviceAndTool = deviceAndTool.Where(dvt => dvt.DeviceCatId == null || deviceCat.Contains(dvt.DeviceCatId.Value));
                }
                if (dvtFilter.AdminOrId == 1)
                {
                    var deviceCat = db.DeviceCategories.Where(dc => dc.Type == (DeviceCategory.TypeDevice)dvtFilter.AdminOrId).Select(dvc => dvc.Id).ToArray();
                    deviceAndTool = deviceAndTool.Where(dvt => dvt.DeviceCatId != null && deviceCat.Contains(dvt.DeviceCatId.Value));
                }
            }
            if (dvtFilter.locatioId != null)
            {
                var location = db.Locations.Find(dvtFilter.locatioId);
                if (location != null)
                {
                    string sqlQuery = "WITH    q AS " +
                                        "(" +
                                        "SELECT  * " +
                                        "FROM    Location " +
                                        "WHERE  Id = " + location.Id + " " +
                                        "UNION ALL " +
                                        "SELECT  m.* " +
                                        "FROM    Location m " +
                                        "JOIN    q " +
                                        "ON      m.parentID = q.Id" +
                                        ")" +
                                    "SELECT  Id " +
                                    "FROM    q";

                    var locationIds = db.Database.SqlQuery<int>(sqlQuery).ToArray();
                    deviceAndTool = deviceAndTool.Where(dvt => locationIds.Contains(dvt.HistoryUses.OrderByDescending(h => h.HandedDate).FirstOrDefault().LocationId.Value));
                }
            }

            DateTime? startDate = new DateTime();
            DateTime? endDate = new DateTime();
            SetFromDateToDate(dvtFilter.fromDate, dvtFilter.toDate, ref startDate, ref endDate);

            if (startDate.HasValue)
            {
                if (endDate.HasValue)
                    deviceAndTool = deviceAndTool.Where(e => e.CheckedDate >= startDate && e.CheckedDate <= endDate);
                else
                    deviceAndTool = deviceAndTool.Where(e => e.CheckedDate >= startDate);
            }
            else
            {
                if (endDate.HasValue)
                    deviceAndTool = deviceAndTool.Where(e => e.CheckedDate <= endDate);
            }

            int totalRow = deviceAndTool.Count();

            deviceAndTool = deviceAndTool.OrderByDescending(e => e.Id);

            List<ImagePrinterBarcode> listImage = new List<ImagePrinterBarcode>();

            FileInfo file = new FileInfo(Server.MapPath(@"~/ExcelTemp/ExportBarcode.xlsx"));
            ExcelPackage pck = new ExcelPackage(file);
            var ws = pck.Workbook.Worksheets[1];

            ws.Cells[1, 1].Value = "Mã";
            ws.Cells[1, 2].Value = "Tên";
            ws.Cells[1, 3].Value = "Mô tả";
            ws.Cells[1, 4].Value = "Loại";
            ws.Cells[1, 5].Value = "Loại";
            ws.Cells[1, 6].Value = "Ngày mua";
            ws.Cells[1, 7].Value = "Người dùng/Phòng ban";
            ws.Cells[1, 8].Value = "Vị trí";
            ws.Cells[1, 9].Value = "Ngày bàn giao";
            ws.Cells[1, 10].Value = "Tình trạng";

            int row = 2;
            foreach (var _asert in deviceAndTool)
            {
                //Project prj = (Project)group.Key;
                listImage.Add(new ImagePrinterBarcode(_asert.AssetsCode, GetbarCodeImage(_asert.Id)));

                ws.Cells[row, 1].Value = _asert.AssetsCode;
                ws.Cells[row, 2].Value = _asert.DeviceName;
                ws.Cells[row, 3].Value = _asert.DescriptionDevice;
                ws.Cells[row, 4].Value = _asert.DeviceCategory != null ? _asert.DeviceCategory.DeviceCatName : "";
                ws.Cells[row, 5].Value = _asert.ToolCategory != null ? _asert.ToolCategory.ToolCatName : "";

                ws.Cells[row, 6].Value = _asert.BuyDate;
                ws.Cells[row, 6].Style.Numberformat.Format = "dd-MM-yy";

                var history = _asert.HistoryUses.OrderByDescending(hu => hu.HandedDate).First();
                if (history.HandedToStaffId != null)
                    ws.Cells[row, 7].Value = history.Staff.FullName;
                else
                {
                    if (history.Department != null)
                        ws.Cells[row, 7].Value = history.Department.DeptName;
                }
                if (history.Location != null)
                    ws.Cells[row, 8].Value = history.Location.ShortName;

                ws.Cells[row, 9].Value = history.HandedDate;
                ws.Cells[row, 9].Style.Numberformat.Format = "dd-MM-yy";

                ws.Cells[row, 10].Value = history.StatusCategory.Name;

                //ws.Cells[row, 11].Value = @"D:\\Printer barcode\\" + _asert.AssetsCode + ".png";

                //using (var range = ws.Cells[row, 1, row, 7])
                //{
                //    range.Style.Font.Bold = true;
                //    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 153, 255));
                //    range.Style.Font.Color.SetColor(Color.FromArgb(0, 32, 96));
                //}                                
                row++;
            }

            //
            int Total_Column = 10;

            //
            for (int x1 = 1; x1 <= Total_Column; x1++)
            {
                ws.Column(x1).AutoFit();
            }

            //using (var range = ws.Cells[1, 1, row, 9])
            //{
            //    range.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
            //    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            //    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //}

            var outputStream = new MemoryStream();
            using (var zip = new ZipFile())
            {
                zip.AddEntry("Bien ban.xlsx", pck.GetAsByteArray());
                MemoryStream imageStream;
                foreach (var image in listImage)
                {
                    imageStream = new MemoryStream();
                    image.imageCode.Save(imageStream, System.Drawing.Imaging.ImageFormat.Png);
                    zip.AddEntry(image.code + ".png", imageStream.ToArray());
                }
                zip.Save(outputStream);
            }
            FileContentResult result = new FileContentResult(outputStream.ToArray(), "application / zip")
            {
                FileDownloadName = "Printer barcode.zip"
            };
            return result;
        }
        public Image GetbarCodeImage(int? id)
        {
            DeviceAndTool deviceAndTool = db.DeviceAndTools.Find(id);
            if (deviceAndTool == null)
            {
                return null;
            }
            BarcodeSettings setting = new BarcodeSettings();
            setting.Data = deviceAndTool.AssetsCode;
            setting.Type = BarCodeType.Code128;
            setting.ShowText = false;
            setting.BarHeight = 10;
            setting.LeftMargin = 0;
            setting.RightMargin = 0;
            BarCodeGenerator bar = new BarCodeGenerator(setting);

            Image image = bar.GenerateImage();
            return image;
        }
        private static void SetFromDateToDate(string fromDate, string toDate, ref DateTime? startDate, ref DateTime? endDate)
        {   
            DateTime dt;
            if (!String.IsNullOrEmpty(fromDate))
            {
                if (DateTime.TryParseExact(fromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                {
                    startDate = dt.Date;
                    startDate = startDate.Value.AddHours(24);
                }
                else
                    startDate = null;
            }
            else
                startDate = null;
            if (!String.IsNullOrEmpty(toDate))
            {
                if (DateTime.TryParseExact(toDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                {
                    endDate = dt.Date;
                    endDate = endDate.Value.AddHours(24);
                }
                else
                    endDate = null;
            }
            else
                endDate = null;
        }

    }
}
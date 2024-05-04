using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AMS.Models;

using Spire.Barcode;
using System.Drawing;
using System.IO;
using Ionic.Zip;

using OfficeOpenXml;

namespace AMS.Controllers
{
    public class DeviceAndToolsController : Controller
    {
        private AMSEntities db = new AMSEntities();
        private const int defaultPageSize = 30;

        // GET: DeviceAndTools
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

        public ActionResult PrinterBarcode(DeviceToolFilter dvtFilter)
        {
            GroupUser_Authorize gruAu;
            var userSession = CheckPermission.CheckControler("DeviceAndTools", "Index", User.Identity.Name, out gruAu);
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

            int totalRow = deviceAndTool.Count();

            deviceAndTool = deviceAndTool.OrderByDescending(e => e.Id);

            return PartialView("_PrintBarcode", deviceAndTool);
        }

        public FileContentResult ExportBarcode(DeviceToolFilter dvtFilter)
        {
            GroupUser_Authorize gruAu;
            var userSession = CheckPermission.CheckControler("DeviceAndTools", "Index", User.Identity.Name, out gruAu);
            if (userSession == null)
            {
                return null;
            }
            else
            {
                if (gruAu == null)
                    return null;
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

            int totalRow = deviceAndTool.Count();

            deviceAndTool = deviceAndTool.OrderByDescending(e => e.Id).Skip(skip).Take(defaultPageSize);

            List<ImagePrinterBarcode> listImage = new List<ImagePrinterBarcode>();

            FileInfo file = new FileInfo(Server.MapPath(@"~/ExcelTemp/ExportBarcode.xlsx"));
            ExcelPackage pck = new ExcelPackage(file);
            var ws = pck.Workbook.Worksheets[1];

            pck.DoAdjustDrawings = false;

            //ws.Cells[4, 2].Value = staff.FullName.ToString();
            //ws.Cells[5, 2].Value = staff.Department.Company.NameVn.ToString();
            //ws.Cells[5, 4].Value = staff.Department.DeptName.ToString();

            int Count = 1;

            int row = 1;
            foreach (var _asert in deviceAndTool)
            {
                ImagePrinterBarcode BarCodeImg = new ImagePrinterBarcode(_asert.AssetsCode, GetbarCodeImage(_asert.Id));

                if ((Count > 1) && ((Count % 19) == 0))
                {
                    row += 2;
                }

                if (((Count - 1) % 2) == 0)
                {
                    ws.Row(row).Height = 15;
                    ws.Row(row + 1).Height = 15;
                    ws.Row(row + 2).Height = 45;

                    //
                    ws.Cells[row, 1].Value = "Mã tài sản: " + _asert.AssetsCode;
                    ws.Cells[row + 1, 1].Value = "Tên tài sản: " + _asert.DeviceName;

                    var picture = ws.Drawings.AddPicture(Guid.NewGuid().ToString(), BarCodeImg.imageCode);
                    picture.From.Column = 0;
                    picture.From.Row = row + 1;
                    picture.SetSize(246, 45);
                }
                else
                {
                    ws.Cells[row, 3].Value = "Mã tài sản: " + _asert.AssetsCode;
                    ws.Cells[row + 1, 3].Value = "Tên tài sản: " + _asert.DeviceName;

                    var picture = ws.Drawings.AddPicture(Guid.NewGuid().ToString(), BarCodeImg.imageCode);
                    picture.From.Column = 2;
                    picture.From.Row = row + 1;
                    picture.SetSize(246, 45);

                    //
                    row += 3;
                }

                Count++;
            }

            //
            FileContentResult result = new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                 FileDownloadName = "Barcode_" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + "-" + DateTime.Now.Millisecond + ".xlsx"
            };

            //
            return result;
        }

        public ActionResult ExportBarcodeAll(DeviceToolFilter dvtFilter)
        {
            GroupUser_Authorize gruAu;
            var userSession = CheckPermission.CheckControler("DeviceAndTools", "Index", User.Identity.Name, out gruAu);
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
        public ActionResult SearchToSelect(DeviceToolFilter dvtFilter)
        {
            var userSession = db.UserInfoes.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (userSession == null)
            {
                return HttpNotFound();
            }

            var comArray = userSession.Companies.Select(c => c.Id).ToArray();

            IQueryable<DeviceAndTool> deviceAndTool = db.DeviceAndTools.Where(dt => comArray.Contains(dt.CompanyId));
            if (dvtFilter != null)
            {
                if (dvtFilter.companyId == null & dvtFilter.maTS == null & dvtFilter.staffId == null & dvtFilter.statusId == null & dvtFilter.deptId == null)
                {
                    deviceAndTool = deviceAndTool.Take(0);
                }
                else
                {
                    if (dvtFilter.maTS != null)
                        deviceAndTool = deviceAndTool.Where(dvt => dvt.AssetsCode.Contains(dvtFilter.maTS));

                    if (dvtFilter.staffId != null)
                        deviceAndTool = deviceAndTool.Where(dvt => dvt.HistoryUses.OrderByDescending(h => h.HandedDate).FirstOrDefault().HandedToStaffId == dvtFilter.staffId);

                    if (dvtFilter.statusId != null)
                        deviceAndTool = deviceAndTool.Where(dvt => dvt.HistoryUses.OrderByDescending(h => h.HandedDate).FirstOrDefault().StatusId == dvtFilter.statusId);

                    if (dvtFilter.deptId != null)
                        deviceAndTool = deviceAndTool.Where(dvt => dvt.HistoryUses.OrderByDescending(h => h.HandedDate).FirstOrDefault().DeptId == dvtFilter.deptId);

                    if (dvtFilter.companyId != null)
                        deviceAndTool = deviceAndTool.Where(dvt => dvt.CompanyId == dvtFilter.companyId);
                }
            }
            else
            {
                deviceAndTool = deviceAndTool.Take(0);
            }
            return PartialView("_SearchToSelect", deviceAndTool.OrderByDescending(dt => dt.DeviceName).ToList());
        }
        // GET: DeviceAndTools/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceAndTool deviceAndTool = db.DeviceAndTools.Find(id);
            if (deviceAndTool == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Details", deviceAndTool);
        }

        public ActionResult GetbarCode(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceAndTool deviceAndTool = db.DeviceAndTools.Find(id);
            if (deviceAndTool == null)
            {
                return HttpNotFound();
            }
            Image image = GetbarCodeImage(deviceAndTool.Id);
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            byte[] cover = ms.ToArray();
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
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

        // GET: DeviceAndTools/Create
        public ActionResult Create(string typ)
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
            userSession = db.UserInfoes.Find(userSession.Id);
            ViewBag.CompanyId = new SelectList(userSession.Companies, "Id", "NameVn");

            if (typ == "vp")
            {
                ViewBag.DeviceCatId = new SelectList(db.DeviceCategories.Where(dv => dv.Type == DeviceCategory.TypeDevice.VP), "Id", "DeviceCatName");
            }
            if (typ == "it")
            {
                ViewBag.DeviceCatId = new SelectList(db.DeviceCategories.Where(dv => dv.Type == DeviceCategory.TypeDevice.IT), "Id", "DeviceCatName");
            }
            if (typ == "cc")
            {
                ViewBag.ToolCatId = new SelectList(db.ToolCategories, "Id", "ToolCatName");
            }
            ViewBag.StatusId = new SelectList(db.StatusCategories, "Id", "Name");
            return PartialView("_Create");
        }

        // POST: DeviceAndTools/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyId,DeviceCatId,ToolCatId,AutoAssetsCode,AssetsCode,DeviceName,DescriptionDevice,BuyDate,StaffId,DeptId,LocationId,StatusId,StatusDescription,HandedDate")] DeviceToolAndHistory deviceToolAndHistory)
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
            if (deviceToolAndHistory.DeviceCatId != null)
            {
                var deviceCat = db.DeviceCategories.Where(dvc => dvc.Id == deviceToolAndHistory.DeviceCatId).FirstOrDefault();
                if (deviceCat != null)
                {
                    if (deviceCat.Type == DeviceCategory.TypeDevice.VP)
                        ViewBag.DeviceCatId = new SelectList(db.DeviceCategories.Where(dv => dv.Type == DeviceCategory.TypeDevice.VP), "Id", "DeviceCatName");
                    else
                        ViewBag.DeviceCatId = new SelectList(db.DeviceCategories.Where(dv => dv.Type == DeviceCategory.TypeDevice.IT), "Id", "DeviceCatName");
                }
                else
                {
                    ViewBag.DeviceCatId = new SelectList(db.DeviceCategories, "Id", "DeviceCatName", deviceToolAndHistory.DeviceCatId);
                }
            }
            ViewBag.StatusId = new SelectList(db.StatusCategories, "Id", "Name", deviceToolAndHistory.StatusId);

            if (deviceToolAndHistory.ToolCatId != null)
            {
                ViewBag.ToolCatId = new SelectList(db.ToolCategories, "Id", "ToolCatName", deviceToolAndHistory.ToolCatId);
            }

            if (checkExitDeviceAndTool(deviceToolAndHistory.AssetsCode, null))
            {
                ModelState.AddModelError("AssetsCode", "Mã tài sản đã có trong dữ liệu!");
            }
            if (!deviceToolAndHistory.AutoAssetsCode)
            {
                if (String.IsNullOrEmpty(deviceToolAndHistory.AssetsCode))
                {
                    ModelState.AddModelError("AssetsCode", "Chưa nhập mã cho tài sản!");
                }
            }
            if (deviceToolAndHistory.StaffId == null && deviceToolAndHistory.DeptId == null && deviceToolAndHistory.LocationId == null && deviceToolAndHistory.StatusId != 5)
            {
                ModelState.AddModelError(string.Empty, "Cần phải nhập người sử dụng tài sản hoặc phòng ban hoặc địa điểm của tài sản!");
            }
            if (deviceToolAndHistory.BuyDate > deviceToolAndHistory.HandedDate)
            {
                ModelState.AddModelError(string.Empty, "Ngày mua và ngày bàn giao không hợp lệ!");
            }
            if (ModelState.IsValid)
            {
                AMS.Repositories.DeviceAndToolReposity.CreateDeviceAndTool(deviceToolAndHistory, userSession.Id);
                return Json(new { success = true });
            }
            userSession = db.UserInfoes.Find(userSession.Id);
            ViewBag.CompanyId = new SelectList(userSession.Companies, "Id", "NameVn");
            return PartialView("_Create", deviceToolAndHistory);

        }

        public ActionResult CreateToStaff(int Id)
        {
            GroupUser_Authorize gruAu;
            var userSession = CheckPermission.CheckControler("DeviceAndTools", "Create", User.Identity.Name, out gruAu);
            if (userSession == null)
            {
                return RedirectToAction("UserLogin", "Error");
            }
            else
            {
                if (gruAu == null)
                    return RedirectToAction("PermitErrorPop", "Error");
            }
            userSession = db.UserInfoes.Find(userSession.Id);

            UserInfo staff = db.UserInfoes.Find(Id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            DeviceToolAndHistory dvth = new DeviceToolAndHistory();
            dvth.StaffId = Id;
            dvth.HandedDate = DateTime.Now;

            ViewBag.Type = "vp";

            ViewBag.DeviceVPId = new SelectList(db.DeviceCategories.Where(dv => dv.Type == DeviceCategory.TypeDevice.VP), "Id", "DeviceCatName");
            ViewBag.DeviceITId = new SelectList(db.DeviceCategories.Where(dv => dv.Type == DeviceCategory.TypeDevice.IT), "Id", "DeviceCatName");
            ViewBag.ToolCatId = new SelectList(db.ToolCategories, "Id", "ToolCatName");
            ViewBag.CompanyId = new SelectList(userSession.Companies, "Id", "NameVn");
            ViewBag.StatusId = new SelectList(db.StatusCategories.Where(st => st.Id != 5), "Id", "Name");
            return PartialView("_CreateToStaff", dvth);
        }

        // POST: DeviceAndTools/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateToStaff([Bind(Include = "Id,CompanyId,DeviceCatId,ToolCatId,AutoAssetsCode,AssetsCode,DeviceName,DescriptionDevice,BuyDate,StaffId,StatusId,StatusDescription,HandedDate")] DeviceToolAndHistory deviceToolAndHistory, string optToolDevice)
        {
            GroupUser_Authorize gruAu;
            var userSession = CheckPermission.CheckControler("DeviceAndTools", "Create", User.Identity.Name, out gruAu);
            if (userSession == null)
            {
                return RedirectToAction("UserLogin", "Error");
            }
            else
            {
                if (gruAu == null)
                    return RedirectToAction("PermitErrorPop", "Error");
            }
            if (deviceToolAndHistory.DeviceCatId != null)
            {
                var deviceCat = db.DeviceCategories.Where(dvc => dvc.Id == deviceToolAndHistory.DeviceCatId).FirstOrDefault();
                if (deviceCat != null)
                {
                    if (deviceCat.Type == DeviceCategory.TypeDevice.VP)
                        ViewBag.DeviceCatId = new SelectList(db.DeviceCategories.Where(dv => dv.Type == DeviceCategory.TypeDevice.VP), "Id", "DeviceCatName");
                    else
                        ViewBag.DeviceCatId = new SelectList(db.DeviceCategories.Where(dv => dv.Type == DeviceCategory.TypeDevice.IT), "Id", "DeviceCatName");
                }
                else
                {
                    ViewBag.DeviceCatId = new SelectList(db.DeviceCategories, "Id", "DeviceCatName", deviceToolAndHistory.DeviceCatId);
                }
            }
            else
            {
                ViewBag.DeviceCatId = new SelectList(db.DeviceCategories, "Id", "DeviceCatName", deviceToolAndHistory.DeviceCatId);
            }
            ViewBag.StatusId = new SelectList(db.StatusCategories.Where(st => st.Id != 5), "Id", "Name", deviceToolAndHistory.StatusId);

            if (deviceToolAndHistory.ToolCatId != null)
            {
                ViewBag.ToolCatId = new SelectList(db.ToolCategories, "Id", "ToolCatName", deviceToolAndHistory.ToolCatId);
            }

            if (checkExitDeviceAndTool(deviceToolAndHistory.AssetsCode, null))
            {
                ModelState.AddModelError("AssetsCode", "Mã tài sản đã có trong dữ liệu!");
            }
            if (!deviceToolAndHistory.AutoAssetsCode)
            {
                if (String.IsNullOrEmpty(deviceToolAndHistory.AssetsCode))
                {
                    ModelState.AddModelError("AssetsCode", "Chưa nhập mã cho tài sản!");
                }
            }
            if (deviceToolAndHistory.BuyDate > deviceToolAndHistory.HandedDate)
            {
                ModelState.AddModelError(string.Empty, "Ngày mua và ngày bàn giao không hợp lệ!");
            }
            if (ModelState.IsValid)
            {
                AMS.Repositories.DeviceAndToolReposity.CreateDeviceAndTool(deviceToolAndHistory, userSession.Id);
                return Json(new { success = true });
            }
            userSession = db.UserInfoes.Find(userSession.Id);

            ViewBag.CompanyId = new SelectList(userSession.Companies, "Id", "NameVn", deviceToolAndHistory.CompanyId);
            ViewBag.Type = optToolDevice;

            ViewBag.DeviceVPId = new SelectList(db.DeviceCategories.Where(dv => dv.Type == DeviceCategory.TypeDevice.VP), "Id", "DeviceCatName", deviceToolAndHistory.DeviceCatId);

            ViewBag.DeviceITId = new SelectList(db.DeviceCategories.Where(dv => dv.Type == DeviceCategory.TypeDevice.IT), "Id", "DeviceCatName", deviceToolAndHistory.DeviceCatId);

            ViewBag.ToolCatId = new SelectList(db.ToolCategories, "Id", "ToolCatName", deviceToolAndHistory.ToolCatId);

            ViewBag.CompanyId = new SelectList(userSession.Companies, "Id", "NameVn");
            ViewBag.StatusId = new SelectList(db.StatusCategories.Where(st => st.Id != 5), "Id", "Name");
            return PartialView("_CreateToStaff", deviceToolAndHistory);

        }

        bool checkExitDeviceAndTool(string code, int? exepId)
        {
            int countExit = 0;
            if (exepId != null)
            {
                countExit = db.DeviceAndTools.Where(tc => (tc.AssetsCode == code) && tc.Id
                    != exepId).Count();
            }
            else
            {
                countExit = db.DeviceAndTools.Where(tc => (tc.AssetsCode == code) && tc.Id
                   != exepId).Count();
            }
            return countExit > 0 ? true : false;
        }
        // GET: DeviceAndTools/Edit/5
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
            DeviceAndTool deviceAndTool = db.DeviceAndTools.Find(id);
            if (deviceAndTool == null)
            {
                return HttpNotFound();
            }
            userSession = db.UserInfoes.Find(userSession.Id);
            if (deviceAndTool.Company != null)
                ViewBag.CompanyId = new SelectList(userSession.Companies, "Id", "NameVn", deviceAndTool.CompanyId);
            else
                ViewBag.CompanyId = new SelectList(userSession.Companies, "Id", "NameVn");
            CallBackDropdown(deviceAndTool);
            return PartialView("_Edit", deviceAndTool);
        }

        public void CallBackDropdown(DeviceAndTool deviceAndTool)
        {
            ViewBag.Type = "vp";
            if (deviceAndTool.DeviceCategory != null)
            {
                if (deviceAndTool.DeviceCategory.Type == DeviceCategory.TypeDevice.VP)
                    ViewBag.Type = "vp";
                if (deviceAndTool.DeviceCategory.Type == DeviceCategory.TypeDevice.IT)
                    ViewBag.Type = "it";
            }
            if (deviceAndTool.ToolCategory != null)
                ViewBag.Type = "cc";
            ViewBag.DeviceVPId = new SelectList(db.DeviceCategories.Where(dv => dv.Type == DeviceCategory.TypeDevice.VP), "Id", "DeviceCatName", deviceAndTool.DeviceCatId);
            ViewBag.DeviceITId = new SelectList(db.DeviceCategories.Where(dv => dv.Type == DeviceCategory.TypeDevice.IT), "Id", "DeviceCatName", deviceAndTool.DeviceCatId);
            ViewBag.ToolCatId = new SelectList(db.ToolCategories, "Id", "ToolCatName", deviceAndTool.ToolCatId);
        }
        // POST: DeviceAndTools/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyId,AssetsCode,DeviceName,DescriptionDevice,BuyDate")] DeviceAndTool deviceAndTool, int? DeviceVPId, int? DeviceITId, int? ToolCatId, string optToolDevice)
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

            if (optToolDevice == "cc")
            {
                deviceAndTool.DeviceCatId = null;
                deviceAndTool.ToolCatId = ToolCatId;
            }
            else
            {
                deviceAndTool.ToolCatId = null;
                if (optToolDevice == "vp")
                    deviceAndTool.DeviceCatId = DeviceVPId;
                if (optToolDevice == "it")
                    deviceAndTool.DeviceCatId = DeviceITId;
            }

            if (String.IsNullOrEmpty(deviceAndTool.AssetsCode))
            {
                ModelState.AddModelError("AssetsCode", "Chưa nhập mã cho tài sản!");
            }
            var deviceAndToolUpdate = db.DeviceAndTools.Where(dvt => dvt.Id == deviceAndTool.Id).FirstOrDefault();
            if (deviceAndToolUpdate != null)
            {
                if (deviceAndToolUpdate.HistoryUses.OrderBy(dv => dv.HandedDate).First().HandedDate < deviceAndTool.BuyDate)
                    ModelState.AddModelError(string.Empty, "Ngày mua sau ngày bàn giao lần đầu!");
            }

            if (ModelState.IsValid)
            {
                if (checkExitDeviceAndTool(deviceAndTool.AssetsCode, deviceAndTool.Id))
                {
                    CallBackDropdown(deviceAndTool);
                    ModelState.AddModelError(string.Empty, "Mã tài sản đã có trong dữ liệu!");
                    return PartialView("_Create", deviceAndTool);
                }

                if (deviceAndToolUpdate != null)
                {
                    deviceAndToolUpdate.CompanyId = deviceAndTool.CompanyId;
                    deviceAndToolUpdate.DeviceCatId = deviceAndTool.DeviceCatId;
                    deviceAndToolUpdate.ToolCatId = deviceAndTool.ToolCatId;
                    deviceAndToolUpdate.AssetsCode = deviceAndTool.AssetsCode;
                    deviceAndToolUpdate.DeviceName = deviceAndTool.DeviceName;
                    deviceAndToolUpdate.DescriptionDevice = deviceAndTool.DescriptionDevice;
                    deviceAndToolUpdate.BuyDate = deviceAndTool.BuyDate;
                    deviceAndToolUpdate.EditedById = userSession.Id;
                    deviceAndToolUpdate.EditedDate = DateTime.Now;

                    db.Entry(deviceAndToolUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                else
                {
                    CallBackDropdown(deviceAndTool);
                    return PartialView("_Edit", deviceAndTool);
                }
            }
            CallBackDropdown(deviceAndTool);
            userSession = db.UserInfoes.Find(userSession.Id);
            if (deviceAndTool.Company != null)
                ViewBag.CompanyId = new SelectList(userSession.Companies, "Id", "NameVn", deviceAndTool.CompanyId);
            else
                ViewBag.CompanyId = new SelectList(userSession.Companies, "Id", "NameVn");
            return PartialView("_Edit", deviceAndTool);
        }

        // GET: DeviceAndTools/Delete/5
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
            DeviceAndTool deviceAndTool = db.DeviceAndTools.Find(id);
            if (deviceAndTool == null)
            {
                return HttpNotFound();
            }
            //if (deviceAndTool.HistoryUses.Count > 0)
            //{
            //    ModelState.AddModelError(string.Empty, "Không thể xóa! Chỉ được xóa khi đã thanh lý");
            //    ViewBag.UnableDelete = true;
            //}
            //else
            //{
            //    ModelState.AddModelError(string.Empty, "Bạn chắc chắn muốn xóa tài sản?");
            //    ViewBag.UnableDelete = false;
            //}
            ModelState.AddModelError(string.Empty, "Bạn chắc chắn muốn xóa tài sản?");
            ViewBag.UnableDelete = false;
            return PartialView("_Delete", deviceAndTool);
        }

        // POST: DeviceAndTools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeviceAndTool deviceAndTool = db.DeviceAndTools.Find(id);
            if (deviceAndTool.HistoryUses.Count > 0)
            {
                db.HistoryUses.RemoveRange(deviceAndTool.HistoryUses);
                db.DeviceAndTools.Remove(deviceAndTool);
                db.SaveChanges();

                //ModelState.AddModelError(string.Empty, "Không thể xóa! Chỉ được xóa khi đã thanh lý");
                //return PartialView("_Delete", deviceAndTool);
                return Json(new { success = true });
            }
            else
            {
                db.DeviceAndTools.Remove(deviceAndTool);
                try
                {
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Lỗi! Không xóa được tài sản!");
                    return PartialView("_Delete", deviceAndTool);
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

    public class DeviceToolFilter
    {
        public int? page { get; set; }
        public string maTS { get; set; }

        public int? companyId { get; set; }
        public int? deptId { get; set; }

        public int? staffId { get; set; }
        public int? statusId { get; set; }

        public int? deviceCatId { get; set; }
        public int? toolCatId { get; set; }
        public int? locatioId { get; set; }

        public int? AdminOrId { get; set; }

        public string fromDate { get; set; }
        public string toDate { get; set; }
    }

    public class ImagePrinterBarcode
    {

        public string code { get; set; }
        public Image imageCode { get; set; }

        public ImagePrinterBarcode(string Code, Image ImageCode)
        {
            this.code = Code;
            this.imageCode = ImageCode;
        }
    }

}

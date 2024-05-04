using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtcKpi.Model;
using BtcKpi.Model.Enum;
using BtcKpi.Service;
using BtcKpi.Service.Common;
using BtcKpi.Web.ViewModels;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace BtcKpi.Web.Controllers
{
    public class ReportUpfController : BaseController
    {
        #region Constructor
        private readonly IUserService userService;
        private readonly IReportService reportService;
        private readonly IDepartmentService departmentService;

        public ReportUpfController(IUserService userService, IDepartmentService departmentService, IReportService reportService)
        {
            this.userService = userService;
            this.reportService = reportService;
            this.departmentService = departmentService;
        }
        #endregion Constructor

        // GET: Report - UpfMonth
        public ActionResult UpfIndex()
        {
            ReportListViewModel model = new ReportListViewModel();
            model.UserID = CurrentUser.UserId;
            var userInfo = userService.GetUserFullInfo(CurrentUser.UserId);
            model.UserDepartmentID = (int)userInfo.DepartmentID;

            //Công ty - Phòng ban
            model.Companies = new List<SelectListItem>();
            model.Departments = new List<SelectListItem>();
            var deparments = userService.GetDepartmentCrossByUser(CurrentUser.UserId);
            if (deparments.Any())
            {
                var companies = (from d in deparments select new { d.CompanyId, d.CompanyName }).Distinct().ToList();
                model.Companies = new SelectList(companies, "CompanyId", "CompanyName");

                var departments = (from d in deparments select new { d.Id, d.Name }).Distinct().ToList();
                model.Departments = new SelectList(departments, "Id", "Name");
            }
            //Năm
            model.Years = Years;
            model.Year = DateTime.Now.Year.ToString();

            //Trang thai
            var departStatus = new List<ConvertEnum>();
            foreach (var status in Enum.GetValues(typeof(DepartmentStatus)))
                departStatus.Add(new ConvertEnum
                {
                    Value = (int)status,
                    Text = BtcHelper.convertStatus(status.ToString())
                });
            model.Status = new SelectList(departStatus, "Value", "Text");
            model.StatusID = "3";

            model.UpfReports = new List<UpfReport>();
            //Lay danh sach KPI phong ban/ bo phan
            model.DepartmentInfos = new List<DepartmentInfo>();

            return View(model);
        }

        // GET: Report - UpfYear

        #region Ajax
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CompanyChange(string companyId)
        {
            int company;
            var items = userService.GetDepartmentCrossByUser(CurrentUser.UserId);
            var departmentList = (from d in items select new { d.Id, d.Name }).Distinct().ToList();
            SelectList Departments = new SelectList(departmentList, "Id", "Name");
            if (Int32.TryParse(companyId.Replace("\"", ""), out company))
            {
                departmentList = (from d in items.Where(t => t.CompanyId == company) select new { d.Id, d.Name }).Distinct().ToList();
                Departments = new SelectList(departmentList, "Id", "Name");
            }
            return PartialView("_DepartmentDropDownList", Departments);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SearchReportUpfList(ReportListViewModel model)
        {
            model.UserID = CurrentUser.UserId;
            model.DepartmentInfos = departmentService.GetReportDepartment(model.CompanyID, model.DepartmentID, model.Year);
            model.DepartmentInfos = model.DepartmentInfos.OrderBy(o => o.ScheduleID).ThenBy(o => o.ScheduleType).ToList();
            model.DepartmentInfos = SummaryDepartmentInfos(model.DepartmentInfos);

            foreach (var bo in model.DepartmentInfos)
            {
                var janPoint = bo.January ?? 0;
                bo.January = Math.Round(janPoint, 1);
                var febPoint = bo.February ?? 0;
                bo.February = Math.Round(febPoint, 1);
                var marPoint = bo.March ?? 0;
                bo.March = Math.Round(marPoint, 1);
                var aprPoint = bo.April ?? 0;
                bo.April = Math.Round(aprPoint, 1);
                var mayPoint = bo.May ?? 0;
                bo.May = Math.Round(mayPoint, 1);
                var junPoint = bo.June ?? 0;
                bo.June = Math.Round(junPoint, 1);
                var julPoint = bo.July ?? 0;
                bo.July = Math.Round(julPoint, 1);
                var augPoint = bo.August ?? 0;
                bo.August = Math.Round(augPoint, 1);
                var sepPoint = bo.September ?? 0;
                bo.September = Math.Round(sepPoint, 1);
                var octPoint = bo.October ?? 0;
                bo.October = Math.Round(octPoint, 1);
                var novPoint = bo.November ?? 0;
                bo.November = Math.Round(novPoint, 1);
                var decPoint = bo.December ?? 0;
                bo.December = Math.Round(decPoint, 1);
                bo.AveragePoint = Math.Round((janPoint + febPoint + marPoint + aprPoint + mayPoint + junPoint + julPoint +
                                   augPoint + sepPoint + octPoint + novPoint + decPoint) / 12, 1);
                UpfSummary upfSummary = departmentService.GetUpfSummaryByDepartYear(bo.DepartmentID, bo.Year);
                bo.BODSumPoint = upfSummary?.SumBODPoint ?? 0;
            }

            return PartialView("_UpfListTable", model);
        }

        public List<DepartmentInfo> SummaryDepartmentInfos(List<DepartmentInfo> model)
        {
            List<DepartmentInfo> departmentInfos = new List<DepartmentInfo>();
            List<string> lstCheck = new List<string>();
            List<string> lstCheckYear = new List<string>();
            foreach (var bo in model)
            {
                if (departmentInfos.Count > 0)
                {
                    if (lstCheck.Contains(bo.DepartmentID.ToString()) && lstCheckYear.Contains(bo.Year.ToString()))
                    {
                        foreach (var item in departmentInfos)
                        {
                            if (item.DepartmentID == bo.DepartmentID && item.Year == bo.Year)
                            {
                                UpfSchedule upfSchedule = departmentService.GetScheduleById(bo.ScheduleID);
                                if (upfSchedule != null && bo.ScheduleType == 1)
                                {
                                    if (upfSchedule.Value == 1)
                                    {
                                        if (bo.TotalBODPoint != null) item.January = bo.TotalBODPoint;
                                        else item.January = bo.AssessedScore;
                                    }
                                    if (upfSchedule.Value == 2)
                                    {
                                        if (bo.TotalBODPoint != null) item.February = bo.TotalBODPoint;
                                        else item.February = bo.AssessedScore;
                                    }
                                    if (upfSchedule.Value == 3)
                                    {
                                        if (bo.TotalBODPoint != null) item.March = bo.TotalBODPoint;
                                        else item.March = bo.AssessedScore;
                                    }
                                    if (upfSchedule.Value == 4)
                                    {
                                        if (bo.TotalBODPoint != null) item.April = bo.TotalBODPoint;
                                        else item.April = bo.AssessedScore;
                                    }
                                    if (upfSchedule.Value == 5)
                                    {
                                        if (bo.TotalBODPoint != null) item.May = bo.TotalBODPoint;
                                        else item.May = bo.AssessedScore;
                                    }
                                    if (upfSchedule.Value == 6)
                                    {
                                        if (bo.TotalBODPoint != null) item.June = bo.TotalBODPoint;
                                        else item.June = bo.AssessedScore;
                                    }
                                    if (upfSchedule.Value == 7)
                                    {
                                        if (bo.TotalBODPoint != null) item.July = bo.TotalBODPoint;
                                        else item.July = bo.AssessedScore;
                                    }
                                    if (upfSchedule.Value == 8)
                                    {
                                        if (bo.TotalBODPoint != null) item.August = bo.TotalBODPoint;
                                        else item.August = bo.AssessedScore;
                                    }
                                    if (upfSchedule.Value == 9)
                                    {
                                        if (bo.TotalBODPoint != null) item.September = bo.TotalBODPoint;
                                        else item.September = bo.AssessedScore;
                                    }
                                    if (upfSchedule.Value == 10)
                                    {
                                        if (bo.TotalBODPoint != null) item.October = bo.TotalBODPoint;
                                        else item.October = bo.AssessedScore;
                                    }
                                    if (upfSchedule.Value == 11)
                                    {
                                        if (bo.TotalBODPoint != null) item.November = bo.TotalBODPoint;
                                        else item.November = bo.AssessedScore;
                                    }
                                    if (upfSchedule.Value == 12)
                                    {
                                        if (bo.TotalBODPoint != null) item.December = bo.TotalBODPoint;
                                        else item.December = bo.AssessedScore;
                                    }
                                }
                                if (bo.ScheduleType == 0)
                                {
                                    item.EndOfYear = bo.TotalBODPoint ?? bo.AssessedScore;
                                }
                                else if (upfSchedule != null && bo.ScheduleType == 1 && upfSchedule.Value == 12)
                                {
                                    item.EndOfYear = bo.AssessedScore;
                                }
                            }
                        }
                    }
                    else
                    {
                        departmentInfos.Add(ConvertScheduleToMonth(bo));
                        lstCheck.Add(bo.DepartmentID.ToString());
                        lstCheckYear.Add(bo.Year.ToString());
                    }
                }
                else
                {
                    departmentInfos.Add(ConvertScheduleToMonth(bo));
                    lstCheck.Add(bo.DepartmentID.ToString());
                    lstCheckYear.Add(bo.Year.ToString());
                }
            }

            return departmentInfos;
        }

        #endregion Ajax
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ExportExcel(ReportListViewModel model)
        {
            model.DepartmentInfos = departmentService.GetReportDepartment(model.CompanyID, model.DepartmentID, model.Year);

            List<DepartmentInfo> departmentInfos = SummaryDepartmentInfos(model.DepartmentInfos);
            foreach (var bo in departmentInfos)
            {
                UpfSummary upfSummary = departmentService.GetUpfSummaryByDepartYear(bo.DepartmentID, bo.Year);
                if(upfSummary != null)
                {
                    bo.BODSumPoint = upfSummary?.SumBODPoint ?? 0;
                    bo.BODComment = upfSummary.Note;
                }
            }

            ExcelPackage excelEngine = FillDataTableExcel(departmentInfos, model.Year);
            Session["DownloadExcel_UPFSummary"] = excelEngine.GetAsByteArray();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Download()
        {
            if (Session["DownloadExcel_UPFSummary"] != null)
            {
                string dateStr = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy_hhmmss");
                var fileDownloadName = "Báo cáo tổng hợp UPF_" + "_" + dateStr + ".xlsx";
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                byte[] data = Session["DownloadExcel_UPFSummary"] as byte[];
                return File(data, contentType, fileDownloadName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public DepartmentInfo ConvertScheduleToMonth(DepartmentInfo info)
        {
            UpfSchedule upfSchedule = departmentService.GetScheduleById(info.ScheduleID);
            if(upfSchedule != null && info.ScheduleType == 1)
            {
                if (upfSchedule.Value == 1)
                {
                    if (info.TotalBODPoint != null) info.January = (int)info.TotalBODPoint;
                    else info.January = info.AssessedScore;
                }
                if (upfSchedule.Value == 2)
                {
                    if (info.TotalBODPoint != null) info.February = (int)info.TotalBODPoint;
                    else info.February = info.AssessedScore;
                }
                if (upfSchedule.Value == 3)
                {
                    if (info.TotalBODPoint != null) info.March = (int)info.TotalBODPoint;
                    else info.March = info.AssessedScore;
                }
                if (upfSchedule.Value == 4)
                {
                    if (info.TotalBODPoint != null) info.April = (int)info.TotalBODPoint;
                    else info.April = info.AssessedScore;
                }
                if (upfSchedule.Value == 5)
                {
                    if (info.TotalBODPoint != null) info.May = (int)info.TotalBODPoint;
                    else info.May = info.AssessedScore;
                }
                if (upfSchedule.Value == 6)
                {
                    if (info.TotalBODPoint != null) info.June = (int)info.TotalBODPoint;
                    else info.June = info.AssessedScore;
                }
                if (upfSchedule.Value == 7)
                {
                    if (info.TotalBODPoint != null) info.July = (int)info.TotalBODPoint;
                    else info.July = info.AssessedScore;
                }
                if (upfSchedule.Value == 8)
                {
                    if (info.TotalBODPoint != null) info.August = (int)info.TotalBODPoint;
                    else info.August = info.AssessedScore;
                }
                if (upfSchedule.Value == 9)
                {
                    if (info.TotalBODPoint != null) info.September = (int)info.TotalBODPoint;
                    else info.September = info.AssessedScore;
                }
                if (upfSchedule.Value == 10)
                {
                    if (info.TotalBODPoint != null) info.October = (int)info.TotalBODPoint;
                    else info.October = info.AssessedScore;
                }
                if (upfSchedule.Value == 11)
                {
                    if (info.TotalBODPoint != null) info.November = (int)info.TotalBODPoint;
                    else info.November = info.AssessedScore;
                }
                if (upfSchedule.Value == 12)
                {
                    if (info.TotalBODPoint != null) info.December = (int)info.TotalBODPoint;
                    else info.December = info.AssessedScore;
                }
            }
            if (info.ScheduleType == 0)
            {
                info.EndOfYear = info.TotalBODPoint ?? info.AssessedScore;
            }
            else if (upfSchedule != null && info.ScheduleType == 1 && upfSchedule.Value == 12)
            {
                info.EndOfYear = info.AssessedScore;
            }

            return info;
        }

        public ExcelPackage FillDataTableExcel(List<DepartmentInfo> model, string year)
        {
            ExcelPackage excelEngine = new ExcelPackage();
            ExcelWorksheet worksheet = excelEngine.Workbook.Worksheets.Add("UPF-Tong hop");

            //Enter values to the cells from A3 to A5

            worksheet.Cells["A1:X1"].Merge = true;
            worksheet.Cells["A1"].Value = "BẢNG TỔNG HỢP KẾT QUẢ ĐÁNH GIÁ HIỆU SUẤT PHÒNG HÀNG THÁNG - " + year;
            worksheet.Cells["A1"].Style.Font.Bold = true;
            worksheet.Cells["A1:X1"].Style.WrapText = true;
            worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Row(1).Height = 40;
            // format cells - add borders A2:Z2 body
            worksheet.Cells["A2:X2"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A2:X2"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A2:X2"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A2:X2"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A2:X2"].Style.Font.Bold = true;
            worksheet.Cells["A2:X2"].Style.WrapText = true;
            worksheet.Cells["A2:X2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            worksheet.Cells["A2"].Value = "STT/ No.";
            worksheet.Cells["B2"].Value = "Công ty/ Company";
            worksheet.Cells["C2"].Value = "Phòng ban/ Department";
            worksheet.Cells["D2"].Value = "Người phụ trách/ Person in charge";
            worksheet.Cells["E2"].Value = "Chức vụ/ Position";
            worksheet.Cells["F2"].Value = "Tháng 1/ January";
            worksheet.Cells["G2"].Value = "Tháng 2/ February";
            worksheet.Cells["H2"].Value = "Tháng 3/ March";
            worksheet.Cells["I2"].Value = "Tháng 4/ April";
            worksheet.Cells["J2"].Value = "Tháng 5/ May";
            worksheet.Cells["K2"].Value = "Tháng 6/ June";
            worksheet.Cells["L2"].Value = "Tháng 7/ July";
            worksheet.Cells["M2"].Value = "Tháng 8/ August";
            worksheet.Cells["N2"].Value = "Tháng 9/ September";
            worksheet.Cells["O2"].Value = "Tháng 10/ October";
            worksheet.Cells["P2"].Value = "Tháng 11/ November";
            worksheet.Cells["Q2"].Value = "Tháng 12/ December";
            worksheet.Cells["R2"].Value = "Điểm KPI BQ/ Average KPI";
            worksheet.Cells["S2"].Value = "Xếp loại theo Điểm KPI BQ/ Rank based on The";
            worksheet.Cells["T2"].Value = "KPI end of Year";
            worksheet.Cells["U2"].Value = "Xếp loại theo kết quả đánh giá cuối năm.";
            worksheet.Cells["V2"].Value = "BOD đánh giá/ KPI by BOD";
            worksheet.Cells["W2"].Value = "Xếp loại theo kết quả đánh giá cuả BOD/ Rank based on BOD assessment";
            worksheet.Cells["X2"].Value = "Ghi chú";
            Color colFromHex = ColorTranslator.FromHtml("#F8CBAD");
            worksheet.Cells["A2:X2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A2:X2"].Style.Fill.BackgroundColor.SetColor(colFromHex);

            int col = 2;
            int? totalWeight = 0;
            for (int i = 0; i < model.Count; i++)
            {
                col++;
                worksheet.Cells["A" + col].Value = i+1;
                worksheet.Cells["B" + col].Value = model[i].CompanyName;
                worksheet.Cells["C" + col].Value = model[i].DepartmentName;
                worksheet.Cells["D" + col].Value = model[i].PersonInCharge;
                worksheet.Cells["E" + col].Value = model[i].AdministratorshipName;
                var janPoint = model[i].January != null ? model[i].January : 0;
                worksheet.Cells["F" + col].Value = Math.Round((decimal) janPoint, 1);
                var febPoint = model[i].February != null ? model[i].February : 0;
                worksheet.Cells["G" + col].Value = Math.Round((decimal) febPoint, 1);
                var marPoint = model[i].March != null ? model[i].March : 0;
                worksheet.Cells["H" + col].Value = Math.Round((decimal) marPoint, 1);
                var aprilPoint = model[i].April != null ? model[i].April : 0;
                worksheet.Cells["I" + col].Value = Math.Round((decimal) aprilPoint, 1);
                var mayPoint = model[i].May != null ? model[i].May : 0;
                worksheet.Cells["J" + col].Value = Math.Round((decimal) mayPoint, 1);
                var junPoint = model[i].June != null ? model[i].June : 0;
                worksheet.Cells["K" + col].Value = Math.Round((decimal) junPoint, 1);
                var julPoint = model[i].July != null ? model[i].July : 0;
                worksheet.Cells["L" + col].Value = Math.Round((decimal) julPoint, 1);
                var augPoint = model[i].August != null ? model[i].August : 0;
                worksheet.Cells["M" + col].Value = Math.Round((decimal) augPoint, 1);
                var sepPoint = model[i].September != null ? model[i].September : 0;
                worksheet.Cells["N" + col].Value = Math.Round((decimal) sepPoint, 1);
                var octPoint = model[i].October != null ? model[i].October : 0;
                worksheet.Cells["O" + col].Value = Math.Round((decimal) octPoint, 1);
                var novPoint = model[i].November != null ? model[i].November : 0;
                worksheet.Cells["P" + col].Value = Math.Round((decimal) novPoint, 1);
                var decPoint = model[i].December != null ? model[i].December : 0;
                worksheet.Cells["Q" + col].Value = Math.Round((decimal) decPoint, 1);
                worksheet.Cells["R" + col].Value = Math.Round((decimal) ((janPoint + febPoint + marPoint + aprilPoint + mayPoint + junPoint + julPoint + augPoint + sepPoint + octPoint + novPoint + decPoint) /12), 1);
                worksheet.Cells["S" + col].Value = BtcHelper.ConvertSroreToRank((janPoint + febPoint + marPoint + aprilPoint + mayPoint + junPoint + julPoint + augPoint + sepPoint + octPoint + novPoint + decPoint) / 12);
                var endOfYear = model[i].EndOfYear != null ? model[i].EndOfYear : 0;
                worksheet.Cells["T" + col].Value = Math.Round((decimal) endOfYear, 1);
                worksheet.Cells["U" + col].Value = BtcHelper.ConvertSroreToRank(endOfYear);
                worksheet.Cells["V" + col].Value = Math.Round((decimal) model[i].BODSumPoint, 1);
                worksheet.Cells["W" + col].Value = BtcHelper.ConvertSroreToRank(model[i].BODSumPoint);
                worksheet.Cells["X" + col].Value = model[i].BODComment;

                worksheet.Cells["A" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["R" + col + ":W" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            // format cells - add borders A5:I...
            worksheet.Cells["A2:X" + col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A2:X" + col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A2:X" + col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A2:X" + col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            col++;
            col = col + 1;

            worksheet.Cells["A" + col + ":C" + col].Merge = true;
            worksheet.Cells["A" + col].Value = "Người lập";
            worksheet.Cells["E" + col + ":H" + col].Merge = true;
            worksheet.Cells["E" + col].Value = "Trưởng phòng HCNS";
            worksheet.Cells["A" + col + ":H" + col].Style.Font.Bold = true;
            worksheet.Cells["A" + col + ":H" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["A1:X" + col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["A3:X" + col].Style.WrapText = true;
            //Apply row height and column width to look good
            worksheet.Column(1).Width = 5;
            worksheet.Column(2).Width = 25;
            worksheet.Column(3).Width = 25;
            worksheet.Column(4).Width = 20;
            worksheet.Column(5).Width = 15;
            worksheet.Column(6).Width = 7;
            worksheet.Column(7).Width = 7;
            worksheet.Column(8).Width = 7;
            worksheet.Column(9).Width = 7;
            worksheet.Column(10).Width = 7;
            worksheet.Column(11).Width = 7;
            worksheet.Column(12).Width = 7;
            worksheet.Column(13).Width = 7;
            worksheet.Column(14).Width = 7;
            worksheet.Column(15).Width = 7;
            worksheet.Column(16).Width = 7;
            worksheet.Column(17).Width = 7;
            worksheet.Column(18).Width = 10;
            worksheet.Column(19).Width = 15;
            worksheet.Column(20).Width = 10;
            worksheet.Column(21).Width = 15;
            worksheet.Column(22).Width = 10;
            worksheet.Column(23).Width = 15;
            worksheet.Column(24).Width = 20;

            return excelEngine;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpfSumApproved(UpfSummaryViewModel model)
        {
            List<UpfNameDetail> nameDetails = new List<UpfNameDetail>();
            List<UpfPersRewProposal> persRewPropDetail = new List<UpfPersRewProposal>();
            List<UpfComment> comments = new List<UpfComment>();
            Upf upf = departmentService.GetDepartmentInfo(model.UpfId, ref nameDetails, ref persRewPropDetail, ref comments);
            UpfSummary upfSummary = departmentService.GetUpfSummaryByDepartYear(upf.DepartmentID, upf.Year);
            if(upfSummary == null)
            {
                upfSummary = new UpfSummary {SumBODPoint = model.SumBODPoint, Note = model.Note};
                departmentService.InsertUpfSummary(upfSummary, upf.DepartmentID, upf.Year, CurrentUser.UserId);
            }
            else
            {
                upfSummary.SumBODPoint = model.SumBODPoint;
                upfSummary.Note = model.Note;
                departmentService.UpdateUpfSummary(upfSummary, CurrentUser.UserId);
            }
                //return RedirectToAction("UpfIndex", "ReportUpf");
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult UpdateBodApproved(int upfId)
        {
            UpfSummaryViewModel model = new UpfSummaryViewModel();
            List<UpfNameDetail> nameDetails = new List<UpfNameDetail>();
            List<UpfPersRewProposal> persRewPropDetail = new List<UpfPersRewProposal>();
            List<UpfComment> comments = new List<UpfComment>();
            Upf upf = departmentService.GetDepartmentInfo(upfId, ref nameDetails, ref persRewPropDetail, ref comments);
            UpfSummary upfSummary = departmentService.GetUpfSummaryByDepartYear(upf.DepartmentID, upf.Year);
            if (upfSummary != null)
            {
                model.UpfId = upf.ID;
                model.SumBODPoint = upfSummary.SumBODPoint;
                model.Note = upfSummary.Note;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}
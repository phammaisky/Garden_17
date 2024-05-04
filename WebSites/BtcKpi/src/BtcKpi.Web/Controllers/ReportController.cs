using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtcKpi.Model;
using BtcKpi.Model.Enum;
using BtcKpi.Service;
using BtcKpi.Service.Common;
using BtcKpi.Web.CustomActionFilter;
using BtcKpi.Web.ViewModels;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace BtcKpi.Web.Controllers
{
    public class ReportController : BaseController
    {
        #region Constructor
        private readonly IUserService userService;
        private readonly IReportService reportService;
        private readonly IDepartmentService departmentService;

        public ReportController(IUserService userService, IDepartmentService departmentService, IReportService reportService)
        {
            this.userService = userService;
            this.reportService = reportService;
            this.departmentService = departmentService;
        }
        #endregion Constructor

        // GET: Report - IpfYear
        public ActionResult IpfYear()
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

            model.UpfReports = new List<UpfReport>();
            return View(model);
        }

        // POST: Report - IpfYear
        [HttpPost]
        public ActionResult IpfYear(ReportListViewModel model, string submitButton)
        {
            string updateMsg = "";
            //ViewBag.CurrentUser = CurrentUser;
            //model.ErrorMessage = "";
            //string updateMsg = "";
            //bool isApprove = submitButton == "Cancel" ? false : true;
            //if (!isApprove & string.IsNullOrEmpty(model.Comment))
            //{
            //    model = GetIpfById(model.Ipf.ID);
            //    model.ErrorMessage = "Phải nhập lý do không duyệt";
            //    return View(model);
            //}

            ////Mapping data
            //List<IpfDetail> details = new List<IpfDetail>();
            //List<PersonalPlan> personalPlans = new List<PersonalPlan>();
            //foreach (var item in model.CompleteWorks.Where(t => t.IsWorkTitle == false))
            //{
            //    item.IsNextYear = 0;
            //    item.WorkType = 0;
            //    details.Add(item);
            //}
            //foreach (var item in model.Competencies)
            //{
            //    item.IsNextYear = 0;
            //    item.WorkType = 1;
            //    item.WorkCompleteID = 0;
            //    details.Add(item);
            //}

            //// Nếu là KPIs năm thì có thêm Kế hoạch phát triển bản thân và Mục tiêu năm tiếp theo
            //if (model.Ipf.ScheduleType == 0)
            //{
            //    foreach (var item in model.PersonalPlanCompetencies)
            //    {
            //        item.Type = 0;
            //        personalPlans.Add(item);
            //    }
            //    foreach (var item in model.PersonalPlanCareers)
            //    {
            //        item.Type = 1;
            //        personalPlans.Add(item);
            //    }

            //    foreach (var item in model.CompleteWorksNextYear.Where(t => t.IsWorkTitle == false))
            //    {
            //        item.IsNextYear = 1;
            //        item.WorkType = 0;
            //        details.Add(item);
            //    }
            //    foreach (var item in model.CompetenciesNextYear)
            //    {
            //        item.IsNextYear = 1;
            //        item.WorkType = 1;
            //        item.WorkCompleteID = 0;
            //        details.Add(item);
            //    }
            //}

            if (reportService.UpdateIpfReport(CurrentUser.UserId, model.IpfReportInfos,ref updateMsg))
            {
                
                return RedirectToAction("IpfYear", "Report");
            }
            else
            {
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
                model.ErrorMessage = updateMsg;
                return View(model);
            }

        }

        // GET: Report - UpfMonth
        public ActionResult UpfMonth()
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

            model.DepartSchedules = new SelectList(departmentService.DepartSchedulesByDeparment((int)model.UserDepartmentID, (int)userInfo.CompanyID), "ID", "Name");
            var month = DateTime.Now.Month - 1;
            var monthItem = model.DepartSchedules.FirstOrDefault(p => p.Text.Contains(month.ToString()));
            if (monthItem != null)
            {
                model.ScheduleID = monthItem.Value;
            }

            model.UpfReports = new List<UpfReport>();
            return View(model);
        }

        // GET: Report - UpfYear
        public ActionResult UpfYear()
        {
            return View();
        }

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
        public ActionResult UpfMonthSearch(ReportListViewModel model)
        {
            model.UserID = CurrentUser.UserId;
            string errorMsg = "";
            model.UpfReports = reportService.GetUpfMonthByConditions(CurrentUser.UserId, model.CompanyID, model.DepartmentID, model.Year, model.ScheduleID, ref errorMsg);

            return PartialView("_UpfMonthTable", model);
        }
        

        public ActionResult UpfMonthView(int company, int year)
        {
            UpfMonthViewModel model = new UpfMonthViewModel();
            model.Year = year;
            string errorMessage = "";
            var userInfo = userService.GetUserFullInfo(CurrentUser.UserId);

            //model.Departments = userService.GetDepartmentByCompany(company);
            
            //model.Schedules = departmentService.DepartSchedulesByDeparment((int)userInfo.DepartmentID, (int)userInfo.CompanyID);
            
            var modelDepartments = model.Departments;
            var modelSchedules = model.Schedules;
            model.UpfMonthItems = reportService.GetReportMonth(CurrentUser.UserId, (int)userInfo.DepartmentID, company, year, ref errorMessage, ref modelDepartments, ref modelSchedules);
            model.Departments = modelDepartments;
            model.Schedules = modelSchedules;
            return View(model);
        }

        public ActionResult UpfYearView(int company, int year)
        {
            UpfMonthViewModel model = new UpfMonthViewModel();
            model.Year = year;
            string errorMessage = "";
            var userInfo = userService.GetUserFullInfo(CurrentUser.UserId);

            //model.Departments = userService.GetDepartmentByCompany(company);

            //model.Schedules = departmentService.DepartSchedulesByDeparment((int)userInfo.DepartmentID, (int)userInfo.CompanyID);

            var modelDepartments = model.Departments;
            var modelSchedules = model.Schedules;
            model.UpfMonthItems = reportService.GetReportMonth(CurrentUser.UserId, (int)userInfo.DepartmentID, company, year, ref errorMessage, ref modelDepartments, ref modelSchedules);
            model.Departments = modelDepartments;
            model.Schedules = modelSchedules;
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult IpfYearSearch(ReportListViewModel model)
        {
            model.UserID = CurrentUser.UserId;
            string errorMsg = "";
            model.IpfReportInfos = reportService.GetIpfReportInfos(BtcHelper.RemoveComman(model.Year), BtcHelper.RemoveComman(model.CompanyID), BtcHelper.RemoveComman(model.DepartmentID));
            
            return PartialView("_IpfYearTable", model);
        }
        #endregion Ajax

        #region Excel
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ExportToExcel(ReportListViewModel model)
        {
            model.UserID = CurrentUser.UserId;
            string errorMsg = "";
            model.IpfReportInfos = reportService.GetIpfReportInfos(BtcHelper.RemoveComman(model.Year), BtcHelper.RemoveComman(model.CompanyID), BtcHelper.RemoveComman(model.DepartmentID));

            //Save the workbook to disk in xlsx format
            string dateStr = Convert.ToDateTime(DateTime.Now).ToString("ddMMyyyy_hhmmss");
            var fileDownloadName = "IPF_" + model.Year + "_" + dateStr + ".xlsx";
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //var fileStream = new MemoryStream();
            ////
            //ExcelPackage excelEngine = FillDataTableExcel(model);
            //excelEngine.SaveAs(fileStream);
            //fileStream.Position = 0;
            //var fsr = new FileStreamResult(fileStream, contentType);
            //fsr.FileDownloadName = fileDownloadName;

            //save the file to server temp folder
            string fullPath = Path.Combine(Server.MapPath("~/temp"), fileDownloadName);
            using (var fileStream = new MemoryStream())
            {
                ExcelPackage excelEngine = FillDataTableExcel(model);
                excelEngine.SaveAs(fileStream);
                fileStream.Position = 0;
                var fsr = new FileStreamResult(fileStream, contentType);
                fsr.FileDownloadName = fileDownloadName;

                FileStream file = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                fileStream.WriteTo(file);
                file.Close();
            }

            //return fsr;
            //return the Excel file name
            return Json(new { fileName = fileDownloadName, errorMessage = "" });
        }

        [AllowAnonymous]
        [HttpGet]
        [DeleteFile] //Action Filter, it will auto delete the file after download, 
        //I will explain it later
        public ActionResult Download(string file)
        {
            //get the temp folder and file path in server
            string fullPath = Path.Combine(Server.MapPath("~/temp"), file);

            //return the file for download, this is an Excel 
            //so I set the file content type to "application/vnd.ms-excel"
            return File(fullPath, "application/vnd.ms-excel", file);
        }

        public ExcelPackage FillDataTableExcel(ReportListViewModel model)
        {
            ExcelPackage excelEngine = new ExcelPackage();
            ExcelWorksheet worksheet;
            worksheet = excelEngine.Workbook.Worksheets.Add("IPF-" + model.Year);
            //Enter values to the cells from A3 to A5
            int row = 1;
            int col = 1;
            List<IpfReportSumary> sumaries = new List<IpfReportSumary>();

            /*
             * Bắt đầu ghi dữ liệu
             * Chỉ ghi dữ liệu khi có ( row > 1)
             */
            if (model.IpfReportInfos.Count > 1)
            {
                List<string> ipfSheduleNames = new List<string>();
                ipfSheduleNames = model.IpfReportInfos[1].IpfScores.Where(p => p.ScheduleType == 1).OrderBy(p => p.ID).Select(p => p.ScheduleName).ToList();
                int deptColSpan = ipfSheduleNames.Count + 15;
                int deptRowNum = 0;
                int staftRowNum = 0;

                //Row1
                worksheet.Cells[row, 1, row, deptColSpan + 1].Merge = true;
                worksheet.Cells[row,col].Value = string.Format("BẢNG ĐÁNH GIÁ HIỆU SUẤT HÀNG THÁNG - {0} \r\n IPF - {0}", model.Year);

                worksheet.Cells[row, 1, row, deptColSpan + 1].Style.Font.Bold = true;
                worksheet.Cells[row, 1, row, deptColSpan + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[row, 1, row, deptColSpan + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[row, 1, row, deptColSpan + 1].Style.WrapText = true;
                worksheet.Cells[row, 1, row, deptColSpan + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[row, 1, row, deptColSpan + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[row, 1, row, deptColSpan + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[row, 1, row, deptColSpan + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Row(1).Height = 58;


                #region Header
                //Row3: Header
                row = 3;
                col = 1;
                //Cột 1~9: Thông tin chung
                worksheet.Cells[row, col].Value = "STT/No.";
                col++;
                worksheet.Cells[row, col].Value = "Mã CBNV";
                col++;
                worksheet.Cells[row, col].Value = "Họ và tên/ Full Name";
                col++;
                worksheet.Cells[row, col].Value = "PHÒNG";
                col++;
                worksheet.Cells[row, col].Value = "Vị trí/ Chức danh / Position";
                col++;
                worksheet.Cells[row, col].Value = "Cấp bậc";
                col++;
                worksheet.Cells[row, col].Value = "Ngày bắt đầu làm việc/ Joining date";
                col++;
                worksheet.Cells[row, col].Value = "Thâm niên làm việc (tháng)";
                col++;
                worksheet.Cells[row, col].Value = "Số ngày nghỉ KL (Ốm, TS..)";
                col++;
                //Tên các tháng
                for (int i = 0; i < ipfSheduleNames.Count; i++)
                {
                    worksheet.Cells[row, col].Value = ipfSheduleNames[i];
                    col++;
                }

                //Các cột cuối
                worksheet.Cells[row, col].Value = "Điểm KPT BQ/ Average KPI";
                col++;
                worksheet.Cells[row, col].Value = "Xếp loại theo điểm KPT BQ/ Rank base on the average KPI";
                col++;
                worksheet.Cells[row, col].Value = "KPI end of Year";
                col++;
                worksheet.Cells[row, col].Value = "Xếp loại theo kết quả đánh giá cuối năm";
                col++;
                worksheet.Cells[row, col].Value = "BOD đánh giá";
                col++;
                worksheet.Cells[row, col].Value = "KPI by BOD";
                col++;
                worksheet.Cells[row, col].Value = "Ghi chú";

                //Style
                worksheet.Cells[row, 1, row, col].Style.Font.Bold = true;
                worksheet.Cells[row, 1, row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[row, 1, row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[row, 1, row, col].Style.WrapText = true;
                worksheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(100, 248, 203, 172);                worksheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(100, 248, 203, 172);
                #endregion Header

                #region Datatable
                /*
                 * Dữ liệu các dòng
                 * Bắt đầu từ dòng 4
                 */
                row = 4;
                col = 1;
                for (int i = 0; i < model.IpfReportInfos.Count; i++)
                {
                    col = 1;
                    //Trường hợp là row tổng ( tên phòng ban)
                    if (model.IpfReportInfos[i].IsDeparmentRow == true)
                    {
                        //Thêm dòng tính tổng
                        IpfReportSumary  sumary = new IpfReportSumary();
                        sumary.DepartmentName = model.IpfReportInfos[i].DepartmentName.ToUpper();
                        sumaries.Add(sumary);

                        deptRowNum++;
                        staftRowNum = 0;
                        worksheet.Cells[row, col].Value = BtcHelper.ToRoman(deptRowNum); //"STT/No.";
                        col++;
                        worksheet.Cells[row, col].Value = model.IpfReportInfos[i].DepartmentName.ToUpper(); //"Tên phòng ban";
                        col = col + deptColSpan - 1;
                        //Style
                        worksheet.Cells[row, 1, row, col].Style.Font.Bold = true;
                        worksheet.Cells[row, 1, row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 1, row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet.Cells[row, 1, row, col].Style.WrapText = true;
                        worksheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(100, 198, 223, 181);
                        worksheet.Cells[row, 2, row, col].Merge = true;
                        worksheet.Cells[row, 2, row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    }
                    //Trường hợp là dòng dữ liệu thông thường
                    else
                    {
                        //Dòng dữ liệu tính tổng
                        IpfReportSumary sumary = sumaries.LastOrDefault();

                        staftRowNum++;
                        //Cột 1~9: Thông tin chung
                        worksheet.Cells[row, col].Value = staftRowNum;  // "STT/No.";
                        col++;
                        worksheet.Cells[row, col].Value = BtcHelper.ConvertIdToCode(model.IpfReportInfos[i].UserId);    // "Mã CBNV";
                        col++;
                        worksheet.Cells[row, col].Value = model.IpfReportInfos[i].FullName;     //"Họ và tên/ Full Name";
                        col++;
                        worksheet.Cells[row, col].Value = model.IpfReportInfos[i].DepartmentName; //"PHÒNG";
                        col++;
                        worksheet.Cells[row, col].Value = model.IpfReportInfos[i].AdministratorshipName; //"Vị trí/ Chức danh / Position";
                        col++;
                        worksheet.Cells[row, col].Value = ""; //"Cấp bậc";
                        col++;
                        worksheet.Cells[row, col].Value = model.IpfReportInfos[i].Created; //"Ngày bắt đầu làm việc/ Joining date";
                        col++;
                        worksheet.Cells[row, col].Value = BtcHelper.ConvertSeniority(model.IpfReportInfos[i].Created);// "Thâm niên làm việc (tháng)";
                        col++;
                        worksheet.Cells[row, col].Value = ""; //"Số ngày nghỉ KL (Ốm, TS..)";
                        col++;
                        //Tên các tháng
                        for (int j = 0; j < ipfSheduleNames.Count; j++)
                        {
                            worksheet.Cells[row, col].Value = model.IpfReportInfos[i].IpfScores[j].Score;
                            col++;
                        }

                        //Các cột cuối
                        worksheet.Cells[row, col].Value = model.IpfReportInfos[i].AverageScore; // "Điểm KPT BQ/ Average KPI";
                        CountScore(model.IpfReportInfos[i].AverageScore, sumary);
                        col++;
                        worksheet.Cells[row, col].Value = model.IpfReportInfos[i].AverageRank; //"Xếp loại theo điểm KPT BQ/ Rank base on the average KPI";
                        col++;

                        var ipfEndYear = model.IpfReportInfos[i].IpfScores.FirstOrDefault(p => p.ScheduleID == -1);
                        if (ipfEndYear == null)
                        {
                            ipfEndYear = new IpfScore();
                        }

                        worksheet.Cells[row, col].Value = ipfEndYear.Score; //"KPI end of Year";
                        col++;
                        worksheet.Cells[row, col].Value = BtcHelper.ConvertScoreToRankScheme10(ipfEndYear.Score); //"Xếp loại theo kết quả đánh giá cuối năm";
                        col++;
                        worksheet.Cells[row, col].Value = model.IpfReportInfos[i].BodScore; //"BOD đánh giá";
                        col++;
                        worksheet.Cells[row, col].Value =
                            BtcHelper.ConvertScoreToRankScheme10(model.IpfReportInfos[i].BodScore); //"KPI by BOD";
                        col++;
                        worksheet.Cells[row, col].Value = model.IpfReportInfos[i].Remark; //"Ghi chú";
                    }

                    row++;
                }

                //Style
                // format cells - add borders A5:I...
                worksheet.Cells[3, 1, row, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[3, 1, row, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[3, 1, row, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[3, 1, row, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[3, 1, row, col].Style.WrapText = true;
                worksheet.Cells[5, 1, row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //Dòng cuối
                //Style
                worksheet.Cells[row, 1, row, col].Style.Font.Bold = true;
                worksheet.Cells[row, 1, row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[row, 1, row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[row, 1, row, col].Style.WrapText = true;
                worksheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(100, 198, 223, 181);
                worksheet.Cells[row, 3].Value = "TỔNG";
                worksheet.Cells[row, 5].Value = model.IpfReportInfos.Count(p => p.IsDeparmentRow == false);

                #endregion Datatable
            }

            #region SumDatas

            row = row + 10;
            col = 4;

            //Style
            // format cells - add borders A5:I...
            worksheet.Cells[row, col, row + sumaries.Count + 1, col + 9].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[row, col, row + sumaries.Count + 1, col + 9].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[row, col, row + sumaries.Count + 1, col + 9].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[row, col, row + sumaries.Count + 1, col + 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[row, col, row + sumaries.Count + 1, col + 9].Style.WrapText = true;
            worksheet.Cells[row, col, row + sumaries.Count + 1, col + 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[row, col, row + sumaries.Count + 1, col + 9].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[row, col, row + sumaries.Count + 1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            //Dòng đầu
            //Style
            worksheet.Cells[row, col, row, col + 9].Style.Font.Bold = true;
            worksheet.Cells[row, col, row, col + 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[row, col, row, col + 9].Style.Fill.BackgroundColor.SetColor(100, 146, 209, 79);
            //Data
            worksheet.Cells[row, col].Value = "PHÒNG";
            col++;
            worksheet.Cells[row, col].Value = "XẾP LOẠI";
            col++;
            worksheet.Cells[row, col].Value = "A+";
            col++;
            worksheet.Cells[row, col].Value = "A";
            col++;
            worksheet.Cells[row, col].Value = "B+";
            col++;
            worksheet.Cells[row, col].Value = "B";
            col++;
            worksheet.Cells[row, col].Value = "B-";
            col++;
            worksheet.Cells[row, col].Value = "C";
            col++;
            worksheet.Cells[row, col].Value = "M";
            col++;
            worksheet.Cells[row, col].Value = "Tổng";
            //Data
            row++;
            col = 4;
            worksheet.Cells[row, col].LoadFromCollection(sumaries, false);

            //Dòng cuối
            row = row + sumaries.Count;
            //Style
            worksheet.Cells[row, col, row, col + 9].Style.Font.Bold = true;
            worksheet.Cells[row, col, row, col + 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[row, col, row, col + 9].Style.Fill.BackgroundColor.SetColor(100, 213, 220, 228);
            //Data
            worksheet.Cells[row, col].Value = "TỔNG";
            col = col + 2;
            worksheet.Cells[row, col].Value = sumaries.Sum(p => p.A_Plus_Count);
            col++;
            worksheet.Cells[row, col].Value = sumaries.Sum(p => p.A_Count);
            col++;
            worksheet.Cells[row, col].Value = sumaries.Sum(p => p.B_Plus_Count);
            col++;
            worksheet.Cells[row, col].Value = sumaries.Sum(p => p.B_Count);
            col++;
            worksheet.Cells[row, col].Value = sumaries.Sum(p => p.B_Minus_Count);
            col++;
            worksheet.Cells[row, col].Value = sumaries.Sum(p => p.C_Count);
            col = col + 2;
            worksheet.Cells[row, col].Value = sumaries.Sum(p => p.Total_Count);
            col++;
            #endregion SumData


            ////Apply row height and column width to look good
            worksheet.Column(3).Width = 25;
            worksheet.Column(4).Width = 25;
            worksheet.Column(5).Width = 10;
            worksheet.Column(6).Width = 10;

            return excelEngine;
        }

        private void CountScore(decimal? score, IpfReportSumary sumary)
        {
            sumary.Total_Count++;
            if (score == null)
            {
                return;
            }
            else if (score >= 10)
            {
                sumary.A_Plus_Count++;
            }
            else if (score < 10 & score >= 8)
            {
                sumary.A_Count++;
            }
            else if (score < 8 & score >= 6)
            {
                sumary.B_Plus_Count++;
            }
            else if (score < 6 & score >= 4)
            {
                sumary.B_Count++;
            }
            else if (score < 4 & score >= 2)
            {
                sumary.B_Minus_Count++;
            }
            else
            {
                sumary.C_Count++;
            }
        }
        #endregion Excel
    }

    public class IpfReportSumary
    {
        public string DepartmentName { get; set; }
        public string Rank { get; set; }
        public int A_Plus_Count { get; set; }
        public int A_Count { get; set; }
        public int B_Plus_Count { get; set; }
        public int B_Count { get; set; }
        public int B_Minus_Count { get; set; }
        public int C_Count { get; set; }
        public int M_Count { get; set; }
        public int Total_Count { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using BtcKpi.Model;
using BtcKpi.Service;
using BtcKpi.Service.Common;
using BtcKpi.Web.ViewModels;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace BtcKpi.Web.Controllers
{
    public class ReportUpfCrossController : BaseController
    {
        #region Constructor
        private readonly IUserService userService;
        private readonly IReportService reportService;
        private readonly IDepartmentService departmentService;
        private readonly IUpfCrossService upfCrossService;

        public ReportUpfCrossController(IUserService userService, IDepartmentService departmentService, IReportService reportService, IUpfCrossService upfCrossService)
        {
            this.userService = userService;
            this.reportService = reportService;
            this.departmentService = departmentService;
            this.upfCrossService = upfCrossService;
        }
        #endregion Constructor

        #region Month
        // GET: Report - UpfCrossMonth
        public ActionResult UpfCrossMonthIndex()
        {
            UpfCrossListViewModel model = new UpfCrossListViewModel();
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
            model.Month = (DateTime.Now.Year).ToString();

            //Tháng
            model.Months = Months;
            model.Month = (DateTime.Now.Month - 1).ToString();

            model.UpfCrossInfos= new List<UpfCrossInfo>();

            return View(model);
        }

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
        public ActionResult SearchUpfCrossReport(UpfCrossListViewModel model)
        {
            model.UserID = CurrentUser.UserId;
            var userInfo = userService.GetUserFullInfo(CurrentUser.UserId);
            if (userInfo.DepartmentID != null) model.UserDepartmentID = (int) userInfo.DepartmentID;
            model.UpfCrossInfos = upfCrossService.GetUpfCrossReport(CurrentUser.UserId, model.UserDepartmentID, model.CompanyID, model.DepartmentID, model.Year, model.Month);
            model.UpfCrossInfos = model.UpfCrossInfos.OrderBy(o => o.Month).ThenBy(o => o.Year).ToList();
            model.UpfCrossInfos = SummaryUpfCrossInfos(model.UpfCrossInfos);

            List <DepartmentInfo> departmentInfos = departmentService.GetReportDepartment(model.CompanyID, model.DepartmentID, model.Year);
            DepartmentInfo departmentInfo = departmentInfos.Where(w=> w.ScheduleID == int.Parse(model.Month)).OrderBy(o => o.ScheduleID).ThenBy(o => o.ScheduleType).FirstOrDefault();
            foreach (var bo in model.UpfCrossInfos)
            {
                UpfCrossSummary upfSummary = departmentService.GetUpfCrossSummary(bo.ToDepartment, bo.Month, bo.Year);
                bo.ScoreBOD = upfSummary?.BodDependPoint ?? 0;
                bo.ScoreAssessed = Math.Round((decimal) (bo.ScoreAssessed / (bo.CountAverage > 1 ? bo.CountAverage : 1)), 2);
                if (departmentInfo != null && departmentInfo.TotalBODPoint != null) bo.ScoreAssessment = departmentInfo.TotalBODPoint;
                else if (departmentInfo != null && departmentInfo.TotalBODPoint == null) bo.ScoreAssessment = departmentInfo.AssessedScore;
                decimal? bodPoint = upfSummary != null && upfSummary.BodDependPoint > 0 ? upfSummary.BodDependPoint : bo.ScoreAssessed;
                var bodWeight = upfSummary != null && upfSummary.BodDependWeight > 0 ? upfSummary.BodDependWeight : bo.TotalDependWeight;
                bo.PerformancePoint = upfSummary?.BodPerforPoint ?? 0;
                var performanceWeight = upfSummary?.BodPerforWeight ?? 0;
                decimal? scoreAverage = 0;
                if (bo.PerformancePoint != null && bo.PerformancePoint > 0 && performanceWeight > 0)
                {
                    scoreAverage = Math.Round((decimal)((bodPoint * bodWeight) / 100 + (bo.PerformancePoint * performanceWeight) / 100 + (bo.ScoreAssessment * (100 - (bodWeight + performanceWeight))) / 100), 2);
                } else
                {
                    scoreAverage = Math.Round((decimal) ((bodPoint * bodWeight) / 100 + (bo.ScoreAssessment * (100 - bodWeight)) / 100), 2);
                }

                bo.ScoreAverage = upfSummary?.TotalScoreAverage ?? scoreAverage;
                bo.Rated = BtcHelper.ConvertSroreToRank(bo.ScoreAverage);
            }

            return PartialView("_UpfCrossMonthListTable", model);
        }

        public List<UpfCrossInfo> SummaryUpfCrossInfos(List<UpfCrossInfo> model)
        {
            List<UpfCrossInfo> upfCrossInfos = new List<UpfCrossInfo>();
            List<string> lstCheck = new List<string>();
            List<string> lstCheckYear = new List<string>();

            foreach (var bo in model)
            {
                if (upfCrossInfos.Count > 0)
                {
                    if (lstCheck.Contains(bo.ToDepartment.ToString()) && lstCheckYear.Contains(bo.Year.ToString()))
                    {
                        var count = 0;
                        foreach (var item in upfCrossInfos)
                        {
                            if (item.ToDepartment == bo.ToDepartment && item.Year == bo.Year && item.Month == bo.Month)
                            {
                                if (bo.FromScore != null)
                                {
                                    if (item.CountAverage != null && item.CountAverage > 0)
                                    {
                                        item.CountAverage++;
                                    }
                                    else
                                    {
                                        count++;
                                        item.CountAverage = count;
                                    }
                                }
                                item.ScoreAssessed = item.ScoreAssessed != null ? item.ScoreAssessed + bo.FromScore : bo.FromScore;
                                item.TotalDependWeight = (byte?)(item.TotalDependWeight != null ? item.TotalDependWeight + bo.FromWeight : bo.FromWeight);
                                if (bo.FromName.ToLower().Contains("Leasing".ToLower()))
                                {
                                    item.LeasingWeight = (byte?) (item.LeasingWeight != null ? item.LeasingWeight + bo.FromWeight : bo.FromWeight);
                                    item.LeasingScore = item.LeasingScore != null ? item.LeasingScore + bo.FromScore : bo.FromScore;
                                }
                                if (bo.FromName.ToLower().Contains("F&B".ToLower()) || bo.FromName.ToLower().Contains("FB".ToLower()))
                                {
                                    item.FbWeight = (byte?)(item.FbWeight != null ? item.FbWeight + bo.FromWeight : bo.FromWeight);
                                    item.FbScore = item.FbScore != null ? item.FbScore + bo.FromScore : bo.FromScore;
                                }
                                if (bo.FromName.ToLower().Contains("Ops".ToLower()) || bo.FromName.ToLower().Contains("Vận hành".ToLower()))
                                {
                                    item.OpsWeight = (byte?)(item.OpsWeight != null ? item.OpsWeight + bo.FromWeight : bo.FromWeight);
                                    item.OpsScore = item.OpsScore != null ? item.OpsScore + bo.FromScore : bo.FromScore;
                                }
                                if (bo.FromName.ToLower().Contains("Marketing".ToLower()) || bo.FromName.ToLower().Contains("MAR".ToLower()))
                                {
                                    item.MktWeight = (byte?)(item.MktWeight != null ? item.MktWeight + bo.FromWeight : bo.FromWeight);
                                    item.MktScore = item.MktScore != null ? item.MktScore + bo.FromScore : bo.FromScore;
                                }
                                if (bo.FromName.ToLower().Contains("Accounting".ToLower()) || bo.FromName.ToLower().Contains("ACC".ToLower()))
                                {
                                    item.AccWeight = (byte?)(item.AccWeight != null ? item.AccWeight + bo.FromWeight : bo.FromWeight);
                                    item.AccScore = item.AccScore != null ? item.AccScore + bo.FromScore : bo.FromScore;
                                }
                                if (bo.FromName.ToLower().Contains("HR".ToLower()) || bo.FromName.ToLower().Contains("Nhân sự".ToLower()))
                                {
                                    item.HrWeight = (byte?)(item.HrWeight != null ? item.HrWeight + bo.FromWeight : bo.FromWeight);
                                    item.HrScore = item.HrScore != null ? item.HrScore + bo.FromScore : bo.FromScore;
                                }
                                if (bo.FromName.ToLower().Contains("EPL".ToLower()) || bo.FromName.ToLower().Contains("Mua sắm".ToLower()))
                                {
                                    item.EplWeight = (byte?)(item.EplWeight != null ? item.EplWeight + bo.FromWeight : bo.FromWeight);
                                    item.EplScore = item.EplScore != null ? item.EplScore + bo.FromScore : bo.FromScore;
                                }
                                if (bo.FromName.ToLower().Contains("IT".ToLower()) || bo.FromName.ToLower().Contains("Công nghệ thông tin".ToLower()))
                                {
                                    item.ItWeight = (byte?)(item.ItWeight != null ? item.ItWeight + bo.FromWeight : bo.FromWeight);
                                    item.ItScore = item.ItScore != null ? item.ItScore + bo.FromScore : bo.FromScore;
                                }
                                if (bo.FromName.ToLower().Contains("Design".ToLower()) || bo.FromName.ToLower().Contains("Thiết kế".ToLower()))
                                {
                                    item.DesignWeight = (byte?)(item.DesignWeight != null ? item.DesignWeight + bo.FromWeight : bo.FromWeight);
                                    item.DesignScore = item.DesignScore != null ? item.DesignScore + bo.FromScore : bo.FromScore;
                                }
                                if (bo.FromName.ToLower().Contains("CRM".ToLower()))
                                {
                                    item.CrmWeight = (byte?)(item.CrmWeight != null ? item.CrmWeight + bo.FromWeight : bo.FromWeight);
                                    item.CrmScore = item.CrmScore != null ? item.CrmScore + bo.FromScore : bo.FromScore;
                                }
                                if (bo.FromName.ToLower().Contains("Legal".ToLower()))
                                {
                                    item.LegalWeight = (byte?)(item.LegalWeight != null ? item.LegalWeight + bo.FromWeight : bo.FromWeight);
                                    item.LegalScore = item.LegalScore != null ? item.LegalScore + bo.FromScore : bo.FromScore;
                                }
                                if (bo.FromName.ToLower().Contains("Kiểm Phẩm".ToLower()) || bo.FromName.ToLower().Contains("GC".ToLower()))
                                {
                                    item.GcWeight = (byte?)(item.GcWeight != null ? item.GcWeight + bo.FromWeight : bo.FromWeight);
                                    item.GcScore = item.GcScore != null ? item.GcScore + bo.FromScore : bo.FromScore;
                                }
                                if (bo.FromName.ToLower().Contains("Cashiers".ToLower()) || bo.FromName.ToLower().Contains("Thu Ngân".ToLower()))
                                {
                                    item.CashiersWeight = (byte?)(item.CashiersWeight != null ? item.CashiersWeight + bo.FromWeight : bo.FromWeight);
                                    item.CashiersScore = item.CashiersScore != null ? item.CashiersScore + bo.FromScore : bo.FromScore;
                                }
                                if (bo.FromName.ToLower().Contains("Tech".ToLower()) || bo.FromName.ToLower().Contains("Kỹ Thuật".ToLower()))
                                {
                                    item.TechWeight = (byte?)(item.TechWeight != null ? item.TechWeight + bo.FromWeight : bo.FromWeight);
                                    item.TechScore = item.TechScore != null ? item.TechScore + bo.FromScore : bo.FromScore;
                                }
                                if (bo.FromName.ToLower().Contains("SF".ToLower()) || bo.FromName.ToLower().Contains("StarFitness".ToLower()))
                                {
                                    item.SfWeight = (byte?)(item.SfWeight != null ? item.SfWeight + bo.FromWeight : bo.FromWeight);
                                    item.SfScore = item.SfScore != null ? item.SfScore + bo.FromScore : bo.FromScore;
                                }
                                if (bo.FromName.ToLower().Contains("CC".ToLower()) || bo.FromName.ToLower().Contains("Kiểm Ngân".ToLower()))
                                {
                                    item.CcWeight = (byte?)(item.CcWeight != null ? item.CcWeight + bo.FromWeight : bo.FromWeight);
                                    item.CcScore = item.CcScore != null ? item.CcScore + bo.FromScore : bo.FromScore;
                                }
                            }
                        }
                    }
                    else
                    {
                        AddWeightScoreDepend(bo);
                        upfCrossInfos.Add(bo);
                        lstCheck.Add(bo.ToDepartment.ToString());
                        lstCheckYear.Add(bo.Year.ToString());
                    }
                }
                else
                {
                    AddWeightScoreDepend(bo);
                    upfCrossInfos.Add(bo);
                    lstCheck.Add(bo.ToDepartment.ToString());
                    lstCheckYear.Add(bo.Year.ToString());
                }
            }

            return upfCrossInfos;
        }

        public UpfCrossInfo AddWeightScoreDepend(UpfCrossInfo bo)
        {
            var count = 0;
            bo.ScoreAssessed = bo.ScoreAssessed != null ? bo.ScoreAssessed + bo.FromScore : bo.FromScore;
            bo.TotalDependWeight = (byte?)(bo.TotalDependWeight != null ? bo.TotalDependWeight + bo.FromWeight : bo.FromWeight);
            if (bo.FromScore != null)
            {
                if (bo.CountAverage != null && bo.CountAverage > 0)
                {
                    bo.CountAverage++;
                }
                else
                {
                    count++;
                    bo.CountAverage = count;
                }
            }
            if (bo.FromName.ToLower().Contains("Leasing".ToLower()))
            {
                bo.LeasingWeight = bo.FromWeight;
                bo.LeasingScore = bo.FromScore;
            }
            if (bo.FromName.ToLower().Contains("F&B".ToLower()) || bo.FromName.ToLower().Contains("FB".ToLower()))
            {
                bo.FbWeight = bo.FromWeight;
                bo.FbScore = bo.FromScore;
            }
            if (bo.FromName.ToLower().Contains("Ops".ToLower()) || bo.FromName.ToLower().Contains("Vận hành".ToLower()))
            {
                bo.OpsWeight = bo.FromWeight;
                bo.OpsScore = bo.FromScore;
            }
            if (bo.FromName.ToLower().Contains("Marketing".ToLower()) || bo.FromName.ToLower().Contains("MAR".ToLower()))
            {
                bo.MktWeight = bo.FromWeight;
                bo.MktScore = bo.FromScore;
            }
            if (bo.FromName.ToLower().Contains("Accounting".ToLower()) || bo.FromName.ToLower().Contains("ACC".ToLower()))
            {
                bo.AccWeight = bo.FromWeight;
                bo.AccScore = bo.FromScore;
            }
            if (bo.FromName.ToLower().Contains("HR".ToLower()) || bo.FromName.ToLower().Contains("Nhân sự".ToLower()))
            {
                bo.HrWeight = bo.FromWeight;
                bo.HrScore = bo.FromScore;
            }
            if (bo.FromName.ToLower().Contains("EPL".ToLower()) || bo.FromName.ToLower().Contains("Mua sắm".ToLower()))
            {
                bo.EplWeight = bo.FromWeight;
                bo.EplScore = bo.FromScore;
            }
            if (bo.FromName.ToLower().Contains("IT".ToLower()) || bo.FromName.ToLower().Contains("Công nghệ thông tin".ToLower()))
            {
                bo.ItWeight = bo.FromWeight;
                bo.ItScore = bo.FromScore;
            }
            if (bo.FromName.ToLower().Contains("Design".ToLower()) || bo.FromName.ToLower().Contains("Thiết kế".ToLower()))
            {
                bo.DesignWeight = bo.FromWeight;
                bo.DesignScore = bo.FromScore;
            }
            if (bo.FromName.ToLower().Contains("CRM".ToLower()))
            {
                bo.CrmWeight = bo.FromWeight;
                bo.CrmScore = bo.FromScore;
            }
            if (bo.FromName.ToLower().Contains("Legal".ToLower()))
            {
                bo.LegalWeight = bo.FromWeight;
                bo.LegalScore = bo.FromScore;
            }
            if (bo.FromName.ToLower().Contains("Kiểm Phẩm".ToLower()) || bo.FromName.ToLower().Contains("GC".ToLower()))
            {
                bo.GcWeight = bo.FromWeight;
                bo.GcScore = bo.FromScore;
            }
            if (bo.FromName.ToLower().Contains("Cashiers".ToLower()) || bo.FromName.ToLower().Contains("Thu Ngân".ToLower()))
            {
                bo.CashiersWeight = bo.FromWeight;
                bo.CashiersScore = bo.FromScore;
            }
            if (bo.FromName.ToLower().Contains("Tech".ToLower()) || bo.FromName.ToLower().Contains("Kỹ Thuật".ToLower()))
            {
                bo.TechWeight = bo.FromWeight;
                bo.TechScore = bo.FromScore;
            }
            if (bo.FromName.ToLower().Contains("SF".ToLower()) || bo.FromName.ToLower().Contains("StarFitness".ToLower()))
            {
                bo.SfWeight = bo.FromWeight;
                bo.SfScore = bo.FromScore;
            }
            if (bo.FromName.ToLower().Contains("CC".ToLower()) || bo.FromName.ToLower().Contains("Kiểm Ngân".ToLower()))
            {
                bo.CcWeight = bo.FromWeight;
                bo.CcScore = bo.FromScore;
            }
            return bo;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ExportExcel(UpfCrossListViewModel model)
        {
            model.UserID = CurrentUser.UserId;
            var userInfo = userService.GetUserFullInfo(CurrentUser.UserId);
            if (userInfo.DepartmentID != null) model.UserDepartmentID = (int)userInfo.DepartmentID;
            model.UpfCrossInfos = upfCrossService.GetUpfCrossReport(CurrentUser.UserId, model.UserDepartmentID, model.CompanyID, model.DepartmentID, model.Year, model.Month);
            model.UpfCrossInfos = model.UpfCrossInfos.OrderBy(o => o.Month).ThenBy(o => o.Year).ToList();
            List<UpfCrossInfo> crossDetail = upfCrossService.GetUpfCrossDetailByUpfCrossId(model.CompanyID, model.DepartmentID, model.Year, model.Month);

            model.UpfCrossInfos = SummaryUpfCrossInfos(model.UpfCrossInfos);
            List<DepartmentInfo> departmentInfos = departmentService.GetDepartByConditions(model.CompanyID, model.DepartmentID, null, model.Year, model.Month, null);
            DepartmentInfo departmentInfo = departmentInfos.Where(w => w.ScheduleID == int.Parse(model.Month)).OrderBy(o => o.ScheduleID).ThenBy(o => o.ScheduleType).FirstOrDefault();

            foreach (var bo in model.UpfCrossInfos)
            {
                UpfCrossSummary upfSummary = departmentService.GetUpfCrossSummary(bo.ToDepartment, bo.Month, bo.Year);
                bo.ScoreBOD = upfSummary?.BodDependPoint ?? 0;
                bo.ScoreAssessed = Math.Round((decimal) (bo.ScoreAssessed / (bo.CountAverage > 1 ? bo.CountAverage : 1)), 2);
                if (departmentInfo != null && departmentInfo.TotalBODPoint != null) bo.ScoreAssessment = departmentInfo.TotalBODPoint;
                else if (departmentInfo != null && departmentInfo.TotalBODPoint == null) bo.ScoreAssessment = departmentInfo.AssessedScore;
                decimal? bodPoint = upfSummary != null && upfSummary.BodDependPoint > 0 ? upfSummary.BodDependPoint : bo.ScoreAssessed;
                var bodWeight = upfSummary != null && upfSummary.BodDependWeight > 0 ? upfSummary.BodDependWeight : bo.TotalDependWeight;
                bo.PerformancePoint = upfSummary?.BodPerforPoint ?? 0;
                var performanceWeight = upfSummary?.BodPerforWeight ?? 0;
                decimal? scoreAverage = 0;
                if (bo.PerformancePoint != null && bo.PerformancePoint > 0 && performanceWeight > 0)
                {
                    scoreAverage = Math.Round((decimal)((bodPoint * bodWeight) / 100 + (bo.PerformancePoint * performanceWeight) / 100 + (bo.ScoreAssessment * (100 - (bodWeight + performanceWeight))) / 100), 2);
                }
                else
                {
                    scoreAverage = Math.Round((decimal)((bodPoint * bodWeight) / 100 + (bo.ScoreAssessment * (100 - bodWeight)) / 100), 2);
                }

                bo.ScoreAverage = upfSummary?.TotalScoreAverage ?? scoreAverage;
                bo.Rated = BtcHelper.ConvertSroreToRank(bo.ScoreAverage);
                bo.BodDependWeight = upfSummary?.BodDependWeight;
                bo.BodDependScore = upfSummary?.BodDependPoint;
                bo.PerformanceWeight = upfSummary?.BodPerforWeight;
            }

            model.UpfCrossInfos = model.UpfCrossInfos.OrderBy(o => o.ToName).ToList();

            ExcelPackage excelEngine = FillDataTableExcel(model.UpfCrossInfos, crossDetail, model.Month, model.Year);
            Session["DownloadExcel_UPFCrossSummary"] = excelEngine.GetAsByteArray();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Download()
        {
            if (Session["DownloadExcel_UPFCrossSummary"] != null)
            {
                string dateStr = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy_hhmmss");
                var fileDownloadName = "Báo cáo tổng hợp đánh giá KPI tháng các phòng_" + dateStr + ".xlsx";
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                byte[] data = Session["DownloadExcel_UPFCrossSummary"] as byte[];
                return File(data, contentType, fileDownloadName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ExcelPackage FillDataTableExcel(List<UpfCrossInfo> model, List<UpfCrossInfo> crossDetail, string month, string year)
        {
            List<UpfCrossInfo> upfCrossInfos = new List<UpfCrossInfo>();
            List<UpfCrossInfo> upfCross = new List<UpfCrossInfo>();
            List<string> lstCheck = new List<string>();
            foreach (var bo in crossDetail)
            {
                if (upfCrossInfos.Count > 0 && lstCheck.Contains(bo.FromDepartment.ToString()))
                {
                    foreach (var item in upfCrossInfos)
                    {
                        if (item.FromDepartment == bo.FromDepartment)
                        {
                            if (item.CountFromDepartment != null)
                            {
                                item.CountFromDepartment++;
                                bo.FromWeight = bo.FromWeight ?? 0;
                                item.TotalFromWeight = (byte?) (item.TotalFromWeight != null && item.TotalFromWeight > 0 ? item.TotalFromWeight + bo.FromWeight : bo.FromWeight);
                                bo.ToWeight = bo.ToWeight ?? 0;
                                item.TotalToWeight = (byte?) (item.TotalToWeight != null && item.TotalToWeight > 0 ? item.TotalToWeight + bo.ToWeight : bo.ToWeight);
                                bo.CountFromDepartment = item.CountFromDepartment;
                                bo.TotalFromWeight = item.TotalFromWeight;
                                bo.TotalToWeight = item.TotalToWeight;
                                upfCross.Add(bo);
                            }
                            else
                            {
                                item.CountFromDepartment = 0;
                            }
                        }
                    }
                }
                else
                {
                    bo.FromWeight = bo.FromWeight ?? 0;
                    bo.ToWeight = bo.ToWeight ?? 0;
                    bo.CountFromDepartment = 0;
                    bo.TotalFromWeight = bo.FromWeight;
                    bo.TotalToWeight = bo.ToWeight;
                    upfCrossInfos.Add(bo);
                    upfCross.Add(bo);
                    lstCheck.Add(bo.FromDepartment.ToString());
                }
            }

            ExcelPackage excelEngine = new ExcelPackage();
            ExcelWorksheet worksheet1 = excelEngine.Workbook.Worksheets.Add("UPF-MONTHLY");
            AddWorksheet1(worksheet1, model, month, year);
            ExcelWorksheet worksheet2 = excelEngine.Workbook.Worksheets.Add("UPF Cross");
            AddWorksheet2(worksheet2, upfCross, month, year);



            return excelEngine;
        }

        public ExcelWorksheet AddWorksheet1(ExcelWorksheet worksheet1, List<UpfCrossInfo> model, string month, string year)
        {
            //Enter values to the cells from A3 to A5
            worksheet1.Cells["A1:W1"].Merge = true;
            worksheet1.Cells["A1"].Value = "BẢNG TỔNG HỢP ĐÁNH GIÁ KPI THÁNG " + month + "  NĂM " + year + " CỦA CÁC PHÒNG \r\n\t TABLE OF REVIEW DEPARTMENTS KPI OF MONTH " + month + " YEAR " + year;
            worksheet1.Cells["A1:W1"].Style.Font.Size = 14;
            worksheet1.Cells["A1"].Style.Font.Bold = true;
            worksheet1.Cells["A1:W1"].Style.WrapText = true;
            worksheet1.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet1.Row(1).Height = 40;
            Color colFromHexHeader = ColorTranslator.FromHtml("#FFFF00");
            worksheet1.Cells["A1:W1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet1.Cells["A1:W1"].Style.Fill.BackgroundColor.SetColor(colFromHexHeader);

            // format cells - add borders A2:Z2 body
            worksheet1.Cells["A2:W3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet1.Cells["A2:W3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet1.Cells["A2:W3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet1.Cells["A2:W3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet1.Cells["A2:W3"].Style.Font.Bold = true;
            worksheet1.Cells["A2:W3"].Style.WrapText = true;
            worksheet1.Cells["A2:W3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            worksheet1.Cells["A2:A3"].Merge = true;
            worksheet1.Cells["B2:B3"].Merge = true;
            worksheet1.Cells["C2:G2"].Merge = true;

            worksheet1.Cells["A2"].Value = "STT";
            worksheet1.Cells["B2"].Value = "PHÒNG/BỘ PHẬN ĐƯỢC ĐÁNH GIÁ";
            worksheet1.Cells["C2"].Value = "KPI";
            worksheet1.Cells["C3"].Value = "KPI";
            worksheet1.Cells["D3"].Value = "BOD đánh giá";
            worksheet1.Cells["E3"].Value = "Tỷ trọng Điểm BQ";
            worksheet1.Cells["F3"].Value = "Điểm BQ";
            worksheet1.Cells["G3"].Value = "Xếp loại";
            worksheet1.Cells["H2"].Value = "1";
            worksheet1.Cells["H3"].Value = "LS";
            worksheet1.Cells["I2"].Value = "2";
            worksheet1.Cells["I3"].Value = "F&B";
            worksheet1.Cells["J2"].Value = "3";
            worksheet1.Cells["J3"].Value = "Ops";
            worksheet1.Cells["K2"].Value = "4";
            worksheet1.Cells["K3"].Value = "MAR";
            worksheet1.Cells["L2"].Value = "5";
            worksheet1.Cells["L3"].Value = "ACC";
            worksheet1.Cells["M2"].Value = "6";
            worksheet1.Cells["M3"].Value = "ADHR";
            worksheet1.Cells["N2"].Value = "7";
            worksheet1.Cells["N3"].Value = "EPL";
            worksheet1.Cells["O2"].Value = "8";
            worksheet1.Cells["O3"].Value = "IT";
            worksheet1.Cells["P2"].Value = "9";
            worksheet1.Cells["P3"].Value = "DESIGN";
            worksheet1.Cells["Q2"].Value = "10";
            worksheet1.Cells["Q3"].Value = "CRM";
            worksheet1.Cells["R2"].Value = "11";
            worksheet1.Cells["R3"].Value = "LEGAL";
            worksheet1.Cells["S2"].Value = "12";
            worksheet1.Cells["S3"].Value = "GC";
            worksheet1.Cells["T2"].Value = "13";
            worksheet1.Cells["T3"].Value = "CASHIERS";
            worksheet1.Cells["U2"].Value = "14";
            worksheet1.Cells["U3"].Value = "TECH";
            worksheet1.Cells["V2"].Value = "15";
            worksheet1.Cells["V3"].Value = "SF";
            worksheet1.Cells["W2"].Value = "16";
            worksheet1.Cells["W3"].Value = "CC";
            Color colFromHex = ColorTranslator.FromHtml("#92D050");
            worksheet1.Cells["A2:W3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet1.Cells["A2:W3"].Style.Fill.BackgroundColor.SetColor(colFromHex);

            Color colFromHexBody = ColorTranslator.FromHtml("#D6DCE4");
            int col = 3;
            for (int i = 0; i < model.Count; i++)
            {
                col++;
                if (model[i].ToName.ToLower().Contains("Leasing".ToLower()) || model[i].ToName.ToLower().Contains("FB".ToLower())
                   || model[i].ToName.ToLower().Contains("Vận hành".ToLower()) || model[i].ToName.ToLower().Contains("Ops".ToLower()))
                {
                    worksheet1.Cells["C" + col + ":E" + col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet1.Cells["C" + col + ":E" + col].Style.Fill.BackgroundColor.SetColor(colFromHexBody);
                    worksheet1.Cells["H" + col + ":W" + col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet1.Cells["H" + col + ":W" + col].Style.Fill.BackgroundColor.SetColor(colFromHexBody);
                    worksheet1.Cells["C" + (col + 2) + ":E" + (col + 2)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet1.Cells["C" + (col + 2) + ":E" + (col + 2)].Style.Fill.BackgroundColor.SetColor(colFromHexBody);
                    worksheet1.Cells["H" + (col + 2) + ":W" + (col + 2)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet1.Cells["H" + (col + 2) + ":W" + (col + 2)].Style.Fill.BackgroundColor.SetColor(colFromHexBody);
                    worksheet1.Cells["C" + (col + 4) + ":E" + (col + 4)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet1.Cells["C" + (col + 4) + ":E" + (col + 4)].Style.Fill.BackgroundColor.SetColor(colFromHexBody);
                    worksheet1.Cells["H" + (col + 4) + ":W" + (col + 4)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet1.Cells["H" + (col + 4) + ":W" + (col + 4)].Style.Fill.BackgroundColor.SetColor(colFromHexBody);

                    worksheet1.Cells["A" + col + ":A" + (col + 5)].Merge = true;
                    worksheet1.Cells["A" + col].Value = i + 1;
                    worksheet1.Cells["A" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["B" + col + ":B" + (col + 5)].Merge = true;
                    worksheet1.Cells["B" + col].Value = model[i].ToName;
                    worksheet1.Cells["C" + col].Value = "Tỷ trọng phụ thuộc";
                    worksheet1.Cells["C" + (col + 1)].Value = "Điểm được đánh giá";
                    worksheet1.Cells["C" + (col + 2)].Value = "Tỷ trọng BOD";
                    worksheet1.Cells["C" + (col + 3)].Value = "Điểm BOD đánh giá";
                    worksheet1.Cells["C" + (col + 4)].Value = "Tỷ trọng chuyên môn";
                    worksheet1.Cells["C" + (col + 5)].Value = "Điểm tự đánh giá";
                    worksheet1.Cells["D" + col].Value = model[i].BodDependWeight != null && model[i].BodDependWeight > 0 ? model[i].BodDependWeight.ToString() + "%" : "";
                    worksheet1.Cells["D" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["D" + (col + 1)].Value = model[i].BodDependScore != null && model[i].BodDependScore > 0 ? model[i].BodDependScore : null;
                    worksheet1.Cells["D" + (col + 2)].Value = model[i].PerformanceWeight != null && model[i].PerformanceWeight > 0 ? model[i].PerformanceWeight.ToString() + "%" : "";
                    worksheet1.Cells["D" + (col + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["D" + (col + 3)].Value = model[i].PerformancePoint != null && model[i].PerformancePoint > 0 ? model[i].PerformancePoint : null;
                    worksheet1.Cells["D" + (col + 4)].Value = model[i].BodDependScore != null && model[i].BodDependScore > 0 ? (100 - (model[i].BodDependWeight + model[i].PerformanceWeight)).ToString() + "%" : "";
                    worksheet1.Cells["D" + (col + 4)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["D" + (col + 5)].Value = model[i].BodDependScore != null && model[i].BodDependScore > 0 ? model[i].ScoreAssessment : null;
                    worksheet1.Cells["E" + col].Value = model[i].TotalDependWeight.ToString() + "%";
                    worksheet1.Cells["E" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["E" + (col + 1)].Value = model[i].ScoreAssessed;
                    worksheet1.Cells["E" + (col + 2)].Value = "";
                    worksheet1.Cells["E" + (col + 3)].Value = "";
                    worksheet1.Cells["E" + (col + 4)].Value = (100 - model[i].TotalDependWeight).ToString() + "%";
                    worksheet1.Cells["E" + (col + 4)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["E" + (col + 5)].Value = model[i].ScoreAssessment;
                    worksheet1.Cells["F" + col + ":F" + (col + 5)].Merge = true;
                    worksheet1.Cells["F" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["F" + col].Value = model[i].ScoreAverage;
                    worksheet1.Cells["G" + col + ":G" + (col + 5)].Merge = true;
                    worksheet1.Cells["G" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["G" + col].Value = BtcHelper.ConvertSroreToRank(model[i].ScoreAverage);
                    Color colFromHex1 = ColorTranslator.FromHtml("#FFD966");
                    if (model[i].ToName.ToLower().Contains("Leasing".ToLower()))
                    {
                        worksheet1.Cells["H" + col + ":H" + (col + 5)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet1.Cells["H" + col + ":H" + (col + 5)].Style.Fill.BackgroundColor.SetColor(colFromHex1);
                        worksheet1.Cells["H" + col + ":H" + (col + 5)].Value = "";
                    }
                    if (model[i].ToName.ToLower().Contains("F&B".ToLower()) || model[i].ToName.ToLower().Contains("FB".ToLower()))
                    {
                        worksheet1.Cells["I" + col + ":I" + (col + 5)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet1.Cells["I" + col + ":I" + (col + 5)].Style.Fill.BackgroundColor.SetColor(colFromHex1);
                        worksheet1.Cells["I" + col + ":I" + (col + 5)].Value = "";
                    }
                    if (model[i].ToName.ToLower().Contains("Ops".ToLower()) || model[i].ToName.ToLower().Contains("Vận hành".ToLower()))
                    {
                        worksheet1.Cells["J" + col + ":J" + (col + 5)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet1.Cells["J" + col + ":J" + (col + 5)].Style.Fill.BackgroundColor.SetColor(colFromHex1);
                        worksheet1.Cells["J" + col + ":J" + (col + 5)].Value = "";
                    }
                    worksheet1.Cells["H" + col].Value = model[i].LeasingWeight != null ? model[i].LeasingWeight.ToString() + "%" : "";
                    worksheet1.Cells["H" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["H" + (col + 1)].Value = model[i].LeasingScore;
                    worksheet1.Cells["I" + col].Value = model[i].FbWeight != null ? model[i].FbWeight.ToString() + "%" : "";
                    worksheet1.Cells["I" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["I" + (col + 1)].Value = model[i].FbScore;
                    worksheet1.Cells["J" + col].Value = model[i].OpsWeight != null ? model[i].OpsWeight.ToString() + "%" : "";
                    worksheet1.Cells["J" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["J" + (col + 1)].Value = model[i].OpsScore;
                    worksheet1.Cells["K" + col].Value = model[i].MktWeight != null ? model[i].MktWeight.ToString() + "%" : "";
                    worksheet1.Cells["K" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["K" + (col + 1)].Value = model[i].MktScore;
                    worksheet1.Cells["L" + col].Value = model[i].AccWeight != null ? model[i].AccWeight.ToString() + "%" : "";
                    worksheet1.Cells["L" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["L" + (col + 1)].Value = model[i].AccScore;
                    worksheet1.Cells["M" + col].Value = model[i].HrWeight != null ? model[i].HrWeight.ToString() + "%" : "";
                    worksheet1.Cells["M" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["M" + (col + 1)].Value = model[i].HrScore;
                    worksheet1.Cells["N" + col].Value = model[i].EplWeight != null ? model[i].EplWeight.ToString() + "%" : "";
                    worksheet1.Cells["N" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["N" + (col + 1)].Value = model[i].EplScore;
                    worksheet1.Cells["O" + col].Value = model[i].ItWeight != null ? model[i].ItWeight.ToString() + "%" : "";
                    worksheet1.Cells["O" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["O" + (col + 1)].Value = model[i].ItScore;
                    worksheet1.Cells["P" + col].Value = model[i].DesignWeight != null ? model[i].DesignWeight.ToString() + "%" : "";
                    worksheet1.Cells["P" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["P" + (col + 1)].Value = model[i].DesignScore;
                    worksheet1.Cells["Q" + col].Value = model[i].CrmWeight != null ? model[i].CrmWeight.ToString() + "%" : "";
                    worksheet1.Cells["Q" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["Q" + (col + 1)].Value = model[i].CrmScore;
                    worksheet1.Cells["R" + col].Value = model[i].LegalWeight != null ? model[i].LegalWeight.ToString() + "%" : "";
                    worksheet1.Cells["R" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["R" + (col + 1)].Value = model[i].LegalScore;
                    worksheet1.Cells["S" + col].Value = model[i].GcWeight != null ? model[i].GcWeight.ToString() + "%" : "";
                    worksheet1.Cells["S" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["S" + (col + 1)].Value = model[i].GcScore;
                    worksheet1.Cells["T" + col].Value = model[i].CashiersWeight != null ? model[i].CashiersWeight.ToString() + "%" : "";
                    worksheet1.Cells["T" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["T" + (col + 1)].Value = model[i].CashiersScore;
                    worksheet1.Cells["V" + col].Value = model[i].SfWeight != null ? model[i].SfWeight.ToString() + "%" : "";
                    worksheet1.Cells["V" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["V" + (col + 1)].Value = model[i].SfScore;
                    worksheet1.Cells["W" + col].Value = model[i].CcWeight != null ? model[i].CcWeight.ToString() + "%" : "";
                    worksheet1.Cells["W" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["W" + (col + 1)].Value = model[i].CcScore;
                    worksheet1.Cells["F" + col + ":G" + col].Style.Font.Color.SetColor(Color.Red);

                    col = col + 5;
                }
                else
                {
                    worksheet1.Cells["C" + col + ":E" + col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet1.Cells["C" + col + ":E" + col].Style.Fill.BackgroundColor.SetColor(colFromHexBody);
                    worksheet1.Cells["H" + col + ":W" + col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet1.Cells["H" + col + ":W" + col].Style.Fill.BackgroundColor.SetColor(colFromHexBody);
                    worksheet1.Cells["C" + (col + 2) + ":E" + (col + 2)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet1.Cells["C" + (col + 2) + ":E" + (col + 2)].Style.Fill.BackgroundColor.SetColor(colFromHexBody);
                    worksheet1.Cells["H" + (col + 2) + ":W" + (col + 2)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet1.Cells["H" + (col + 2) + ":W" + (col + 2)].Style.Fill.BackgroundColor.SetColor(colFromHexBody);

                    worksheet1.Cells["A" + col + ":A" + (col + 3)].Merge = true;
                    worksheet1.Cells["A" + col].Value = i + 1;
                    worksheet1.Cells["A" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["B" + col + ":B" + (col + 3)].Merge = true;
                    worksheet1.Cells["B" + col].Value = model[i].ToName;
                    worksheet1.Cells["C" + col].Value = "Tỷ trọng phụ thuộc";
                    worksheet1.Cells["C" + (col + 1)].Value = "Điểm được đánh giá";
                    worksheet1.Cells["C" + (col + 2)].Value = "Tỷ trọng chuyên môn";
                    worksheet1.Cells["C" + (col + 3)].Value = "Điểm tự đánh giá";
                    worksheet1.Cells["D" + col].Value = model[i].BodDependWeight != null && model[i].BodDependWeight > 0 ? model[i].BodDependWeight.ToString() + "%" : "";
                    worksheet1.Cells["D" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["D" + (col + 1)].Value = model[i].BodDependScore != null && model[i].BodDependScore > 0 ? model[i].BodDependScore : null;
                    worksheet1.Cells["D" + (col + 2)].Value = model[i].BodDependWeight != null && model[i].BodDependWeight > 0 ? (100 - model[i].BodDependWeight).ToString() + "%" : "";
                    worksheet1.Cells["D" + (col + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["D" + (col + 3)].Value = model[i].BodDependScore != null && model[i].BodDependScore > 0 ? model[i].ScoreAssessment : null;
                    worksheet1.Cells["E" + col].Value = model[i].TotalDependWeight.ToString() + "%";
                    worksheet1.Cells["E" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["E" + (col + 1)].Value = model[i].ScoreAssessed;
                    worksheet1.Cells["E" + (col + 2)].Value = (100 - model[i].TotalDependWeight).ToString() + "%";
                    worksheet1.Cells["E" + (col + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["E" + (col + 3)].Value = model[i].ScoreAssessment;
                    worksheet1.Cells["F" + col + ":F" + (col + 3)].Merge = true;
                    worksheet1.Cells["F" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["F" + col].Value = model[i].ScoreAverage;
                    worksheet1.Cells["G" + col + ":G" + (col + 3)].Merge = true;
                    worksheet1.Cells["G" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["G" + col].Value = BtcHelper.ConvertSroreToRank(model[i].ScoreAverage);

                    worksheet1.Cells["H" + col].Value = model[i].LeasingWeight != null ? model[i].LeasingWeight.ToString() + "%" : "";
                    worksheet1.Cells["H" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["H" + (col + 1)].Value = model[i].LeasingScore;
                    worksheet1.Cells["I" + col].Value = model[i].FbWeight != null ? model[i].FbWeight.ToString() + "%" : "";
                    worksheet1.Cells["I" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["I" + (col + 1)].Value = model[i].FbScore;
                    worksheet1.Cells["J" + col].Value = model[i].OpsWeight != null ? model[i].OpsWeight.ToString() + "%" : "";
                    worksheet1.Cells["J" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["J" + (col + 1)].Value = model[i].OpsScore;
                    worksheet1.Cells["K" + col].Value = model[i].MktWeight != null ? model[i].MktWeight.ToString() + "%" : "";
                    worksheet1.Cells["K" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["K" + (col + 1)].Value = model[i].MktScore;
                    worksheet1.Cells["L" + col].Value = model[i].AccWeight != null ? model[i].AccWeight.ToString() + "%" : "";
                    worksheet1.Cells["L" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["L" + (col + 1)].Value = model[i].AccScore;
                    worksheet1.Cells["M" + col].Value = model[i].HrWeight != null ? model[i].HrWeight.ToString() + "%" : "";
                    worksheet1.Cells["M" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["M" + (col + 1)].Value = model[i].HrScore;
                    worksheet1.Cells["N" + col].Value = model[i].EplWeight != null ? model[i].EplWeight.ToString() + "%" : "";
                    worksheet1.Cells["N" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["N" + (col + 1)].Value = model[i].EplScore;
                    worksheet1.Cells["O" + col].Value = model[i].ItWeight != null ? model[i].ItWeight.ToString() + "%" : "";
                    worksheet1.Cells["O" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["O" + (col + 1)].Value = model[i].ItScore;
                    worksheet1.Cells["P" + col].Value = model[i].DesignWeight != null ? model[i].DesignWeight.ToString() + "%" : "";
                    worksheet1.Cells["P" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["P" + (col + 1)].Value = model[i].DesignScore;
                    worksheet1.Cells["Q" + col].Value = model[i].CrmWeight != null ? model[i].CrmWeight.ToString() + "%" : "";
                    worksheet1.Cells["Q" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["Q" + (col + 1)].Value = model[i].CrmScore;
                    worksheet1.Cells["R" + col].Value = model[i].LegalWeight != null ? model[i].LegalWeight.ToString() + "%" : "";
                    worksheet1.Cells["R" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["R" + (col + 1)].Value = model[i].LegalScore;
                    worksheet1.Cells["S" + col].Value = model[i].GcWeight != null ? model[i].GcWeight.ToString() + "%" : "";
                    worksheet1.Cells["S" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["S" + (col + 1)].Value = model[i].GcScore;
                    worksheet1.Cells["T" + col].Value = model[i].CashiersWeight != null ? model[i].CashiersWeight.ToString() + "%" : "";
                    worksheet1.Cells["T" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["T" + (col + 1)].Value = model[i].CashiersScore;
                    worksheet1.Cells["U" + col].Value = model[i].TechWeight != null ? model[i].TechWeight.ToString() + "%" : "";
                    worksheet1.Cells["U" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["U" + (col + 1)].Value = model[i].TechScore;
                    worksheet1.Cells["V" + col].Value = model[i].SfWeight != null ? model[i].SfWeight.ToString() + "%" : "";
                    worksheet1.Cells["V" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["V" + (col + 1)].Value = model[i].SfScore;
                    worksheet1.Cells["W" + col].Value = model[i].CcWeight != null ? model[i].CcWeight.ToString() + "%" : "";
                    worksheet1.Cells["W" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells["W" + (col + 1)].Value = model[i].CcScore;
                    Color colFromHex1 = ColorTranslator.FromHtml("#FFD966");
                    if (model[i].ToName.ToLower().Contains("Marketing".ToLower()) || model[i].ToName.ToLower().Contains("MAR".ToLower()))
                    {
                        worksheet1.Cells["K" + col + ":K" + (col + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet1.Cells["K" + col + ":K" + (col + 3)].Style.Fill.BackgroundColor.SetColor(colFromHex1);
                        worksheet1.Cells["K" + col + ":K" + (col + 3)].Value = "";
                    }
                    if (model[i].ToName.ToLower().Contains("Accounting".ToLower()) || model[i].ToName.ToLower().Contains("ACC".ToLower()))
                    {
                        worksheet1.Cells["L" + col + ":L" + (col + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet1.Cells["L" + col + ":L" + (col + 3)].Style.Fill.BackgroundColor.SetColor(colFromHex1);
                        worksheet1.Cells["L" + col + ":L" + (col + 3)].Value = "";
                    }
                    if (model[i].ToName.ToLower().Contains("HR".ToLower()))
                    {
                        worksheet1.Cells["M" + col + ":M" + (col + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet1.Cells["M" + col + ":M" + (col + 3)].Style.Fill.BackgroundColor.SetColor(colFromHex1);
                        worksheet1.Cells["M" + col + ":M" + (col + 3)].Value = "";
                    }
                    if (model[i].ToName.ToLower().Contains("EPL".ToLower()) || model[i].ToName.ToLower().Contains("Mua sắm".ToLower()))
                    {
                        worksheet1.Cells["N" + col + ":N" + (col + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet1.Cells["N" + col + ":N" + (col + 3)].Style.Fill.BackgroundColor.SetColor(colFromHex1);
                        worksheet1.Cells["N" + col + ":N" + (col + 3)].Value = "";
                    }
                    if (model[i].ToName.ToLower().Contains("IT".ToLower()) || model[i].ToName.ToLower().Contains("Công nghệ thông tin".ToLower()))
                    {
                        worksheet1.Cells["O" + col + ":O" + (col + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet1.Cells["O" + col + ":O" + (col + 3)].Style.Fill.BackgroundColor.SetColor(colFromHex1);
                        worksheet1.Cells["O" + col + ":O" + (col + 3)].Value = "";
                    }
                    if (model[i].ToName.ToLower().Contains("Design".ToLower()) || model[i].ToName.ToLower().Contains("Thiết kế".ToLower()))
                    {
                        worksheet1.Cells["P" + col + ":P" + (col + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet1.Cells["P" + col + ":P" + (col + 3)].Style.Fill.BackgroundColor.SetColor(colFromHex1);
                        worksheet1.Cells["P" + col + ":P" + (col + 3)].Value = "";
                    }
                    if (model[i].ToName.ToLower().Contains("CRM".ToLower()))
                    {
                        worksheet1.Cells["Q" + col + ":Q" + (col + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet1.Cells["Q" + col + ":Q" + (col + 3)].Style.Fill.BackgroundColor.SetColor(colFromHex1);
                        worksheet1.Cells["Q" + col + ":Q" + (col + 3)].Value = "";
                    }
                    if (model[i].ToName.ToLower().Contains("Legal".ToLower()))
                    {
                        worksheet1.Cells["R" + col + ":R" + (col + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet1.Cells["R" + col + ":R" + (col + 3)].Style.Fill.BackgroundColor.SetColor(colFromHex1);
                        worksheet1.Cells["R" + col + ":R" + (col + 3)].Value = "";
                    }
                    if (model[i].ToName.ToLower().Contains("Kiểm Phẩm".ToLower()) || model[i].ToName.ToLower().Contains("GC".ToLower()))
                    {
                        worksheet1.Cells["S" + col + ":S" + (col + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet1.Cells["S" + col + ":S" + (col + 3)].Style.Fill.BackgroundColor.SetColor(colFromHex1);
                        worksheet1.Cells["S" + col + ":S" + (col + 3)].Value = "";
                    }
                    if (model[i].ToName.ToLower().Contains("Cashiers".ToLower()) || model[i].ToName.ToLower().Contains("Thu Ngân".ToLower()))
                    {
                        worksheet1.Cells["T" + col + ":T" + (col + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet1.Cells["T" + col + ":T" + (col + 3)].Style.Fill.BackgroundColor.SetColor(colFromHex1);
                        worksheet1.Cells["T" + col + ":T" + (col + 3)].Value = "";
                    }
                    if (model[i].ToName.ToLower().Contains("Tech".ToLower()) || model[i].ToName.ToLower().Contains("Kỹ Thuật".ToLower()))
                    {
                        worksheet1.Cells["U" + col + ":U" + (col + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet1.Cells["U" + col + ":U" + (col + 3)].Style.Fill.BackgroundColor.SetColor(colFromHex1);
                        worksheet1.Cells["U" + col + ":U" + (col + 3)].Value = "";
                    }
                    if (model[i].ToName.ToLower().Contains("SF".ToLower()) || model[i].ToName.ToLower().Contains("StarFitness".ToLower()))
                    {
                        worksheet1.Cells["V" + col + ":V" + (col + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet1.Cells["V" + col + ":V" + (col + 3)].Style.Fill.BackgroundColor.SetColor(colFromHex1);
                        worksheet1.Cells["V" + col + ":V" + (col + 3)].Value = "";
                    }
                    if (model[i].ToName.ToLower().Contains("CC".ToLower()) || model[i].ToName.ToLower().Contains("Kiểm Ngân".ToLower()))
                    {
                        worksheet1.Cells["W" + col + ":W" + (col + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet1.Cells["W" + col + ":W" + (col + 3)].Style.Fill.BackgroundColor.SetColor(colFromHex1);
                        worksheet1.Cells["W" + col + ":W" + (col + 3)].Value = "";
                    }
                    worksheet1.Cells["F" + col + ":G" + col].Style.Font.Color.SetColor(Color.Red);

                    col = col + 3;
                }
            }

            worksheet1.Cells["A2:W" + col].Style.Font.Size = 10;
            // format cells - add borders A5:I...
            worksheet1.Cells["A2:W" + col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet1.Cells["A2:W" + col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet1.Cells["A2:W" + col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet1.Cells["A2:W" + col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            col++;
            col = col + 1;

            worksheet1.Cells["A" + col + ":D" + col].Merge = true;
            worksheet1.Cells["A" + col].Value = "Người lập";
            worksheet1.Cells["A" + col].Style.Font.Bold = true;
            worksheet1.Cells["A" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet1.Cells["A1:W" + col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet1.Cells["A3:W" + col].Style.WrapText = true;
            //Font
            worksheet1.Cells["A" + col].Style.Font.Size = 12;
            worksheet1.Cells["A1:W" + col].Style.Font.Name = "Times New Roman";
            //Apply row height and column width to look good
            worksheet1.Column(1).Width = 5;
            worksheet1.Column(2).Width = 25;
            worksheet1.Column(3).Width = 18;
            worksheet1.Column(4).Width = 10;
            worksheet1.Column(5).Width = 10;
            worksheet1.Column(6).Width = 9;
            worksheet1.Column(7).Width = 9;
            worksheet1.Column(8).Width = 8;
            worksheet1.Column(9).Width = 8;
            worksheet1.Column(10).Width = 8;
            worksheet1.Column(11).Width = 8;
            worksheet1.Column(12).Width = 8;
            worksheet1.Column(13).Width = 8;
            worksheet1.Column(14).Width = 8;
            worksheet1.Column(15).Width = 8;
            worksheet1.Column(16).Width = 8;
            worksheet1.Column(17).Width = 8;
            worksheet1.Column(18).Width = 8;
            worksheet1.Column(19).Width = 8;
            worksheet1.Column(20).Width = 10;
            worksheet1.Column(21).Width = 8;
            worksheet1.Column(22).Width = 8;
            worksheet1.Column(23).Width = 8;

            return worksheet1;
        }

        public ExcelWorksheet AddWorksheet2(ExcelWorksheet worksheet2, List<UpfCrossInfo> model, string month, string year)
        {
            worksheet2.Cells["A1:U1"].Merge = true;
            worksheet2.Cells["A1"].Value = "BẢNG TỔNG HỢP ĐÁNH GIÁ KPI THÁNG " + month + "  NĂM " + year +
                                           " CỦA CÁC PHÒNG \r\n\t TABLE OF REVIEW DEPARTMENTS KPI OF MONTH " + month + " YEAR " + year;
            worksheet2.Cells["A1:U1"].Style.Font.Size = 14;
            worksheet2.Cells["A1"].Style.Font.Bold = true;
            worksheet2.Cells["A1:U1"].Style.WrapText = true;
            worksheet2.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet2.Row(1).Height = 40;
            Color colFromHexHeader = ColorTranslator.FromHtml("#FFFF00");
            worksheet2.Cells["A1:U1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet2.Cells["A1:U1"].Style.Fill.BackgroundColor.SetColor(colFromHexHeader);

            // format cells - add borders A2:Z2 body
            worksheet2.Cells["A2:U3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet2.Cells["A2:U3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet2.Cells["A2:U3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet2.Cells["A2:U3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet2.Cells["A2:U3"].Style.Font.Bold = true;
            worksheet2.Cells["A2:U3"].Style.WrapText = true;
            worksheet2.Cells["A2:U3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            worksheet2.Cells["A2:A3"].Merge = true;
            worksheet2.Cells["B2:B3"].Merge = true;
            worksheet2.Cells["C2:L2"].Merge = true;
            worksheet2.Cells["G3:H3"].Merge = true;
            worksheet2.Cells["M2:S2"].Merge = true;
            worksheet2.Cells["Q3:R3"].Merge = true;
            worksheet2.Cells["T2:U2"].Merge = true;

            worksheet2.Cells["A2"].Value = "No.\r\n\t(1)";
            worksheet2.Cells["B2"].Value = "KPI/Objective\r\n\tKPI / Mục tiêu\r\n\t(2)";
            worksheet2.Cells["C2"].Value =
                "Requested by Department\r\n\tPhòng đưa yêu cầu liên quan đến công việc mục tiêu";
            worksheet2.Cells["C3"].Value = "Requested by Dept.\r\n\tPhòng yêu cầu\r\n\t(3)";
            worksheet2.Cells["D3"].Value = "Contents requested / \r\n\tNội dung yêu cầu\r\n\t(4)";
            worksheet2.Cells["E3"].Value = "Estimated Time of completion / \r\n\tThời hạn cần hoàn thành\r\n\t(5)";
            worksheet2.Cells["F3"].Value = "Expected benefits/result\r\n\tKết quả/ Lợi ích mong đợi / \r\n\t(6)";
            worksheet2.Cells["G3"].Value = "Weight / \r\n\tTỷ trọng\r\n\t(7)";
            worksheet2.Cells["I3"].Value = "Department in charge\r\n\tPhòng / bộ phận thực hiện\r\n\t(8)";
            worksheet2.Cells["J3"].Value = "Time of completion\r\n\tThời gian hoàn thành\r\n\t(9)";
            worksheet2.Cells["K3"].Value = "Results /Ghi nhận kết quả thực hiện\r\n\t(10)";
            worksheet2.Cells["L3"].Value = "Điểm đánh giá /Assessment mark\r\n\t(11)";
            worksheet2.Cells["M2"].Value = "Phòng thực hiện / Department in charge";
            worksheet2.Cells["M3"].Value = "Kế hoạch thực hiện yêu cầu / Plan to do the requests\r\n\t(12)";
            worksheet2.Cells["N3"].Value =
                "Phân tích/ giải trình cho kết quả thực hiện\r\n\tAnalysis / Explaination for results\r\n\t(13)";
            worksheet2.Cells["O3"].Value = "Giải pháp / Solutions\r\n\t(14)";
            worksheet2.Cells["P3"].Value = "Thời hạn / Timeline\r\n\t(15)";
            worksheet2.Cells["Q3"].Value = "Weight / Tỷ trọng\r\n\t(16)";
            worksheet2.Cells["S3"].Value = "Tự đánh giá /Self-assessment mark\r\n\t(17)";
            worksheet2.Cells["T2"].Value = "Assessment";
            worksheet2.Cells["T3"].Value = "Assessment mark by the council\r\n\t(18)";
            worksheet2.Cells["U3"].Value = "Total mark\r\n\t(19)";
            Color colFromHex = ColorTranslator.FromHtml("#92D050");
            worksheet2.Cells["A2:U3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet2.Cells["A2:U3"].Style.Fill.BackgroundColor.SetColor(colFromHex);

            worksheet2.Cells["A4:U4"].Style.Font.Bold = true;
            worksheet2.Cells["A4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet2.Cells["B4:U4"].Merge = true;
            worksheet2.Cells["A4"].Value = "A";
            worksheet2.Cells["B4"].Value = "Routine works / Công việc thường xuyên";

            int col = 4;
            List<string> lstFromDepartment = new List<string>();
            int countForm = 0;
            for (int i = 0; i < model.Count; i++)
            {
                if (!lstFromDepartment.Contains(model[i].FromDepartment.ToString()))
                {
                    countForm = 0;
                    lstFromDepartment.Add(model[i].FromDepartment.ToString());
                }

                col++;
                if (lstFromDepartment.Contains(model[i].FromDepartment.ToString()))
                {
                    for (int j = 0; j < lstFromDepartment.Count; j++)
                    {
                        if(lstFromDepartment[j].Equals(model[i].FromDepartment.ToString()) && countForm == 0)
                        {
                            var totalFromWeight = model.Where(w => w.FromDepartment == model[i].FromDepartment).Max(m => m.TotalFromWeight);
                            var totalToWeight = model.Where(w => w.ToDepartment == model[i].ToDepartment).Max(m => m.TotalToWeight);
                            var countFromDepartment = model.Where(w => w.FromDepartment == model[i].FromDepartment).Max(m => m.CountFromDepartment);
                            worksheet2.Cells["A" + col + ":A" + (col + countFromDepartment)].Merge = true;
                            worksheet2.Cells["C" + col + ":C" + (col + countFromDepartment)].Merge = true;
                            worksheet2.Cells["H" + col + ":H" + (col + countFromDepartment)].Merge = true;
                            worksheet2.Cells["A" + col + ":A" + (col + countFromDepartment)].Value = j + 1;
                            worksheet2.Cells["C" + col + ":C" + (col + countFromDepartment)].Value = model[i].FromName;
                            worksheet2.Cells["H" + col + ":H" + (col + countFromDepartment)].Value = totalFromWeight + "%";
                            worksheet2.Cells["R" + col + ":R" + (col + countFromDepartment)].Merge = true;
                            worksheet2.Cells["R" + col + ":R" + (col + countFromDepartment)].Value = totalToWeight + "%";

                            countForm++;
                        }
                    }
                }
                worksheet2.Cells["A" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet2.Cells["B" + col].Value = model[i].Objective;
                worksheet2.Cells["D" + col].Value = model[i].ContentsRequested;
                worksheet2.Cells["E" + col].Value = model[i].ExpectedTimeOfCompletion;
                worksheet2.Cells["F" + col].Value = model[i].ExpectedResult;
                worksheet2.Cells["G" + col].Value = model[i].FromWeight + "%";
                worksheet2.Cells["G" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet2.Cells["H" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet2.Cells["I" + col].Value = model[i].ToName;
                worksheet2.Cells["J" + col].Value = model[i].TimeOfCompletion;
                worksheet2.Cells["K" + col].Value = model[i].Result;
                worksheet2.Cells["L" + col].Value = model[i].FromScore;
                worksheet2.Cells["L" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet2.Cells["M" + col].Value = model[i].PlanToDo;
                worksheet2.Cells["N" + col].Value = model[i].ExplainationForResults;
                worksheet2.Cells["O" + col].Value = model[i].Solutions;
                worksheet2.Cells["P" + col].Value = model[i].Timeline;
                worksheet2.Cells["Q" + col].Value = model[i].ToWeight + "%";
                worksheet2.Cells["Q" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet2.Cells["R" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet2.Cells["S" + col].Value = model[i].ToScore;
                worksheet2.Cells["S" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet2.Cells["T" + col].Value = model[i].AssessmentByCouncil;
            }

            worksheet2.Cells["A2:U" + col].Style.Font.Size = 11;
            // format cells - add borders A5:I...
            worksheet2.Cells["A2:U" + col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet2.Cells["A2:U" + col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet2.Cells["A2:U" + col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet2.Cells["A2:U" + col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            col++;
            col = col + 1;

            worksheet2.Cells["B" + col + ":D" + col].Merge = true;
            worksheet2.Cells["B" + col].Style.Font.Bold = true;
            worksheet2.Cells["B" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet2.Cells["B" + col].Value = "ASSESSMENT COUNCIL CHAIRMAN";
            worksheet2.Cells["P" + col + ":R" + col].Merge = true;
            worksheet2.Cells["P" + col].Style.Font.Bold = true;
            worksheet2.Cells["P" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet2.Cells["P" + col].Value = "HEAD OF UNIT";

            worksheet2.Cells["A1:U" + col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet2.Cells["A2:U" + col].Style.WrapText = true;
            //Font
            worksheet2.Cells["A" + col].Style.Font.Size = 12;
            worksheet2.Cells["A1:U" + col].Style.Font.Name = "Times New Roman";
            //Apply row height and column width to look good
            worksheet2.Column(1).Width = 6;
            worksheet2.Column(2).Width = 30;
            worksheet2.Column(3).Width = 23;
            worksheet2.Column(4).Width = 36;
            worksheet2.Column(5).Width = 22;
            worksheet2.Column(6).Width = 45;
            worksheet2.Column(7).Width = 10;
            worksheet2.Column(8).Width = 10;
            worksheet2.Column(9).Width = 24;
            worksheet2.Column(10).Width = 14;
            worksheet2.Column(11).Width = 24;
            worksheet2.Column(12).Width = 19;
            worksheet2.Column(13).Width = 19;
            worksheet2.Column(14).Width = 19;
            worksheet2.Column(15).Width = 19;
            worksheet2.Column(16).Width = 19;
            worksheet2.Column(17).Width = 10;
            worksheet2.Column(18).Width = 10;
            worksheet2.Column(19).Width = 14;
            worksheet2.Column(20).Width = 14;
            worksheet2.Column(21).Width = 19;

            return worksheet2;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpfCrossSumApproved(UpfSummaryViewModel model)
        {
            List<UpfCrossDetail> upfCrossDetails = new List<UpfCrossDetail>();
            UpfCross upfCross = upfCrossService.GetUpfCrossById(model.UpfCrossId, ref upfCrossDetails);
            UpfCrossDetail crossDetail = upfCrossDetails.FirstOrDefault(w => w.ID == model.UpfCrossDetailId && w.DeleteFlg == 0);
            if (crossDetail != null)
            {
                UpfCrossSummary crossSummary = departmentService.GetUpfCrossSummary(crossDetail.ToDepartment, upfCross.Month, upfCross.Year);
                List<DepartmentInfo> departmentInfos = departmentService.GetReportDepartment(null, crossDetail.ToDepartment.ToString(), upfCross.Year.ToString());
                DepartmentInfo departmentInfo = departmentInfos.Where(w => w.ScheduleID == upfCross.Month).OrderBy(o => o.ScheduleID).ThenBy(o => o.ScheduleType).FirstOrDefault();
                decimal? scoreAverage = 0;
                if (crossSummary == null)
                {
                    if (departmentInfo != null)
                    {
                        if (model.BodPerforPoint != null && model.BodPerforWeight != null && model.BodPerforPoint > 0 && model.BodPerforWeight > 0)
                        {
                            departmentInfo.TotalBODPoint = departmentInfo != null && departmentInfo.TotalBODPoint != null ? departmentInfo.TotalBODPoint : departmentInfo.AssessedScore;
                            scoreAverage = Math.Round((decimal)((model.BodDependPoint * model.BodDependWeight) / 100 + (model.BodPerforPoint * model.BodPerforWeight) / 100 + (departmentInfo.TotalBODPoint * (100 - (model.BodDependWeight + model.BodPerforWeight))) / 100), 2);
                        }
                        else
                        {
                            departmentInfo.TotalBODPoint = departmentInfo != null && departmentInfo.TotalBODPoint != null ? departmentInfo.TotalBODPoint : departmentInfo.AssessedScore;
                            scoreAverage = Math.Round((decimal)((model.BodDependPoint * model.BodDependWeight) / 100 + (departmentInfo.TotalBODPoint * (100 - model.BodDependWeight) / 100)), 2);
                        }
                    }

                    crossSummary = new UpfCrossSummary { DepartmentID = crossDetail.ToDepartment, Month = upfCross.Month, Year = upfCross.Year, BodDependPoint = model.BodDependPoint, TotalScoreAverage = scoreAverage,
                        BodDependWeight = model.BodDependWeight, BodPerforPoint = model.BodPerforPoint, BodPerforWeight = model.BodPerforWeight, Active = true, Created = DateTime.Now, CreatedBy = CurrentUser.UserId };
                    departmentService.InsertUpfCrossSummary(crossSummary);
                }
                else
                {
                    if (departmentInfo != null)
                    {
                        if (model.BodPerforPoint != null && model.BodPerforWeight != null && model.BodPerforPoint > 0 && model.BodPerforWeight > 0)
                        {
                            departmentInfo.TotalBODPoint = departmentInfo != null && departmentInfo.TotalBODPoint != null ? departmentInfo.TotalBODPoint : departmentInfo.AssessedScore;
                            scoreAverage = Math.Round((decimal)((model.BodDependPoint * model.BodDependWeight) / 100 + (model.BodPerforPoint * model.BodPerforWeight) / 100 + (departmentInfo.TotalBODPoint * (100 - (model.BodDependWeight + model.BodPerforWeight))) / 100), 2);
                        }
                        else
                        {
                            departmentInfo.TotalBODPoint = departmentInfo != null && departmentInfo.TotalBODPoint != null ? departmentInfo.TotalBODPoint : departmentInfo.AssessedScore;
                            scoreAverage = Math.Round((decimal)((model.BodDependPoint * model.BodDependWeight) / 100 + (departmentInfo.TotalBODPoint * (100 - model.BodDependWeight) / 100)), 2);
                        }
                    }
                    crossSummary.BodDependPoint = model.BodDependPoint;
                    crossSummary.BodDependWeight = model.BodDependWeight;
                    crossSummary.BodPerforPoint = model.BodPerforPoint;
                    crossSummary.BodPerforWeight = model.BodPerforWeight;
                    crossSummary.TotalScoreAverage = scoreAverage;
                    crossSummary.Updated = DateTime.Now;
                    crossSummary.UpdateBy = CurrentUser.UserId;
                    departmentService.UpdateUpfCrossSummary(crossSummary);
                }
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult UpdateCrossBodApproved(int upfCrossId, int upfCrossDetailId)
        {
            UpfSummaryViewModel model = new UpfSummaryViewModel();
            List<UpfCrossDetail> upfCrossDetails = new List<UpfCrossDetail>();
            UpfCross upfCross = upfCrossService.GetUpfCrossById(upfCrossId, ref upfCrossDetails);
            UpfCrossDetail crossDetail = upfCrossDetails.FirstOrDefault(w => w.ID == upfCrossDetailId && w.DeleteFlg == 0);
            if (crossDetail != null)
            {
                UpfCrossSummary crossSummary = departmentService.GetUpfCrossSummary(crossDetail.ToDepartment, upfCross.Month, upfCross.Year);
                if (crossSummary != null)
                {
                    model.UpfCrossId = upfCross.ID;
                    model.UpfCrossDetailId = crossDetail.ID;
                    model.BodDependWeight = crossSummary.BodDependWeight;
                    model.BodDependPoint = crossSummary.BodDependPoint;
                    model.BodPerforWeight = crossSummary.BodPerforWeight;
                    model.BodPerforPoint = crossSummary.BodPerforPoint;
                }
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion Ajax

        #region Year
        // GET: Report - UpfCrossYear
        public ActionResult UpfCrossYearIndex()
        {
            UpfCrossListViewModel model = new UpfCrossListViewModel();
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
            model.Month = (DateTime.Now.Year).ToString();

            model.UpfCrossInfos = new List<UpfCrossInfo>();

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SearchUpfCrossYearReport(UpfCrossListViewModel model)
        {
            model.UserID = CurrentUser.UserId;
            var userInfo = userService.GetUserFullInfo(CurrentUser.UserId);
            if (userInfo.DepartmentID != null) model.UserDepartmentID = (int)userInfo.DepartmentID;
            model.UpfCrossInfos = upfCrossService.GetUpfCrossReport(CurrentUser.UserId, model.UserDepartmentID, model.CompanyID, model.DepartmentID, model.Year, model.Month);
            model.UpfCrossInfos = model.UpfCrossInfos.OrderBy(o => o.Year).ToList();
            model.UpfCrossInfos = SummaryUpfCrossInfos(model.UpfCrossInfos);

            List<DepartmentInfo> departmentInfos = departmentService.GetReportDepartment(model.CompanyID, model.DepartmentID, model.Year);
            
            foreach (var bo in model.UpfCrossInfos)
            {
                UpfCrossSummary upfSummary = departmentService.GetUpfCrossSummary(bo.ToDepartment, bo.Month, bo.Year);
                decimal? scoreAverage = 0;
                if (upfSummary == null)
                {
                    DepartmentInfo departmentInfo = departmentInfos.Where(w => w.ScheduleID == bo.Month).OrderBy(o => o.ScheduleID).ThenBy(o => o.ScheduleType).FirstOrDefault();
                    bo.ScoreAssessed = Math.Round((decimal)(bo.ScoreAssessed / (bo.CountAverage > 1 ? bo.CountAverage : 1)), 2);
                    if (departmentInfo != null && departmentInfo.TotalBODPoint != null) bo.ScoreAssessment = departmentInfo.TotalBODPoint;
                    else if (departmentInfo != null && departmentInfo.TotalBODPoint == null) bo.ScoreAssessment = departmentInfo.AssessedScore;
                    decimal? bodPoint = bo.ScoreAssessed;
                    var bodWeight = bo.TotalDependWeight;
                    bo.PerformancePoint = 0;
                    var performanceWeight = 0;
                    if (bo.PerformancePoint != null && bo.PerformancePoint > 0 && performanceWeight > 0)
                    {
                        scoreAverage = Math.Round((decimal)((bodPoint * bodWeight) / 100 + (bo.PerformancePoint * performanceWeight) / 100 + (bo.ScoreAssessment * (100 - (bodWeight + performanceWeight))) / 100), 2);
                    }
                    else
                    {
                        scoreAverage = Math.Round((decimal)((bodPoint * bodWeight) / 100 + (bo.ScoreAssessment * (100 - bodWeight)) / 100), 2);
                    }
                } else
                {
                    scoreAverage = upfSummary.TotalScoreAverage;
                }

                bo.ScoreAverage = scoreAverage;
            }

            model.UpfCrossInfos = SummaryUpfCrossYear(model.UpfCrossInfos);
            foreach (var bo in model.UpfCrossInfos)
            {
                int i = 0;
                var janPoint = bo.January ?? 0;
                bo.January = Math.Round(janPoint, 2);
                if(janPoint > 0)
                {
                    i++;
                }
                var febPoint = bo.February ?? 0;
                bo.February = Math.Round(febPoint, 2);
                if (febPoint > 0)
                {
                    i++;
                }
                var marPoint = bo.March ?? 0;
                bo.March = Math.Round(marPoint, 2);
                if (marPoint > 0)
                {
                    i++;
                }
                var aprPoint = bo.April ?? 0;
                bo.April = Math.Round(aprPoint, 2);
                if (aprPoint > 0)
                {
                    i++;
                }
                var mayPoint = bo.May ?? 0;
                bo.May = Math.Round(mayPoint, 2);
                if (mayPoint > 0)
                {
                    i++;
                }
                var junPoint = bo.June ?? 0;
                bo.June = Math.Round(junPoint, 2);
                if (junPoint > 0)
                {
                    i++;
                }
                var julPoint = bo.July ?? 0;
                bo.July = Math.Round(julPoint, 2);
                if (julPoint > 0)
                {
                    i++;
                }
                var augPoint = bo.August ?? 0;
                bo.August = Math.Round(augPoint, 2);
                if (augPoint > 0)
                {
                    i++;
                }
                var sepPoint = bo.September ?? 0;
                bo.September = Math.Round(sepPoint, 2);
                if (sepPoint > 0)
                {
                    i++;
                }
                var octPoint = bo.October ?? 0;
                bo.October = Math.Round(octPoint, 2);
                if (octPoint > 0)
                {
                    i++;
                }
                var novPoint = bo.November ?? 0;
                bo.November = Math.Round(novPoint, 2);
                if (novPoint > 0)
                {
                    i++;
                }
                var decPoint = bo.December ?? 0;
                bo.December = Math.Round(decPoint, 2);
                if (decPoint > 0)
                {
                    i++;
                }
                bo.TotalYearPoint = i > 0 ? Math.Round((janPoint + febPoint + marPoint + aprPoint + mayPoint + junPoint + julPoint +
                                                                   augPoint + sepPoint + octPoint + novPoint + decPoint) / i, 2) : 0;
                bo.TotalYearRated = BtcHelper.ConvertSroreToRank(bo.TotalYearPoint);

                UpfCrossSummary crossSummary = departmentService.GetUpfCrossSummaryByYear(bo.ToDepartment, bo.Year);
                bo.ScoreBOD = crossSummary?.TotalScoreAverage;
                bo.RatedBod = bo.ScoreBOD != null ? BtcHelper.ConvertSroreToRank(bo.ScoreBOD) : null;
            }

            return PartialView("_UpfCrossYearListTable", model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ExportExcelYear(UpfCrossListViewModel model)
        {
            model.UserID = CurrentUser.UserId;
            var userInfo = userService.GetUserFullInfo(CurrentUser.UserId);
            if (userInfo.DepartmentID != null) model.UserDepartmentID = (int)userInfo.DepartmentID;
            model.UpfCrossInfos = upfCrossService.GetUpfCrossReport(CurrentUser.UserId, model.UserDepartmentID, model.CompanyID, model.DepartmentID, model.Year, model.Month);
            model.UpfCrossInfos = model.UpfCrossInfos.OrderBy(o => o.Year).ToList();
            model.UpfCrossInfos = SummaryUpfCrossInfos(model.UpfCrossInfos);

            List<DepartmentInfo> departmentInfos = departmentService.GetReportDepartment(model.CompanyID, model.DepartmentID, model.Year);

            foreach (var bo in model.UpfCrossInfos)
            {
                UpfCrossSummary upfSummary = departmentService.GetUpfCrossSummary(bo.ToDepartment, bo.Month, bo.Year);
                decimal? scoreAverage = 0;
                if (upfSummary == null)
                {
                    DepartmentInfo departmentInfo = departmentInfos.Where(w => w.ScheduleID == bo.Month).OrderBy(o => o.ScheduleID).ThenBy(o => o.ScheduleType).FirstOrDefault();
                    bo.ScoreAssessed = Math.Round((decimal)(bo.ScoreAssessed / (bo.CountAverage > 1 ? bo.CountAverage : 1)), 2);
                    if (departmentInfo != null && departmentInfo.TotalBODPoint != null) bo.ScoreAssessment = departmentInfo.TotalBODPoint;
                    else if (departmentInfo != null && departmentInfo.TotalBODPoint == null) bo.ScoreAssessment = departmentInfo.AssessedScore;
                    bo.ScoreBOD = 0;
                    decimal? bodPoint = bo.ScoreAssessed;
                    var bodWeight = bo.TotalDependWeight;
                    bo.PerformancePoint = 0;
                    var performanceWeight = 0;
                    if (bo.PerformancePoint != null && bo.PerformancePoint > 0 && performanceWeight > 0)
                    {
                        scoreAverage = Math.Round((decimal)((bodPoint * bodWeight) / 100 + (bo.PerformancePoint * performanceWeight) / 100 + (bo.ScoreAssessment * (100 - (bodWeight + performanceWeight))) / 100), 2);
                    }
                    else
                    {
                        scoreAverage = Math.Round((decimal)((bodPoint * bodWeight) / 100 + (bo.ScoreAssessment * (100 - bodWeight)) / 100), 2);
                    }
                }
                else
                {
                    scoreAverage = upfSummary.TotalScoreAverage;
                }

                bo.ScoreAverage = scoreAverage;
            }

            model.UpfCrossInfos = SummaryUpfCrossYear(model.UpfCrossInfos);
            foreach (var bo in model.UpfCrossInfos)
            {
                int i = 0;
                var janPoint = bo.January ?? 0;
                bo.January = Math.Round(janPoint, 2);
                if (janPoint > 0)
                {
                    i++;
                }
                var febPoint = bo.February ?? 0;
                bo.February = Math.Round(febPoint, 2);
                if (febPoint > 0)
                {
                    i++;
                }
                var marPoint = bo.March ?? 0;
                bo.March = Math.Round(marPoint, 2);
                if (marPoint > 0)
                {
                    i++;
                }
                var aprPoint = bo.April ?? 0;
                bo.April = Math.Round(aprPoint, 2);
                if (aprPoint > 0)
                {
                    i++;
                }
                var mayPoint = bo.May ?? 0;
                bo.May = Math.Round(mayPoint, 2);
                if (mayPoint > 0)
                {
                    i++;
                }
                var junPoint = bo.June ?? 0;
                bo.June = Math.Round(junPoint, 2);
                if (junPoint > 0)
                {
                    i++;
                }
                var julPoint = bo.July ?? 0;
                bo.July = Math.Round(julPoint, 2);
                if (julPoint > 0)
                {
                    i++;
                }
                var augPoint = bo.August ?? 0;
                bo.August = Math.Round(augPoint, 2);
                if (augPoint > 0)
                {
                    i++;
                }
                var sepPoint = bo.September ?? 0;
                bo.September = Math.Round(sepPoint, 2);
                if (sepPoint > 0)
                {
                    i++;
                }
                var octPoint = bo.October ?? 0;
                bo.October = Math.Round(octPoint, 2);
                if (octPoint > 0)
                {
                    i++;
                }
                var novPoint = bo.November ?? 0;
                bo.November = Math.Round(novPoint, 2);
                if (novPoint > 0)
                {
                    i++;
                }
                var decPoint = bo.December ?? 0;
                bo.December = Math.Round(decPoint, 2);
                if (decPoint > 0)
                {
                    i++;
                }

                var totalMonthPoint = janPoint + febPoint + marPoint + aprPoint + mayPoint + junPoint + julPoint +
                    augPoint + sepPoint + octPoint + novPoint + decPoint;
                bo.TotalYearPoint = totalMonthPoint > 0 ? Math.Round((totalMonthPoint) / i, 2) : 0;
                bo.TotalYearRated = BtcHelper.ConvertSroreToRank(bo.TotalYearPoint);

                UpfCrossSummary crossSummary = departmentService.GetUpfCrossSummaryByYear(bo.ToDepartment, bo.Year);
                bo.ScoreBOD = crossSummary?.TotalScoreAverage;
                bo.RatedBod = bo.ScoreBOD != null ? BtcHelper.ConvertSroreToRank(bo.ScoreBOD) : null;
            }

            model.UpfCrossInfos = model.UpfCrossInfos.OrderBy(o => o.ToName).ToList();

            ExcelPackage excelEngine = FillDataTableExcelYear(model.UpfCrossInfos, model.Year);
            Session["DownloadExcel_UPFCrossYear"] = excelEngine.GetAsByteArray();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult DownloadYear()
        {
            if (Session["DownloadExcel_UPFCrossYear"] != null)
            {
                string dateStr = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy_hhmmss");
                var fileDownloadName = "BÁO CÁO TỔNG HỢP ĐÁNH GIÁ KPI NĂM_" + dateStr + ".xlsx";
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                byte[] data = Session["DownloadExcel_UPFCrossYear"] as byte[];
                return File(data, contentType, fileDownloadName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ExcelPackage FillDataTableExcelYear(List<UpfCrossInfo> model, string year)
        {
            ExcelPackage excelEngine = new ExcelPackage();
            ExcelWorksheet worksheet = excelEngine.Workbook.Worksheets.Add("UPF-YEAR");

            //Enter values to the cells from A3 to A5

            worksheet.Cells["A1:AE1"].Merge = true;
            worksheet.Cells["A1"].Value = "BẢNG TỔNG HỢP ĐÁNH GIÁ KPI NĂM " + year + " CỦA CÁC PHÒNG \r\n\t TABLE OF REVIEW DEPARTMENTS KPI OF YEAR " + year;
            worksheet.Cells["A1:AE1"].Style.Font.Size = 14;
            worksheet.Cells["A1"].Style.Font.Bold = true;
            worksheet.Cells["A1:AE1"].Style.WrapText = true;
            worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Row(1).Height = 40;
            Color colFromHexHeader = ColorTranslator.FromHtml("#FFFF00");
            worksheet.Cells["A1:AE1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A1:AE1"].Style.Fill.BackgroundColor.SetColor(colFromHexHeader);

            // format cells - add borders A2:Z2 body
            worksheet.Cells["A2:AE3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A2:AE3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A2:AE3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A2:AE3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A2:AE3"].Style.Font.Bold = true;
            worksheet.Cells["A2:AE3"].Style.WrapText = true;
            worksheet.Cells["A2:AE3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            worksheet.Cells["A2:A3"].Merge = true;
            worksheet.Cells["B2:B3"].Merge = true;
            worksheet.Cells["C2:C3"].Merge = true;
            worksheet.Cells["D2:E2"].Merge = true;
            worksheet.Cells["F2:G2"].Merge = true;
            worksheet.Cells["H2:I2"].Merge = true;
            worksheet.Cells["J2:K2"].Merge = true;
            worksheet.Cells["L2:M2"].Merge = true;
            worksheet.Cells["N2:O2"].Merge = true;
            worksheet.Cells["P2:Q2"].Merge = true;
            worksheet.Cells["R2:S2"].Merge = true;
            worksheet.Cells["T2:U2"].Merge = true;
            worksheet.Cells["V2:W2"].Merge = true;
            worksheet.Cells["X2:Y2"].Merge = true;
            worksheet.Cells["Z2:AA2"].Merge = true;
            worksheet.Cells["AB2:AC2"].Merge = true;
            worksheet.Cells["AD2:AE2"].Merge = true;

            worksheet.Cells["A2"].Value = "STT";
            worksheet.Cells["B2"].Value = "PHÒNG/BỘ PHẬN ĐƯỢC ĐÁNH GIÁ";
            worksheet.Cells["C2"].Value = "KPI";
            worksheet.Cells["D2"].Value = "Tháng 1";
            worksheet.Cells["D3"].Value = "Điểm BQ";
            worksheet.Cells["E3"].Value = "Xếp loại";
            worksheet.Cells["F2"].Value = "Tháng 2";
            worksheet.Cells["F3"].Value = "Điểm BQ";
            worksheet.Cells["G3"].Value = "Xếp loại";
            worksheet.Cells["H2"].Value = "Tháng 3";
            worksheet.Cells["H3"].Value = "Điểm BQ";
            worksheet.Cells["I3"].Value = "Xếp loại";
            worksheet.Cells["J2"].Value = "Tháng 4";
            worksheet.Cells["J3"].Value = "Điểm BQ";
            worksheet.Cells["K3"].Value = "Xếp loại";
            worksheet.Cells["L2"].Value = "Tháng 5";
            worksheet.Cells["L3"].Value = "Điểm BQ";
            worksheet.Cells["M3"].Value = "Xếp loại";
            worksheet.Cells["N2"].Value = "Tháng 6";
            worksheet.Cells["N3"].Value = "Điểm BQ";
            worksheet.Cells["O3"].Value = "Xếp loại";
            worksheet.Cells["P2"].Value = "Tháng 7";
            worksheet.Cells["P3"].Value = "Điểm BQ";
            worksheet.Cells["Q3"].Value = "Xếp loại";
            worksheet.Cells["R2"].Value = "Tháng 8";
            worksheet.Cells["R3"].Value = "Điểm BQ";
            worksheet.Cells["S3"].Value = "Xếp loại";
            worksheet.Cells["T2"].Value = "Tháng 9";
            worksheet.Cells["T3"].Value = "Điểm BQ";
            worksheet.Cells["U3"].Value = "Xếp loại";
            worksheet.Cells["V2"].Value = "Tháng 10";
            worksheet.Cells["V3"].Value = "Điểm BQ";
            worksheet.Cells["W3"].Value = "Xếp loại";
            worksheet.Cells["X2"].Value = "Tháng 11";
            worksheet.Cells["X3"].Value = "Điểm BQ";
            worksheet.Cells["Y3"].Value = "Xếp loại";
            worksheet.Cells["Z2"].Value = "Tháng 12";
            worksheet.Cells["Z3"].Value = "Điểm BQ";
            worksheet.Cells["AA3"].Value = "Xếp loại";
            worksheet.Cells["AB2"].Value = "Tổng " + year;
            worksheet.Cells["AB3"].Value = "Điểm BQ";
            worksheet.Cells["AC3"].Value = "Xếp loại";
            worksheet.Cells["AD2"].Value = "BOD đánh giá";
            worksheet.Cells["AD3"].Value = "Điểm BQ";
            worksheet.Cells["AE3"].Value = "Xếp loại";
            Color colFromHex = ColorTranslator.FromHtml("#92D050");
            worksheet.Cells["A2:AE3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A2:AE3"].Style.Fill.BackgroundColor.SetColor(colFromHex);

            Color colFromHexBody = ColorTranslator.FromHtml("#D6DCE4");
            int col = 3;
            for (int i = 0; i < model.Count; i++)
            {
                col++;
                if (model[i].ToName.ToLower().Contains("Leasing".ToLower()) || model[i].ToName.ToLower().Contains("FB".ToLower())
                   || model[i].ToName.ToLower().Contains("Vận hành".ToLower()) || model[i].ToName.ToLower().Contains("Ops".ToLower()))
                {
                    worksheet.Cells["C" + col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["C" + col].Style.Fill.BackgroundColor.SetColor(colFromHexBody);
                    worksheet.Cells["C" + (col + 2)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["C" + (col + 2)].Style.Fill.BackgroundColor.SetColor(colFromHexBody);
                    worksheet.Cells["C" + (col + 4)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["C" + (col + 4)].Style.Fill.BackgroundColor.SetColor(colFromHexBody);
                    worksheet.Cells["AB" + col + ":AE" + col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["AB" + col + ":AE" + col].Style.Fill.BackgroundColor.SetColor(colFromHexBody);

                    worksheet.Cells["A" + col + ":A" + (col + 5)].Merge = true;
                    worksheet.Cells["A" + col].Value = i + 1;
                    worksheet.Cells["A" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["B" + col + ":B" + (col + 5)].Merge = true;
                    worksheet.Cells["D" + col + ":D" + (col + 5)].Merge = true;
                    worksheet.Cells["E" + col + ":E" + (col + 5)].Merge = true;
                    worksheet.Cells["F" + col + ":F" + (col + 5)].Merge = true;
                    worksheet.Cells["G" + col + ":G" + (col + 5)].Merge = true;
                    worksheet.Cells["H" + col + ":H" + (col + 5)].Merge = true;
                    worksheet.Cells["I" + col + ":I" + (col + 5)].Merge = true;
                    worksheet.Cells["J" + col + ":J" + (col + 5)].Merge = true;
                    worksheet.Cells["K" + col + ":K" + (col + 5)].Merge = true;
                    worksheet.Cells["L" + col + ":L" + (col + 5)].Merge = true;
                    worksheet.Cells["M" + col + ":M" + (col + 5)].Merge = true;
                    worksheet.Cells["N" + col + ":N" + (col + 5)].Merge = true;
                    worksheet.Cells["O" + col + ":O" + (col + 5)].Merge = true;
                    worksheet.Cells["P" + col + ":P" + (col + 5)].Merge = true;
                    worksheet.Cells["Q" + col + ":Q" + (col + 5)].Merge = true;
                    worksheet.Cells["R" + col + ":R" + (col + 5)].Merge = true;
                    worksheet.Cells["S" + col + ":S" + (col + 5)].Merge = true;
                    worksheet.Cells["T" + col + ":T" + (col + 5)].Merge = true;
                    worksheet.Cells["U" + col + ":U" + (col + 5)].Merge = true;
                    worksheet.Cells["V" + col + ":V" + (col + 5)].Merge = true;
                    worksheet.Cells["W" + col + ":W" + (col + 5)].Merge = true;
                    worksheet.Cells["X" + col + ":X" + (col + 5)].Merge = true;
                    worksheet.Cells["Y" + col + ":Y" + (col + 5)].Merge = true;
                    worksheet.Cells["Z" + col + ":Z" + (col + 5)].Merge = true;
                    worksheet.Cells["AA" + col + ":AA" + (col + 5)].Merge = true;
                    worksheet.Cells["AB" + col + ":AB" + (col + 5)].Merge = true;
                    worksheet.Cells["AC" + col + ":AC" + (col + 5)].Merge = true;
                    worksheet.Cells["AD" + col + ":AD" + (col + 5)].Merge = true;
                    worksheet.Cells["AE" + col + ":AE" + (col + 5)].Merge = true;
                    worksheet.Cells["B" + col].Value = model[i].ToName;
                    worksheet.Cells["C" + col].Value = "Tỷ trọng phụ thuộc";
                    worksheet.Cells["C" + (col + 1)].Value = "Điểm được đánh giá";
                    worksheet.Cells["C" + (col + 2)].Value = "Tỷ trọng BOD";
                    worksheet.Cells["C" + (col + 3)].Value = "Điểm BOD đánh giá";
                    worksheet.Cells["C" + (col + 4)].Value = "Tỷ trọng chuyên môn";
                    worksheet.Cells["C" + (col + 5)].Value = "Điểm tự đánh giá";
                    worksheet.Cells["D" + col].Value = model[i].January > 0 ? model[i].January : null;
                    worksheet.Cells["E" + col].Value = model[i].RatedJanuary;
                    worksheet.Cells["F" + col].Value = model[i].February > 0 ? model[i].February : null;
                    worksheet.Cells["G" + col].Value = model[i].RatedFebruary;
                    worksheet.Cells["H" + col].Value = model[i].March > 0 ? model[i].March : null;
                    worksheet.Cells["I" + col].Value = model[i].RatedMarch;
                    worksheet.Cells["J" + col].Value = model[i].April > 0 ? model[i].April : null;
                    worksheet.Cells["K" + col].Value = model[i].RatedApril;
                    worksheet.Cells["L" + col].Value = model[i].May > 0 ? model[i].May : null;
                    worksheet.Cells["M" + col].Value = model[i].RatedMay;
                    worksheet.Cells["N" + col].Value = model[i].June > 0 ? model[i].June : null;
                    worksheet.Cells["O" + col].Value = model[i].RatedJune;
                    worksheet.Cells["P" + col].Value = model[i].July > 0 ? model[i].July : null;
                    worksheet.Cells["Q" + col].Value = model[i].RatedJuly;
                    worksheet.Cells["R" + col].Value = model[i].August > 0 ? model[i].August : null;
                    worksheet.Cells["S" + col].Value = model[i].RatedAugust;
                    worksheet.Cells["T" + col].Value = model[i].September > 0 ? model[i].September : null;
                    worksheet.Cells["U" + col].Value = model[i].RatedSeptember;
                    worksheet.Cells["V" + col].Value = model[i].October > 0 ? model[i].October : null;
                    worksheet.Cells["W" + col].Value = model[i].RatedOctober;
                    worksheet.Cells["X" + col].Value = model[i].November > 0 ? model[i].November : null;
                    worksheet.Cells["Y" + col].Value = model[i].RatedNovember;
                    worksheet.Cells["Z" + col].Value = model[i].December > 0 ? model[i].December : null;
                    worksheet.Cells["AA" + col].Value = model[i].RatedDecember;
                    worksheet.Cells["AB" + col].Value = model[i].TotalYearPoint > 0 ? model[i].TotalYearPoint : null;
                    worksheet.Cells["AC" + col].Value = model[i].TotalYearRated;
                    worksheet.Cells["AD" + col].Value = model[i].ScoreBOD > 0 ? model[i].ScoreBOD : null;
                    worksheet.Cells["AE" + col].Value = model[i].RatedBod;

                    worksheet.Cells["AB" + col + ":AE" + col].Style.Font.Color.SetColor(Color.Red);

                    col = col + 5;
                }
                else
                {
                    worksheet.Cells["C" + col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["C" + col].Style.Fill.BackgroundColor.SetColor(colFromHexBody);
                    worksheet.Cells["C" + (col + 2)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["C" + (col + 2)].Style.Fill.BackgroundColor.SetColor(colFromHexBody);
                    worksheet.Cells["AB" + col + ":AE" + col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["AB" + col + ":AE" + col].Style.Fill.BackgroundColor.SetColor(colFromHexBody);

                    worksheet.Cells["A" + col + ":A" + (col + 3)].Merge = true;
                    worksheet.Cells["A" + col].Value = i + 1;
                    worksheet.Cells["A" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["B" + col + ":B" + (col + 3)].Merge = true;
                    worksheet.Cells["D" + col + ":D" + (col + 3)].Merge = true;
                    worksheet.Cells["E" + col + ":E" + (col + 3)].Merge = true;
                    worksheet.Cells["F" + col + ":F" + (col + 3)].Merge = true;
                    worksheet.Cells["G" + col + ":G" + (col + 3)].Merge = true;
                    worksheet.Cells["H" + col + ":H" + (col + 3)].Merge = true;
                    worksheet.Cells["I" + col + ":I" + (col + 3)].Merge = true;
                    worksheet.Cells["J" + col + ":J" + (col + 3)].Merge = true;
                    worksheet.Cells["K" + col + ":K" + (col + 3)].Merge = true;
                    worksheet.Cells["L" + col + ":L" + (col + 3)].Merge = true;
                    worksheet.Cells["M" + col + ":M" + (col + 3)].Merge = true;
                    worksheet.Cells["N" + col + ":N" + (col + 3)].Merge = true;
                    worksheet.Cells["O" + col + ":O" + (col + 3)].Merge = true;
                    worksheet.Cells["P" + col + ":P" + (col + 3)].Merge = true;
                    worksheet.Cells["Q" + col + ":Q" + (col + 3)].Merge = true;
                    worksheet.Cells["R" + col + ":R" + (col + 3)].Merge = true;
                    worksheet.Cells["S" + col + ":S" + (col + 3)].Merge = true;
                    worksheet.Cells["T" + col + ":T" + (col + 3)].Merge = true;
                    worksheet.Cells["U" + col + ":U" + (col + 3)].Merge = true;
                    worksheet.Cells["V" + col + ":V" + (col + 3)].Merge = true;
                    worksheet.Cells["W" + col + ":W" + (col + 3)].Merge = true;
                    worksheet.Cells["X" + col + ":X" + (col + 3)].Merge = true;
                    worksheet.Cells["Y" + col + ":Y" + (col + 3)].Merge = true;
                    worksheet.Cells["Z" + col + ":Z" + (col + 3)].Merge = true;
                    worksheet.Cells["AA" + col + ":AA" + (col + 3)].Merge = true;
                    worksheet.Cells["AB" + col + ":AB" + (col + 3)].Merge = true;
                    worksheet.Cells["AC" + col + ":AC" + (col + 3)].Merge = true;
                    worksheet.Cells["AD" + col + ":AD" + (col + 3)].Merge = true;
                    worksheet.Cells["AE" + col + ":AE" + (col + 3)].Merge = true;
                    worksheet.Cells["B" + col].Value = model[i].ToName;
                    worksheet.Cells["C" + col].Value = "Tỷ trọng phụ thuộc";
                    worksheet.Cells["C" + (col + 1)].Value = "Điểm được đánh giá";
                    worksheet.Cells["C" + (col + 2)].Value = "Tỷ trọng chuyên môn";
                    worksheet.Cells["C" + (col + 3)].Value = "Điểm tự đánh giá";
                    worksheet.Cells["D" + col].Value = model[i].January > 0 ? model[i].January : null;
                    worksheet.Cells["E" + col].Value = model[i].RatedJanuary;
                    worksheet.Cells["F" + col].Value = model[i].February > 0 ? model[i].February : null;
                    worksheet.Cells["G" + col].Value = model[i].RatedFebruary;
                    worksheet.Cells["H" + col].Value = model[i].March > 0 ? model[i].March : null;
                    worksheet.Cells["I" + col].Value = model[i].RatedMarch;
                    worksheet.Cells["J" + col].Value = model[i].April > 0 ? model[i].April : null;
                    worksheet.Cells["K" + col].Value = model[i].RatedApril;
                    worksheet.Cells["L" + col].Value = model[i].May > 0 ? model[i].May : null;
                    worksheet.Cells["M" + col].Value = model[i].RatedMay;
                    worksheet.Cells["N" + col].Value = model[i].June > 0 ? model[i].June : null;
                    worksheet.Cells["O" + col].Value = model[i].RatedJune;
                    worksheet.Cells["P" + col].Value = model[i].July > 0 ? model[i].July : null;
                    worksheet.Cells["Q" + col].Value = model[i].RatedJuly;
                    worksheet.Cells["R" + col].Value = model[i].August > 0 ? model[i].August : null;
                    worksheet.Cells["S" + col].Value = model[i].RatedAugust;
                    worksheet.Cells["T" + col].Value = model[i].September > 0 ? model[i].September : null;
                    worksheet.Cells["U" + col].Value = model[i].RatedSeptember;
                    worksheet.Cells["V" + col].Value = model[i].October > 0 ? model[i].October : null;
                    worksheet.Cells["W" + col].Value = model[i].RatedOctober;
                    worksheet.Cells["X" + col].Value = model[i].November > 0 ? model[i].November : null;
                    worksheet.Cells["Y" + col].Value = model[i].RatedNovember;
                    worksheet.Cells["Z" + col].Value = model[i].December > 0 ? model[i].December : null;
                    worksheet.Cells["AA" + col].Value = model[i].RatedDecember;
                    worksheet.Cells["AB" + col].Value = model[i].TotalYearPoint > 0 ? model[i].TotalYearPoint : null;
                    worksheet.Cells["AC" + col].Value = model[i].TotalYearRated;
                    worksheet.Cells["AD" + col].Value = model[i].ScoreBOD > 0 ? model[i].ScoreBOD : null;
                    worksheet.Cells["AE" + col].Value = model[i].RatedBod;

                    worksheet.Cells["AB" + col + ":AE" + col].Style.Font.Color.SetColor(Color.Red);

                    col = col + 3;
                }
            }

            worksheet.Cells["D4:AE" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["A2:AE" + col].Style.Font.Size = 10;
            // format cells - add borders A5:I...
            worksheet.Cells["A2:AE" + col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A2:AE" + col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A2:AE" + col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A2:AE" + col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            col++;
            col = col + 1;

            worksheet.Cells["A" + col + ":D" + col].Merge = true;
            worksheet.Cells["A" + col].Value = "Người lập";
            worksheet.Cells["A" + col].Style.Font.Bold = true;
            worksheet.Cells["A" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["A1:AE" + col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["A3:AE" + col].Style.WrapText = true;
            //Font
            worksheet.Cells["A" + col].Style.Font.Size = 12;
            worksheet.Cells["A1:AE" + col].Style.Font.Name = "Times New Roman";
            //Apply row height and column width to look good
            worksheet.Column(1).Width = 5;
            worksheet.Column(2).Width = 25;
            worksheet.Column(3).Width = 18;
            worksheet.Column(4).Width = 9;
            worksheet.Column(5).Width = 9;
            worksheet.Column(6).Width = 9;
            worksheet.Column(7).Width = 9;
            worksheet.Column(8).Width = 9;
            worksheet.Column(9).Width = 9;
            worksheet.Column(10).Width = 9;
            worksheet.Column(11).Width = 9;
            worksheet.Column(12).Width = 9;
            worksheet.Column(13).Width = 9;
            worksheet.Column(14).Width = 9;
            worksheet.Column(15).Width = 9;
            worksheet.Column(16).Width = 9;
            worksheet.Column(17).Width = 9;
            worksheet.Column(18).Width = 9;
            worksheet.Column(19).Width = 9;
            worksheet.Column(20).Width = 9;
            worksheet.Column(21).Width = 9;
            worksheet.Column(22).Width = 9;
            worksheet.Column(23).Width = 9;
            worksheet.Column(24).Width = 9;
            worksheet.Column(25).Width = 9;
            worksheet.Column(26).Width = 9;
            worksheet.Column(27).Width = 9;
            worksheet.Column(28).Width = 9;
            worksheet.Column(29).Width = 9;
            worksheet.Column(30).Width = 9;
            worksheet.Column(31).Width = 9;
            worksheet.Column(32).Width = 9;

            return excelEngine;
        }

        public List<UpfCrossInfo> SummaryUpfCrossYear(List<UpfCrossInfo> model)
        {
            List<UpfCrossInfo> crossInfos = new List<UpfCrossInfo>();
            List<string> lstCheck = new List<string>();
            List<string> lstCheckYear = new List<string>();
            foreach (var bo in model)
            {
                if (crossInfos.Count > 0)
                {
                    if (lstCheck.Contains(bo.ToDepartment.ToString()) && lstCheckYear.Contains(bo.Year.ToString()))
                    {
                        foreach (var item in crossInfos)
                        {
                            if (item.ToDepartment == bo.ToDepartment && item.Year == bo.Year)
                            {
                                if (bo.Month == 1)
                                {
                                    item.January = bo.ScoreAverage;
                                    item.RatedJanuary = BtcHelper.ConvertSroreToRank(bo.ScoreAverage);
                                }
                                if (bo.Month == 2)
                                {
                                    item.February = bo.ScoreAverage;
                                    item.RatedFebruary = BtcHelper.ConvertSroreToRank(bo.ScoreAverage);
                                }
                                if (bo.Month == 3)
                                {
                                    item.March = bo.ScoreAverage;
                                    item.RatedMarch = BtcHelper.ConvertSroreToRank(bo.ScoreAverage);
                                }
                                if (bo.Month == 4)
                                {
                                    item.April = bo.ScoreAverage;
                                    item.RatedApril = BtcHelper.ConvertSroreToRank(bo.ScoreAverage);
                                }
                                if (bo.Month == 5)
                                {
                                    item.May = bo.ScoreAverage;
                                    item.RatedMay = BtcHelper.ConvertSroreToRank(bo.ScoreAverage);
                                }
                                if (bo.Month == 6)
                                {
                                    item.June = bo.ScoreAverage;
                                    item.RatedJune = BtcHelper.ConvertSroreToRank(bo.ScoreAverage);
                                }
                                if (bo.Month == 7)
                                {
                                    item.July = bo.ScoreAverage;
                                    item.RatedJuly = BtcHelper.ConvertSroreToRank(bo.ScoreAverage);
                                }
                                if (bo.Month == 8)
                                {
                                    item.August = bo.ScoreAverage;
                                    item.RatedAugust = BtcHelper.ConvertSroreToRank(bo.ScoreAverage);
                                }
                                if (bo.Month == 9)
                                {
                                    item.September = bo.ScoreAverage;
                                    item.RatedSeptember = BtcHelper.ConvertSroreToRank(bo.ScoreAverage);
                                }
                                if (bo.Month == 10)
                                {
                                    item.October = bo.ScoreAverage;
                                    item.RatedOctober = BtcHelper.ConvertSroreToRank(bo.ScoreAverage);
                                }
                                if (bo.Month == 11)
                                {
                                    item.November = bo.ScoreAverage;
                                    item.RatedNovember = BtcHelper.ConvertSroreToRank(bo.ScoreAverage);
                                }
                                if (bo.Month == 12)
                                {
                                    item.December = bo.ScoreAverage;
                                    item.RatedDecember = BtcHelper.ConvertSroreToRank(bo.ScoreAverage);
                                }
                            }
                        }
                    }
                    else
                    {
                        crossInfos.Add(ConvertScheduleToMonth(bo));
                        lstCheck.Add(bo.ToDepartment.ToString());
                        lstCheckYear.Add(bo.Year.ToString());
                    }
                }
                else
                {
                    crossInfos.Add(ConvertScheduleToMonth(bo));
                    lstCheck.Add(bo.ToDepartment.ToString());
                    lstCheckYear.Add(bo.Year.ToString());
                }
            }

            return crossInfos;
        }

        public UpfCrossInfo ConvertScheduleToMonth(UpfCrossInfo info)
        {
            if (info.Month == 1)
            {
                info.January = info.ScoreAverage;
                info.RatedJanuary = BtcHelper.ConvertSroreToRank(info.ScoreAverage);
            }
            if (info.Month == 2)
            {
                info.February = info.ScoreAverage;
                info.RatedFebruary = BtcHelper.ConvertSroreToRank(info.ScoreAverage);
            }
            if (info.Month == 3)
            {
                info.March = info.ScoreAverage;
                info.RatedMarch = BtcHelper.ConvertSroreToRank(info.ScoreAverage);
            }
            if (info.Month == 4)
            {
                info.April = info.ScoreAverage;
                info.RatedApril = BtcHelper.ConvertSroreToRank(info.ScoreAverage);
            }
            if (info.Month == 5)
            {
                info.May = info.ScoreAverage;
                info.RatedMay = BtcHelper.ConvertSroreToRank(info.ScoreAverage);
            }
            if (info.Month == 6)
            {
                info.June = info.ScoreAverage;
                info.RatedJune = BtcHelper.ConvertSroreToRank(info.ScoreAverage);
            }
            if (info.Month == 7)
            {
                info.July = info.ScoreAverage;
                info.RatedJuly = BtcHelper.ConvertSroreToRank(info.ScoreAverage);
            }
            if (info.Month == 8)
            {
                info.August = info.ScoreAverage;
                info.RatedAugust = BtcHelper.ConvertSroreToRank(info.ScoreAverage);
            }
            if (info.Month == 9)
            {
                info.September = info.ScoreAverage;
                info.RatedSeptember = BtcHelper.ConvertSroreToRank(info.ScoreAverage);
            }
            if (info.Month == 10)
            {
                info.October = info.ScoreAverage;
                info.RatedOctober = BtcHelper.ConvertSroreToRank(info.ScoreAverage);
            }
            if (info.Month == 11)
            {
                info.November = info.ScoreAverage;
                info.RatedNovember = BtcHelper.ConvertSroreToRank(info.ScoreAverage);
            }
            if (info.Month == 12)
            {
                info.December = info.ScoreAverage;
                info.RatedDecember = BtcHelper.ConvertSroreToRank(info.ScoreAverage);
            }

            return info;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpfCrossApprovedYear(UpfSummaryViewModel model)
        {
            List<UpfCrossDetail> upfCrossDetails = new List<UpfCrossDetail>();
            UpfCross upfCross = upfCrossService.GetUpfCrossById(model.UpfCrossId, ref upfCrossDetails);
            UpfCrossDetail crossDetail = upfCrossDetails.FirstOrDefault(w => w.ID == model.UpfCrossDetailId && w.DeleteFlg == 0);
            if (crossDetail != null)
            {
                UpfCrossSummary crossSummary = departmentService.GetUpfCrossSummaryByYear(crossDetail.ToDepartment, upfCross.Year);
                if (crossSummary == null)
                {
                    crossSummary = new UpfCrossSummary
                    {
                        DepartmentID = crossDetail.ToDepartment,
                        Year = upfCross.Year,
                        TotalScoreAverage = model.TotalYearPoint,
                        Active = true,
                        Created = DateTime.Now,
                        CreatedBy = CurrentUser.UserId
                    };
                    departmentService.InsertUpfCrossSummary(crossSummary);
                }
                else
                {
                    crossSummary.TotalScoreAverage = model.TotalYearPoint;
                    crossSummary.Updated = DateTime.Now;
                    crossSummary.UpdateBy = CurrentUser.UserId;
                    departmentService.UpdateUpfCrossSummary(crossSummary);
                }
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult UpdateCrossApprovedYear(int upfCrossId, int upfCrossDetailId)
        {
            UpfSummaryViewModel model = new UpfSummaryViewModel();
            List<UpfCrossDetail> upfCrossDetails = new List<UpfCrossDetail>();
            UpfCross upfCross = upfCrossService.GetUpfCrossById(upfCrossId, ref upfCrossDetails);
            UpfCrossDetail crossDetail = upfCrossDetails.FirstOrDefault(w => w.ID == upfCrossDetailId && w.DeleteFlg == 0);
            if (crossDetail != null)
            {
                UpfCrossSummary crossSummary = departmentService.GetUpfCrossSummaryByYear(crossDetail.ToDepartment, upfCross.Year);
                if (crossSummary != null)
                {
                    model.UpfCrossId = upfCross.ID;
                    model.UpfCrossDetailId = crossDetail.ID;
                    model.TotalYearPoint = crossSummary.TotalScoreAverage;
                }
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion Ajax
    }
}
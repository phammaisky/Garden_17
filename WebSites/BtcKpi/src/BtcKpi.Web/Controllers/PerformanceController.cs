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
    [RoutePrefix("Performance")]
    public class PerformanceController : BaseController
    {
        #region Performance
        private readonly IUserService userService;
        private readonly IPerformanceService performanceService;

        public PerformanceController(IUserService userService, IPerformanceService performanceService)
        {
            this.userService = userService;
            this.performanceService = performanceService;
        }

        // GET: Index
        public ActionResult Index()
        {
            Session[string.Format("performance-{0}", CurrentUser.UserId)] = null;
            PerformanceListViewModel model = new PerformanceListViewModel();
            model.UserInfo = userService.GetUserFullInfo(CurrentUser.UserId);

            //Du an
            List<Projects> lstProjects = performanceService.GetListProjects();
            model.Projects = new SelectList(lstProjects, "Id", "Name", lstProjects[0].Id);
            //Loai hieu suat
            List<TypePerformance> lstTypePerformance = performanceService.GetListTypePerformance(lstProjects[0].Id);
            model.TypePerformances = new SelectList(lstTypePerformance, "Id", "Name", lstTypePerformance[0].Id);
            //Nam
            model.Years = Years;
            model.Year = DateTime.Now.Year.ToString();
            //Type FB
            var typeFBs = new List<ConvertEnum>();
            foreach (var typeFB in Enum.GetValues(typeof(PerformanceTypeFB)))
                typeFBs.Add(new ConvertEnum
                {
                    Value = (int)typeFB,
                    Text = typeFB.ToString() == "FB" ? "FB" : "Non FB"
                });
            model.TypeFBs = new SelectList(typeFBs, "Value", "Text");
            //Lay danh sach performance cong viec
            model.PerformanceInfos = new List<PerformanceInfo>();
            return View(model);
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult SearchPerformance(PerformanceListViewModel model)
        {
            model.PerformanceInfos = performanceService.GetPerformanceByConditions(model.ProjectID, model.TypePerformanceID, model.Year, model.TypeFBID);
            model.UserID = CurrentUser.UserId;

            model.PerformanceInfos = model.PerformanceInfos.OrderBy(o => o.Month).ToList();
            foreach (var bo in model.PerformanceInfos)
            {
                var project = performanceService.GetProjectById(bo.ProjectId);
                var typePerformance = performanceService.GetTypePerformanceById(bo.TypePerformanceId);
                bo.ProjectName = project.Name;
                bo.TypePerformanceName = typePerformance.Name;
                bo.MonthStr = "Tháng " + bo.Month;
            }

            var projectCheck = performanceService.GetProjectById(int.Parse(model.ProjectID));
            var typePerformanceCheck = performanceService.GetTypePerformanceById(int.Parse(model.TypePerformanceID));
            if (projectCheck.Code.ToUpper().Contains("GARDEN") && typePerformanceCheck.Code.ToUpper().Contains("LS"))
            {
                model.ShowFormByProjType = 1;
            }
            else if (projectCheck.Code.ToUpper().Contains("GARDEN") && typePerformanceCheck.Code.ToUpper().Contains("FB"))
            {
                model.ShowFormByProjType = 2;
            }
            else if (projectCheck.Code.ToUpper().Contains("MANOR") && typePerformanceCheck.Code.ToUpper().Contains("LS"))
            {
                model.ShowFormByProjType = 3;
            }
            else if (projectCheck.Code.ToUpper().Contains("BFT") && typePerformanceCheck.Code.ToUpper().Contains("LS"))
            {
                model.ShowFormByProjType = 4;
            }
            else if (projectCheck.Code.ToUpper().Contains("BFT") && typePerformanceCheck.Code.ToUpper().Contains("FB"))
            {
                model.ShowFormByProjType = 5;
            }
            else if (projectCheck.Code.ToUpper().Contains("BFT") && typePerformanceCheck.Code.ToUpper().Contains("ICON68"))
            {
                model.ShowFormByProjType = 6;
            }
            else if (projectCheck.Code.ToUpper().Contains("BFT") && typePerformanceCheck.Code.ToUpper().Contains("OFFICE"))
            {
                model.ShowFormByProjType = 7;
            }
            else if (projectCheck.Code.ToUpper().Contains("BFT") && typePerformanceCheck.Code.ToUpper().Contains("SKY"))
            {
                model.ShowFormByProjType = 8;
            }
            else if (projectCheck.Code.ToUpper().Contains("BFT") && typePerformanceCheck.Code.ToUpper().Contains("PARK"))
            {
                model.ShowFormByProjType = 9;
            }

            return PartialView("_ListTable", model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ProjectChange(string projectId)
        {
            int project;
            List<TypePerformance> lstTypePerformance = performanceService.GetListTypePerformance(0);
            SelectList typePerformances = new SelectList(lstTypePerformance, "Id", "Name");
            if (Int32.TryParse(projectId.Replace("\"", ""), out project))
            {
                List<TypePerformance> typePerformance = performanceService.GetListTypePerformance(project);
                typePerformances = new SelectList(typePerformance, "Id", "Name", typePerformance[0].Id);
            }

            return PartialView("_PerformanceDropDownList", typePerformances);
        }

        // GET: Performance/Create
        [HttpGet]
        [Route("PerformanceCreate")]
        public ActionResult Create()
        {
            PerformanceViewModel model = Session[string.Format("performance-{0}", CurrentUser.UserId)] as PerformanceViewModel;
            if (model == null)
            {
                model = InitData();
            }
            Session[string.Format("performance-{0}", CurrentUser.UserId)] = model;

            return View(model);
        }

        public PerformanceViewModel InitData()
        {
            PerformanceViewModel model = new PerformanceViewModel();
            model.FirstLoad = true;
            model.ErrorMesages = new List<string>();
            model.ErrorMessage = "";

            //Init properties
            model.PerformanceLsfb = new PerformanceLSFB();

            // UserInfo
            model.UserInfo = userService.GetUserFullInfo(CurrentUser.UserId);
            model.ManagerInfo = userService.GetManagerByUserId(CurrentUser.UserId);

            //Du an
            List<Projects> lstProjects = performanceService.GetListProjects();
            model.Projects = new SelectList(lstProjects, "Id", "Name");

            //Loai hieu suat
            List<TypePerformance> lstTypePerformance = performanceService.GetListTypePerformance(0);
            model.TypePerformances = new SelectList(lstTypePerformance, "Id", "Name");

            //Loai
            var scheduleTypes = new List<ConvertEnum>();
            foreach (var schedule in Enum.GetValues(typeof(IpfScheduleType)))
                scheduleTypes.Add(new ConvertEnum
                {
                    Value = (int)schedule,
                    Text = schedule.ToString() == "Year" ? "Năm" : "Quý"
                });
            model.ScheduleTypes = new SelectList(scheduleTypes, "Value", "Text");

            //Nam
            model.Years = Years;

            //Thang
            model.Months = Months;

            //Quy
            var quarterTypes = new List<ConvertEnum>();
            foreach (var quarter in Enum.GetValues(typeof(QuarterType)))
                quarterTypes.Add(new ConvertEnum
                {
                    Value = (int)quarter,
                    Text = BtcHelper.convertQuarter(quarter.ToString())
                });
            model.QuarterTypes = new SelectList(quarterTypes, "Value", "Text");
            //Type FB
            var typeFBs = new List<ConvertEnum>();
            foreach (var typeFB in Enum.GetValues(typeof(PerformanceTypeFB)))
                typeFBs.Add(new ConvertEnum
                {
                    Value = (int)typeFB,
                    Text = typeFB.ToString() == "FB" ? "FB" : "Non FB"
                });
            model.TypeFBs = new SelectList(typeFBs, "Value", "Text");
            //Nguoi duyet
            if (model.UserInfo.AdministratorshipID != null)
            {
                model.Approves = new SelectList(userService.GetListManageInfo((int)model.UserInfo.AdministratorshipID),
                    "ID", "FullName");
            }

            return model;
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Start(PerformanceLSFB model)
        {
            var currentModel = Session[string.Format("performance-{0}", CurrentUser.UserId)] as PerformanceViewModel;
            if (currentModel != null)
            {
                currentModel.PerformanceLsfb.ProjectId = model.ProjectId;
                currentModel.PerformanceLsfb.TypePerformanceId = model.TypePerformanceId;
                currentModel.PerformanceLsfb.Type = model.Type;
                currentModel.PerformanceLsfb.Year = model.Year;
                if (model.Type == 0)
                {
                    currentModel.PerformanceLsfb.Month = model.Month;
                }
                else
                {
                    currentModel.PerformanceLsfb.QuarterId = model.QuarterId;
                }
            }

            model.CheckExistsPerformance = performanceService.CheckCreatePerformance(model, CurrentUser.UserId);
            var project = performanceService.GetProjectById(model.ProjectId);
            var typePerformance = performanceService.GetTypePerformanceById(model.TypePerformanceId);
            if (project.Code.ToUpper().Contains("GARDEN") && typePerformance.Code.ToUpper().Contains("LS"))
            {
                model.ShowFormByProjType = 1;
            }
            else if (project.Code.ToUpper().Contains("GARDEN") && typePerformance.Code.ToUpper().Contains("FB"))
            {
                model.ShowFormByProjType = 2;
            }
            else if (project.Code.ToUpper().Contains("MANOR") && typePerformance.Code.ToUpper().Contains("LS"))
            {
                model.ShowFormByProjType = 3;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult Reset()
        {
            Session[string.Format("performance-{0}", CurrentUser.UserId)] = null;
            return Json(new PerformanceViewModel(), JsonRequestBehavior.AllowGet);
        }


        // POST: Performance/Create
        [HttpPost]
        [Route("PerformanceCreate")]
        public ActionResult Create(PerformanceViewModel model, string submitButton)
        {
            var currentModel = Session[string.Format("performance-{0}", CurrentUser.UserId)] as PerformanceViewModel;
            bool isValid = true;
            currentModel.ErrorMessage = "";
            string insertMsg = "";
            if (ModelState.IsValid)
            {
                PerformanceLSFB performanceInsert = new PerformanceLSFB();
                if (currentModel.PerformanceLsfb.ApprovedBy != null)
                {
                    performanceInsert.ApprovedBy = currentModel.PerformanceLsfb.ApprovedBy;
                }
                if (currentModel.PerformanceLsfb.ProjectId != null)
                {
                    performanceInsert.ProjectId = currentModel.PerformanceLsfb.ProjectId;
                }
                if (currentModel.PerformanceLsfb.TypePerformanceId != null)
                {
                    performanceInsert.TypePerformanceId = currentModel.PerformanceLsfb.TypePerformanceId;
                }
                if (currentModel.PerformanceLsfb.Type != null)
                {
                    performanceInsert.Type = currentModel.PerformanceLsfb.Type;
                }
                if (currentModel.PerformanceLsfb.Year != null)
                {
                    performanceInsert.Year = currentModel.PerformanceLsfb.Year;
                }
                if (currentModel.PerformanceLsfb.Month != null)
                {
                    performanceInsert.Month = currentModel.PerformanceLsfb.Month;
                }
                if (currentModel.PerformanceLsfb.QuarterId != null)
                {
                    performanceInsert.QuarterId = currentModel.PerformanceLsfb.QuarterId;
                }
                if (model.PerformanceLsfb.OfficeArea != null)
                {
                    performanceInsert.OfficeArea = model.PerformanceLsfb.OfficeArea;
                }
                if (model.PerformanceLsfb.OfficeMonthMoney != null)
                {
                    performanceInsert.OfficeMonthMoney = model.PerformanceLsfb.OfficeMonthMoney;
                }
                if (model.PerformanceLsfb.OfficeTY != null)
                {
                    performanceInsert.OfficeTY = model.PerformanceLsfb.OfficeTY;
                }
                if (model.PerformanceLsfb.OfficeLY != null)
                {
                    performanceInsert.OfficeLY = model.PerformanceLsfb.OfficeLY;
                }
                if (model.PerformanceLsfb.RetailArea != null)
                {
                    performanceInsert.RetailArea = model.PerformanceLsfb.RetailArea;
                }
                if (model.PerformanceLsfb.RetailMonthMoney != null)
                {
                    performanceInsert.RetailMonthMoney = model.PerformanceLsfb.RetailMonthMoney;
                }
                if (model.PerformanceLsfb.RetailTY != null)
                {
                    performanceInsert.RetailTY = model.PerformanceLsfb.RetailTY;
                }
                if (model.PerformanceLsfb.RetailLY != null)
                {
                    performanceInsert.RetailLY = model.PerformanceLsfb.RetailLY;
                }
                if (model.PerformanceLsfb.NewArea != null)
                {
                    performanceInsert.NewArea = model.PerformanceLsfb.NewArea;
                }
                if (model.PerformanceLsfb.NewMonthMoney != null)
                {
                    performanceInsert.NewMonthMoney = model.PerformanceLsfb.NewMonthMoney;
                }
                if (model.PerformanceLsfb.NewTotalRev != null)
                {
                    performanceInsert.NewTotalRev = model.PerformanceLsfb.NewTotalRev;
                }
                if (model.PerformanceLsfb.TotalMonthMoney != null)
                {
                    performanceInsert.TotalMonthMoney = model.PerformanceLsfb.TotalMonthMoney;
                }
                if (model.PerformanceLsfb.TotalRevTY != null)
                {
                    performanceInsert.TotalRevTY = model.PerformanceLsfb.TotalRevTY;
                }
                if (model.PerformanceLsfb.TotalRevLY != null)
                {
                    performanceInsert.TotalRevLY = model.PerformanceLsfb.TotalRevLY;
                }
                if (model.PerformanceLsfb.TotalGrossTY != null)
                {
                    performanceInsert.TotalGrossTY = model.PerformanceLsfb.TotalGrossTY;
                }
                if (model.PerformanceLsfb.TotalGrossLY != null)
                {
                    performanceInsert.TotalGrossLY = model.PerformanceLsfb.TotalGrossLY;
                }
                if (model.PerformanceLsfb.TypeFBLS != null)
                {
                    model.PerformanceLsfb.TypeFB = model.PerformanceLsfb.TypeFBLS;
                }
                if (model.PerformanceLsfb.TypeFB != null)
                {
                    performanceInsert.TypeFB = model.PerformanceLsfb.TypeFB;
                }
                if (model.PerformanceLsfb.SalesLineToLineLS != null)
                {
                    model.PerformanceLsfb.SalesLineToLine = model.PerformanceLsfb.SalesLineToLineLS;
                }
                if (model.PerformanceLsfb.SalesLineToLine != null)
                {
                    performanceInsert.SalesLineToLine = model.PerformanceLsfb.SalesLineToLine;
                }
                if (model.PerformanceLsfb.SalesAllLS != null)
                {
                    model.PerformanceLsfb.SalesAll = model.PerformanceLsfb.SalesAllLS;
                }
                if (model.PerformanceLsfb.SalesAll != null)
                {
                    performanceInsert.SalesAll = model.PerformanceLsfb.SalesAll;
                }
                if (model.PerformanceLsfb.SalesCashFlowTYLS != null)
                {
                    model.PerformanceLsfb.SalesCashFlowTY = model.PerformanceLsfb.SalesCashFlowTYLS;
                }
                if (model.PerformanceLsfb.SalesCashFlowTY != null)
                {
                    performanceInsert.SalesCashFlowTY = model.PerformanceLsfb.SalesCashFlowTY;
                }
                if (model.PerformanceLsfb.SalesCashFlowLYLS != null)
                {
                    model.PerformanceLsfb.SalesCashFlowLY = model.PerformanceLsfb.SalesCashFlowLYLS;
                }
                if (model.PerformanceLsfb.SalesCashFlowLY != null)
                {
                    performanceInsert.SalesCashFlowLY = model.PerformanceLsfb.SalesCashFlowLY;
                }
                if (model.PerformanceLsfb.RevLineTOSNoMG != null)
                {
                    performanceInsert.RevLineTOSNoMG = model.PerformanceLsfb.RevLineTOSNoMG;
                }
                if (model.PerformanceLsfb.RevLineTOSWithMG != null)
                {
                    performanceInsert.RevLineTOSWithMG = model.PerformanceLsfb.RevLineTOSWithMG;
                }
                if (model.PerformanceLsfb.RevLineNoMG != null)
                {
                    performanceInsert.RevLineNoMG = model.PerformanceLsfb.RevLineNoMG;
                }
                if (model.PerformanceLsfb.RevLineWithMG != null)
                {
                    performanceInsert.RevLineWithMG = model.PerformanceLsfb.RevLineWithMG;
                }
                if (model.PerformanceLsfb.RevAllTOSNoMG != null)
                {
                    performanceInsert.RevAllTOSNoMG = model.PerformanceLsfb.RevAllTOSNoMG;
                }
                if (model.PerformanceLsfb.RevAllTOSWithMG != null)
                {
                    performanceInsert.RevAllTOSWithMG = model.PerformanceLsfb.RevAllTOSWithMG;
                }
                if (model.PerformanceLsfb.RevAllNoMG != null)
                {
                    performanceInsert.RevAllNoMG = model.PerformanceLsfb.RevAllNoMG;
                }
                if (model.PerformanceLsfb.RevAllWithMG != null)
                {
                    performanceInsert.RevAllWithMG = model.PerformanceLsfb.RevAllWithMG;
                }
                if (model.PerformanceLsfb.RevAllLY != null)
                {
                    performanceInsert.RevAllLY = model.PerformanceLsfb.RevAllLY;
                }
                if (model.PerformanceLsfb.RevAllOPMonthMoney != null)
                {
                    performanceInsert.RevAllOPMonthMoney = model.PerformanceLsfb.RevAllOPMonthMoney;
                }
                if (model.PerformanceLsfb.RevTotalNoMGLS != null)
                {
                    model.PerformanceLsfb.RevTotalNoMG = model.PerformanceLsfb.RevTotalNoMGLS;
                }
                if (model.PerformanceLsfb.RevTotalNoMG != null)
                {
                    performanceInsert.RevTotalNoMG = model.PerformanceLsfb.RevTotalNoMG;
                }
                if (model.PerformanceLsfb.RevTotalWithMGLS != null)
                {
                    model.PerformanceLsfb.RevTotalWithMG = model.PerformanceLsfb.RevTotalWithMGLS;
                }
                if (model.PerformanceLsfb.RevTotalWithMG != null)
                {
                    performanceInsert.RevTotalWithMG = model.PerformanceLsfb.RevTotalWithMG;
                }
                if (model.PerformanceLsfb.RevTotalLYLS != null)
                {
                    model.PerformanceLsfb.RevTotalLY = model.PerformanceLsfb.RevTotalLYLS;
                }
                if (model.PerformanceLsfb.RevTotalLY != null)
                {
                    performanceInsert.RevTotalLY = model.PerformanceLsfb.RevTotalLY;
                }
                if (model.PerformanceLsfb.ComProfitTYLS != null)
                {
                    model.PerformanceLsfb.ComProfitTY = model.PerformanceLsfb.ComProfitTYLS;
                }
                else if (model.PerformanceLsfb.ComProfitTY != null)
                {
                    performanceInsert.ComProfitTY = model.PerformanceLsfb.ComProfitTY;
                }
                if (model.PerformanceLsfb.ComProfitLYLS != null)
                {
                    model.PerformanceLsfb.ComProfitLY = model.PerformanceLsfb.ComProfitLYLS;
                }
                if (model.PerformanceLsfb.ComProfitLY != null)
                {
                    performanceInsert.ComProfitLY = model.PerformanceLsfb.ComProfitLY;
                }
                if (model.PerformanceLsfb.RevLSArea != null)
                {
                    performanceInsert.RevLSArea = model.PerformanceLsfb.RevLSArea;
                }
                if (model.PerformanceLsfb.RevLSMonthMoney != null)
                {
                    performanceInsert.RevLSMonthMoney = model.PerformanceLsfb.RevLSMonthMoney;
                }
                if (model.PerformanceLsfb.RevLSRev != null)
                {
                    performanceInsert.RevLSRev = model.PerformanceLsfb.RevLSRev;
                }
                if (model.PerformanceLsfb.RevLSOPMonthMoney != null)
                {
                    performanceInsert.RevLSOPMonthMoney = model.PerformanceLsfb.RevLSOPMonthMoney;
                }
                if (model.PerformanceLsfb.BusinessProfit != null)
                {
                    performanceInsert.BusinessProfit = model.PerformanceLsfb.BusinessProfit;
                }
                if (model.PerformanceLsfb.RevAllOccRate != null)
                {
                    performanceInsert.RevAllOccRate = model.PerformanceLsfb.RevAllOccRate;
                }
                if (model.PerformanceLsfb.RevNormalLine != null)
                {
                    performanceInsert.RevNormalLine = model.PerformanceLsfb.RevNormalLine;
                }
                if (model.PerformanceLsfb.RevNormalAll != null)
                {
                    performanceInsert.RevNormalAll = model.PerformanceLsfb.RevNormalAll;
                }
                if (model.PerformanceLsfb.RevAllRev != null)
                {
                    performanceInsert.RevAllRev = model.PerformanceLsfb.RevAllRev;
                }
                if (model.PerformanceLsfb.Note != null && !"".Equals(model.PerformanceLsfb.Note))
                {
                    performanceInsert.Note = model.PerformanceLsfb.Note;
                }
                performanceInsert.StatusId = 0;
                performanceInsert.DeleteFlg = 0;
                performanceInsert.CreatedDate = DateTime.Now;
                performanceInsert.CreatedBy = CurrentUser.UserId;
                if (performanceService.InsertPerformance(performanceInsert, ref insertMsg))
                {
                    return RedirectToAction("Index", "Performance");
                }
                else
                {
                    currentModel.ErrorMessage = insertMsg;
                    return View(currentModel);
                }
            }
            else
            {
                currentModel.ErrorMessage = string.Join("<br />", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));
                return View(currentModel);
            }
        }

        // GET: Performance/Edit
        [HttpGet]
        [Route("PerformanceEdit")]
        public ActionResult Edit(int id)
        {
            var model = Session[string.Format("performance-{0}", CurrentUser.UserId)] as PerformanceViewModel;
            if (id != 0)
            {
                if (model == null)
                {
                    model = GePerformanceById(id);
                    model.FirstLoad = false;
                }
            }
            else
            {
                if (model == null)
                {
                    model = InitData();
                }
            }
            Session[string.Format("performance-{0}", CurrentUser.UserId)] = model;

            return View("Create", model);
        }

        public PerformanceViewModel GePerformanceById(int id)
        {
            PerformanceViewModel model = new PerformanceViewModel();
            model.FirstLoad = true;
            model.ErrorMesages = new List<string>();
            model.ErrorMessage = "";

            //Init properties
            model.PerformanceLsfb = performanceService.GetPerformanceById(id);
            // UserInfo
            if (model.PerformanceLsfb.CreatedBy != null)
            {
                model.UserInfo = userService.GetUserFullInfo((int)model.PerformanceLsfb.CreatedBy);
                model.ManagerInfo = userService.GetManagerByUserId((int)model.PerformanceLsfb.CreatedBy);
            }
            if (model.UserInfo.AdministratorshipID != null)
            {
                model.Approves = new SelectList(userService.GetListManageInfo((int)model.UserInfo.AdministratorshipID), "ID", "FullName", model.PerformanceLsfb.ApprovedBy);
            }
            //Du an
            List<Projects> lstProjects = performanceService.GetListProjects();
            model.Projects = new SelectList(lstProjects, "Id", "Name", model.PerformanceLsfb.ProjectId);

            //Loai hieu suat
            List<TypePerformance> lstTypePerformance = performanceService.GetListTypePerformance(0);
            model.TypePerformances = new SelectList(lstTypePerformance, "Id", "Name", model.PerformanceLsfb.TypePerformanceId);

            //Loai
            var scheduleTypes = new List<ConvertEnum>();
            foreach (var schedule in Enum.GetValues(typeof(IpfScheduleType)))
                scheduleTypes.Add(new ConvertEnum
                {
                    Value = (int)schedule,
                    Text = schedule.ToString() == "Year" ? "Năm" : "Quý"
                });
            model.ScheduleTypes = new SelectList(scheduleTypes, "Value", "Text", model.PerformanceLsfb.Type);

            //Nam
            model.Years = new SelectList(Years, "Value", "Text", model.PerformanceLsfb.Year);

            //Thang
            model.Months = new SelectList(Months, "Value", "Text", model.PerformanceLsfb.Month);
            //Quy
            var quarterTypes = new List<ConvertEnum>();
            foreach (var quarter in Enum.GetValues(typeof(QuarterType)))
                quarterTypes.Add(new ConvertEnum
                {
                    Value = (int)quarter,
                    Text = BtcHelper.convertQuarter(quarter.ToString())
                });
            model.QuarterTypes = new SelectList(quarterTypes, "Value", "Text", model.PerformanceLsfb.QuarterId);
            //Type FB
            var typeFBs = new List<ConvertEnum>();
            foreach (var typeFB in Enum.GetValues(typeof(PerformanceTypeFB)))
                typeFBs.Add(new ConvertEnum
                {
                    Value = (int)typeFB,
                    Text = typeFB.ToString() == "FB" ? "FB" : "Non FB"
                });
            model.TypeFBs = new SelectList(typeFBs, "Value", "Text", model.PerformanceLsfb.TypeFB);

            var project = performanceService.GetProjectById(model.PerformanceLsfb.ProjectId);
            var typePerformance = performanceService.GetTypePerformanceById(model.PerformanceLsfb.TypePerformanceId);
            if (project.Code.ToUpper().Contains("GARDEN") && typePerformance.Code.ToUpper().Contains("LS"))
            {
                model.PerformanceLsfb.ShowFormByProjType = 1;
                model.PerformanceLsfb.ComProfitLYLS = model.PerformanceLsfb.ComProfitLY;
                model.PerformanceLsfb.ComProfitTYLS = model.PerformanceLsfb.ComProfitTY;
                model.PerformanceLsfb.SalesLineToLineLS = model.PerformanceLsfb.SalesLineToLine;
                model.PerformanceLsfb.SalesAllLS = model.PerformanceLsfb.SalesAll;
                model.PerformanceLsfb.RevTotalLYLS = model.PerformanceLsfb.RevTotalLY;
                model.PerformanceLsfb.RevTotalNoMGLS = model.PerformanceLsfb.RevTotalNoMG;
                model.PerformanceLsfb.RevTotalWithMGLS = model.PerformanceLsfb.RevTotalWithMG;
                model.PerformanceLsfb.SalesCashFlowLYLS = model.PerformanceLsfb.SalesCashFlowLY;
                model.PerformanceLsfb.SalesCashFlowTYLS = model.PerformanceLsfb.SalesCashFlowTY;
                model.PerformanceLsfb.TypeFBLS = model.PerformanceLsfb.TypeFB;
            }
            else if (project.Code.ToUpper().Contains("GARDEN") && typePerformance.Code.ToUpper().Contains("FB"))
            {
                model.PerformanceLsfb.ShowFormByProjType = 2;
            }
            else if (project.Code.ToUpper().Contains("MANOR") && typePerformance.Code.ToUpper().Contains("LS"))
            {
                model.PerformanceLsfb.ShowFormByProjType = 3;
            }

            return model;
        }

        // POST: Performance/Edit
        [HttpPost]
        [Route("PerformanceEdit")]
        public ActionResult Edit(PerformanceViewModel model, string submitButton)
        {
            var currentModel = Session[string.Format("performance-{0}", CurrentUser.UserId)] as PerformanceViewModel;
            bool isValid = true;
            currentModel.ErrorMessage = "";
            string insertMsg = "";
            if (ModelState.IsValid)
            {
                if (model.PerformanceLsfb.Id != 0)
                {
                    var performanceUpdate = performanceService.GetPerformanceById(model.PerformanceLsfb.Id);
                    if (model.PerformanceLsfb.OfficeArea != performanceUpdate.OfficeArea)
                    {
                        performanceUpdate.OfficeArea = model.PerformanceLsfb.OfficeArea;
                    }
                    if (model.PerformanceLsfb.OfficeMonthMoney != performanceUpdate.OfficeMonthMoney)
                    {
                        performanceUpdate.OfficeMonthMoney = model.PerformanceLsfb.OfficeMonthMoney;
                    }
                    if (model.PerformanceLsfb.OfficeTY != performanceUpdate.OfficeTY)
                    {
                        performanceUpdate.OfficeTY = model.PerformanceLsfb.OfficeTY;
                    }
                    if (model.PerformanceLsfb.OfficeLY != performanceUpdate.OfficeLY)
                    {
                        performanceUpdate.OfficeLY = model.PerformanceLsfb.OfficeLY;
                    }
                    if (model.PerformanceLsfb.RetailArea != performanceUpdate.RetailArea)
                    {
                        performanceUpdate.RetailArea = model.PerformanceLsfb.RetailArea;
                    }
                    if (model.PerformanceLsfb.RetailMonthMoney != performanceUpdate.RetailMonthMoney)
                    {
                        performanceUpdate.RetailMonthMoney = model.PerformanceLsfb.RetailMonthMoney;
                    }
                    if (model.PerformanceLsfb.RetailTY != performanceUpdate.RetailTY)
                    {
                        performanceUpdate.RetailTY = model.PerformanceLsfb.RetailTY;
                    }
                    if (model.PerformanceLsfb.RetailLY != performanceUpdate.RetailLY)
                    {
                        performanceUpdate.RetailLY = model.PerformanceLsfb.RetailLY;
                    }
                    if (model.PerformanceLsfb.NewArea != performanceUpdate.NewArea)
                    {
                        performanceUpdate.NewArea = model.PerformanceLsfb.NewArea;
                    }
                    if (model.PerformanceLsfb.NewMonthMoney != performanceUpdate.NewMonthMoney)
                    {
                        performanceUpdate.NewMonthMoney = model.PerformanceLsfb.NewMonthMoney;
                    }
                    if (model.PerformanceLsfb.NewTotalRev != performanceUpdate.NewTotalRev)
                    {
                        performanceUpdate.NewTotalRev = model.PerformanceLsfb.NewTotalRev;
                    }
                    if (model.PerformanceLsfb.TotalMonthMoney != performanceUpdate.TotalMonthMoney)
                    {
                        performanceUpdate.TotalMonthMoney = model.PerformanceLsfb.TotalMonthMoney;
                    }
                    if (model.PerformanceLsfb.TotalRevTY != performanceUpdate.TotalRevTY)
                    {
                        performanceUpdate.TotalRevTY = model.PerformanceLsfb.TotalRevTY;
                    }
                    if (model.PerformanceLsfb.TotalRevLY != performanceUpdate.TotalRevLY)
                    {
                        performanceUpdate.TotalRevLY = model.PerformanceLsfb.TotalRevLY;
                    }
                    if (model.PerformanceLsfb.TotalGrossTY != performanceUpdate.TotalGrossTY)
                    {
                        performanceUpdate.TotalGrossTY = model.PerformanceLsfb.TotalGrossTY;
                    }
                    if (model.PerformanceLsfb.TotalGrossLY != performanceUpdate.TotalGrossLY)
                    {
                        performanceUpdate.TotalGrossLY = model.PerformanceLsfb.TotalGrossLY;
                    }
                    if (currentModel.PerformanceLsfb.ShowFormByProjType == 1)
                    {
                        model.PerformanceLsfb.TypeFB = model.PerformanceLsfb.TypeFBLS;
                    }
                    if (model.PerformanceLsfb.TypeFB != performanceUpdate.TypeFB)
                    {
                        performanceUpdate.TypeFB = model.PerformanceLsfb.TypeFB;
                    }
                    if (currentModel.PerformanceLsfb.ShowFormByProjType == 1)
                    {
                        model.PerformanceLsfb.SalesLineToLine = model.PerformanceLsfb.SalesLineToLineLS;
                    }
                    if (model.PerformanceLsfb.SalesLineToLine != performanceUpdate.SalesLineToLine)
                    {
                        performanceUpdate.SalesLineToLine = model.PerformanceLsfb.SalesLineToLine;
                    }
                    if (currentModel.PerformanceLsfb.ShowFormByProjType == 1)
                    {
                        model.PerformanceLsfb.SalesAll = model.PerformanceLsfb.SalesAllLS;
                    }
                    if (model.PerformanceLsfb.SalesAll != performanceUpdate.SalesAll)
                    {
                        performanceUpdate.SalesAll = model.PerformanceLsfb.SalesAll;
                    }
                    if (currentModel.PerformanceLsfb.ShowFormByProjType == 1)
                    {
                        model.PerformanceLsfb.SalesCashFlowTY = model.PerformanceLsfb.SalesCashFlowTYLS;
                    }
                    if (model.PerformanceLsfb.SalesCashFlowTY != performanceUpdate.SalesCashFlowTY)
                    {
                        performanceUpdate.SalesCashFlowTY = model.PerformanceLsfb.SalesCashFlowTY;
                    }
                    if (currentModel.PerformanceLsfb.ShowFormByProjType == 1)
                    {
                        model.PerformanceLsfb.SalesCashFlowLY = model.PerformanceLsfb.SalesCashFlowLYLS;
                    }
                    if (model.PerformanceLsfb.SalesCashFlowLY != performanceUpdate.SalesCashFlowLY)
                    {
                        performanceUpdate.SalesCashFlowLY = model.PerformanceLsfb.SalesCashFlowLY;
                    }
                    if (model.PerformanceLsfb.RevLineTOSNoMG != performanceUpdate.RevLineTOSNoMG)
                    {
                        performanceUpdate.RevLineTOSNoMG = model.PerformanceLsfb.RevLineTOSNoMG;
                    }
                    if (model.PerformanceLsfb.RevLineTOSWithMG != performanceUpdate.RevLineTOSWithMG)
                    {
                        performanceUpdate.RevLineTOSWithMG = model.PerformanceLsfb.RevLineTOSWithMG;
                    }
                    if (model.PerformanceLsfb.RevLineNoMG != performanceUpdate.RevLineNoMG)
                    {
                        performanceUpdate.RevLineNoMG = model.PerformanceLsfb.RevLineNoMG;
                    }
                    if (model.PerformanceLsfb.RevLineWithMG != performanceUpdate.RevLineWithMG)
                    {
                        performanceUpdate.RevLineWithMG = model.PerformanceLsfb.RevLineWithMG;
                    }
                    if (model.PerformanceLsfb.RevAllTOSNoMG != performanceUpdate.RevAllTOSNoMG)
                    {
                        performanceUpdate.RevAllTOSNoMG = model.PerformanceLsfb.RevAllTOSNoMG;
                    }
                    if (model.PerformanceLsfb.RevAllTOSWithMG != performanceUpdate.RevAllTOSWithMG)
                    {
                        performanceUpdate.RevAllTOSWithMG = model.PerformanceLsfb.RevAllTOSWithMG;
                    }
                    if (model.PerformanceLsfb.RevAllNoMG != performanceUpdate.RevAllNoMG)
                    {
                        performanceUpdate.RevAllNoMG = model.PerformanceLsfb.RevAllNoMG;
                    }
                    if (model.PerformanceLsfb.RevAllWithMG != performanceUpdate.RevAllWithMG)
                    {
                        performanceUpdate.RevAllWithMG = model.PerformanceLsfb.RevAllWithMG;
                    }
                    if (model.PerformanceLsfb.RevAllLY != performanceUpdate.RevAllLY)
                    {
                        performanceUpdate.RevAllLY = model.PerformanceLsfb.RevAllLY;
                    }
                    if (model.PerformanceLsfb.RevAllOPMonthMoney != performanceUpdate.RevAllOPMonthMoney)
                    {
                        performanceUpdate.RevAllOPMonthMoney = model.PerformanceLsfb.RevAllOPMonthMoney;
                    }
                    if (currentModel.PerformanceLsfb.ShowFormByProjType == 1)
                    {
                        model.PerformanceLsfb.RevTotalNoMG = model.PerformanceLsfb.RevTotalNoMGLS;
                    }
                    if (model.PerformanceLsfb.RevTotalNoMG != performanceUpdate.RevTotalNoMG)
                    {
                        performanceUpdate.RevTotalNoMG = model.PerformanceLsfb.RevTotalNoMG;
                    }
                    if (currentModel.PerformanceLsfb.ShowFormByProjType == 1)
                    {
                        model.PerformanceLsfb.RevTotalWithMG = model.PerformanceLsfb.RevTotalWithMGLS;
                    }
                    if (model.PerformanceLsfb.RevTotalWithMG != performanceUpdate.RevTotalWithMG)
                    {
                        performanceUpdate.RevTotalWithMG = model.PerformanceLsfb.RevTotalWithMG;
                    }
                    if (currentModel.PerformanceLsfb.ShowFormByProjType == 1)
                    {
                        model.PerformanceLsfb.RevTotalLY = model.PerformanceLsfb.RevTotalLYLS;
                    }
                    if (model.PerformanceLsfb.RevTotalLY != performanceUpdate.RevTotalLY)
                    {
                        performanceUpdate.RevTotalLY = model.PerformanceLsfb.RevTotalLY;
                    }
                    if (currentModel.PerformanceLsfb.ShowFormByProjType == 1)
                    {
                        model.PerformanceLsfb.ComProfitTY = model.PerformanceLsfb.ComProfitTYLS;
                    }
                    if (model.PerformanceLsfb.ComProfitTY != performanceUpdate.ComProfitTY)
                    {
                        performanceUpdate.ComProfitTY = model.PerformanceLsfb.ComProfitTY;
                    }
                    if (currentModel.PerformanceLsfb.ShowFormByProjType == 1)
                    {
                        model.PerformanceLsfb.ComProfitLY = model.PerformanceLsfb.ComProfitLYLS;
                    }
                    if (model.PerformanceLsfb.ComProfitLY != performanceUpdate.ComProfitLY)
                    {
                        performanceUpdate.ComProfitLY = model.PerformanceLsfb.ComProfitLY;
                    }
                    if (model.PerformanceLsfb.RevLSArea != performanceUpdate.RevLSArea)
                    {
                        performanceUpdate.RevLSArea = model.PerformanceLsfb.RevLSArea;
                    }
                    if (model.PerformanceLsfb.RevLSMonthMoney != performanceUpdate.RevLSMonthMoney)
                    {
                        performanceUpdate.RevLSMonthMoney = model.PerformanceLsfb.RevLSMonthMoney;
                    }
                    if (model.PerformanceLsfb.RevLSRev != performanceUpdate.RevLSRev)
                    {
                        performanceUpdate.RevLSRev = model.PerformanceLsfb.RevLSRev;
                    }
                    if (model.PerformanceLsfb.RevLSOPMonthMoney != performanceUpdate.RevLSOPMonthMoney)
                    {
                        performanceUpdate.RevLSOPMonthMoney = model.PerformanceLsfb.RevLSOPMonthMoney;
                    }
                    if (model.PerformanceLsfb.BusinessProfit != performanceUpdate.BusinessProfit)
                    {
                        performanceUpdate.BusinessProfit = model.PerformanceLsfb.BusinessProfit;
                    }
                    if (model.PerformanceLsfb.RevAllOccRate != performanceUpdate.RevAllOccRate)
                    {
                        performanceUpdate.RevAllOccRate = model.PerformanceLsfb.RevAllOccRate;
                    }
                    if (model.PerformanceLsfb.RevNormalLine != performanceUpdate.RevNormalLine)
                    {
                        performanceUpdate.RevNormalLine = model.PerformanceLsfb.RevNormalLine;
                    }
                    if (model.PerformanceLsfb.RevNormalAll != performanceUpdate.RevNormalAll)
                    {
                        performanceUpdate.RevNormalAll = model.PerformanceLsfb.RevNormalAll;
                    }
                    if (model.PerformanceLsfb.RevAllRev != performanceUpdate.RevAllRev)
                    {
                        performanceUpdate.RevAllRev = model.PerformanceLsfb.RevAllRev;
                    }
                    if (model.PerformanceLsfb.Note != null && !model.PerformanceLsfb.Note.Equals(performanceUpdate.Note))
                    {
                        performanceUpdate.Note = model.PerformanceLsfb.Note;
                    }
                    performanceUpdate.UpdatedDate = DateTime.Now;
                    performanceUpdate.UpdatedBy = CurrentUser.UserId;

                    if (performanceService.UpdatePerformance(performanceUpdate, ref insertMsg))
                    {
                        return RedirectToAction("Index", "Performance");
                    }
                    else
                    {
                        currentModel.ErrorMessage = insertMsg;
                        return View("Create", currentModel);
                    }
                }
                else
                {
                    return View("Create", currentModel);
                }
            }
            else
            {
                currentModel.ErrorMessage = string.Join("<br />", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));
                return View("Create", currentModel);
            }
        }

        // GET: Performance/Approved
        [HttpGet]
        [Route("PerformanceApprove")]
        public ActionResult Approve(int id)
        {
            var model = Session[string.Format("performance-{0}", CurrentUser.UserId)] as PerformanceViewModel;
            if (id != 0)
            {
                if (model == null)
                {
                    model = GePerformanceById(id);
                    model.FirstLoad = false;
                    model.isApprove = true;
                }
            }
            else
            {
                if (model == null)
                {
                    model = InitData();
                }
            }
            Session[string.Format("performance-{0}", CurrentUser.UserId)] = model;

            return View("Create", model);
        }

        // POST: Performance/Approved
        [HttpPost]
        [Route("PerformanceApprove")]
        public ActionResult Approve(PerformanceViewModel model, string submitButton)
        {
            var currentModel = Session[string.Format("performance-{0}", CurrentUser.UserId)] as PerformanceViewModel;
            bool isValid = true;
            currentModel.ErrorMessage = "";
            string insertMsg = "";
            if (ModelState.IsValid)
            {
                if (model.PerformanceLsfb.Id != 0)
                {
                    var performanceUpdate = performanceService.GetPerformanceById(model.PerformanceLsfb.Id);
                    if("Cancel".Equals(submitButton))
                    {
                        performanceUpdate.StatusId = 2;
                    }
                    else
                    {
                        performanceUpdate.StatusId = 1;
                    }
                    performanceUpdate.Comment = model.PerformanceLsfb.Comment;

                    if (performanceService.UpdatePerformance(performanceUpdate, ref insertMsg))
                    {
                        return RedirectToAction("Index", "Performance");
                    }
                    else
                    {
                        currentModel.ErrorMessage = insertMsg;
                        return View("Create", currentModel);
                    }
                }
                else
                {
                    return View("Create", currentModel);
                }
            }
            else
            {
                currentModel.ErrorMessage = string.Join("<br />", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));
                return View("Create", currentModel);
            }
        }
        #endregion Performance
    }
}
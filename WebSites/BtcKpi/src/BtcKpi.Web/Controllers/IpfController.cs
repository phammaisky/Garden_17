using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Antlr.Runtime.Misc;
using BtcKpi.Model;
using BtcKpi.Model.Enum;
using BtcKpi.Service;
using BtcKpi.Service.Common;
using BtcKpi.Web.ViewModels;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace BtcKpi.Web.Controllers
{
    [RoutePrefix("Ipf")]
    public class IpfController : BaseController
    {
        private readonly IUserService userService;
        private readonly IIpfService ipfService;
        private readonly IEmailService emailService;

        public IpfController(IUserService userService, IIpfService ipfService, IEmailService emailService)
        {
            this.userService = userService;
            this.ipfService = ipfService;
            this.emailService = emailService;
        }

        #region Pending List
        // GET: Ipf Pending
        public ActionResult Pending()
        {
            Session[string.Format("ipf-{0}", CurrentUser.UserId)] = null;
            IpfListViewModel model = new IpfListViewModel();
            model.Status = ""; //Tất cả
            model.UserInfo = userService.GetUserFullInfo(CurrentUser.UserId);

            //Công ty - Phòng ban
            model.Companies = new List<SelectListItem>();
            model.Departments = new List<SelectListItem>();
            var deparments = userService.GetDepartmentByUser(CurrentUser.UserId);
            if (deparments.Any())
            {
                var companies = (from d in deparments select new { d.CompanyId, d.CompanyName }).Distinct().ToList();
                model.Companies = new SelectList(companies, "CompanyId", "CompanyName");

                var departments = (from d in deparments select new { d.Id, d.Name }).Distinct().ToList();
                model.Departments = new SelectList(departments, "Id", "Name");
            }
            //Loại
            var scheduleTypes = new List<ConvertEnum>();
            foreach (var schedule in Enum.GetValues(typeof(IpfScheduleType)))
                scheduleTypes.Add(new ConvertEnum
                {
                    Value = (int)schedule,
                    Text = schedule.ToString() == "Year" ? "Năm" : "Định kỳ"
                });
            model.ScheduleTypes = new SelectList(scheduleTypes, "Value", "Text");
            model.ScheduleType = "1";

            //Năm
            model.Years = Years;
            model.Year = DateTime.Now.Year.ToString();
            //Kỳ
            model.IpfSchedules = new SelectList(ipfService.IpfSchedulesByDeparment((int)model.UserInfo.DepartmentID), "ID", "Name");
            var month = DateTime.Now.Month - 1;
            var monthItem = model.IpfSchedules.FirstOrDefault(p => p.Text.Contains(month.ToString()));
            if (monthItem != null)
            {
                model.ScheduleID = monthItem.Value;
            }

            //Trạng thái
            var ipfStatus = new List<ConvertEnum>();
            foreach (var status in Enum.GetValues(typeof(KpiStatus)))
                ipfStatus.Add(new ConvertEnum
                {
                    Value = (int)status,
                    Text = status.ToString() == "Draft" ? "Lưu nháp" : (status.ToString() == "Complete" ? "Hoàn thành" : "Đã duyệt")
                });
            model.IpfStatus = new SelectList(ipfStatus, "Value", "Text");

            model.IpfInfos = new List<IpfInfo>();
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult CompanyChange(string companyId)
        {
            int company;
            var items = userService.GetDepartmentByUser(CurrentUser.UserId);
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
        public ActionResult SearchPending(IpfListViewModel model)
        {
            model.IpfInfos = ipfService.GetIpfByConditions(model.Status, model.CompanyID, model.DepartmentID, model.ScheduleType, model.Year, model.ScheduleID, model.FullName);
            model.UserID = CurrentUser.UserId;
            return PartialView("_IpfPendingTable", model);
        }
        #endregion Pending List

        #region Done List
        // GET: Ipf Done
        public ActionResult Done()
        {
            Session[string.Format("ipf-{0}", CurrentUser.UserId)] = null;
            IpfListViewModel model = new IpfListViewModel();
            model.UserID = CurrentUser.UserId;
            model.Status = "1"; //Đã duyệt
            model.UserInfo = userService.GetUserFullInfo(CurrentUser.UserId);


            //Công ty - Phòng ban
            model.Companies = new List<SelectListItem>();
            model.Departments = new List<SelectListItem>();
            var deparments = userService.GetDepartmentByUser(CurrentUser.UserId);
            if (deparments.Any())
            {
                var companies = (from d in deparments select new { d.CompanyId, d.CompanyName }).Distinct().ToList();
                model.Companies = new SelectList(companies, "CompanyId", "CompanyName");

                var departments = (from d in deparments select new { d.Id, d.Name }).Distinct().ToList();
                model.Departments = new SelectList(departments, "Id", "Name");
            }
            //Loại
            var scheduleTypes = new List<ConvertEnum>();
            foreach (var schedule in Enum.GetValues(typeof(IpfScheduleType)))
                scheduleTypes.Add(new ConvertEnum
                {
                    Value = (int)schedule,
                    Text = schedule.ToString() == "Year" ? "Năm" : "Định kỳ"
                });
            model.ScheduleTypes = new SelectList(scheduleTypes, "Value", "Text");
            //Năm
            model.Years = Years;
            //Kỳ
            model.IpfSchedules = new SelectList(ipfService.IpfSchedulesByDeparment((int)model.UserInfo.DepartmentID), "ID", "Name");
            model.IpfInfos = new List<IpfInfo>();
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SearchDone(IpfListViewModel model)
        {
            model.IpfInfos = ipfService.GetIpfByConditions(model.Status, model.CompanyID, model.DepartmentID, model.ScheduleType, model.Year, model.ScheduleID, model.FullName);
            model.UserID = CurrentUser.UserId;
            return PartialView("_IpfDoneTable", model);
        }
        #endregion Done List

        #region All List
        //GET: Ipf All
        public ActionResult All()
        {
            Session[string.Format("ipf-{0}", CurrentUser.UserId)] = null;
            return View();
        }
        #endregion All List

        #region CRUD Pending
        [AllowAnonymous]
        [HttpPost]
        public JsonResult Start(Ipf model)
        {
            var currentModel = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            currentModel.ErrorMessage = "";

            currentModel.Ipf.ScheduleType = model.ScheduleType;
            currentModel.Ipf.Year = model.Year;
            currentModel.Ipf.ScheduleID = model.ScheduleID;
            currentModel.Ipf.BodId = model.BodId;
            int scheduleID = 0;
            int.TryParse(model.ScheduleID.ToString(), out scheduleID);

            if (ipfService.IsExist(CurrentUser.UserId, (int) model.Year, (int) model.ScheduleType, scheduleID))
            {
                if (model.ScheduleType == 0)
                {
                    currentModel.ErrorMessage = string.Format("Bạn đã tạo KPIs cá nhân loại {0} cho năm {1} !",
                        BtcHelper.ConvertIpfScheduleType(currentModel.Ipf.ScheduleType), model.Year);
                }
                else
                {
                    currentModel.ErrorMessage = string.Format("Bạn đã tạo KPIs cá nhân loại {0} cho kỳ {1} năm {2} !",
                        BtcHelper.ConvertIpfScheduleType(currentModel.Ipf.ScheduleType), model.ScheduleID, model.Year);
                }
            }
            
            return Json(currentModel, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Reset()
        {
            Session[string.Format("ipf-{0}", CurrentUser.UserId)] = null;
            return Json(new IpfViewModel(), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("TabClick")]
        public JsonResult TabClick(string tabId)
        {
            IpfViewModel model = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            model.CurrentTab = tabId;
            model.FirstLoad = false;
            Session[string.Format("ipf-{0}", CurrentUser.UserId)] = model;
            return Json(tabId, JsonRequestBehavior.AllowGet);
        }
        // GET: Ipf/Create
        [HttpGet]
        [Route("PendingCreate")]
        public ActionResult Create()
        {

            IpfViewModel model = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            if (model == null)
            {
                model = InitData();
            }

            Session[string.Format("ipf-{0}", CurrentUser.UserId)] = model;

            ViewBag.CurrentUser = CurrentUser;
            return View(model);
        }

        // POST: Ipf/Create
        [HttpPost]
        [Route("PendingCreate")]
        public ActionResult Create(IpfViewModel model, string submitButton)
        {
            ViewBag.CurrentUser = CurrentUser;
            var currentModel = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            bool isValid = true;
            currentModel.ErrorMessage = "";
            string insertMsg = "";

            if (ModelState.IsValid)
            {
                //Kiểm tra lại tổng tỷ trọng
                //Công việc
                if (currentModel.CompleteWorkWeightToal != 100)
                {
                    isValid = false;
                    currentModel.ErrorMessage += (string.IsNullOrEmpty(currentModel.ErrorMessage) ? "" : " - ") +
                                                 "Tổng tỷ trọng mục tiêu hoàn thành công việc trong năm/kỳ chưa bằng 100%.";
                }

                //Năng lực
                byte competencyWeightTotal = 0;
                foreach (var item in model.Competencies)
                {
                    competencyWeightTotal += item.Weight ?? (byte)0;
                }
                if (competencyWeightTotal != 100)
                {
                    isValid = false;
                    currentModel.ErrorMessage += (string.IsNullOrEmpty(currentModel.ErrorMessage) ? "" : " - ") +
                                                 "Tổng tỷ trọng mục tiêu năng lực chưa bằng 100%.";
                }

                // Không hợp lệ thông báo lỗi
                if (!isValid)
                    return View(currentModel);
                else
                {
                    //Mapping data
                    List<IpfDetail> details = new List<IpfDetail>();
                    List<PersonalPlan> personalPlans = new List<PersonalPlan>();
                    foreach (var item in currentModel.CompleteWorks.Where(t => t.IsWorkTitle == false))
                    {
                        item.IsNextYear = 0;
                        item.WorkType = 0;
                        details.Add(item);
                    }
                    foreach (var item in model.Competencies)
                    {
                        item.IsNextYear = 0;
                        item.WorkType = 1;
                        item.WorkCompleteID = 0;
                        details.Add(item);
                    }

                    // Nếu là KPIs năm thì có thêm Kế hoạch phát triển bản thân và Mục tiêu năm tiếp theo
                    if (currentModel.Ipf.ScheduleType == 0)
                    {
                        foreach (var item in currentModel.PersonalPlanCompetencies)
                        {
                            item.Type = 0;
                            personalPlans.Add(item);
                        }
                        foreach (var item in model.PersonalPlanCareers)
                        {
                            item.Type = 1;
                            personalPlans.Add(item);
                        }

                        foreach (var item in currentModel.CompleteWorksNextYear.Where(t => t.IsWorkTitle == false))
                        {
                            item.IsNextYear = 1;
                            item.WorkType = 0;
                            details.Add(item);
                        }
                        foreach (var item in model.CompetenciesNextYear)
                        {
                            item.IsNextYear = 1;
                            item.WorkType = 1;
                            item.WorkCompleteID = 0;
                            details.Add(item);
                        }
                    }

                    //Trạng thái Lưu nháp or Hoàn thành
                    currentModel.Ipf.Status = submitButton == "OK" ? 1 : 0;
                    currentModel.Ipf.SelfComment = model.Ipf.SelfComment;
                    if (ipfService.AddIpf(CurrentUser.UserId, currentModel.ManagerInfo.ID, currentModel.UserInfo.AdministratorshipID > 5 ? 0 : 1, currentModel.Ipf, details, personalPlans,
                        ref insertMsg))
                    {

                        if (currentModel.Ipf.Status == 1)
                        {
                            string sendMailError = SendMailCreate(currentModel);
                        }

                        return RedirectToAction("Pending", "Ipf");
                    }
                    else
                    {
                        currentModel.ErrorMessage = insertMsg;
                        return View(currentModel);
                    }
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

        [AllowAnonymous]
        private string SendMailCreate(IpfViewModel currentModel)
        {
            if (currentModel.Ipf.ScheduleType == 1)
            {
                //Kỳ
                currentModel.IpfSchedules = new SelectList(ipfService.IpfSchedulesByDeparment((int)currentModel.UserInfo.DepartmentID), "ID", "Name");
                currentModel.Ipf.ScheduleName = currentModel.IpfSchedules
                    .Where(p => p.Value == ((int)currentModel.Ipf.ScheduleID).ToString()).Select(p => p.Text)
                    .FirstOrDefault();
            }
            else
            {
                currentModel.Ipf.ScheduleName = "-";
            }
            return emailService.SendMailCreateIpf(currentModel.Ipf);
        }

        [AllowAnonymous]
        private string SendMailApprove(IpfViewModel currentModel)
        {
            if (currentModel.Ipf.ScheduleType == 1)
            {
                var userCreated = userService.GetUserFullInfo((int) currentModel.Ipf.CreatedBy);
                //Kỳ
                currentModel.IpfSchedules = new SelectList(ipfService.IpfSchedulesByDeparment((int)userCreated.DepartmentID), "ID", "Name");
                currentModel.Ipf.ScheduleName = currentModel.IpfSchedules
                    .Where(p => p.Value == ((int)currentModel.Ipf.ScheduleID).ToString()).Select(p => p.Text)
                    .FirstOrDefault();
            }
            else
            {
                currentModel.Ipf.ScheduleName = "-";
            }
            return emailService.SendMailApproveIpf(currentModel.Ipf);
        }

        public IpfViewModel InitData()
        {
            IpfViewModel model = new IpfViewModel();
            model.FirstLoad = true;
            model.ErrorMesages = new List<string>();
            model.ErrorMessage = "";

            //Init properties
            model.Ipf = new Ipf();
            model.CompleteWorks = new List<IpfDetail>();
            model.Competencies = new List<IpfDetail>();
            model.PersonalPlanCompetencies = new List<PersonalPlan>();
            model.PersonalPlanCareers = new List<PersonalPlan>();
            model.CompleteWorksNextYear = new List<IpfDetail>();
            model.CompetenciesNextYear = new List<IpfDetail>();

            // UserInfo
            model.UserInfo = userService.GetUserFullInfo(CurrentUser.UserId);
            model.ManagerInfo = userService.GetManagerByUserId(CurrentUser.UserId);

            //Loại
            var scheduleTypes = new List<ConvertEnum>();
            foreach (var schedule in Enum.GetValues(typeof(IpfScheduleType)))
                scheduleTypes.Add(new ConvertEnum
                {
                    Value = (int)schedule,
                    Text = schedule.ToString() == "Year" ? "Năm" : "Định kỳ"
                });
            model.ScheduleTypes = new SelectList(scheduleTypes, "Value", "Text");

            //Năm
            model.Years = Years;

            //Kỳ
            model.IpfSchedules = new SelectList(ipfService.IpfSchedulesByDeparment((int)model.UserInfo.DepartmentID), "ID", "Name");

            //Nhóm công việc
            var completeWorkTitles = ipfService.CompleteWorkTitleByDepartment((int)model.UserInfo.DepartmentID);
            model.CompleteWorkTitles = new SelectList(completeWorkTitles, "ID", "Name");

            //Active tab
            model.CurrentTab = "liOnYear";

            int stt = 0;
            // On Year Tab
            foreach (var completeWorkTitle in completeWorkTitles)
            {
                stt++;
                model.CompleteWorks.Add(new IpfDetail() {Stt = stt, WorkType = 0, Seq = 0, IsWorkTitle = true, WorkCompleteID = completeWorkTitle.ID, Objective = completeWorkTitle.Name, Weight = 0 });
                model.CompleteWorksNextYear.Add(new IpfDetail() {Stt = stt, WorkType = 0, Seq = 0, IsWorkTitle = true, WorkCompleteID = completeWorkTitle.ID, Objective = completeWorkTitle.Name, Weight = 0 });
            }

            var competencys = new List<IpfDetail>(BtcData.IpfCompetencyDefault());
            model.Competencies = new List<IpfDetail>(competencys);
            model.CompetenciesNextYear = new List<IpfDetail>(competencys);

            // Personal Plan Tab
            model.PersonalPlanCompetencies = new List<PersonalPlan>();
            model.PersonalPlanCareers = new List<PersonalPlan>(BtcData.PersonalCareerDefault());

            //BOD duyet
            if (model.UserInfo.AdministratorshipID != null)
            {
                model.BODApproves = new SelectList(userService.GetListManageInfo((int)model.UserInfo.AdministratorshipID),
                    "ID", "FullName");
            }

            return model;
        }

        // GET: Ipf/Edit
        [HttpGet]
        [Route("PendingEdit")]
        public ActionResult Edit(int id)
        {
            ViewBag.CurrentUser = CurrentUser;
            IpfViewModel model = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            if (model == null)
            {
                model = GetIpfById(id);
            }

            model.FirstLoad = false;
            Session[string.Format("ipf-{0}", CurrentUser.UserId)] = model;
            return View(model);
        }

        // POST: Ipf/Edit
        [HttpPost]
        [Route("PendingEdit")]
        public ActionResult Edit(IpfViewModel model, string submitButton)
        {
            ViewBag.CurrentUser = CurrentUser;
            var currentModel = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            bool isValid = true;
            currentModel.ErrorMessage = "";
            string updateMsg = "";

            if (ModelState.IsValid)
            {
                //Kiểm tra lại tổng tỷ trọng
                //Công việc
                if (currentModel.CompleteWorkWeightToal != 100)
                {
                    isValid = false;
                    currentModel.ErrorMessage += (string.IsNullOrEmpty(currentModel.ErrorMessage) ? "" : "<br />") +
                                                 "Tổng tỷ trọng mục tiêu hoàn thành công việc trong năm/kỳ chưa bằng 100%.";
                }
                //Năng lực
                byte competencyWeightTotal = 0;
                foreach (var item in model.Competencies)
                {
                    competencyWeightTotal += item.Weight ?? (byte)0;
                }
                if (competencyWeightTotal != 100)
                {
                    isValid = false;
                    currentModel.ErrorMessage += (string.IsNullOrEmpty(currentModel.ErrorMessage) ? "" : " - ") +
                                                 "Tổng tỷ trọng mục tiêu năng lực chưa bằng 100%.";
                }

                // Không hợp lệ thông báo lỗi
                if (!isValid)
                    return View(currentModel);
                else
                {
                    //Mapping data
                    List<IpfDetail> details = new List<IpfDetail>();
                    List<PersonalPlan> personalPlans = new List<PersonalPlan>();
                    foreach (var item in currentModel.CompleteWorks.Where(t => t.IsWorkTitle == false))
                    {
                        item.IsNextYear = 0;
                        item.WorkType = 0;
                        details.Add(item);
                    }
                    foreach (var item in model.Competencies)
                    {
                        item.IsNextYear = 0;
                        item.WorkType = 1;
                        item.WorkCompleteID = 0;
                        details.Add(item);
                    }

                    // Nếu là KPIs năm thì có thêm Kế hoạch phát triển bản thân và Mục tiêu năm tiếp theo
                    if (currentModel.Ipf.ScheduleType == 0)
                    {
                        foreach (var item in currentModel.PersonalPlanCompetencies)
                        {
                            item.Type = 0;
                            personalPlans.Add(item);
                        }
                        foreach (var item in model.PersonalPlanCareers)
                        {
                            item.Type = 1;
                            personalPlans.Add(item);
                        }

                        foreach (var item in currentModel.CompleteWorksNextYear.Where(t => t.IsWorkTitle == false))
                        {
                            item.IsNextYear = 1;
                            item.WorkType = 0;
                            details.Add(item);
                        }
                        foreach (var item in model.CompetenciesNextYear)
                        {
                            item.IsNextYear = 1;
                            item.WorkType = 1;
                            item.WorkCompleteID = 0;
                            details.Add(item);
                        }
                    }

                    //Trạng thái Lưu nháp or Hoàn thành
                    currentModel.Ipf.Status = submitButton == "OK" ? 1 : 0;
                    currentModel.Ipf.SelfComment = model.Ipf.SelfComment;
                    if (ipfService.UpdateIpf(CurrentUser.UserId, currentModel.Ipf, details, personalPlans,
                        ref updateMsg))
                    {
                        if (currentModel.Ipf.Status == 1)
                        {
                            string sendMailError = SendMailCreate(currentModel);
                        }

                        return RedirectToAction("Pending", "Ipf");
                    }
                    else
                    {
                        currentModel.ErrorMessage = updateMsg;
                        currentModel.FirstLoad = false;
                        return View(currentModel);
                    }
                }
            }
            else
            {
                currentModel.FirstLoad = false;
                currentModel.ErrorMessage = string.Join("<br />", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));
                return View(currentModel);
            }

        }

        public IpfViewModel GetIpfById(int id)
        {
            IpfViewModel model = new IpfViewModel();
            model.FirstLoad = true;
            model.ErrorMesages = new List<string>();
            model.ErrorMessage = "";

            //Init properties
            model.Ipf = new Ipf();
            model.CompleteWorks = new List<IpfDetail>();
            model.Competencies = new List<IpfDetail>();
            model.PersonalPlanCompetencies = new List<PersonalPlan>();
            model.PersonalPlanCareers = new List<PersonalPlan>();
            model.CompleteWorksNextYear = new List<IpfDetail>();
            model.CompetenciesNextYear = new List<IpfDetail>();

            //Loại
            var scheduleTypes = new List<ConvertEnum>();
            foreach (var schedule in Enum.GetValues(typeof(IpfScheduleType)))
                scheduleTypes.Add(new ConvertEnum
                {
                    Value = (int)schedule,
                    Text = schedule.ToString() == "Year" ? "Năm" : "Định kỳ"
                });
            model.ScheduleTypes = new SelectList(scheduleTypes, "Value", "Text");

            //Năm
            model.Years = Years;

            //Active tab
            model.CurrentTab = "liOnYear";

            List<IpfDetail> ipfDetails = new List<IpfDetail>();
            List<PersonalPlan> personalPlans = new List<PersonalPlan>();
            List<IpfComment> comments = new ListStack<IpfComment>();

            //Ipf
            model.Ipf = ipfService.GetIpfInfo(id, ref ipfDetails, ref personalPlans, ref comments);

            // UserInfo
            model.UserInfo = userService.GetUserFullInfo((int)model.Ipf.CreatedBy);
            model.ManagerInfo = userService.GetManagerByUserId((int)model.Ipf.CreatedBy);

            //Kỳ
            model.IpfSchedules = new SelectList(ipfService.IpfSchedulesByDeparment((int)model.UserInfo.DepartmentID), "ID", "Name");

            //Nhóm công việc
            var completeWorkTitles = ipfService.CompleteWorkTitleByDepartment((int)model.UserInfo.DepartmentID);
            model.CompleteWorkTitles = new SelectList(completeWorkTitles, "ID", "Name");

            //CompleteWork
            int stt = 0;
            List<IpfDetail> completeWorks = new List<IpfDetail>(ipfDetails.Where(t => t.IsNextYear == 0 & t.WorkType == 0));
            foreach (var completeWorkTitle in completeWorkTitles)
            {
                stt++;
                completeWorks.Add(new IpfDetail() { Stt = stt, WorkType = 0, Seq = 0, IsWorkTitle = true, WorkCompleteID = completeWorkTitle.ID, Objective = completeWorkTitle.Name, Weight = 0 });

            }
            model.CompleteWorks = new List<IpfDetail>(completeWorks.OrderBy(t => t.WorkCompleteID).ThenBy(t => t.Seq));
            //Weight Total
            model.CompleteWorkWeightToal = completeWorks.Sum(t => (int)t.Weight);

            //Competency
            model.Competencies = new List<IpfDetail>(ipfDetails.Where(t => t.IsNextYear == 0 & t.WorkType == 1).OrderBy(t => t.Seq));

            //Next year
            if (model.Ipf.ScheduleType == 0)
            {
                // Personal Plan Tab
                model.PersonalPlanCompetencies = new List<PersonalPlan>(personalPlans.Where(t => t.Type == 0).OrderBy(t => t.Seq)); personalPlans.Where(t => t.Type == 0).OrderBy(t => t.Seq);
                model.PersonalPlanCareers = new List<PersonalPlan>(personalPlans.Where(t => t.Type == 1).OrderBy(t => t.Seq)); personalPlans.Where(t => t.Type == 0).OrderBy(t => t.Seq);

                //CompleteWork
                stt = 0;
                List<IpfDetail> completeWorksNextYear = new List<IpfDetail>(ipfDetails.Where(t => t.IsNextYear == 1 & t.WorkType == 0));
                foreach (var completeWorkTitle in completeWorkTitles)
                {
                    stt++;
                    completeWorksNextYear.Add(new IpfDetail() { Stt = stt, WorkType = 0, Seq = 0, IsWorkTitle = true, WorkCompleteID = completeWorkTitle.ID, Objective = completeWorkTitle.Name, Weight = 0 });

                }
                model.CompleteWorksNextYear = new List<IpfDetail>(completeWorksNextYear.OrderBy(t => t.WorkCompleteID).ThenBy(t => t.Seq));
                //Weight Total
                model.CompleteWorkNextWeightToal = completeWorksNextYear.Sum(t => (int)t.Weight);

                //Competency
                model.CompetenciesNextYear = new List<IpfDetail>(ipfDetails.Where(t => t.IsNextYear == 1 & t.WorkType == 1).OrderBy(t => t.Seq));
            }

            //Comment
            model.IpfComments = new List<IpfComment>(comments.OrderBy(t => t.Created));

            return model;
        }

        // GET: Ipf/Approve
        [HttpGet]
        [Route("PendingApprove")]
        public ActionResult Approve(int id, int isBod = 0)
        {
            ViewBag.CurrentUser = CurrentUser;
            IpfViewModel model = GetIpfById(id);
            return View(model);
        }

        // POST: Ipf/Approve
        [HttpPost]
        [Route("PendingApprove")]
        public ActionResult Approve(IpfViewModel model, string submitButton)
        {
            ViewBag.CurrentUser = CurrentUser;
            model.ErrorMessage = "";
            string updateMsg = "";
            bool isApprove = submitButton == "Cancel" ? false : true;
            if (!isApprove & string.IsNullOrEmpty(model.Comment))
            {
                model = GetIpfById(model.Ipf.ID);
                model.ErrorMessage = "Phải nhập lý do không duyệt";
                return View(model);
            }

            //Mapping data
            List<IpfDetail> details = new List<IpfDetail>();
            List<PersonalPlan> personalPlans = new List<PersonalPlan>();
            foreach (var item in model.CompleteWorks.Where(t => t.IsWorkTitle == false))
            {
                item.IsNextYear = 0;
                item.WorkType = 0;
                details.Add(item);
            }
            foreach (var item in model.Competencies)
            {
                item.IsNextYear = 0;
                item.WorkType = 1;
                item.WorkCompleteID = 0;
                details.Add(item);
            }

            // Nếu là KPIs năm thì có thêm Kế hoạch phát triển bản thân và Mục tiêu năm tiếp theo
            if (model.Ipf.ScheduleType == 0)
            {
                foreach (var item in model.PersonalPlanCompetencies)
                {
                    item.Type = 0;
                    personalPlans.Add(item);
                }
                foreach (var item in model.PersonalPlanCareers)
                {
                    item.Type = 1;
                    personalPlans.Add(item);
                }

                foreach (var item in model.CompleteWorksNextYear.Where(t => t.IsWorkTitle == false))
                {
                    item.IsNextYear = 1;
                    item.WorkType = 0;
                    details.Add(item);
                }
                foreach (var item in model.CompetenciesNextYear)
                {
                    item.IsNextYear = 1;
                    item.WorkType = 1;
                    item.WorkCompleteID = 0;
                    details.Add(item);
                }
            }

            if (ipfService.ApproveIpf(CurrentUser.UserId, (int) model.UserInfo.AdministratorshipID, model.Ipf.ID, isApprove, model.Ipf.ManagerComment, model.Comment, details, personalPlans,
                ref updateMsg))
            {
                if (isApprove)
                {
                    model.Ipf = ipfService.GetById(model.Ipf.ID);
                    string sendMailError = SendMailApprove(model);
                }

                return RedirectToAction("Pending", "Ipf");
            }
            else
            {
                model = GetIpfById(model.Ipf.ID);
                model.ErrorMessage = updateMsg;
                return View(model);
            }

        }

        // GET: Ipf/BodApprove
        [HttpGet]
        [Route("PendingBodApprove")]
        public ActionResult BodApprove(int id, int isBod = 0)
        {
            ViewBag.CurrentUser = CurrentUser;
            IpfViewModel model = GetIpfById(id);
            return View(model);
        }

        // POST: Ipf/Approve
        [HttpPost]
        [Route("PendingBodApprove")]
        public ActionResult BodApprove(IpfViewModel model, string submitButton)
        {
            ViewBag.CurrentUser = CurrentUser;
            model.ErrorMessage = "";
            string updateMsg = "";
            bool isApprove = submitButton == "Cancel" ? false : true;
            if (!isApprove & string.IsNullOrEmpty(model.Comment))
            {
                model = GetIpfById(model.Ipf.ID);
                model.ErrorMessage = "Phải nhập lý do không duyệt";
                return View(model);
            }

            //Mapping data
            List<IpfDetail> details = new List<IpfDetail>();
            List<PersonalPlan> personalPlans = new List<PersonalPlan>();
            foreach (var item in model.CompleteWorks.Where(t => t.IsWorkTitle == false))
            {
                item.IsNextYear = 0;
                item.WorkType = 0;
                details.Add(item);
            }
            foreach (var item in model.Competencies)
            {
                item.IsNextYear = 0;
                item.WorkType = 1;
                item.WorkCompleteID = 0;
                details.Add(item);
            }

            // Nếu là KPIs năm thì có thêm Kế hoạch phát triển bản thân và Mục tiêu năm tiếp theo
            if (model.Ipf.ScheduleType == 0)
            {
                foreach (var item in model.PersonalPlanCompetencies)
                {
                    item.Type = 0;
                    personalPlans.Add(item);
                }
                foreach (var item in model.PersonalPlanCareers)
                {
                    item.Type = 1;
                    personalPlans.Add(item);
                }

                foreach (var item in model.CompleteWorksNextYear.Where(t => t.IsWorkTitle == false))
                {
                    item.IsNextYear = 1;
                    item.WorkType = 0;
                    details.Add(item);
                }
                foreach (var item in model.CompetenciesNextYear)
                {
                    item.IsNextYear = 1;
                    item.WorkType = 1;
                    item.WorkCompleteID = 0;
                    details.Add(item);
                }
            }

            if (ipfService.BodApproveIpf(CurrentUser.UserId, (int)model.UserInfo.AdministratorshipID, model.Ipf.ID, isApprove, model.Ipf.BodComment, model.Comment, details, personalPlans,
                ref updateMsg))
            {
                if (isApprove)
                {
                    model.Ipf = ipfService.GetById(model.Ipf.ID);
                }

                return RedirectToAction("Pending", "Ipf");
            }
            else
            {
                model = GetIpfById(model.Ipf.ID);
                model.ErrorMessage = updateMsg;
                return View(model);
            }

        }

        // GET: Ipf/View
        [HttpGet]
        [Route("PendingView")]
        public ActionResult View(int id)
        {
            ViewBag.CurrentUser = CurrentUser;
            IpfViewModel model = GetIpfById(id);

            return View(model);
        }

        // GET: Ipf/Delete
        [HttpGet]
        [Route("PendingDelete")]
        public ActionResult Delete(int id)
        {
            ViewBag.CurrentUser = CurrentUser;
            IpfViewModel model = GetIpfById(id);
            return View(model);
        }

        // POST: Ipf/Delete
        [HttpPost]
        [Route("PendingDelete")]
        public ActionResult Delete(IpfViewModel model)
        {
            ViewBag.CurrentUser = CurrentUser;
            bool isValid = true;
            string updateMsg = "";
            if (string.IsNullOrEmpty(model.Comment))
            {
                model = GetIpfById(model.Ipf.ID);
                model.ErrorMessage = "Vui lòng nhập lý do xóa";
                return View(model);
            }

            if (ipfService.DeleteIpf(CurrentUser.UserId, model.Ipf.ID, model.Comment,
                ref updateMsg))
            {
                return RedirectToAction("Pending", "Ipf");
            }
            else
            {
                model = GetIpfById(model.Ipf.ID);
                model.ErrorMessage = updateMsg;
                return View(model);
            }

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SavePersonalPlanCareer")]
        public JsonResult SavePersonalPlanCareer(IEnumerable<PersonalPlanModel> careers)
        {
            IpfViewModel currentModel = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            foreach (var personalPlanCareer in currentModel.PersonalPlanCareers)
            {
                var postItem = careers.Where(t => t.Seq == personalPlanCareer.Seq).FirstOrDefault();
                if (postItem != null)
                {
                    personalPlanCareer.WishesOfStaff = postItem.WishesOfStaff;
                    personalPlanCareer.RequestOfManager = postItem.RequestOfManager;
                }
            }

            Session[string.Format("ipf-{0}", CurrentUser.UserId)] = currentModel;
            return Json(new PersonalPlan(), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult SaveCompetency(IEnumerable<IpfDetail> competencies)
        {
            IpfViewModel currentModel = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            byte competencyWeightToal = 0;
            if (competencies.Any())
            {
                if (competencies.FirstOrDefault().IsNextYear == 0)
                {
                    foreach (var competency in currentModel.Competencies)
                    {
                        var postItem = competencies.Where(t => t.Seq == competency.Seq).FirstOrDefault();
                        if (postItem != null)
                        {
                            competency.Weight = postItem.Weight;
                            competency.SelfScore = postItem.SelfScore;
                            competency.ManagerScore = postItem.ManagerScore;
                        }

                        competencyWeightToal += competency.Weight ?? (byte)0;
                    }

                    currentModel.CompetencyWeightToal = competencyWeightToal;
                }
                else
                {
                    foreach (var competency in currentModel.CompetenciesNextYear)
                    {
                        var postItem = competencies.Where(t => t.Seq == competency.Seq).FirstOrDefault();
                        if (postItem != null)
                        {
                            competency.Weight = postItem.Weight;
                            competency.SelfScore = postItem.SelfScore;
                            competency.ManagerScore = postItem.ManagerScore;
                        }
                        competencyWeightToal += competency.Weight ?? (byte)0;
                    }
                    currentModel.CompetencyNextWeightToal = competencyWeightToal;
                }
            }
            

            Session[string.Format("ipf-{0}", CurrentUser.UserId)] = currentModel;
            var ipfDetail = new IpfDetail();
            ipfDetail.Weight = competencyWeightToal;
            return Json(ipfDetail, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult AddCompleteWork(IpfDetail model)
        {
            var currentModel = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            currentModel.FirstLoad = false;
            currentModel.ErrorMessage = "";

            model.WorkType = 0;
            model.IsWorkTitle = false;

            IpfDetail lastItem = new IpfDetail();
            List<IpfDetail> newCompleteWorks = new List<IpfDetail>();
            int seq = 0;

            if (currentModel.CurrentTab == "liOnYear")
            {
                //Seq
                lastItem = currentModel.CompleteWorks.Where(t => t.WorkType == 0 & t.WorkCompleteID == model.WorkCompleteID).OrderByDescending(p => p.Seq).FirstOrDefault();
                if (lastItem == null || lastItem.Seq == null)
                {
                    model.Seq = 1;
                }
                else
                {
                    model.Seq = (byte?)((int)lastItem.Seq + 1);
                }

                newCompleteWorks = new List<IpfDetail>(currentModel.CompleteWorks);
                newCompleteWorks.Add(model);

                //Weight Total
                currentModel.CompleteWorkWeightToal = newCompleteWorks.Sum(t => (int)t.Weight);

                currentModel.CompleteWorks = newCompleteWorks.OrderBy(t => t.WorkCompleteID).ThenBy(t => t.Seq).ToList();
                //Re index SEQ
                for (int i = 0; i < currentModel.CompleteWorks.Count; i++)
                {
                    if (!currentModel.CompleteWorks[i].IsWorkTitle)
                    {
                        seq++;
                        currentModel.CompleteWorks[i].Seq = (byte?)seq;
                    }
                }
            }
            else
            {
                //Seq
                lastItem = currentModel.CompleteWorksNextYear.Where(t => t.WorkType == 0 & t.WorkCompleteID == model.WorkCompleteID).OrderByDescending(p => p.Seq).FirstOrDefault();
                if (lastItem == null || lastItem.Seq == null)
                {
                    model.Seq = 1;
                }
                else
                {
                    model.Seq = (byte?)((int)lastItem.Seq + 1);
                }

                newCompleteWorks = new List<IpfDetail>(currentModel.CompleteWorksNextYear);
                newCompleteWorks.Add(model);

                //Weight Total
                currentModel.CompleteWorkNextWeightToal = newCompleteWorks.Sum(t => (int)t.Weight);

                currentModel.CompleteWorksNextYear = newCompleteWorks.OrderBy(t => t.WorkCompleteID).ThenBy(t => t.Seq).ToList();
                //Re index SEQ
                for (int i = 0; i < currentModel.CompleteWorksNextYear.Count; i++)
                {
                    if (!currentModel.CompleteWorksNextYear[i].IsWorkTitle)
                    {
                        seq++;
                        currentModel.CompleteWorksNextYear[i].Seq = (byte?)seq;
                    }
                }
            }


            Session[string.Format("ipf-{0}", CurrentUser.UserId)] = currentModel;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ViewCompleteWork")]
        public JsonResult ViewCompleteWork(int seq)
        {
            IpfViewModel currentModel = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            IpfDetail viewItem = new IpfDetail();
            //Create or Update
            if (currentModel != null)
            {
                if (currentModel.CurrentTab == "liOnYear")
                {
                    viewItem = currentModel.CompleteWorks.Where(t => t.Seq == seq).FirstOrDefault();
                }
                else
                {
                    viewItem = currentModel.CompleteWorksNextYear.Where(t => t.Seq == seq).FirstOrDefault();
                }
            }
            else
            {
                viewItem = ipfService.GetIpfDetailById(seq);
            }
            return Json(viewItem, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("UpdateCompleteWork")]
        public JsonResult UpdateCompleteWork(int seq)
        {
            IpfViewModel currentModel = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            IpfDetail updateItem;
            if (currentModel.CurrentTab == "liOnYear")
            {
                updateItem = currentModel.CompleteWorks.Where(t => t.Seq == seq).FirstOrDefault();
            }
            else
            {
                updateItem = currentModel.CompleteWorksNextYear.Where(t => t.Seq == seq).FirstOrDefault();
            }

            return Json(updateItem, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("UpdateCompleteWorkPost")]
        public JsonResult UpdateCompleteWorkPost(IpfDetail model)
        {
            var currentModel = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            currentModel.FirstLoad = false;
            currentModel.ErrorMessage = "";

            model.WorkType = 0;
            model.IsWorkTitle = false;

            int seq = 0;
            List < IpfDetail > newCompleteWorks = new List<IpfDetail>();
            if (currentModel.CurrentTab == "liOnYear")
            {
                for (int i = 0; i < currentModel.CompleteWorks.Count; i++)
                {
                    if (currentModel.CompleteWorks[i].Seq == model.Seq)
                    {
                        currentModel.CompleteWorks[i] = model;
                        break;
                    }
                }

                //Weight Total
                currentModel.CompleteWorkWeightToal = currentModel.CompleteWorks.Sum(t => (int)t.Weight);
                
                //Re index SEQ
                for (int i = 0; i < currentModel.CompleteWorks.Count; i++)
                {
                    if (!currentModel.CompleteWorks[i].IsWorkTitle)
                    {
                        seq++;
                        currentModel.CompleteWorks[i].Seq = (byte?)seq;
                    }
                }
                newCompleteWorks = new List<IpfDetail>(currentModel.CompleteWorks);
                currentModel.CompleteWorks = newCompleteWorks.OrderBy(t => t.WorkCompleteID).ThenBy(t => t.Seq).ToList();

            }
            else
            {
                for (int i = 0; i < currentModel.CompleteWorksNextYear.Count; i++)
                {
                    if (currentModel.CompleteWorksNextYear[i].Seq == model.Seq)
                    {
                        currentModel.CompleteWorksNextYear[i] = model;
                        break;
                    }
                }
                //Re index SEQ
                for (int i = 0; i < currentModel.CompleteWorksNextYear.Count; i++)
                {
                    if (!currentModel.CompleteWorksNextYear[i].IsWorkTitle)
                    {
                        seq++;
                        currentModel.CompleteWorksNextYear[i].Seq = (byte?)seq;
                    }
                }
                newCompleteWorks = new List<IpfDetail>(currentModel.CompleteWorksNextYear);
                currentModel.CompleteWorksNextYear = newCompleteWorks.OrderBy(t => t.WorkCompleteID).ThenBy(t => t.Seq).ToList();

                //Weight Total
                currentModel.CompleteWorkNextWeightToal = currentModel.CompleteWorksNextYear.Sum(t => (int)t.Weight);
            }

            Session[string.Format("ipf-{0}", CurrentUser.UserId)] = currentModel;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("DeleteCompleteWork")]
        public JsonResult DeleteCompleteWork(int seq)
        {
            IpfViewModel currentModel = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            IpfDetail deleteItem;
            if (currentModel.CurrentTab == "liOnYear")
            {
                deleteItem = currentModel.CompleteWorks.Where(t => t.Seq == seq).FirstOrDefault();
            }
            else
            {
                deleteItem = currentModel.CompleteWorksNextYear.Where(t => t.Seq == seq).FirstOrDefault();
            }

            return Json(deleteItem, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("DeleteCompleteWorkPost")]
        public JsonResult DeleteCompleteWorkPost(IpfDetail model)
        {
            IpfViewModel currentModel = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            currentModel.ErrorMessage = "";
            IpfDetail deleteItem;
            if (currentModel.CurrentTab == "liOnYear")
            {
                deleteItem = currentModel.CompleteWorks.Where(t => t.Seq == model.Seq).FirstOrDefault();
                currentModel.CompleteWorks.Remove(deleteItem);

                //Weight Total
                currentModel.CompleteWorkWeightToal = currentModel.CompleteWorks.Sum(t => (int)t.Weight);

                //Re index SEQ
                int seq = 0;
                for (int i = 0; i < currentModel.CompleteWorks.Count; i++)
                {
                    if (!currentModel.CompleteWorks[i].IsWorkTitle)
                    {
                        seq++;
                        currentModel.CompleteWorks[i].Seq = (byte?)seq;
                    }
                }
            }
            else
            {
                deleteItem = currentModel.CompleteWorksNextYear.Where(t => t.Seq == model.Seq).FirstOrDefault();
                currentModel.CompleteWorksNextYear.Remove(deleteItem);

                //Weight Total
                currentModel.CompleteWorkNextWeightToal = currentModel.CompleteWorks.Sum(t => (int)t.Weight);

                //Re index SEQ
                int seq = 0;
                for (int i = 0; i < currentModel.CompleteWorksNextYear.Count; i++)
                {
                    if (!currentModel.CompleteWorksNextYear[i].IsWorkTitle)
                    {
                        seq++;
                        currentModel.CompleteWorksNextYear[i].Seq = (byte?)seq;
                    }
                }
            }


            Session[string.Format("ipf-{0}", CurrentUser.UserId)] = currentModel;

            return Json(deleteItem, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AddPersonalPlanCompetency")]
        public JsonResult AddPersonalPlanCompetency(PersonalPlan model)
        {
            IpfViewModel currentModel = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            currentModel.FirstLoad = false;
            model.Type = 0;
            //Seq
            var lastItem = currentModel.PersonalPlanCompetencies.Where(t => t.Type == 0).OrderByDescending(p => p.Seq).FirstOrDefault();
            if (lastItem == null || lastItem.Seq == null)
            {
                model.Seq = 1;
            }
            else
            {
                model.Seq = (byte?)((int)lastItem.Seq + 1);
            }
            currentModel.PersonalPlanCompetencies.Add(model);
            Session[string.Format("ipf-{0}", CurrentUser.UserId)] = currentModel;
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("UpdatePersonalPlanCompetency")]
        public JsonResult UpdatePersonalPlanCompetency(int seq)
        {
            IpfViewModel model = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            var updateItem = model.PersonalPlanCompetencies.Where(t => t.Seq == seq).FirstOrDefault();
            return Json(updateItem, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("UpdatePersonalPlanCompetencyPost")]
        public JsonResult UpdatePersonalPlanCompetencyPost(PersonalPlan model)
        {
            var currentModel = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            currentModel.FirstLoad = false;

            model.Type = 0;

            for (int i = 0; i < currentModel.PersonalPlanCompetencies.Count; i++)
            {
                if (currentModel.PersonalPlanCompetencies[i].Seq == model.Seq)
                {
                    currentModel.PersonalPlanCompetencies[i] = model;
                    break;
                }
            }

            Session[string.Format("ipf-{0}", CurrentUser.UserId)] = currentModel;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("DeletePersonalPlanCompetency")]
        public JsonResult DeletePersonalPlanCompetency(int seq)
        {
            IpfViewModel model = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            var deleteItem = model.PersonalPlanCompetencies.Where(t => t.Seq == seq).FirstOrDefault();
            return Json(deleteItem, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("DeletePersonalPlanCompetencyPost")]
        public JsonResult DeletePersonalPlanCompetencyPost(int seq)
        {
            IpfViewModel currentModel = Session[string.Format("ipf-{0}", CurrentUser.UserId)] as IpfViewModel;
            var deleteItem = currentModel.PersonalPlanCompetencies.Where(t => t.Seq == (byte?)seq).FirstOrDefault();

            currentModel.PersonalPlanCompetencies.Remove(deleteItem);

            //Re index SEQ
            int newSeq = 0;
            for (int i = 0; i < currentModel.PersonalPlanCompetencies.Count; i++)
            {
                newSeq++;
                currentModel.PersonalPlanCompetencies[i].Seq = (byte?)newSeq;
            }
            return Json(deleteItem, JsonRequestBehavior.AllowGet);
        }
        #endregion CRUD Pending

        #region CRUD Done
        // GET: Ipf/Edit
        [HttpGet]
        [Route("DoneEdit")]
        public ActionResult DoneEdit(int id)
        {
            IpfViewModel model = GetIpfDoneById(id);
            model.Action = "Edit";
            return View(model);
        }

        // POST: Ipf/Edit
        [HttpPost]
        [Route("DoneEdit")]
        public ActionResult DoneEdit(IpfViewModel model)
        {
            string updateMsg = "";
            bool isValid = true;

            if (ModelState.IsValid)
            {
                //Kiểm tra 

                // Không hợp lệ thông báo lỗi
                if (!isValid)
                    return View(model);
                else
                {
                    //Mapping data
                    List<IpfDetail> details = new List<IpfDetail>();
                    List<PersonalPlan> personalPlans = new List<PersonalPlan>();
                    foreach (var item in model.CompleteWorks.Where(t => t.ID != 0))
                    {
                        item.IsNextYear = 0;
                        item.WorkType = 0;
                        details.Add(item);
                    }
                    foreach (var item in model.Competencies)
                    {
                        item.IsNextYear = 0;
                        item.WorkType = 1;
                        item.WorkCompleteID = 0;
                        details.Add(item);
                    }

                    if (ipfService.AssessmentIpf(model.Action, CurrentUser.UserId, (int)model.UserInfo.AdministratorshipID, model.Ipf, details, ref updateMsg))
                    {
                        return RedirectToAction("Done", "Ipf");
                    }
                    else
                    {
                        model.ErrorMessage = updateMsg;
                        return View(model);
                    }
                }
            }
            else
            {
                //model = GetIpfDoneById(model.Ipf.ID);
                model.ErrorMessage = string.Join("<br />", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));
                return View(model);
            }

        }

        public IpfViewModel GetIpfDoneById(int id)
        {
            IpfViewModel model = new IpfViewModel();
            model.FirstLoad = true;
            model.ErrorMesages = new List<string>();
            model.ErrorMessage = "";

            //Init properties
            model.Ipf = new Ipf();
            model.CompleteWorks = new List<IpfDetail>();
            model.Competencies = new List<IpfDetail>();
            model.PersonalPlanCompetencies = new List<PersonalPlan>();
            model.PersonalPlanCareers = new List<PersonalPlan>();
            model.CompleteWorksNextYear = new List<IpfDetail>();
            model.CompetenciesNextYear = new List<IpfDetail>();

            //Loại
            var scheduleTypes = new List<ConvertEnum>();
            foreach (var schedule in Enum.GetValues(typeof(IpfScheduleType)))
                scheduleTypes.Add(new ConvertEnum
                {
                    Value = (int)schedule,
                    Text = schedule.ToString() == "Year" ? "Năm" : "Định kỳ"
                });
            model.ScheduleTypes = new SelectList(scheduleTypes, "Value", "Text");

            //Năm
            model.Years = Years;

            //Active tab
            model.CurrentTab = "liOnYear";

            List<IpfDetail> ipfDetails = new List<IpfDetail>();
            List<PersonalPlan> personalPlans = new List<PersonalPlan>();
            List<IpfComment> comments = new ListStack<IpfComment>();

            //Ipf
            model.Ipf = ipfService.GetIpfInfo(id, ref ipfDetails, ref personalPlans, ref comments);

            // UserInfo
            model.UserInfo = userService.GetUserFullInfo((int)model.Ipf.CreatedBy);
            model.ManagerInfo = userService.GetManagerByUserId((int)model.Ipf.CreatedBy);

            //Kỳ
            model.IpfSchedules = new SelectList(ipfService.IpfSchedulesByDeparment((int)model.UserInfo.DepartmentID), "ID", "Name");

            //Nhóm công việc
            var completeWorkTitles = ipfService.CompleteWorkTitleByDepartment((int)model.UserInfo.DepartmentID);
            model.CompleteWorkTitles = new SelectList(completeWorkTitles, "ID", "Name");

            //CompleteWork
            int stt = 0;
            List<IpfDetail> completeWorks = new List<IpfDetail>(ipfDetails.Where(t => t.IsNextYear == 0 & t.WorkType == 0));
            foreach (var completeWorkTitle in completeWorkTitles)
            {
                stt++;
                completeWorks.Add(new IpfDetail() { Stt = stt, WorkType = 0, Seq = 0, IsWorkTitle = true, WorkCompleteID = completeWorkTitle.ID, Objective = completeWorkTitle.Name, Weight = 0 });

            }
            model.CompleteWorks = new List<IpfDetail>(completeWorks.OrderBy(t => t.WorkCompleteID).ThenBy(t => t.Seq));
            //Weight Total
            model.CompleteWorkWeightToal = completeWorks.Sum(t => (int)t.Weight);

            //Competency
            model.Competencies = new List<IpfDetail>(ipfDetails.Where(t => t.IsNextYear == 0 & t.WorkType == 1).OrderBy(t => t.Seq));

            //Next year
            if (model.Ipf.ScheduleType == 0)
            {
                // Personal Plan Tab
                model.PersonalPlanCompetencies = new List<PersonalPlan>(personalPlans.Where(t => t.Type == 0).OrderBy(t => t.Seq)); personalPlans.Where(t => t.Type == 0).OrderBy(t => t.Seq);
                model.PersonalPlanCareers = new List<PersonalPlan>(personalPlans.Where(t => t.Type == 1).OrderBy(t => t.Seq)); personalPlans.Where(t => t.Type == 0).OrderBy(t => t.Seq);

                //CompleteWork
                stt = 0;
                List<IpfDetail> completeWorksNextYear = new List<IpfDetail>(ipfDetails.Where(t => t.IsNextYear == 1 & t.WorkType == 0));
                foreach (var completeWorkTitle in completeWorkTitles)
                {
                    stt++;
                    completeWorksNextYear.Add(new IpfDetail() { Stt = stt, WorkType = 0, Seq = 0, IsWorkTitle = true, WorkCompleteID = completeWorkTitle.ID, Objective = completeWorkTitle.Name, Weight = 0 });

                }
                model.CompleteWorksNextYear = new List<IpfDetail>(completeWorksNextYear.OrderBy(t => t.WorkCompleteID).ThenBy(t => t.Seq));
                //Weight Total
                model.CompleteWorkNextWeightToal = completeWorksNextYear.Sum(t => (int)t.Weight);

                //Competency
                model.CompetenciesNextYear = new List<IpfDetail>(ipfDetails.Where(t => t.IsNextYear == 1 & t.WorkType == 1).OrderBy(t => t.Seq));
            }

            //Comment
            model.IpfComments = new List<IpfComment>(comments.OrderBy(t => t.Created));

            return model;
        }

        // GET: Ipf/Approve
        [HttpGet]
        [Route("DoneApprove")]
        public ActionResult DoneApprove(int id)
        {
            IpfViewModel model = GetIpfDoneById(id);
            model.Action = "Approve";
            return View("DoneEdit", model);
        }

        // POST: Ipf/Approve
        [HttpPost]
        [Route("DoneApprove")]
        public ActionResult DoneApprove(IpfViewModel model, string submitButton)
        {
            string updateMsg = "";
            bool isValid = true;

            if (ModelState.IsValid)
            {
                //Kiểm tra 

                // Không hợp lệ thông báo lỗi
                if (!isValid)
                    return View(model);
                else
                {
                    //Mapping data
                    List<IpfDetail> details = new List<IpfDetail>();
                    List<PersonalPlan> personalPlans = new List<PersonalPlan>();
                    foreach (var item in model.CompleteWorks.Where(t => t.ID != 0))
                    {
                        item.IsNextYear = 0;
                        item.WorkType = 0;
                        details.Add(item);
                    }
                    foreach (var item in model.Competencies)
                    {
                        item.IsNextYear = 0;
                        item.WorkType = 1;
                        item.WorkCompleteID = 0;
                        details.Add(item);
                    }

                    if (ipfService.AssessmentIpf(model.Action, CurrentUser.UserId, (int)model.UserInfo.AdministratorshipID, model.Ipf, details, ref updateMsg))
                    {
                        return RedirectToAction("Done", "Ipf");
                    }
                    else
                    {
                        model.ErrorMessage = updateMsg;
                        return View(model);
                    }
                }
            }
            else
            {
                //model = GetIpfDoneById(model.Ipf.ID);
                model.ErrorMessage = string.Join("<br />", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));
                return View(model);
            }
        }

        // GET: Ipf/View
        [HttpGet]
        [Route("DoneView")]
        public ActionResult DoneView(int id)
        {
            IpfViewModel model = GetIpfDoneById(id);

            return View(model);
        }

        // GET: Ipf/Comment
        [HttpGet]
        [Route("DoneComment")]
        public ActionResult DoneComment(int id)
        {
            IpfViewModel model = GetIpfDoneById(id);
            model.Action = "Comment";
            return View(model);
        }

        // POST: Ipf/Comment
        [HttpPost]
        [Route("DoneComment")]
        public ActionResult DoneComment(IpfViewModel model)
        {
            string updateMsg = "";
            bool isValid = true;

            if (ModelState.IsValid)
            {
                //Kiểm tra 

                // Không hợp lệ thông báo lỗi
                if (!isValid)
                    return View(model);
                else
                {
                    //Set comment to Ipf.ManagerComment
                    model.Ipf.ManagerComment = model.Comment;
                    if (ipfService.AssessmentIpf(model.Action, CurrentUser.UserId, (int)model.UserInfo.AdministratorshipID, model.Ipf, null, ref updateMsg))
                    {
                        return RedirectToAction("Pending", "Ipf");
                    }
                    else
                    {
                        model.ErrorMessage = updateMsg;
                        return View(model);
                    }
                }
            }
            else
            {
                //model = GetIpfDoneById(model.Ipf.ID);
                model.ErrorMessage = string.Join("<br />", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));
                return View(model);
            }

        }
        #endregion CRUD Done

        [AllowAnonymous]
        [HttpGet]
        [Route("IpfKPIsDownload")]
        public ActionResult ExportToExcel(int id)
        {
            IpfViewModel model = GetIpfById(id);

            //Save the workbook to disk in xlsx format
            string dateStr = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy_hhmmss");
            var fileDownloadName = "IPF_" + model.UserInfo.UserName.ToUpper() + "_" + dateStr + ".xlsx";
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileStream = new MemoryStream();
            //
            ExcelPackage excelEngine = FillDataTableExcel(model);
            excelEngine.SaveAs(fileStream);
            fileStream.Position = 0;
            var fsr = new FileStreamResult(fileStream, contentType);
            fsr.FileDownloadName = fileDownloadName;

            return fsr;
        }

        public ExcelPackage FillDataTableExcel(IpfViewModel model)
        {
            ExcelPackage excelEngine = new ExcelPackage();
            ExcelWorksheet worksheet;
            if (model.Ipf.ScheduleType == 1)
            {
                worksheet = excelEngine.Workbook.Worksheets.Add("IPF-M");
            } else
            {
                worksheet = excelEngine.Workbook.Worksheets.Add("IPF-Y");
            }
            //Enter values to the cells from A3 to A5

            worksheet.Cells["A1:G1"].Merge = true;
            worksheet.Cells["A2:G2"].Merge = true;
            worksheet.Cells["A1"].Value = "COMPANY: " + model.UserInfo.CompanyName;
            worksheet.Cells["A2"].Value = "DEPARTMENT: " + model.UserInfo.DepartmentName;
            //Make the text bold
            worksheet.Cells["A1:U11"].Style.Font.Bold = true;
            //Merge cells
            worksheet.Cells["A3"].Value = "QUẢN LÝ VÀ ĐÁNH GIÁ KẾT QUẢ CÔNG VIỆC CÁ NHÂN / INDIVIDUAL PERFROMANCE FORM";
            worksheet.Cells["T4"].Value = "Năm:";
            worksheet.Cells["U4"].Value = model.Ipf.Year.ToString();
            worksheet.Cells["A3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Row(3).Height = 30;
            worksheet.Cells["A3:V3"].Style.WrapText = true;
            worksheet.Cells["A3:V3"].Merge = true;
            worksheet.Cells["U4:V4"].Merge = true;
            // format cells - add borders H4:I4
            worksheet.Cells["T4:V4"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["T4:V4"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["T4:V4"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["T4:V4"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            //Merge cells
            worksheet.Cells["A5:C5"].Merge = true;
            worksheet.Cells["U5:V5"].Merge = true;
            worksheet.Cells["D5:I5"].Merge = true;
            worksheet.Cells["J5:L5"].Merge = true;
            worksheet.Cells["M5:S5"].Merge = true;
            worksheet.Cells["A6:C6"].Merge = true;
            worksheet.Cells["U6:V6"].Merge = true;
            worksheet.Cells["D6:I6"].Merge = true;
            worksheet.Cells["J6:L6"].Merge = true;
            worksheet.Cells["M6:O6"].Merge = true;
            worksheet.Cells["P6:S6"].Merge = true;
            worksheet.Cells["T6:V6"].Merge = true;
            worksheet.Cells["A7:C7"].Merge = true;
            worksheet.Cells["D7:I7"].Merge = true;
            worksheet.Cells["J7:L7"].Merge = true;
            worksheet.Cells["M7:V7"].Merge = true;
            worksheet.Cells["A8:V8"].Merge = true;
            worksheet.Cells["A9:V9"].Merge = true;
            worksheet.Cells["A10:R10"].Merge = true;
            worksheet.Cells["S10:V10"].Merge = true;

            worksheet.Cells["A5"].Value = "Tên nhân viên / Staff name:";
            worksheet.Cells["D5"].Value = model.UserInfo.FullName;
            worksheet.Cells["J5"].Value = "Chức danh / Title:";
            worksheet.Cells["M5"].Value = model.UserInfo.AdministratorshipName;
            worksheet.Cells["T5"].Value = "Bậc / Level:";
            worksheet.Cells["U5"].Value = "";

            worksheet.Cells["A6"].Value = "Phòng / Department:";
            worksheet.Cells["D6"].Value = model.UserInfo.DepartmentName;
            worksheet.Cells["J6"].Value = "Ngày vào Cty / Joining date:";
            worksheet.Cells["M6"].Value = "";
            worksheet.Cells["P6"].Value = "Ngày bắt đầu CV hiện tại / Date starting current job:";
            worksheet.Cells["T6"].Value = "";
            worksheet.Row(5).Height = 22;
            worksheet.Row(6).Height = 32;
            worksheet.Row(7).Height = 22;

            worksheet.Cells["A7"].Value = "Tên người quản lý / Manager's name: ";
            worksheet.Cells["D7"].Value = model.ManagerInfo.FullName;
            worksheet.Cells["J7"].Value = "Chức danh / Title:";
            worksheet.Cells["M7"].Value = model.ManagerInfo.AdministratorshipName;
            worksheet.Cells["A8"].Value = "Thang điểm: 5- Xuất sắc; 4- Tốt; 3-Đạt ; 2- Chưa đạt nhiều yêu cầu; 1-không đạt yêu cầu\r\nScoring: 5-Excellently exceed requirements; 4-Exceed some requirements; 3-Meet requirements; 2-Satisfactory some of requirements; 1-Unsatisfactory";
            worksheet.Cells["A9"].Value = "1 - CAM KẾT MỤC TIÊU CÔNG VIỆC TRONG NĂM " + model.Ipf.Year + " / WORK COMMITMENTS IN " + model.Ipf.Year;
            worksheet.Row(8).Height = 35;
            worksheet.Cells["A8:U8"].Style.WrapText = true;

            //Muc tieu hoan thanh cong viec
            worksheet.Cells["A10"].Value = "A. MỤC TIÊU VỀ HOÀN THÀNH CÔNG VIỆC (80%) / OBJECTIVES OF COMPLETED WORK (80%)";
            worksheet.Cells["S10"].Value = "Điểm (1-5)";
            worksheet.Cells["A3:U8"].Style.WrapText = true;
            worksheet.Cells["D11:I11"].Merge = true;
            worksheet.Cells["K11:R11"].Merge = true;
            worksheet.Cells["A11"].Value = "No.";
            worksheet.Cells["B11"].Value = "KPI/Objective\r\nKPI/Mục tiêu";
            worksheet.Cells["C11"].Value = "No.";
            worksheet.Cells["D11"].Value = "Chỉ tiêu kế hoạch/yêu cầu cụ thể cần phải đạt được\r\nTargets / specific requirements must be achieved";
            worksheet.Cells["J11"].Value = "Tỷ trọng / \r\nWeight";
            worksheet.Cells["K11"].Value = "Kết quả thực hiện / Performance results";
            worksheet.Cells["S11"].Value = "NV tự đánh giá / Self assessment";
            worksheet.Cells["T11"].Value = "Quản lý đánh giá / Manager's assessment";
            worksheet.Cells["U11"].Value = "Tổng điểm hoàn thành / \r\nTotal Score";
            worksheet.Cells["V11"].Value = "BOD đánh giá / \r\nBOD's assessment";
            worksheet.Cells["A9:V11"].Style.Font.Bold = true;
            worksheet.Cells["A11:V11"].Style.WrapText = true;
            worksheet.Cells["A8:U8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["S10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // format cells - add borders A5:I...
            worksheet.Cells["A5:V11"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A5:V11"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A5:V11"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A5:V11"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A11:V11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            int col = 11;
            decimal? completeWorkTotalScore = 0;
            decimal? completeWorkWeightScore = 0;
            for (int i = 0; i < model.CompleteWorks.Count; i++)
            {
                if(model.CompleteWorks[i].WorkType == 0 && (model.CompleteWorks[i].IsNextYear == null || model.CompleteWorks[i].IsNextYear == 0))
                {
                    if(model.CompleteWorks[i].Weight == null || model.CompleteWorks[i].Weight == 0)
                    {
                        col++;
                        worksheet.Cells["A" + col].Value = BtcHelper.ToRoman(model.CompleteWorks[i].Stt);
                        worksheet.Cells["B" + col + ":V" + col].Merge = true;
                        worksheet.Cells["B" + col].Value = model.CompleteWorks[i].Objective;
                        worksheet.Cells["A" + col + ":V" + col].Style.Font.Bold = true;
                    } else {
                        col++;
                        worksheet.Cells["A" + col].Value = model.CompleteWorks[i].Seq;
                        worksheet.Cells["B" + col].Value = model.CompleteWorks[i].Objective;
                        worksheet.Cells["C" + col].Value = model.CompleteWorks[i].Seq;
                        worksheet.Cells["D" + col + ":I" + col].Merge = true;
                        worksheet.Cells["D" + col].Value = model.CompleteWorks[i].Target;
                        worksheet.Cells["J" + col].Value = model.CompleteWorks[i].Weight;
                        worksheet.Cells["K" + col + ":R" + col].Merge = true;
                        worksheet.Cells["K" + col].Value = model.CompleteWorks[i].Result;
                        worksheet.Cells["S" + col].Value = model.CompleteWorks[i].SelfScore;
                        worksheet.Cells["T" + col].Value = model.CompleteWorks[i].ManagerScore;
                        model.CompleteWorks[i].TotalScore = model.CompleteWorks[i].Weight * model.CompleteWorks[i].ManagerScore / 100;
                        worksheet.Cells["U" + col].Value = model.CompleteWorks[i].TotalScore;
                        worksheet.Cells["V" + col].Value = model.CompleteWorks[i].BodScore;
                        worksheet.Cells["A" + col + ":V" + col].Style.WrapText = true;
                        worksheet.Cells["J" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["S" + col + ":V" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["C" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        completeWorkTotalScore = completeWorkTotalScore + model.CompleteWorks[i].TotalScore;
                        completeWorkWeightScore = completeWorkWeightScore + model.CompleteWorks[i].Weight;
                    }

                    // format cells - add borders A...:I...
                    worksheet.Cells["A" + col + ":V" + col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A" + col + ":V" + col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A" + col + ":V" + col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A" + col + ":V" + col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
            }
            col++;
            worksheet.Cells["A" + col + ":I" + col].Merge = true;
            worksheet.Cells["K" + col + ":T" + col].Merge = true;
            worksheet.Cells["A" + col].Value = "Total /Tổng cộng";
            worksheet.Cells["J" + col].Value = completeWorkWeightScore;
            worksheet.Cells["U" + col].Value = completeWorkTotalScore;
            worksheet.Cells["A" + col + ":V" + col].Style.Font.Bold = true;
            // format cells - add borders A...:I...
            worksheet.Cells["A" + col + ":V" + col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["U" + col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["V" + col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + col + ":J" + col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["U" + col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + col + ":V" + col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["J" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["U" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["V" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //bo qua 1 dong
            worksheet.Row(col + 1).Height = 5;

            decimal? completeWorkTotalScore1 = 0;
            decimal? completeWorkWeightScore1 = 0;
            int lastRow = col + 2;
            worksheet.Cells["A" + lastRow + ":R" + lastRow].Merge = true;
            worksheet.Cells["S" + lastRow + ":V" + lastRow].Merge = true;
            //Muc tieu nang luc
            worksheet.Cells["A" + lastRow].Value = "B. MỤC TIÊU VỀ NĂNG LỰC / OBJECTIVES OF COMPETENCY (20%)";
            worksheet.Cells["S" + lastRow].Value = "Điểm (1-5)";
            worksheet.Cells["S" + lastRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            int lastRowB = lastRow + 1;
            worksheet.Cells["D" + lastRowB + ":I" + lastRowB].Merge = true;
            worksheet.Cells["K" + lastRowB + ":R" + lastRowB].Merge = true;
            worksheet.Cells["A" + lastRowB].Value = "No.";
            worksheet.Cells["B" + lastRowB].Value = "Kiến thức/Kỹ năng/Thái độ\r\nKnowledge / Skills / Attitudes";
            worksheet.Cells["C" + lastRowB].Value = "No.";
            worksheet.Cells["D" + lastRowB].Value = "Chỉ tiêu kế hoạch/yêu cầu cụ thể cần phải đạt được\r\nTargets / specific requirements must be achieved";
            worksheet.Cells["J" + lastRowB].Value = "Tỷ trọng / \r\nWeight";
            worksheet.Cells["K" + lastRowB].Value = "Kết quả thực hiện / Performance results";
            worksheet.Cells["S" + lastRowB].Value = "NV tự đánh giá / Self assessment";
            worksheet.Cells["T" + lastRowB].Value = "Quản lý đánh giá / Manager's assessment";
            worksheet.Cells["U" + lastRowB].Value = "Tổng điểm hoàn thành / \r\nTotal Score";
            worksheet.Cells["V" + lastRowB].Value = "BOD đánh giá / \r\nBOD's assessment";
            worksheet.Cells["A" + lastRow + ":V" + lastRowB].Style.Font.Bold = true;
            worksheet.Cells["A" + lastRowB + ":V" + lastRowB].Style.WrapText = true;
            worksheet.Cells["A" + lastRow + ":V" + lastRowB].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow + ":V" + lastRowB].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow + ":V" + lastRowB].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow + ":V" + lastRowB].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRowB + ":V" + lastRowB].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            for (int i = 0; i < model.Competencies.Count; i++)
            {
                if (model.Competencies[i].WorkType == 1 && (model.Competencies[i].IsNextYear == null || model.Competencies[i].IsNextYear == 0))
                {
                    lastRowB++;
                    worksheet.Cells["A" + lastRowB].Value = model.Competencies[i].Seq;
                    worksheet.Cells["B" + lastRowB].Value = model.Competencies[i].Objective;
                    worksheet.Cells["C" + lastRowB].Value = model.Competencies[i].Seq;
                    worksheet.Cells["D" + lastRowB + ":I" + lastRowB].Merge = true;
                    worksheet.Cells["D" + lastRowB].Value = model.Competencies[i].Target;
                    worksheet.Cells["J" + lastRowB].Value = model.Competencies[i].Weight;
                    worksheet.Cells["K" + lastRowB + ":R" + lastRowB].Merge = true;
                    worksheet.Cells["K" + lastRowB].Value = model.Competencies[i].Result;
                    worksheet.Cells["S" + lastRowB].Value = model.Competencies[i].SelfScore;
                    worksheet.Cells["T" + lastRowB].Value = model.Competencies[i].ManagerScore;
                    model.Competencies[i].TotalScore = model.Competencies[i].Weight * model.Competencies[i].ManagerScore / 100;
                    worksheet.Cells["U" + lastRowB].Value = model.Competencies[i].TotalScore;
                    worksheet.Cells["V" + lastRowB].Value = model.Competencies[i].BodScore;
                    worksheet.Cells["A" + lastRowB + ":U" + lastRowB].Style.WrapText = true;

                    completeWorkTotalScore1 = completeWorkTotalScore1 + model.Competencies[i].TotalScore;
                    completeWorkWeightScore1 = completeWorkWeightScore1 + model.Competencies[i].Weight;

                    // format cells - add borders A...:I...
                    worksheet.Cells["A" + lastRowB + ":V" + lastRowB].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A" + lastRowB + ":V" + lastRowB].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A" + lastRowB + ":V" + lastRowB].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A" + lastRowB + ":V" + lastRowB].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A" + lastRowB].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["C" + lastRowB].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["J" + lastRowB].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["S" + lastRowB + ":U" + lastRowB].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
            }
            lastRowB++;
            worksheet.Cells["A" + lastRowB + ":I" + lastRowB].Merge = true;
            worksheet.Cells["K" + lastRowB + ":T" + lastRowB].Merge = true;
            worksheet.Cells["A" + lastRowB].Value = "Total /Tổng cộng";
            worksheet.Cells["J" + lastRowB].Value = completeWorkWeightScore1;
            worksheet.Cells["U" + lastRowB].Value = completeWorkTotalScore1;
            worksheet.Cells["A" + lastRowB + ":V" + lastRowB].Style.Font.Bold = true;
            // format cells - add borders A...:I...
            worksheet.Cells["A" + lastRowB + ":V" + lastRowB].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRowB].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["U" + lastRowB].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["V" + lastRowB].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRowB + ":J" + lastRowB].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["U" + lastRowB].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRowB + ":V" + lastRowB].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRowB].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["J" + lastRowB].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["U" + lastRowB].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["V" + lastRowB].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            //Bo qua 1 dong
            worksheet.Row(lastRowB + 1).Height = 5;

            //Ket qua
            int lastRowR = lastRowB + 2;
            worksheet.Cells["A" + lastRowR + ":I" + (lastRowR + 1)].Merge = true;
            worksheet.Cells["J" + lastRowR + ":N" + lastRowR].Merge = true;
            worksheet.Cells["O" + lastRowR + ":R" + lastRowR].Merge = true;
            worksheet.Cells["S" + lastRowR + ":V" + lastRowR].Merge = true;
            worksheet.Cells["J" + (lastRowR + 1) + ":N" + (lastRowR + 1)].Merge = true;
            worksheet.Cells["O" + (lastRowR + 1) + ":R" + (lastRowR + 1)].Merge = true;
            worksheet.Cells["S" + (lastRowR + 1) + ":V" + (lastRowR + 1)].Merge = true;
            worksheet.Cells["A" + lastRowR].Value = "KẾT QUẢ / RESULTS";
            worksheet.Cells["J" + lastRowR].Value = "Hoàn thành CV / \r\nCompletion (80%)";
            worksheet.Cells["O" + lastRowR].Value = "Năng lực / \r\nCompetency (20%)";
            worksheet.Cells["S" + lastRowR].Value = "Tổng điểm / \r\nTotal Score";
            worksheet.Cells["K" + lastRowR + ":V" + lastRowR].Style.WrapText = true;
            worksheet.Row(lastRowR).Height = 32;
            worksheet.Cells["J" + (lastRowR + 1)].Value = (completeWorkTotalScore*80)/100;
            worksheet.Cells["O" + (lastRowR + 1)].Value = (completeWorkTotalScore1*20)/100;
            worksheet.Cells["S" + (lastRowR + 1)].Value = (((completeWorkTotalScore * 80) / 100) + ((completeWorkTotalScore1 * 20) / 100))*2;
            // format cells - add borders A...:I...
            worksheet.Cells["A" + lastRowR + ":V" + (lastRowR + 1)].Style.Font.Bold = true;
            worksheet.Cells["A" + lastRowR + ":V" + (lastRowR + 1)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRowR + ":V" + (lastRowR + 1)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRowR + ":V" + (lastRowR + 1)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRowR + ":V" + (lastRowR + 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRowR + ":V" + (lastRowR + 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            //Nhan xet cua nhan vien
            int lastRowSE = lastRowR + 3;
            worksheet.Cells["A" + lastRowSE + ":V" + lastRowSE].Merge = true;
            worksheet.Cells["A" + (lastRowSE + 1) + ":V" + (lastRowSE + 1)].Merge = true;
            worksheet.Cells["A" + lastRowSE].Value = "NHẬN XÉT CỦA NHÂN VIÊN / COMMENTS OF STAFF";
            worksheet.Cells["A" + lastRowSE + ":V" + lastRowSE].Style.Font.Bold = true;
            worksheet.Cells["A" + (lastRowSE + 1)].Value = model.Ipf.SelfComment;
            worksheet.Cells["A" + (lastRowSE + 1) + ":V" + (lastRowSE + 1)].Style.WrapText = true;
            worksheet.Cells["A" + (lastRowSE + 1) + ":V" + (lastRowSE + 1)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + (lastRowSE + 1)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["V" + (lastRowSE + 1)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + (lastRowSE + 1) + ":V" + (lastRowSE + 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Row(lastRowSE + 1).Height = 50;
            //Nhan xet cua lanh dao
            int lastRowSM = lastRowSE + 2;
            worksheet.Cells["A" + lastRowSM + ":V" + lastRowSM].Merge = true;
            worksheet.Cells["A" + (lastRowSM + 1) + ":V" + (lastRowSM + 1)].Merge = true;
            worksheet.Cells["A" + lastRowSM].Value = "NHẬN XÉT CỦA LÃNH ĐẠO PHÒNG/BỘ PHẬN / COMMENTS OF DEPARTMENT HEAD";
            worksheet.Cells["A" + lastRowSM + ":V" + lastRowSM].Style.Font.Bold = true;
            worksheet.Cells["A" + (lastRowSM + 1)].Value = model.Ipf.ManagerComment;
            worksheet.Cells["A" + (lastRowSM + 1) + ":V" + (lastRowSM + 1)].Style.WrapText = true;
            worksheet.Cells["A" + (lastRowSM + 1) + ":V" + (lastRowSM + 1)].Style.WrapText = true;
            worksheet.Cells["A" + (lastRowSM + 1) + ":V" + (lastRowSM + 1)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + (lastRowSM + 1)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["U" + (lastRowSM + 1)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + (lastRowSM + 1) + ":V" + (lastRowSM + 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Row(lastRowSM + 1).Height = 50;
            //Nhan xet cua BOD
            int lastRowBod = lastRowSM + 2;
            worksheet.Cells["A" + lastRowBod + ":V" + lastRowBod].Merge = true;
            worksheet.Cells["A" + (lastRowBod + 1) + ":V" + (lastRowBod + 1)].Merge = true;
            worksheet.Cells["A" + lastRowBod].Value = "NHẬN XÉT CỦA BOD / COMMENTS OF BOD";
            worksheet.Cells["A" + lastRowBod + ":V" + lastRowBod].Style.Font.Bold = true;
            worksheet.Cells["A" + (lastRowBod + 1)].Value = model.Ipf.BodComment;
            worksheet.Cells["A" + (lastRowBod + 1) + ":V" + (lastRowBod + 1)].Style.WrapText = true;
            worksheet.Cells["A" + (lastRowBod + 1) + ":V" + (lastRowBod + 1)].Style.WrapText = true;
            worksheet.Cells["A" + (lastRowBod + 1) + ":V" + (lastRowBod + 1)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + (lastRowBod + 1)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["U" + (lastRowBod + 1)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["V" + (lastRowBod + 1)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + (lastRowBod + 1) + ":V" + (lastRowBod + 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Row(lastRowBod + 1).Height = 50;

            int lastRowNB = 0;
            if (model.Ipf.ScheduleType == 0)
            {
                //Ke hoach phat trien ca nhan
                int lastRow2 = lastRowBod + 2;
                worksheet.Cells["A" + lastRow2 + ":U" + lastRow2].Merge = true;
                worksheet.Cells["A" + lastRow2].Value = "2. KẾ HOẠCH PHÁT TRIỂN CÁ NHÂN / PERSONAL DEVELOPMENT PLAN";
                worksheet.Cells["A" + lastRow2].Style.Font.Bold = true;
                //Phat trien nang luc
                int lastRowP = lastRow2 + 1;
                worksheet.Cells["A" + lastRowP + ":U" + lastRowP].Merge = true;
                worksheet.Cells["A" + lastRowP].Value = "A. KẾ HOẠCH PHÁT TRIỂN NĂNG LỰC (Kiến thức, kỹ năng, kinh nghiệm, thái độ… cần phát triển để phục vụ công việc) \r\n         COMPETENCY DEVELOPMENT PLAN (knowledge, skills, experience, attitude ... should be developed to serve the work)";
                worksheet.Row(lastRowP).Height = 32;
                worksheet.Cells["A" + lastRowP + ":U" + lastRowP].Style.WrapText = true;
                worksheet.Cells["A" + lastRowP].Style.Font.Bold = true;
                int lastRowPT = lastRowP + 1;
                worksheet.Cells["B" + lastRowPT + ":J" + lastRowPT].Merge = true;
                worksheet.Cells["L" + lastRowPT + ":O" + lastRowPT].Merge = true;
                worksheet.Cells["P" + lastRowPT + ":U" + lastRowPT].Merge = true;
                worksheet.Cells["A" + lastRowPT].Value = "No.";
                worksheet.Cells["B" + lastRowPT].Value = "Các hoạt động đào tạo & phát triển trong năm\r\nThe training and development activities during the year";
                worksheet.Cells["L" + lastRowPT].Value = "Ngày hoàn tất\r\nCompletion date";
                worksheet.Cells["P" + lastRowPT].Value = "Ghi chú / Remarks";
                worksheet.Cells["A" + lastRowPT + ":U" + lastRowPT].Style.WrapText = true;
                worksheet.Cells["A" + lastRowPT + ":U" + lastRowPT].Style.Font.Bold = true;
                worksheet.Cells["A" + lastRow2 + ":U" + lastRowPT].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + lastRow2 + ":U" + lastRowPT].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + lastRow2 + ":U" + lastRowPT].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + lastRow2 + ":U" + lastRowPT].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + lastRowPT + ":U" + lastRowPT].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Row(lastRowPT).Height = 32;
                for (int i = 0; i < model.PersonalPlanCareers.Count; i++)
                {
                    if (model.PersonalPlanCareers[i].Type == 0)
                    {
                        lastRowPT++;
                        worksheet.Cells["B" + lastRowPT + ":L" + lastRowPT].Merge = true;
                        worksheet.Cells["M" + lastRowPT + ":O" + lastRowPT].Merge = true;
                        worksheet.Cells["P" + lastRowPT + ":U" + lastRowPT].Merge = true;
                        worksheet.Cells["A" + lastRowPT].Value = model.PersonalPlanCareers[i].Seq;
                        worksheet.Cells["B" + lastRowPT].Value = model.PersonalPlanCareers[i].Activity;
                        worksheet.Cells["M" + lastRowPT].Value = model.PersonalPlanCareers[i].CompleteDateString;
                        worksheet.Cells["P" + lastRowPT].Value = model.PersonalPlanCareers[i].Remark;
                        worksheet.Cells["A" + lastRowPT + ":U" + lastRowPT].Style.WrapText = true;
                        worksheet.Cells["A" + lastRowPT].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["M" + lastRowPT].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A" + lastRowPT + ":U" + lastRowPT].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["A" + lastRowPT + ":U" + lastRowPT].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["A" + lastRowPT + ":U" + lastRowPT].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["A" + lastRowPT + ":U" + lastRowPT].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                }
                //Phat trien nghe nghiep
                worksheet.Row(lastRowPT + 1).Height = 5;
                int lastRowPW = lastRowPT + 2;
                worksheet.Cells["A" + lastRowPW + ":U" + lastRowPW].Merge = true;
                worksheet.Cells["A" + lastRowPW].Value = "B. KẾ HOẠCH PHÁT TRIỂN NGHỀ NGHIỆP  / CAREER DEVELOPMENT PLAN";
                int lastRowPWT = lastRowPW + 1;
                worksheet.Cells["C" + lastRowPWT + ":J" + lastRowPWT].Merge = true;
                worksheet.Cells["K" + lastRowPWT + ":U" + lastRowPWT].Merge = true;
                worksheet.Cells["A" + lastRowPWT].Value = "No.";
                worksheet.Cells["B" + lastRowPWT].Value = "Thời hạn / Term";
                worksheet.Cells["C" + lastRowPWT].Value = "Theo nguyện vọng của nhân viên /\r\n According to the wishes of staffs";
                worksheet.Cells["K" + lastRowPWT].Value = "Theo đề nghị của quản lý / At the request of manager";
                worksheet.Cells["A" + lastRowPWT + ":U" + lastRowPWT].Style.WrapText = true;
                worksheet.Cells["A" + lastRowPW + ":U" + lastRowPWT].Style.Font.Bold = true;
                worksheet.Cells["A" + lastRowPW + ":U" + lastRowPWT].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + lastRowPW + ":U" + lastRowPWT].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + lastRowPW + ":U" + lastRowPWT].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + lastRowPW + ":U" + lastRowPWT].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + lastRowPWT + ":U" + lastRowPWT].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Row(lastRowPWT).Height = 32;
                for (int i = 0; i < model.PersonalPlanCareers.Count; i++)
                {
                    if (model.PersonalPlanCareers[i].Type == 1)
                    {
                        lastRowPWT++;
                        worksheet.Cells["C" + lastRowPWT + ":J" + lastRowPWT].Merge = true;
                        worksheet.Cells["K" + lastRowPWT + ":U" + lastRowPWT].Merge = true;
                        worksheet.Cells["A" + lastRowPWT].Value = model.PersonalPlanCareers[i].Seq;
                        worksheet.Cells["B" + lastRowPWT].Value = model.PersonalPlanCareers[i].Term == 2 ? "Ngắn hạn / Short term\r\n (2 năm/ year)" : "Dài hạn / Long term\r\n (3 - 5 năm/year)";
                        worksheet.Cells["C" + lastRowPWT].Value = model.PersonalPlanCareers[i].WishesOfStaff;
                        worksheet.Cells["K" + lastRowPWT].Value = model.PersonalPlanCareers[i].RequestOfManager;
                        worksheet.Cells["A" + lastRowPWT + ":U" + lastRowPWT].Style.WrapText = true;
                        worksheet.Cells["A" + lastRowPWT + ":U" + lastRowPWT].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["A" + lastRowPWT + ":U" + lastRowPWT].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["A" + lastRowPWT + ":U" + lastRowPWT].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["A" + lastRowPWT + ":U" + lastRowPWT].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["A" + lastRowPWT].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                }

                int lastRow3 = lastRowPWT + 2;
                worksheet.Cells["A" + lastRow3 + ":U" + lastRow3].Merge = true;
                worksheet.Cells["A" + (lastRow3 + 1) + ":R" + (lastRow3 + 1)].Merge = true;
                worksheet.Cells["S" + (lastRow3 + 1) + ":U" + (lastRow3 + 1)].Merge = true;
                worksheet.Cells["A" + lastRow3].Value = "3 - CAM KẾT MỤC TIÊU CÔNG VIỆC NĂM TIẾP THEO- " + (model.Ipf.Year + 1) + " / WORK COMMITMENTS IN THE NEXT YEAR- " + (model.Ipf.Year + 1);
                //Muc tieu hoan thanh cong viec
                worksheet.Cells["A" + (lastRow3 + 1)].Value = "A. MỤC TIÊU VỀ HOÀN THÀNH CÔNG VIỆC (80%)  / OBJECTIVES OF COMPLETED WORK (80%)";
                worksheet.Cells["S" + (lastRow3 + 1)].Value = "Điểm (1-5)";
                worksheet.Cells["S" + (lastRow3 + 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                int lastRowNT = lastRow3 + 2;
                worksheet.Cells["D" + lastRowNT + ":I" + lastRowNT].Merge = true;
                worksheet.Cells["K" + lastRowNT + ":R" + lastRowNT].Merge = true;
                worksheet.Cells["A" + lastRowNT].Value = "";
                worksheet.Cells["B" + lastRowNT].Value = "KPI/Objective\r\nKPI/Mục tiêu";
                worksheet.Cells["C" + lastRowNT].Value = "No.";
                worksheet.Cells["D" + lastRowNT].Value = "Chỉ tiêu kế hoạch/yêu cầu cụ thể cần phải đạt được\r\nTargets / specific requirements must be achieved";
                worksheet.Cells["J" + lastRowNT].Value = "Tỷ trọng / \r\nWeight";
                worksheet.Cells["K" + lastRowNT].Value = "Kết quả thực hiện / Performance results";
                worksheet.Cells["S" + lastRowNT].Value = "NV tự đánh giá / Self assessment";
                worksheet.Cells["T" + lastRowNT].Value = "Quản lý đánh giá / Manager's assessment";
                worksheet.Cells["U" + lastRowNT].Value = "Tổng điểm hoàn thành / \r\nTotal Score";
                worksheet.Cells["A" + lastRow3 + ":U" + lastRowNT].Style.Font.Bold = true;
                worksheet.Cells["A" + lastRowNT + ":U" + lastRowNT].Style.WrapText = true;
                worksheet.Cells["A" + (lastRow3 + 1) + ":U" + lastRowNT].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + (lastRow3 + 1) + ":U" + lastRowNT].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + (lastRow3 + 1) + ":U" + lastRowNT].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + (lastRow3 + 1) + ":U" + lastRowNT].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + lastRowNT + ":U" + lastRowNT].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                int colN = lastRowNT;
                decimal? completeWorkTotalScoreN = 0;
                decimal? completeWorkWeightScoreN = 0;
                for (int i = 0; i < model.CompleteWorksNextYear.Count; i++)
                {
                    if (model.CompleteWorksNextYear[i].WorkType == 0 && model.CompleteWorksNextYear[i].IsNextYear == 1)
                    {
                        if (model.CompleteWorksNextYear[i].Weight == null)
                        {
                            colN++;
                            worksheet.Cells["A" + colN].Value = BtcHelper.ToRoman(model.CompleteWorksNextYear[i].Seq);
                            worksheet.Cells["B" + colN + ":U" + colN].Merge = true;
                            worksheet.Cells["B" + colN].Value = model.CompleteWorksNextYear[i].Objective;
                            worksheet.Cells["A" + colN].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        } else {
                            colN++;
                            worksheet.Cells["A" + colN].Value = model.CompleteWorksNextYear[i].Seq;
                            worksheet.Cells["B" + colN].Value = model.CompleteWorksNextYear[i].Objective;
                            worksheet.Cells["C" + colN].Value = model.CompleteWorksNextYear[i].Seq;
                            worksheet.Cells["D" + colN + ":I" + colN].Merge = true;
                            worksheet.Cells["D" + colN].Value = model.CompleteWorksNextYear[i].Target;
                            worksheet.Cells["J" + colN].Value = model.CompleteWorksNextYear[i].Weight;
                            worksheet.Cells["K" + colN + ":R" + colN].Merge = true;
                            worksheet.Cells["K" + colN].Value = model.CompleteWorksNextYear[i].Result;
                            worksheet.Cells["S" + colN].Value = model.CompleteWorksNextYear[i].SelfScore;
                            worksheet.Cells["T" + colN].Value = model.CompleteWorksNextYear[i].ManagerScore;
                            model.CompleteWorksNextYear[i].TotalScore =
                                model.CompleteWorksNextYear[i].Weight * model.CompleteWorksNextYear[i].ManagerScore / 100;
                            worksheet.Cells["U" + colN].Value = model.CompleteWorksNextYear[i].TotalScore;
                            worksheet.Cells["A" + colN + ":U" + colN].Style.WrapText = true;
                            worksheet.Cells["A" + colN].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["C" + colN].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["J" + colN].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["S" + colN + ":U" + colN].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            completeWorkTotalScoreN = completeWorkTotalScoreN + model.CompleteWorksNextYear[i].TotalScore;
                            completeWorkWeightScoreN = completeWorkWeightScoreN + model.CompleteWorksNextYear[i].Weight;
                        }
                        worksheet.Cells["A" + colN + ":U" + colN].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["A" + colN + ":U" + colN].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["A" + colN + ":U" + colN].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["A" + colN + ":U" + colN].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                }
                colN++;
                worksheet.Cells["A" + colN + ":I" + colN].Merge = true;
                worksheet.Cells["K" + colN + ":T" + colN].Merge = true;
                worksheet.Cells["A" + colN].Value = "Total /Tổng cộng";
                worksheet.Cells["J" + colN].Value = completeWorkWeightScoreN;
                worksheet.Cells["I" + colN].Value = completeWorkTotalScoreN;
                worksheet.Cells["A" + colN + ":U" + colN].Style.Font.Bold = true;
                worksheet.Cells["A" + colN + ":U" + colN].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + colN + ":U" + colN].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + colN + ":U" + colN].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + colN + ":U" + colN].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + colN + ":U" + colN].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //bo qua 1 dong
                decimal? completeWorkTotalScoreN1 = 0;
                decimal? completeWorkWeightScoreN1 = 0;
                int lastRowN = colN + 1;
                worksheet.Cells["A" + lastRowN + ":R" + lastRowN].Merge = true;
                worksheet.Cells["S" + lastRowN + ":U" + lastRowN].Merge = true;
                //Muc tieu nang luc
                worksheet.Cells["A" + lastRowN].Value = "B. MỤC TIÊU VỀ NĂNG LỰC / OBJECTIVES OF COMPETENCY (20%)";
                worksheet.Cells["S" + lastRowN].Value = "Điểm (1-5)";
                worksheet.Cells["S" + lastRowN].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                lastRowNB = lastRowN + 1;
                worksheet.Cells["D" + lastRowNB + ":I" + lastRowNB].Merge = true;
                worksheet.Cells["K" + lastRowNB + ":R" + lastRowNB].Merge = true;
                worksheet.Cells["A" + lastRowNB].Value = "No.";
                worksheet.Cells["B" + lastRowNB].Value = "Kiến thức/Kỹ năng/Thái độ\r\nKnowledge / Skills / Attitudes";
                worksheet.Cells["C" + lastRowNB].Value = "No.";
                worksheet.Cells["D" + lastRowNB].Value = "Chỉ tiêu kế hoạch/yêu cầu cụ thể cần phải đạt được\r\nTargets / specific requirements must be achieved";
                worksheet.Cells["J" + lastRowNB].Value = "Tỷ trọng / \r\nWeight";
                worksheet.Cells["K" + lastRowNB].Value = "Kết quả thực hiện / Performance results";
                worksheet.Cells["S" + lastRowNB].Value = "NV tự đánh giá / Self assessment";
                worksheet.Cells["T" + lastRowNB].Value = "Quản lý đánh giá / Manager's assessment";
                worksheet.Cells["U" + lastRowNB].Value = "Tổng điểm hoàn thành / \r\nTotal Score";
                worksheet.Cells["A" + lastRowN + ":U" + lastRowNB].Style.Font.Bold = true;
                worksheet.Cells["A" + lastRowNB + ":U" + lastRowNB].Style.WrapText = true;
                worksheet.Cells["A" + lastRowN + ":U" + lastRowNB].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + lastRowN + ":U" + lastRowNB].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + lastRowN + ":U" + lastRowNB].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + lastRowN + ":U" + lastRowNB].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + lastRowNB + ":U" + lastRowNB].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                for (int i = 0; i < model.CompetenciesNextYear.Count; i++)
                {
                    if (model.CompetenciesNextYear[i].WorkType == 1 && model.CompetenciesNextYear[i].IsNextYear == 1)
                    {
                        lastRowNB++;
                        worksheet.Cells["D" + lastRowNB + ":I" + lastRowNB].Merge = true;
                        worksheet.Cells["K" + lastRowNB + ":R" + lastRowNB].Merge = true;
                        worksheet.Cells["A" + lastRowNB].Value = model.CompetenciesNextYear[i].Seq;
                        worksheet.Cells["B" + lastRowNB].Value = model.CompetenciesNextYear[i].Objective;
                        worksheet.Cells["C" + lastRowNB].Value = model.CompetenciesNextYear[i].Seq;
                        worksheet.Cells["D" + lastRowNB].Value = model.CompetenciesNextYear[i].Target;
                        worksheet.Cells["J" + lastRowNB].Value = model.CompetenciesNextYear[i].Weight;
                        worksheet.Cells["K" + lastRowNB].Value = model.CompetenciesNextYear[i].Result;
                        worksheet.Cells["S" + lastRowNB].Value = model.CompetenciesNextYear[i].SelfScore;
                        worksheet.Cells["T" + lastRowNB].Value = model.CompetenciesNextYear[i].ManagerScore;
                        model.CompetenciesNextYear[i].TotalScore =
                            model.CompetenciesNextYear[i].Weight * model.CompetenciesNextYear[i].ManagerScore / 100;
                        worksheet.Cells["U" + lastRowNB].Value = model.CompetenciesNextYear[i].TotalScore;
                        worksheet.Cells["A" + lastRowNB + ":U" + lastRowNB].Style.WrapText = true;

                        completeWorkTotalScoreN1 = completeWorkTotalScoreN1 + model.CompetenciesNextYear[i].TotalScore;
                        completeWorkWeightScoreN1 = completeWorkWeightScoreN1 + model.CompetenciesNextYear[i].Weight;
                        worksheet.Cells["A" + lastRowNB + ":U" + lastRowNB].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["A" + lastRowNB + ":U" + lastRowNB].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["A" + lastRowNB + ":U" + lastRowNB].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["A" + lastRowNB + ":U" + lastRowNB].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["A" + lastRowNB].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["C" + lastRowNB].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["J" + lastRowNB].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["S" + lastRowNB + ":U" + lastRowNB].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                }
                lastRowNB++;
                worksheet.Cells["A" + lastRowNB + ":I" + lastRowNB].Merge = true;
                worksheet.Cells["K" + lastRowNB + ":T" + lastRowNB].Merge = true;
                worksheet.Cells["A" + lastRowNB].Value = "Total /Tổng cộng";
                worksheet.Cells["J" + lastRowNB].Value = completeWorkWeightScoreN1;
                worksheet.Cells["I" + lastRowNB].Value = completeWorkTotalScoreN1;
                worksheet.Cells["A" + lastRowNB + ":U" + lastRowNB].Style.Font.Bold = true;
                worksheet.Cells["A" + lastRowNB + ":U" + lastRowNB].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + lastRowNB + ":U" + lastRowNB].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + lastRowNB + ":U" + lastRowNB].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + lastRowNB + ":U" + lastRowNB].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + lastRowNB + ":U" + lastRowNB].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            int lastRowS = model.Ipf.ScheduleType == 0 ? lastRowNB + 2 : lastRowBod + 3;
            worksheet.Cells["A" + lastRowS + ":I" + lastRowS].Merge = true;
            worksheet.Cells["K" + lastRowS + ":U" + lastRowS].Merge = true;
            worksheet.Cells["A" + lastRowS].Value = "KÝ XÁC NHẬN VỀ XÂY DỰNG MỤC TIÊU";
            worksheet.Cells["K" + lastRowS].Value = "KÝ XÁC NHẬN VỀ KẾT QỦA ĐÁNH GIÁ";
            worksheet.Cells["A" + lastRowS + ":U" + lastRowS].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            int lastRowS1 = lastRowS + 1;
            worksheet.Cells["A" + lastRowS1 + ":I" + lastRowS1].Merge = true;
            worksheet.Cells["K" + lastRowS1 + ":U" + lastRowS1].Merge = true;
            worksheet.Cells["A" + lastRowS1].Value = "CONMMITMENTS OF BUILDING UP OBJECTIVES";
            worksheet.Cells["K" + lastRowS1].Value = "CERTIFICATION ON REVIEW RESULTS";
            worksheet.Cells["A" + lastRowS1 + ":U" + lastRowS1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            int lastRowS2 = lastRowS1 + 1;
            worksheet.Cells["A" + lastRowS2 + ":C" + lastRowS2].Merge = true;
            worksheet.Cells["D" + lastRowS2 + ":I" + lastRowS2].Merge = true;
            worksheet.Cells["K" + lastRowS2 + ":Q" + lastRowS2].Merge = true;
            worksheet.Cells["R" + lastRowS2 + ":U" + lastRowS2].Merge = true;
            worksheet.Cells["A" + lastRowS2].Value = "Tên NV / Staff";
            worksheet.Cells["D" + lastRowS2].Value = "Lãnh đạo /Leader";
            worksheet.Cells["K" + lastRowS2].Value = "Tên NV / Staff";
            worksheet.Cells["R" + lastRowS2].Value = "Lãnh đạo /Leader";
            worksheet.Cells["A" + lastRowS2 + ":U" + lastRowS2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["A" + lastRowS + ":U" + lastRowS2].Style.Font.Bold = true;

            worksheet.Cells["A1:U" + lastRowS2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //Apply row height and column width to look good
            worksheet.Column(1).Width = 5;
            worksheet.Column(2).Width = 26;
            worksheet.Column(3).Width = 5;
            worksheet.Column(4).Width = 5;
            worksheet.Column(5).Width = 5;
            worksheet.Column(6).Width = 5;
            worksheet.Column(7).Width = 6;
            worksheet.Column(8).Width = 5;
            worksheet.Column(9).Width = 5;
            worksheet.Column(10).Width = 10;
            worksheet.Column(11).Width = 5;
            worksheet.Column(12).Width = 5;
            worksheet.Column(13).Width = 5;
            worksheet.Column(14).Width = 5;
            worksheet.Column(15).Width = 5;
            worksheet.Column(16).Width = 5;
            worksheet.Column(17).Width = 5;
            worksheet.Column(18).Width = 5;
            worksheet.Column(19).Width = 12;
            worksheet.Column(20).Width = 12;
            worksheet.Column(21).Width = 12;

            return excelEngine;
        }

    }

    public class PersonalPlanModel
    {
        public Nullable<int> Seq { get; set; }
        public string WishesOfStaff { get; set; }
        public string RequestOfManager { get; set; }
    }
}
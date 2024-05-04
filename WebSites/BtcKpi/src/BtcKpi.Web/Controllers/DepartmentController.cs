using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    [RoutePrefix("Department")]
    public class DepartmentController : BaseController
    {
        private readonly IUserService userService;
        private readonly IDepartmentService departmentService;
        private readonly IDepartRateService departRateService;

        public DepartmentController(IUserService userService, IDepartmentService departmentService, IDepartRateService departRateService) {
            this.userService = userService;
            this.departmentService = departmentService;
            this.departRateService = departRateService;
        }

        // GET: Department
        public ActionResult UpfList()
        {
            Session[string.Format("department-{0}", CurrentUser.UserId)] = null;
            DepartmentListViewModel model = new DepartmentListViewModel();
            model.UserInfo = userService.GetUserFullInfo(CurrentUser.UserId);

            //Cong ty - Phong ban/ bo phan
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
            //Loai
            var scheduleTypes = new List<ConvertEnum>();
            foreach (var schedule in Enum.GetValues(typeof(IpfScheduleType)))
                scheduleTypes.Add(new ConvertEnum
                {
                    Value = (int)schedule,
                    Text = schedule.ToString() == "Year" ? "Năm" : "Định kỳ"
                });
            model.ScheduleTypes = new SelectList(scheduleTypes, "Value", "Text");
            model.ScheduleType = "1";

            //Nam
            model.Years = Years;
            model.Year = DateTime.Now.Year.ToString();

            //Ky
            model.DepartSchedules = new SelectList(departmentService.DepartSchedulesByDeparment((int)model.UserInfo.DepartmentID, (int)model.UserInfo.CompanyID), "ID", "Name");
            var month = DateTime.Now.Month - 1;
            var monthItem = model.DepartSchedules.FirstOrDefault(p => p.Text.Contains(month.ToString()));
            if (monthItem != null)
            {
                model.ScheduleID = monthItem.Value;
            }

            //Trang thai
            var departStatus = new List<ConvertEnum>();
            foreach (var status in Enum.GetValues(typeof(DepartmentStatus)))
                departStatus.Add(new ConvertEnum
            {
                Value = (int)status,
                Text = BtcHelper.convertStatus(status.ToString())
            });
            model.Status = new SelectList(departStatus, "Value", "Text");
            //Lay danh sach KPI phong ban/ bo phan
            model.DepartmentInfos = new List<DepartmentInfo>();

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
        public JsonResult OutsAchievChange(Upf model)
        {
            var currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            currentModel.Upf.OutsAchiev = model.OutsAchiev;
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult SelfRatingChange(Upf model)
        {
            var currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            currentModel.Upf.SelfRating = model.SelfRating;
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SearchDepartmentList(DepartmentListViewModel model)
        {
            List<DepartmentInfo> departmentInfos = new List<DepartmentInfo>();
            model.DepartmentInfos = departmentService.GetDepartByConditions(model.CompanyID, model.DepartmentID, model.ScheduleType, model.Year, model.ScheduleID, model.StatusID);
            model.UserID = CurrentUser.UserId;
            foreach (var item in model.DepartmentInfos)
            {
                if (CurrentUser.UserId != item.CreatedBy)
                {
                    if(item.StatusID > 0 && item.StatusID < 4)
                    {
                        departmentInfos.Add(item);
                    }
                }
                else
                {
                    departmentInfos.Add(item);
                }
            }

            model.DepartmentInfos = departmentInfos.OrderBy(o=>o.ScheduleID).ThenBy(o=>o.ScheduleType).ToList();

            return PartialView("_DepartmentListTable", model);
        }

        // GET: Upf/Create
        [HttpGet]
        [Route("DepartKPIsCreate")]
        public ActionResult Create()
        {
            DepartmentViewModel model = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            if (model == null)
            {
                model = InitData();
            }
            Session[string.Format("department-{0}", CurrentUser.UserId)] = model;

            return View(model);
        }

        // POST: Upf/Create
        [HttpPost]
        [Route("DepartKPIsCreate")]
        public ActionResult Create(DepartmentViewModel model, string submitButton)
        {
            var currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            bool isValid = true;
            currentModel.ErrorMessage = "";
            string insertMsg = "";
            if (ModelState.IsValid)
            {
                currentModel.Upf.StatusID = submitButton == "Success" ? 1 : 0;
                if (departmentService.InsertDepartment(CurrentUser.UserId, currentModel.UserInfo.DepartmentID, currentModel.Upf,
                    currentModel.PersRewProposals, currentModel.NameDetails, ref insertMsg))
                {
                    return RedirectToAction("UpfList", "Department");
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

        // GET: Upf/Create
        [HttpGet]
        [Route("DepartKPIsEdit")]
        public ActionResult Edit(int id)
        {
            DepartmentViewModel model = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            if (id != 0)
            {
                if (model == null)
                {
                    model = GetDepartmentById(id);
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
            Session[string.Format("department-{0}", CurrentUser.UserId)] = model;

            return View("Create", model);
        }

        // POST: Upf/Create
        [HttpPost]
        [Route("DepartKPIsEdit")]
        public ActionResult Edit(DepartmentViewModel model, string submitButton)
        {
            var currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            bool isValid = true;
            currentModel.ErrorMessage = "";
            string insertMsg = "";
            if (ModelState.IsValid)
            {
                if (model.Upf.ID != 0)
                {
                    currentModel.Upf.StatusID = submitButton == "Success" ? 1 : 0;
                    if (departmentService.UpdateDepartment(CurrentUser.UserId, currentModel.Upf,
                        currentModel.PersRewProposals, currentModel.NameDetails, ref insertMsg))
                    {
                        return RedirectToAction("UpfList", "Department");
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

        // GET: Upf/Approve
        [HttpGet]
        [Route("DepartKPIsApprove")]
        public ActionResult Approve(int id, int isApproved)
        {
            DepartmentViewModel model = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            if (id != 0)
            {
                if (model == null)
                {
                    model = GetDepartmentById(id);
                    model.FirstLoad = false;
                    if (isApproved == 1)
                    {
                        model.isApprove = true;
                    }
                }
            }
            else
            {
                if (model == null)
                {
                    model = InitData();
                }
            }
            Session[string.Format("department-{0}", CurrentUser.UserId)] = model;

            return View("Create", model);
        }

        // POST: Upf/Create
        [HttpPost]
        [Route("DepartKPIsApprove")]
        public ActionResult Approve(DepartmentViewModel model, string submitButton)
        {
            var currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            bool isValid = true;
            currentModel.ErrorMessage = "";
            string insertMsg = "";
            if (ModelState.IsValid)
            {
                if (model.isApprove == true)
                {
                    string updateMsg = "";
                    bool isApproved = submitButton == "Cancel" ? false : true;
                    if (string.IsNullOrEmpty(model.UpfComment.Comment))
                    {
                        currentModel.ErrorMessage =  isApproved == true ? "Bạn phải nhập nhận xét" : "Bạn phải nhập lý do từ chối";
                        return View("Create", currentModel);
                    }
                    if (departmentService.ApprovedOrNotApprovedUpf(CurrentUser.UserId, currentModel.Upf.ID, isApproved, model.UpfComment.Comment, currentModel.NameDetails, currentModel.Upf.TotalManagePoint, ref updateMsg))
                    {
                        return RedirectToAction("UpfList", "Department");
                    }
                    else
                    {
                        currentModel.ErrorMessage = updateMsg;
                        return View("Create", currentModel);
                    }
                }
                else
                {
                    return RedirectToAction("UpfList", "Department");
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

        // GET: Upf/BODApprove
        [HttpGet]
        [Route("DepartKPIsBODApprove")]
        public ActionResult BodApprove(int id, int isBODApproved)
        {
            DepartmentViewModel model = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            if (id != 0)
            {
                if (model == null)
                {
                    model = GetDepartmentById(id);
                    model.FirstLoad = false;
                    if (isBODApproved == 1)
                    {
                        model.isBODApprove = true;
                    }
                }
            }
            else
            {
                if (model == null)
                {
                    model = InitData();
                }
            }
            Session[string.Format("department-{0}", CurrentUser.UserId)] = model;

            return View("Create", model);
        }

        // POST: Upf/Create
        [HttpPost]
        [Route("DepartKPIsBODApprove")]
        public ActionResult BODApprove(DepartmentViewModel model, string submitButton)
        {
            var currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            bool isValid = true;
            currentModel.ErrorMessage = "";
            string insertMsg = "";
            if (ModelState.IsValid)
            {
                if (model.isBODApprove == true)
                {
                    string updateMsg = "";
                    bool isBODApprove = submitButton == "Cancel" ? false : true;
                    if (string.IsNullOrEmpty(model.UpfComment.Comment))
                    {
                        currentModel.ErrorMessage = isBODApprove == true ? "Bạn phải nhập nhận xét" : "Bạn phải nhập lý do từ chối";
                        return View("Create", currentModel);
                    }
                    if (departmentService.ApprovedOrNotBODApprovedUpf(CurrentUser.UserId, currentModel.Upf.ID, isBODApprove, model.UpfComment.Comment, currentModel.NameDetails, currentModel.Upf.TotalBODPoint, ref updateMsg))
                    {
                        return RedirectToAction("UpfList", "Department");
                    }
                    else
                    {
                        currentModel.ErrorMessage = updateMsg;
                        return View("Create", currentModel);
                    }
                }
                else
                {
                    return RedirectToAction("UpfList", "Department");
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

        // GET: Upf/Create
        [HttpGet]
        [Route("DepartKPIsComment")]
        public ActionResult Comment(int id, int isCommentVal)
        {
            DepartmentViewModel model = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            if (id != 0)
            {
                if (model == null)
                {
                    model = GetDepartmentById(id);
                    model.FirstLoad = false;
                    if (isCommentVal == 1)
                    {
                        model.isComment = true;
                    }
                }
            }
            else
            {
                if (model == null)
                {
                    model = InitData();
                }
            }
            Session[string.Format("department-{0}", CurrentUser.UserId)] = model;

            return View("Create", model);
        }

        // POST: Upf/Create
        [HttpPost]
        [Route("DepartKPIsComment")]
        public ActionResult Comment(DepartmentViewModel model)
        {
            var currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            bool isValid = true;
            currentModel.ErrorMessage = "";
            string insertMsg = "";
            if (ModelState.IsValid)
            {
                if (model.isComment == true)
                {
                    string updateMsg = "";
                    if (string.IsNullOrEmpty(model.UpfComment.Comment))
                    {
                        currentModel.ErrorMessage = "Bạn phải nhập nhận xét";
                        return View("Create", currentModel);
                    }
                    if (departmentService.InsertComment(CurrentUser.UserId, currentModel.Upf.ID, model.UpfComment.Comment, ref updateMsg))
                    {
                        return RedirectToAction("UpfList", "Department");
                    }
                    else
                    {
                        currentModel.ErrorMessage = updateMsg;
                        return View("Create", currentModel);
                    }
                }
                else
                {
                    return RedirectToAction("UpfList", "Department");
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

        public DepartmentViewModel InitData()
        {
            DepartmentViewModel model = new DepartmentViewModel();
            model.FirstLoad = true;
            model.ErrorMesages = new List<string>();
            model.ErrorMessage = "";

            //Init properties
            model.Upf = new Upf();
            model.NameDetails = new List<UpfNameDetail>();
            model.PersRewProposals = new List<UpfPersRewProposal>();

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
            model.DepartSchedules = new SelectList(departmentService.DepartSchedulesByDeparment((int)model.UserInfo.DepartmentID, (int)model.UserInfo.DepartmentID), "ID", "Name");

            model.UpfRates = new SelectList(departRateService.GetUpfRateList(), "ID", "Name");

            //Nguoi duyet
            if (model.UserInfo.AdministratorshipID != null)
            {
                model.Approves = new SelectList(userService.GetListManageInfo((int) model.UserInfo.AdministratorshipID),
                    "ID", "FullName");
            }
            //BOD duyet
            if (model.UserInfo.AdministratorshipID != null)
            {
                model.BODApproves = new SelectList(userService.GetListManageInfo((int)model.UserInfo.AdministratorshipID),
                    "ID", "FullName");
            }
            //Nhóm công việc
            var completeWorkTitles = departmentService.CompleteWorkTitleByDepartment((int)model.UserInfo.DepartmentID);
            model.CompleteWorkTitles = new SelectList(completeWorkTitles, "ID", "Name");

            byte? stt = 0;
            foreach (var completeWorkTitle in completeWorkTitles)
            {
                stt++;
                model.NameDetails.Add(new UpfNameDetail() { Order = stt, WorkCompleteID = completeWorkTitle.ID, NameKPI = completeWorkTitle.Name });
            }

            return model;
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Start(Upf model)
        {
            var currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            currentModel.Upf.ScheduleType = model.ScheduleType;
            currentModel.Upf.Year = model.Year;
            currentModel.Upf.ScheduleID = model.ScheduleID;
            currentModel.Upf.ApproveBy = model.ApproveBy;
            currentModel.Upf.BodApproved = model.BodApproved;
            model.CheckExistsDepartment = departmentService.CheckCreateDepart(model, CurrentUser.UserId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult Reset()
        {
            Session[string.Format("department-{0}", CurrentUser.UserId)] = null;
            return Json(new DepartmentViewModel(), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult AddNameKPIDetail(UpfNameDetail model)
        {
            var currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            currentModel.FirstLoad = false;
            currentModel.ErrorMessage = "";

            UpfNameDetail lastItem = new UpfNameDetail();
            List<UpfNameDetail> newJobObject = new List<UpfNameDetail>();
            int seq = 0;

            //Seq
            if(currentModel.NameDetails != null)
            {
                foreach (var bo in currentModel.NameDetails)
                {
                    lastItem = currentModel.NameDetails.Where(t => t.UpfID == bo.UpfID).OrderByDescending(p => p.Order).FirstOrDefault();
                }
            } else {
                lastItem = currentModel.NameDetails.Where(t => t.UpfID == model.UpfID).OrderByDescending(p => p.Order).FirstOrDefault();
            }
            if (lastItem == null || lastItem.Order == null)
            {
                model.Order = 1;
            }
            else
            {
                model.Order = (byte?)((int)lastItem.Order + 1);
            }

            newJobObject = new List<UpfNameDetail>(currentModel.NameDetails);
            newJobObject.Add(model);

            currentModel.NameDetails = newJobObject.OrderBy(t => t.Order).ThenBy(t => t.Order).ToList();
            //Re index SEQ
            for (int i = 0; i < currentModel.NameDetails.Count; i++)
            {
                if (!"".Equals(currentModel.NameDetails[i].NameKPI))
                {
                    seq++;
                    currentModel.NameDetails[i].Order = (byte?)seq;
                }
            }

            Session[string.Format("department-{0}", CurrentUser.UserId)] = currentModel;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult EditNameKPIDetail(UpfNameDetail model)
        {
            var currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            currentModel.FirstLoad = false;
            currentModel.ErrorMessage = "";
            for (int i = 0; i < currentModel.NameDetails.Count; i++)
            {
                if (currentModel.NameDetails[i].Order == model.Order)
                {
                    currentModel.NameDetails[i] = model;
                    break;
                }
            }

            Session[string.Format("department-{0}", CurrentUser.UserId)] = currentModel;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult AddJobObject(UpfJobDetail model)
        {
            var currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            currentModel.FirstLoad = false;
            currentModel.ErrorMessage = "";

            UpfJobDetail lastItem = new UpfJobDetail();
            List<UpfNameDetail> newJobObject = new List<UpfNameDetail>();
            int seq = 0;

            //Seq
            foreach (UpfNameDetail bo in currentModel.NameDetails)
            {
                if(bo.JobDetails == null)
                {
                    bo.JobDetails = new List<UpfJobDetail>();
                }
                if (bo.JobDetails != null && bo.JobDetails.Count == 0 && bo.WorkCompleteID == model.NameDetailID)
                {
                    bo.JobDetails = new List<UpfJobDetail>();
                    lastItem = bo.JobDetails.Where(t => t.NameDetailID == model.NameDetailID).OrderByDescending(p => p.Order).FirstOrDefault();
                } else if (bo.JobDetails != null && bo.JobDetails.Count > 0 && bo.WorkCompleteID == model.NameDetailID)
                {
                    foreach (var item in bo.JobDetails)
                    {
                        lastItem = item.ID == 0 ? bo.JobDetails.Where(t => t.NameDetailID == model.NameDetailID).OrderByDescending(p => p.Order).FirstOrDefault() : bo.JobDetails.Where(t => t.UpfNameDetailID == item.UpfNameDetailID).OrderByDescending(p => p.Order).FirstOrDefault();
                    }
                }

                if (bo.JobDetails != null && (lastItem == null || lastItem.Order == null) && model.Order == null)
                {
                    model.Order = 1;
                }
                else
                if (bo.JobDetails != null && lastItem != null && lastItem.Order != null && lastItem.Order != null)
                {
                    model.Order = (byte?) ((int) lastItem.Order + 1);
                }
            }

            newJobObject = new List<UpfNameDetail>(currentModel.NameDetails);
            int weightTotal = 0;
            decimal? totalPoint = new decimal();
            foreach (UpfNameDetail bo in newJobObject)
            {
                if(bo.JobDetails == null)
                {
                    bo.JobDetails = new List<UpfJobDetail>();
                }
                if(bo.WorkCompleteID.ToString().Equals(model.NameDetailID.ToString()))
                {
                    bo.JobDetails.Add(model);
                }

                foreach (var jobDetail in bo.JobDetails)
                {
                    weightTotal = weightTotal + Int32.Parse(jobDetail.Weight.ToString());
                    totalPoint = totalPoint + ((jobDetail.Weight * jobDetail.Point) / 100);
                }
            }

            currentModel.Upf.WeightTotal = weightTotal;
            currentModel.Upf.TotalPoint = totalPoint;
            Session[string.Format("department-{0}", CurrentUser.UserId)] = currentModel;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("UpdateJobObjPre")]
        public JsonResult UpdateJobObjPre(int Order, int NameDetailID)
        {
            DepartmentViewModel currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            UpfJobDetail viewItem = new UpfJobDetail();
            foreach (UpfNameDetail boDetail in currentModel.NameDetails)
            {
                viewItem = boDetail.JobDetails.Where(t => t.Order == Order && t.NameDetailID == NameDetailID).FirstOrDefault();
                if (viewItem != null)
                {
                    break;
                }
            }
            return Json(viewItem, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("UpdateJobObject")]
        public JsonResult UpdateJobObject(UpfJobDetail model)
        {
            var currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            currentModel.FirstLoad = false;
            currentModel.ErrorMessage = "";
            int weightTotal = 0;
            decimal? totalPoint = new decimal();
            foreach (UpfNameDetail nameDetail in currentModel.NameDetails)
            {
                for (int i = 0; i < nameDetail.JobDetails.Count; i++)
                {
                    if (nameDetail.JobDetails[i].Order == model.Order && nameDetail.JobDetails[i].NameDetailID == model.NameDetailID)
                    {
                        nameDetail.JobDetails[i] = model;
                        break;
                    }
                }

                foreach (var jobDetail in nameDetail.JobDetails)
                {
                    weightTotal = weightTotal + Int32.Parse(jobDetail.Weight.ToString());
                    totalPoint = totalPoint + ((jobDetail.Weight * jobDetail.Point) / 100);
                }
            }

            currentModel.Upf.WeightTotal = weightTotal;
            currentModel.Upf.TotalPoint = totalPoint;
            Session[string.Format("department-{0}", CurrentUser.UserId)] = currentModel;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("DeleteJobObject")]
        public JsonResult DeleteJobObject(int Order, string NameDetailID)
        {
            DepartmentViewModel currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            UpfJobDetail deleteItem = new UpfJobDetail();
            foreach (UpfNameDetail nameDetail in currentModel.NameDetails)
            {

                deleteItem = nameDetail.JobDetails.Where(t => t.Order == Order & t.NameDetailID == Int32.Parse(NameDetailID)).FirstOrDefault();
                if (deleteItem != null)
                {
                    currentModel.Upf.WeightTotal = currentModel.Upf.WeightTotal - Int32.Parse(deleteItem.Weight.ToString());
                    currentModel.Upf.TotalPoint = currentModel.Upf.TotalPoint - ((deleteItem.Weight * deleteItem.Point) / 100);
                    nameDetail.JobDetails.Remove(deleteItem);
                    if(nameDetail.JobDetails.Count > 0)
                    {
                        int i = 0;
                        foreach (var jobBO in nameDetail.JobDetails)
                        {
                            if(jobBO.Order > 1)
                            {
                                i = Int32.Parse(jobBO.Order.ToString());
                                i--;
                                jobBO.Order = Byte.Parse(i.ToString());
                            }
                        }
                    }
                    break;
                }
            }

            return Json(deleteItem, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult AddPersRewProp(UpfPersRewProposal model)
        {
            var currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            currentModel.FirstLoad = false;
            currentModel.ErrorMessage = "";

            UpfPersRewProposal lastItem = new UpfPersRewProposal();
            List<UpfPersRewProposal> newPersRewProp = new List<UpfPersRewProposal>();
            int seq = 0;

            //Seq
            lastItem = currentModel.PersRewProposals.Where(t => t.ID == model.ID).OrderByDescending(p => p.ID).FirstOrDefault();
            if (lastItem == null || lastItem.Order == null)
            {
                model.Order = 1;
            }
            else
            {
                model.Order = (byte?)((int)lastItem.Order + 1);
            }

            newPersRewProp = new List<UpfPersRewProposal>(currentModel.PersRewProposals);
            newPersRewProp.Add(model);

            currentModel.PersRewProposals = newPersRewProp.OrderBy(t => t.ID).ThenBy(t => t.ID).ToList();
            //Re index SEQ
            for (int i = 0; i < currentModel.PersRewProposals.Count; i++)
            {
                if (!"".Equals(currentModel.PersRewProposals[i].EmployeeName))
                {
                    seq++;
                    currentModel.PersRewProposals[i].Order = (byte?)seq;
                }
            }

            Session[string.Format("department-{0}", CurrentUser.UserId)] = currentModel;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("UpdatePersRawPropPre")]
        public JsonResult UpdatePersRawPropPre(int Order)
        {
            DepartmentViewModel currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            UpfPersRewProposal viewItem = new UpfPersRewProposal();
            viewItem = currentModel.PersRewProposals.Where(t => t.Order == Order).FirstOrDefault();
            return Json(viewItem, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("UpdatePersRawProp")]
        public JsonResult UpdatePersRawProp(UpfPersRewProposal model)
        {
            var currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            currentModel.FirstLoad = false;
            currentModel.ErrorMessage = "";

            for (int i = 0; i < currentModel.PersRewProposals.Count; i++)
            {
                if (currentModel.PersRewProposals[i].Order == model.Order)
                {
                    currentModel.PersRewProposals[i] = model;
                    break;
                }
            }

            Session[string.Format("department-{0}", CurrentUser.UserId)] = currentModel;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("DeletePersRawProp")]
        public JsonResult DeletePersRawProp(int Order)
        {
            DepartmentViewModel currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            UpfPersRewProposal deleteItem = currentModel.PersRewProposals.Where(t => t.Order == Order).FirstOrDefault();
            currentModel.PersRewProposals.Remove(deleteItem);
            if (currentModel.PersRewProposals.Count > 0)
            {
                int i = 0;
                foreach (var persRewProp in currentModel.PersRewProposals)
                {
                    if (persRewProp.Order > 1)
                    {
                        i = Int32.Parse(persRewProp.Order.ToString());
                        i--;
                        persRewProp.Order = Byte.Parse(i.ToString());
                    }
                }
            }

            return Json(deleteItem, JsonRequestBehavior.AllowGet);
        }

        // GET: DepartmentView
        [HttpGet]
        [Route("DepartKPIsView")]
        public ActionResult View(int id)
        {
            DepartmentViewModel model = GetDepartmentById(id);

            return View(model);
        }

        public DepartmentViewModel GetDepartmentById(int id)
        {
            DepartmentViewModel model = new DepartmentViewModel();
            model.FirstLoad = true;
            model.ErrorMesages = new List<string>();
            model.ErrorMessage = "";

            //Init properties
            model.Upf = new Upf();
            model.NameDetails = new List<UpfNameDetail>();
            model.PersRewProposals = new List<UpfPersRewProposal>();
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

            List<UpfNameDetail> nameDetails = new List<UpfNameDetail>();
            List<UpfPersRewProposal> persRewPropDetail = new List<UpfPersRewProposal>();
            List<UpfComment> comments = new List<UpfComment>();
            //Upf
            model.Upf = departmentService.GetDepartmentInfo(id, ref nameDetails, ref persRewPropDetail, ref comments);
            // UserInfo
            model.UserInfo = userService.GetUserFullInfo((int)model.Upf.CreatedBy);
            model.ManagerInfo = userService.GetManagerByUserId((int)model.Upf.CreatedBy);
            //Nhóm công việc
            var completeWorkTitles = departmentService.CompleteWorkTitleByDepartment((int)model.UserInfo.DepartmentID);
            model.CompleteWorkTitles = new SelectList(completeWorkTitles, "ID", "Name");

            byte? stt = 0;
            int weightTotal = 0;
            List<string> listCheck = new List<string>();
            if (nameDetails.Count > 0)
            {
                model.NameDetails = nameDetails;
                foreach (var item in model.NameDetails)
                {
                    foreach (var itemJob in item.JobDetails)
                    {
                        itemJob.BodPoint = itemJob.ManagePoint;
                        weightTotal = weightTotal + Int32.Parse(itemJob.Weight.ToString());
                    }
                    listCheck.Add(item.WorkCompleteID.ToString());
                }
                model.Upf.WeightTotal = weightTotal;
            }
            model.Upf.TotalBODPoint = model.Upf.TotalManagePoint;

            foreach (var completeWorkTitle in completeWorkTitles)
            {
                if (!listCheck.Contains(completeWorkTitle.ID.ToString()))
                {
                    stt++;
                    model.NameDetails.Add(new UpfNameDetail() { Order = stt, WorkCompleteID = completeWorkTitle.ID, NameKPI = completeWorkTitle.Name });
                }
            }

            model.NameDetails = model.NameDetails.OrderBy(o => o.WorkCompleteID).ToList();
            if (persRewPropDetail.Count > 0) {
                model.PersRewProposals = persRewPropDetail;
            }

            //Comment
            model.UpfComments = new List<UpfComment>(comments.OrderBy(t => t.CreatedDate));
            int i = 0;
            foreach (var upfComment in model.UpfComments)
            {
                i++;
                upfComment.Order = i;
            }

            //Kỳ
            model.DepartSchedules = new SelectList(departmentService.DepartSchedulesByDeparment((int)model.UserInfo.DepartmentID, (int)model.UserInfo.CompanyID), "ID", "Name");

            model.UpfRates = new SelectList(departRateService.GetUpfRateList(), "ID", "Name");
            model.Approves = new SelectList(userService.GetListManageInfo((int)model.UserInfo.AdministratorshipID), "ID", "FullName", model.Upf.ApproveBy);
            model.BODApproves = new SelectList(userService.GetListManageInfo((int)model.UserInfo.AdministratorshipID), "ID", "FullName", model.Upf.BodApproved);

            return model;
        }

        [HttpPost]
        [Route("DepartKPIsDelete")]
        public ActionResult DepartmentDelete(DepartmentListViewModel model)
        {
            DepartmentListViewModel currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentListViewModel;
            if (model.ID != null)
            {
                departmentService.deleteDepartment(Int32.Parse(model.ID), CurrentUser.UserId);
            }
            model.DepartmentInfos = departmentService.GetDepartByConditions(model.CompanyID, model.DepartmentID, model.ScheduleType, model.Year, model.ScheduleID, model.StatusID);

            return PartialView("_DepartmentListTable", model);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult UpdateManagePoint(UpfJobDetail model)
        {
            var currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            decimal? totalManagePoint = new decimal();
            foreach (var item in currentModel.NameDetails)
            {
                if(item.JobDetails != null && item.JobDetails.Count > 0)
                {
                    foreach (var jobDetail in item.JobDetails)
                    {
                        if(model.ManagePoint != null)
                        {
                            if (jobDetail.ID == model.ID)
                            {
                                jobDetail.ManagePoint = model.ManagePoint;
                                break;
                            }
                        }
                    }

                    foreach (var bo in item.JobDetails)
                    {
                        if(bo.ManagePoint != null)
                        {
                            totalManagePoint = totalManagePoint + ((bo.ManagePoint*bo.Weight)/100);
                        }
                    }
                }
            }

            currentModel.Upf.TotalManagePoint = totalManagePoint;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult UpdateBODPoint(UpfJobDetail model)
        {
            var currentModel = Session[string.Format("department-{0}", CurrentUser.UserId)] as DepartmentViewModel;
            decimal? totalBodPoint = new decimal();
            foreach (var item in currentModel.NameDetails)
            {
                if (item.JobDetails != null && item.JobDetails.Count > 0)
                {
                    foreach (var jobDetail in item.JobDetails)
                    {
                        if(model.BodPoint != null)
                        {
                            if (jobDetail.ID == model.ID)
                            {
                                jobDetail.BodPoint = model.BodPoint;
                                break;
                            }
                        }
                    }

                    foreach (var bo in item.JobDetails)
                    {
                        if(bo.BodPoint != null)
                        {
                            totalBodPoint = totalBodPoint + ((bo.BodPoint * bo.Weight)/100);
                        }
                    }
                }
            }

            currentModel.Upf.TotalBODPoint = totalBodPoint;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("DepartKPIsDownload")]
        public ActionResult ExportToExcel(int id)
        {
            DepartmentViewModel model = GetDepartmentById(id);

            //Save the workbook to disk in xlsx format
            string dateStr = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy_hhmmss");
            var fileDownloadName = "UPF_" + model.UserInfo.DepartmentEnName + "_" + dateStr + ".xlsx";
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

        public ExcelPackage FillDataTableExcel(DepartmentViewModel model)
        {
            ExcelPackage excelEngine = new ExcelPackage();
            ExcelWorksheet worksheet = excelEngine.Workbook.Worksheets.Add("UPF-Y");

            //Enter values to the cells from A3 to A5

            worksheet.Cells["A1:B1"].Merge = true;
            worksheet.Cells["A2:B2"].Merge = true;
            worksheet.Cells["A1"].Value = "COMPANY: " + model.UserInfo.CompanyName;
            worksheet.Cells["A2"].Value = "DEPARTMENT: " + model.UserInfo.DepartmentName;
            //Make the text bold
            worksheet.Cells["A1:K6"].Style.Font.Bold = true;
            //Merge cells
            worksheet.Cells["A3"].Value = "DEPARTMENT PERFORMANCE MANAGEMENT AND ACCESSMENT \r\nQUẢN LÝ VÀ ĐÁNH GIÁ KẾT QUẢ CÔNG VIỆC PHÒNG/BỘ PHẬN";
            worksheet.Cells["J4"].Value = "Năm:";
            worksheet.Cells["K4"].Value = model.Upf.Year.ToString();
            worksheet.Row(3).Height = 40;
            worksheet.Cells["A3:K3"].Style.WrapText = true;
            worksheet.Cells["A3:K3"].Merge = true;
            // format cells - add borders H4:I4
            worksheet.Cells["J4:K4"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["J4:K4"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["J4:K4"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["J4:K4"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            //Merge cells
            worksheet.Cells["A5:A6"].Merge = true;
            worksheet.Cells["B5:B6"].Merge = true;
            worksheet.Cells["C5:C6"].Merge = true;
            worksheet.Cells["D5:D6"].Merge = true;
            worksheet.Cells["E5:E6"].Merge = true;
            worksheet.Cells["F5:I5"].Merge = true;

            worksheet.Cells["A5"].Value = "No.\r\n(1)";
            worksheet.Cells["B5"].Value = "KPI/Objective\r\nKPI/Mục tiêu\r\n(2)";
            worksheet.Cells["C5"].Value = "Estimated time of completion \r\nThời gian dự kiến hoàn thành\r\n(3)";
            worksheet.Cells["D5"].Value = "Target/ requirements\r\nSố kế hoạch / yêu cầu cụ thể\r\n(4)";
            worksheet.Cells["E5"].Value = "Results /Kết quả thực hiện (5)";
            worksheet.Cells["F5"].Value = "Assessment";
            worksheet.Cells["F6"].Value = "Weight / Tỷ trọng\r\n(6)";
            worksheet.Cells["G6"].Value = "Self-assessment mark by the unit \r\n(7)";
            worksheet.Cells["H6"].Value = "Assessment management\r\n(8)";
            worksheet.Cells["I6"].Value = "Total mark\r\n(9)=(8)*(6)";
            worksheet.Cells["J6"].Value = "BOD Assessment\r\n(10)";
            worksheet.Cells["K6"].Value = "Total BOD mark\r\n(11)=(10)*(6)";
            int col = 6;
            int? totalWeight = 0;
            for (int i = 0; i < model.NameDetails.Count; i++)
            {
                col++;
                worksheet.Cells["A" + col].Value = BtcHelper.ToRoman(model.NameDetails[i].Order);
                worksheet.Cells["B" + col + ":K" + col].Merge = true;
                worksheet.Cells["B" + col].Value = model.NameDetails[i].NameKPI.Trim();
                worksheet.Cells["A" + col + ":A" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A" + col + ":K" + col].Style.Font.Bold = true;
                if(model.NameDetails[i].JobDetails != null && model.NameDetails[i].JobDetails.Count > 0)
                {
                    for (int j = 0; j < model.NameDetails[i].JobDetails.Count; j++)
                    {
                        col++;
                        worksheet.Cells["A" + col].Value = model.NameDetails[i].JobDetails[j].Order.ToString();
                        worksheet.Cells["B" + col].Value = model.NameDetails[i].JobDetails[j].JobName.Trim();
                        worksheet.Cells["C" + col].Value = model.NameDetails[i].JobDetails[j].ScheduleTimeString;
                        worksheet.Cells["D" + col].Value = model.NameDetails[i].JobDetails[j].NumberPlan;
                        worksheet.Cells["E" + col].Value = model.NameDetails[i].JobDetails[j].PerformResults;
                        worksheet.Cells["F" + col].Value = model.NameDetails[i].JobDetails[j].Weight.ToString();
                        worksheet.Cells["G" + col].Value = model.NameDetails[i].JobDetails[j].Point.ToString();
                        worksheet.Cells["H" + col].Value = model.NameDetails[i].JobDetails[j].ManagePoint.ToString();
                        decimal? total = (model.NameDetails[i].JobDetails[j].Weight * model.NameDetails[i].JobDetails[j].ManagePoint) / 100;
                        worksheet.Cells["I" + col].Value = total.ToString();
                        worksheet.Cells["J" + col].Value = model.NameDetails[i].JobDetails[j].BodPoint.ToString();
                        decimal? totalBOD = (model.NameDetails[i].JobDetails[j].Weight * model.NameDetails[i].JobDetails[j].BodPoint) / 100;
                        worksheet.Cells["K" + col].Value = totalBOD.ToString();
                        totalWeight = totalWeight + model.NameDetails[i].JobDetails[j].Weight;
                        worksheet.Cells["A" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["B" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        worksheet.Cells["C" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["D" + col + ":E" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        worksheet.Cells["F" + col + ":K" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                }
            }
            col++;
            worksheet.Cells["A" + col + ":E" + col].Merge = true;
            worksheet.Cells["A" + col].Value = "Total /Tổng cộng";
            worksheet.Cells["F" + col].Value = totalWeight.ToString();
            worksheet.Cells["I" + col].Value = model.Upf.TotalManagePoint.ToString();
            worksheet.Cells["K" + col].Value = model.Upf.TotalBODPoint.ToString();
            worksheet.Cells["A" + col + ":K" + col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["A" + col + ":K" + col].Style.Font.Bold = true;
            worksheet.Cells["A4:K" + col].Style.WrapText = true;
            // format cells - add borders A5:I...
            worksheet.Cells["A5:K" + col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A5:K" + col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A5:K" + col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A5:K" + col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            //bo qua 1 dong
            int lastRow = col + 2;
            //Thanh tich noi bat
            worksheet.Cells["A" + lastRow].Value = "C";
            worksheet.Cells["B" + lastRow].Value = "Thành tích nổi bật của Bộ phận:";
            int rowOutAchi = lastRow + 1;
            worksheet.Cells["A" + rowOutAchi].Value = "1";
            worksheet.Cells["B" + rowOutAchi].Value = model.Upf.OutsAchiev;
            worksheet.Row(rowOutAchi).Height = 60;
            worksheet.Cells["B" + rowOutAchi + ":K" + rowOutAchi].Style.WrapText = true;
            worksheet.Cells["B" + rowOutAchi + ":K" + rowOutAchi].Merge = true;
            worksheet.Cells["A" + lastRow + ":A" + rowOutAchi].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["B" + lastRow + ":K" + rowOutAchi].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells["A" + lastRow + ":B" + lastRow].Style.Font.Bold = true;
            // format cells - add borders A5:I...
            worksheet.Cells["A" + lastRow + ":K" + lastRow].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow + ":B" + lastRow].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow + ":B" + lastRow].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["K" + lastRow].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow + ":B" + lastRow].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + rowOutAchi + ":K" + rowOutAchi].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + rowOutAchi + ":K" + rowOutAchi].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + rowOutAchi + ":K" + rowOutAchi].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + rowOutAchi + ":K" + rowOutAchi].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            //bo qua 1 dong
            int lastRow1 = rowOutAchi + 2;
            //Danh gia xep loai bo phan
            worksheet.Cells["A" + lastRow1].Value = "D";
            worksheet.Cells["B" + lastRow1].Value = "Tự đánh giá xếp loại Bộ phận:";
            worksheet.Cells["A" + lastRow1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["B" + lastRow1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            int lastRow2 = lastRow1 + 1;
            worksheet.Cells["B" + lastRow2].Value = "Xếp loại:";
            worksheet.Cells["C" + lastRow2].Value = model.Upf.SelfRatingName;
            worksheet.Cells["B" + lastRow2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            worksheet.Cells["C" + lastRow2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells["A" + lastRow1 + ":K" + lastRow2].Style.Font.Bold = true;
            // format cells - add borders A...:I...
            worksheet.Cells["A" + lastRow1 + ":K" + lastRow1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow1 + ":B" + lastRow1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow1 + ":B" + lastRow1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["K" + lastRow1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow1 + ":K" + lastRow1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow2 + ":K" + lastRow2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["K" + lastRow2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow2 + ":K" + lastRow2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            //bo qua 1 dong
            int lastRow3 = lastRow2 + 2;
            //De xuat ca nhan duoc khen thuong
            worksheet.Cells["A" + lastRow3].Value = "E";
            worksheet.Cells["B" + lastRow3].Value = "Đề xuất cá nhân được vinh danh";
            worksheet.Cells["A" + lastRow3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["B" + lastRow3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            // format cells - add borders A...:I...
            worksheet.Cells["A" + lastRow3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow3].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["B" + lastRow3 + ":K" + lastRow3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["B" + lastRow3].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["K" + lastRow3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["B" + lastRow3 + ":K" + lastRow3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            int lastRow4 = lastRow3 + 1;
            worksheet.Cells["A" + lastRow4].Value = "STT";
            worksheet.Cells["B" + lastRow4].Value = "Tên nhân viên";
            worksheet.Cells["C" + lastRow4 + ":K" + lastRow4].Merge = true;
            worksheet.Cells["C" + lastRow4].Value = "Thành tích đạt được";
            worksheet.Cells["A" + lastRow4 + ":K" + lastRow4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["A" + lastRow3 + ":K" + lastRow4].Style.Font.Bold = true;
            // format cells - add borders A...:I...
            worksheet.Cells["A" + lastRow4 + ":K" + lastRow4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow4 + ":K" + lastRow4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow4 + ":K" + lastRow4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow4 + ":K" + lastRow4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            for (int i = 0; i < model.PersRewProposals.Count; i++)
            {
                lastRow4++;
                worksheet.Cells["A" + lastRow4].Value = model.PersRewProposals[i].Order.ToString();
                worksheet.Cells["B" + lastRow4].Value = model.PersRewProposals[i].EmployeeName;
                worksheet.Cells["C" + lastRow4].Value = model.PersRewProposals[i].PersOutsAchiev;
                worksheet.Row(lastRow4).Height = 80;
                worksheet.Cells["C" + lastRow4 + ":K" + lastRow4].Style.WrapText = true;
                worksheet.Cells["C" + lastRow4 + ":K" + lastRow4].Merge = true;
                worksheet.Cells["A" + lastRow4 + ":B" + lastRow4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["C" + lastRow4 + ":K" + lastRow4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            }
            // format cells - add borders A...:I...
            worksheet.Cells["A" + lastRow4 + ":K" + lastRow4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow4 + ":K" + lastRow4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow4 + ":K" + lastRow4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A" + lastRow4 + ":K" + lastRow4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            int lastRow5 = lastRow4 + 3;
            worksheet.Cells["A" + lastRow5 + ":B" + lastRow5].Merge = true;
            worksheet.Cells["A" + lastRow5].Value = "ASSESSMENT COUNCIL CHAIRMAN";
            worksheet.Cells["F" + lastRow5 + ":H" + lastRow5].Merge = true;
            worksheet.Cells["F" + lastRow5].Value = "HEAD OF UNIT";
            worksheet.Cells["A" + lastRow5 + ":B" + lastRow5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["F" + lastRow5 + ":H" + lastRow5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["A" + lastRow5 + ":B" + lastRow5].Style.Font.Bold = true;
            worksheet.Cells["F" + lastRow5 + ":H" + lastRow5].Style.Font.Bold = true;

            worksheet.Cells["A3:I3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["H4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            worksheet.Cells["I4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells["A5:I6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["A1:K" + lastRow5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //Apply row height and column width to look good
            worksheet.Column(1).Width = 5;
            worksheet.Column(2).Width = 60;
            worksheet.Column(3).Width = 15;
            worksheet.Column(4).Width = 20;
            worksheet.Column(5).Width = 20;
            worksheet.Column(6).Width = 12;
            worksheet.Column(7).Width = 12;
            worksheet.Column(8).Width = 12;
            worksheet.Column(9).Width = 12;
            worksheet.Column(10).Width = 12;
            worksheet.Column(11).Width = 12;

            return excelEngine;
        }
    }
}
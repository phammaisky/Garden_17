using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Antlr.Runtime.Misc;
using BtcKpi.Model;
using BtcKpi.Model.Enum;
using BtcKpi.Service;
using BtcKpi.Service.Common;
using BtcKpi.Web.ViewModels;

namespace BtcKpi.Web.Controllers
{
    public class UpfController : BaseController
    {
        #region Constructor
        private readonly IUserService userService;
        private readonly IUpfCrossService upfCrossService;
        private readonly IDepartmentService departmentService;

        public UpfController(IUserService userService, IUpfCrossService upfCrossService, IDepartmentService departmentService)
        {
            this.userService = userService;
            this.upfCrossService = upfCrossService;
            this.departmentService = departmentService;
        }
        #endregion Constructor

        #region Data
        public UpfCrossViewModel ModelNew()
        {
            UpfCrossViewModel model = new UpfCrossViewModel();
            model.FirstLoad = true;
            model.ErrorMessage = "";

            //Init properties
            model.UpfCross = new UpfCross();
            model.Detail = new UpfCrossDetail();
            model.UpfCrossDetails = new List<UpfCrossDetail>();
            //model.UpfCrossDetails.Add(upfCrossService.GetUpfCrossDetailById(1));

            // UserInfo
            model.UserID = CurrentUser.UserId;
            model.UserInfo = userService.GetUserFullInfo(CurrentUser.UserId);
            model.ManagerInfo = userService.GetManagerByUserId(CurrentUser.UserId);

            //Year & Month
            InitYearMonth(model);

            //Deparments
            model.Departments = new SelectList(userService.GetDepartmentCrossByUser(CurrentUser.UserId), "ID", "Name");
            //model.Departments = new SelectList(departmentService.DepartSchedulesByDeparment((int)model.UserInfo.DepartmentID, model.UserInfo.CompanyID), "ID", "Name");

            model.Detail.FromDepartment = model.UserInfo.DepartmentID;
            model.Detail.FromName = model.UserInfo.DepartmentName;

            return model;
        }

        public UpfCrossViewModel GetById(int id)
        {
            UpfCrossViewModel model = new UpfCrossViewModel();
            model.FirstLoad = true;
            model.ErrorMessage = "";

            //Init properties
            model.UpfCross = new UpfCross();
            model.Detail = new UpfCrossDetail();
            model.UpfCrossDetails = new List<UpfCrossDetail>();

            // UserInfo
            model.UserInfo = userService.GetUserFullInfo(CurrentUser.UserId);
            model.ManagerInfo = userService.GetManagerByUserId(CurrentUser.UserId);

            //Year & Month
            InitYearMonth(model);

            //Deparments
            model.Departments = new SelectList(userService.GetDepartmentCrossByUser(CurrentUser.UserId), "ID", "Name");

            //Data
            model.UserID = CurrentUser.UserId;
            model.Detail = upfCrossService.GetUpfCrossDetailById(id);

            return model;
        }

        public void InitYearMonth(UpfCrossViewModel model)
        {
            model.Years = Years;
            model.Months = Months;
        }
        #endregion Data

        #region CRUD
        // GET: UpfCross-Delete
        /// <summary>
        /// Chỉnh sửa Upf Cross
        /// </summary>
        /// <returns></returns>
        public ActionResult CrossCreate()
        {
            var model = ModelNew();
            return View(model);
        }

        [HttpPost]
        public ActionResult CrossCreate(UpfCrossViewModel model)
        {
            string insertMsg = "";
            model.ErrorMesage = "";

            if (string.IsNullOrEmpty(model.Year))
            {
                model.ErrorMesage += " Vui lòng chọn năm.";
            }
            if (string.IsNullOrEmpty(model.Month))
            {
                model.ErrorMesage += " Vui lòng chọn tháng.";
            }
            

            int year = 0;
            int month = 0;
            int.TryParse(model.Year, out year);
            int.TryParse(model.Month, out month);
            model.UpfCross = new UpfCross() {Year = year, Month = (byte?)month, DepartmentID = model.UserInfo.DepartmentID};
            var deparments = userService.GetDepartmentCrossByUser(CurrentUser.UserId);

            if (upfCrossService.IsDupllicateUpfCrossDetail(year, (byte?)month, model.Detail.FromDepartment,
                model.Detail.ToDepartment))
            {
                model.ErrorMesage += string.Format("Bạn đã tạo đánh giá chéo tháng {0} năm {1} cho {2}!", month, year, deparments.FirstOrDefault(t => t.Id == model.Detail.ToDepartment).Name);
            }
            if (string.IsNullOrEmpty(model.ErrorMesage) && upfCrossService.CreateUpfCrossDetail((int)model.UserID, model.UpfCross, model.Detail, ref insertMsg))
            {
                return RedirectToAction("Cross", "Upf");
            }

            // UserInfo
            model.UserInfo = userService.GetUserFullInfo(CurrentUser.UserId);
            model.ManagerInfo = userService.GetManagerByUserId(CurrentUser.UserId);
            //Year & Month
            InitYearMonth(model);
            //Deparments
            model.Departments = new SelectList(userService.GetDepartmentCrossByUser(CurrentUser.UserId), "ID", "Name");
            return View(model);
        }

        // GET: UpfCross
        /// <summary>
        /// Danh sách Upf Cross
        /// </summary>
        /// <returns></returns>
        public ActionResult Cross()
        {
            UpfCrossListViewModel model = new UpfCrossListViewModel();
            model.Status = ""; //Mới tạo
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

            model.Months = Months;
            model.Month = (DateTime.Now.Month - 1).ToString();


            //Status
            var upfCrossStatus = new List<ConvertEnum>();
            foreach (var status in Enum.GetValues(typeof(UpfCrossStatus)))
                upfCrossStatus.Add(new ConvertEnum
                {
                    Value = (int)status,
                    Text = status.ToString() == "New" ? "Chưa phản hồi" : "Đã phản hồi"
                });
            model.StatusListItems = new SelectList(upfCrossStatus, "Value", "Text");

            model.UpfCrossInfos = new List<UpfCrossInfo>();
            return View(model);
        }

        // GET: UpfCross-Create
        /// <summary>
        /// Tạo mới Upf Cross
        /// </summary>
        /// <returns></returns>
        public ActionResult CrossCreatev2()
        {
            var model = ModelNew();
            return View(model);
        }

        // POST: UpfCross-Create
        /// <summary>
        /// Tạo mới Upf Cross
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CrossCreatev2(UpfCrossViewModel model)
        {
            string insertMsg = "";
            
            if (model.UpfCrossDetails == null || !model.UpfCrossDetails.Any())
            {
                model.UpfCross = new UpfCross();
                model.Years = Years;
                model.Months = Months;
                model.Departments = new SelectList(userService.GetDepartmentCrossByUser(CurrentUser.UserId), "ID", "Name");
                model.ErrorMessage = "Chưa có đánh giá nào!";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                //Kiểm tra dữ liệu

                //Điều chỉnh dữ liệu
                model.UpfCross = new UpfCross();
                model.UpfCross.Year = (int?)Convert.ToInt16(model.Year) ;
                model.UpfCross.Month = (byte?)Convert.ToInt16(model.Month);
                if (model.UpfCrossDetails != null && model.UpfCrossDetails.Any())
                {
                    foreach (var detail in model.UpfCrossDetails)
                    {
                        detail.FromDepartment = model.UserInfo.DepartmentID;
                    }
                }

                //Insert
                if (upfCrossService.AddUpfCross(CurrentUser.UserId, model.UpfCross, model.UpfCrossDetails, ref insertMsg))
                {
                    return RedirectToAction("Cross", "Upf");
                }
                else
                {
                    model.UpfCross = new UpfCross();
                    model.Years = Years;
                    model.Months = Months;
                    model.Departments = new SelectList(userService.GetDepartmentCrossByUser(CurrentUser.UserId), "ID", "Name");
                    model.ErrorMessage = insertMsg;
                    return View(model);
                }
            }
            else
            {
                model.UpfCross = new UpfCross();
                model.Years = Years;
                model.Months = Months;
                model.Departments = new SelectList(userService.GetDepartmentCrossByUser(CurrentUser.UserId), "ID", "Name");
                model.ErrorMessage = string.Join(" - ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));
                return View(model);
            }
        }

        // GET: UpfCross-View
        /// <summary>
        /// Xem Upf Cross
        /// </summary>
        /// <returns></returns>
        public ActionResult CrossView(int id)
        {
            var model = GetById(id);
            return View(model);
        }

        // GET: UpfCross-Delete
        /// <summary>
        /// Chỉnh sửa Upf Cross
        /// </summary>
        /// <returns></returns>
        public ActionResult CrossEdit(int id)
        {
            var model = GetById(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult CrossEdit(UpfCrossViewModel model)
        {
            string insertMsg = "";

            if(upfCrossService.UpdateUpfCrossDetail((int)model.UserID, model.Detail, ref insertMsg))
            {
                return RedirectToAction("Cross", "Upf");
            }
            
            // UserInfo
            model.UserInfo = userService.GetUserFullInfo(CurrentUser.UserId);
            model.ManagerInfo = userService.GetManagerByUserId(CurrentUser.UserId);
            //Year & Month
            InitYearMonth(model);
            //Deparments
            model.Departments = new SelectList(userService.GetDepartmentCrossByUser(CurrentUser.UserId), "ID", "Name");
            return View(model);
        }

        

        // GET: UpfCross-Approve
        /// <summary>
        /// Đánh giá Upf Cross
        /// </summary>
        /// <returns></returns>
        public ActionResult CrossApprove()
        {
            return View();
        }

        // GET: UpfCross-Delete
        /// <summary>
        /// Xóa Upf Cross
        /// </summary>
        /// <returns></returns>
        public ActionResult CrossDelete(int id)
        {
            var model = GetById(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult CrossDelete(UpfCrossViewModel model)
        {
            string insertMsg = "";

            if (upfCrossService.DeleteUpfCrossDetail((int)model.UserID, model.Detail.ID, ref insertMsg))
            {
                return RedirectToAction("Cross", "Upf");
            }

            // UserInfo
            model.UserInfo = userService.GetUserFullInfo(CurrentUser.UserId);
            model.ManagerInfo = userService.GetManagerByUserId(CurrentUser.UserId);
            //Year & Month
            InitYearMonth(model);
            //Deparments
            model.Departments = new SelectList(userService.GetDepartmentCrossByUser(CurrentUser.UserId), "ID", "Name");
            return View(model);
        }

        #endregion ActionResult

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
        public ActionResult Search(UpfCrossListViewModel model)
        {
            model.UserID = CurrentUser.UserId;
            model.UpfCrossInfos = upfCrossService.GetUpfCrossByConditions(CurrentUser.UserId, model.UserDepartmentID, model.Status, model.CompanyID, model.FromDepartmentID, model.ToDepartmentID, model.Year, model.Month);
            
            return PartialView("_UpfCrossTable", model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult StartCreateCross(UpfCross upf)
        {
            UpfCrossViewModel model = new UpfCrossViewModel();
            model.UpfCrossDetails = new List<UpfCrossDetail>();
            return PartialView("_UpfCrossDetailTable", model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddUpfCrossDetail(IEnumerable<UpfCrossDetail> items)
        {
            var model = new UpfCrossViewModel();
            byte seq = 0;
            //Re indexed
            foreach (var item in items)
            {
                seq++;
                item.Seq = seq;
            }
            model.UpfCrossDetails = items.ToList();
            return PartialView("_UpfCrossDetailTable", model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateUpfCrossDetail(IEnumerable<UpfCrossDetail> items)
        {
            var model = new UpfCrossViewModel();
            byte seq = 0;
            //Re indexed
            foreach (var item in items)
            {
                seq++;
                item.Seq = seq;
            }
            model.UpfCrossDetails = items.ToList();
            return PartialView("_UpfCrossDetailTable", model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult DeleteUpfCrossDetail(IEnumerable<UpfCrossDetail> items)
        {
            var model = new UpfCrossViewModel();
            byte seq = 0;
            //Re indexed
            foreach (var item in items)
            {
                seq++;
                item.Seq = seq;
            }
            model.UpfCrossDetails = items.ToList();
            return PartialView("_UpfCrossDetailTable", model);
        }
        #endregion Ajax

    }
}
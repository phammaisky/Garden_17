using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtcKpi.Model;

namespace BtcKpi.Web.ViewModels
{
    public class DepartmentListViewModel : BaseViewModel
    {
        public User UserInfo { get; set; }
        [Display(Name = "Loại:")]
        public string ScheduleType { get; set; }
        [Display(Name = "Năm:")]
        public string Year { get; set; }
        [Display(Name = "Kỳ:")]
        public string ScheduleID { get; set; }

        [Display(Name = "Công ty:")]
        public string CompanyID { get; set; }
        [Display(Name = "Phòng ban:")]
        public string DepartmentID { get; set; }

        public IEnumerable<SelectListItem> ScheduleTypes { get; set; }
        public IEnumerable<SelectListItem> Years { get; set; }
        public IEnumerable<SelectListItem> DepartSchedules { get; set; }
        public IEnumerable<SelectListItem> Companies { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> Status { get; set; }
        [Display(Name = "Trạng thái:")]
        public string StatusID { get; set; }
        public List<DepartmentInfo> DepartmentInfos { get; set; }
        public string ID { get; set; }
        public int UserID { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtcKpi.Model;

namespace BtcKpi.Web.ViewModels
{
    public class PerformanceViewModel : BaseViewModel
    {
        public string Action { get; set; }
        public User UserInfo { get; set; }

        public User ManagerInfo { get; set; }

        public List<string> ErrorMesages { get; set; }

        public IEnumerable<SelectListItem> Projects { get; set; }
        public IEnumerable<SelectListItem> TypePerformances { get; set; }
        public IEnumerable<SelectListItem> ScheduleTypes { get; set; }
        public IEnumerable<SelectListItem> Years { get; set; }
        public IEnumerable<SelectListItem> Months { get; set; }
        public IEnumerable<SelectListItem> QuarterTypes { get; set; }
        public IEnumerable<SelectListItem> TypeFBs { get; set; }

        public PerformanceLSFB PerformanceLsfb{ get; set; }

        public string ScheduleType { get; set; }
        public string Year { get; set; }
        public string ScheduleId { get; set; }
        public string ProjectID { get; set; }
        public string TypePerformanceID { get; set; }
        public string QuarterId { get; set; }
        public string Comment { get; set; }
        public string TypeFBId { get; set; }
        public IEnumerable<SelectListItem> Approves { get; set; }
        public bool isApprove { get; set; }

    }
}
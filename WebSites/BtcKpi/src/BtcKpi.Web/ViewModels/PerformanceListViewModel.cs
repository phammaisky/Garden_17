using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtcKpi.Model;

namespace BtcKpi.Web.ViewModels
{
    public class PerformanceListViewModel : BaseViewModel
    {
        public User UserInfo { get; set; }
        public string ScheduleType { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string ScheduleID { get; set; }
        public string ProjectID { get; set; }
        public string TypePerformanceID { get; set; }
        public string TypeFBID { get; set; }
        public IEnumerable<SelectListItem> ScheduleTypes { get; set; }
        public IEnumerable<SelectListItem> Years { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }
        public IEnumerable<SelectListItem> TypePerformances { get; set; }
        public IEnumerable<SelectListItem> Months { get; set; }
        public IEnumerable<SelectListItem> TypeFBs { get; set; }
        public List<PerformanceInfo> PerformanceInfos { get; set; }
        public string ID { get; set; }
        public int UserID { get; set; }
        public int ShowFormByProjType { get; set; }
    }
}
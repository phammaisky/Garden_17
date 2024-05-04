using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtcKpi.Model;

namespace BtcKpi.Web.ViewModels
{
    public class ReportListViewModel
    {
        public string ErrorMessage { get; set; }
        public int UserID { get; set; }
        public int UserDepartmentID { get; set; }
        public string CompanyID { get; set; }
        public string DepartmentID { get; set; }
        public string Year { get; set; }
        public string ScheduleID { get; set; }
        public string ScheduleType { get; set; }
        public string StatusID { get; set; }

        public IEnumerable<SelectListItem> Companies { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> Years { get; set; }
        public IEnumerable<SelectListItem> DepartSchedules { get; set; }
        public IEnumerable<SelectListItem> ScheduleTypes { get; set; }
        public IEnumerable<SelectListItem> Status { get; set; }
        public List<UpfReport> UpfReports { get; set; }
        public List<DepartmentInfo> DepartmentInfos { get; set; }
        public Upf Upf { get; set; }
        public List<IpfReportInfo> IpfReportInfos { get; set; }
    }
}
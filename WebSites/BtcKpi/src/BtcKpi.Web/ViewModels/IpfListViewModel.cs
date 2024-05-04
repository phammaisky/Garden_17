using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtcKpi.Model;

namespace BtcKpi.Web.ViewModels
{
    public class IpfListViewModel
    {
        public Nullable<int> UserID { get; set; }
        public string Status { get; set; }
        public User UserInfo { get; set; }
        public string ScheduleType { get; set; }
        public string Year { get; set; }
        public string ScheduleID { get; set; }
        public string CompanyID { get; set; }
        public string DepartmentID { get; set; }

        public IEnumerable<SelectListItem> ScheduleTypes { get; set; }
        public IEnumerable<SelectListItem> Years { get; set; }
        public IEnumerable<SelectListItem> IpfSchedules { get; set; }
        public IEnumerable<SelectListItem> Companies { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> IpfStatus { get; set; }
        public string FullName { get; set; }
        public List<IpfInfo> IpfInfos { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtcKpi.Model;

namespace BtcKpi.Web.ViewModels
{
    public class UpfCrossListViewModel
    {
        //public User UserInfo { get; set; }
        public int UserID { get; set; }
        public int UserDepartmentID { get; set; }
        public string CompanyID { get; set; }
        public string FromDepartmentID { get; set; }
        public string ToDepartmentID { get; set; }
        public string Status { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string DepartmentID { get; set; }

        public IEnumerable<SelectListItem> Companies { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> StatusListItems { get; set; }
        public IEnumerable<SelectListItem> Years { get; set; }
        public IEnumerable<SelectListItem> Months { get; set; }
        public List<UpfCrossInfo> UpfCrossInfos { get; set; }
    }
}
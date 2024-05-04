using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtcKpi.Model;

namespace BtcKpi.Web.ViewModels
{
    public class UpfMonthViewModel
    {
        public string ErrorMessage { get; set; }
        public int UserID { get; set; }
        public int DepartmentID { get; set; }
        public int Year { get; set; }
        public List<Department> Departments { get; set; }
        public List<UpfSchedule> Schedules { get; set; }
        public List<UpfMonthItem> UpfMonthItems { get; set; }
    }
}
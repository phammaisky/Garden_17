using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtcKpi.Model;

namespace BtcKpi.Web.ViewModels
{
    public class UpfCrossViewModel : BaseViewModel
    {
        public Nullable<int> UserID { get; set; }
        public string Action { get; set; }
        public User UserInfo { get; set; }

        public User ManagerInfo { get; set; }

        public string ErrorMesage { get; set; }

        public IEnumerable<SelectListItem> Years { get; set; }
        public IEnumerable<SelectListItem> Months { get; set; }
        public UpfCross UpfCross { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public UpfCrossDetail Detail { get; set; }
        public List<UpfCrossDetail> UpfCrossDetails { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public int FromWeightTotal { get; set; }
        public string DetailAction { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtcKpi.Model;

namespace BtcKpi.Web.ViewModels
{
    public class DepartmentViewModel : BaseViewModel
    {
        public string Action { get; set; }
        public User UserInfo { get; set; }

        public User ManagerInfo { get; set; }

        public List<string> ErrorMesages { get; set; }

        public IEnumerable<SelectListItem> ScheduleTypes { get; set; }

        public IEnumerable<SelectListItem> Years { get; set; }

        public IEnumerable<SelectListItem> DepartSchedules { get; set; }

        public Upf Upf { get; set; }

        public string ScheduleType { get; set; }
        public string Year { get; set; }
        public string ScheduleID { get; set; }

        public List<UpfNameDetail> NameDetails { get; set; }

        public List<UpfPersRewProposal> PersRewProposals { get; set; }

        public IEnumerable<SelectListItem> UpfRates { get; set; }

        public UpfRate UpfRate { get; set; }

        public bool isApprove { get; set; }
        public bool isBODApprove { get; set; }
        public bool isComment { get; set; }

        public string Comment { get; set; }

        public UpfComment UpfComment { get; set; }
        public List<UpfComment> UpfComments { get; set; }
        public IEnumerable<SelectListItem> Approves { get; set; }
        public IEnumerable<SelectListItem> BODApproves { get; set; }
        public IEnumerable<SelectListItem> CompleteWorkTitles { get; set; }

    }
}
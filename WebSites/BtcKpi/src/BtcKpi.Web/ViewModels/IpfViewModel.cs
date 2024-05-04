using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtcKpi.Model;

namespace BtcKpi.Web.ViewModels
{
    public class IpfViewModel : BaseViewModel
    {
        public Nullable<int> UserID { get; set; }
        public string Action { get; set; }
        public User UserInfo { get; set; }

        public User ManagerInfo { get; set; }

        public List<string> ErrorMesages { get; set; }

        public IEnumerable<SelectListItem> ScheduleTypes { get; set; }

        public IEnumerable<SelectListItem> Years { get; set; }

        public IEnumerable<SelectListItem> IpfSchedules { get; set; }

        public IEnumerable<SelectListItem> CompleteWorkTitles { get; set; }

        public Ipf Ipf { get; set; }

        public string ScheduleType { get; set; }
        public string Year { get; set; }
        public string ScheduleID { get; set; }

        public string CurrentTab { get; set; }

        public List<IpfDetail> CompleteWorks { get; set; }

        public int CompleteWorkWeightToal { get; set; }

        public List<IpfDetail> Competencies { get; set; }

        public int CompetencyWeightToal { get; set; }

        public List<PersonalPlan> PersonalPlanCompetencies { get; set; }

        public List<PersonalPlan> PersonalPlanCareers { get; set; }

        public List<IpfDetail> CompleteWorksNextYear { get; set; }

        public int CompleteWorkNextWeightToal { get; set; }

        public List<IpfDetail> CompetenciesNextYear { get; set; }

        public int CompetencyNextWeightToal { get; set; }

        public string Comment { get; set; }

        public IpfComment IpfComment { get; set; }

        public List<IpfComment> IpfComments { get; set; }

        public IEnumerable<SelectListItem> BODApproves { get; set; }
    }
}
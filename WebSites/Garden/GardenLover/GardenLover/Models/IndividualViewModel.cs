using IQWebApp_Blank.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IQWebApp_Blank.Models
{
    public class IndividualVM
    {
        public kReport Report { get; set; }
        public cUserInfo Reporter { get; set; }

        public List<JobAndDetailVM> allJobAndDetail { get; set; } = new List<JobAndDetailVM>();
        public int TotalAddedJob { get; set; } = 0;
        public int JobNumber { get; set; } = 0;
        public int TotalAddedJobDetail { get; set; } = 0;

        public List<kCompe> allCompe { get; set; } = new List<kCompe>();

        public List<NextPlanAndDetailVM> allNextPlanAndDetail { get; set; } = new List<NextPlanAndDetailVM>();
        public int TotalAddedNextPlan { get; set; }
        public int NextPlanNumber { get; set; }
        public int TotalAddedNextPlanDetail { get; set; }

        public long StepId { get; set; }
        public long StateId { get; set; }

        public bool IsReporter { get; set; }
        public bool IsRoleMark { get; set; }
        public bool IsRoleConfirm { get; set; }
        public bool IsBOD { get; set; }        
    }

    public class JobAndDetailVM
    {
        public kJob Job { get; set; }
        public List<kJobDetail> allDetail { get; set; }
    }

    public class NextPlanAndDetailVM
    {
        public kNextPlan NextPlan { get; set; }
        public List<kNextPlanDetail> allDetail { get; set; }
    }
}
using _IQwinwin;
using IQWebApp_Blank.EF;
using IQWebApp_Blank.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQWebApp_Blank.Controllers
{
    [Authorize]
    public class KpiController : Controller
    {
        #region var
        GoodJobEntities dbJob = new GoodJobEntities();
        CompanyEntities dbCompany = new CompanyEntities();

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? (_userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>());
            }
            private set
            {
                _userManager = value;
            }
        }

        public string UserId { get { return User.Identity.GetUserId().ToLower(); } }
        #endregion

        #region Index
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        #endregion


        #region Individual
        public ActionResult Individual()
        {
            var userId = User.Identity.GetUserId().ToLower();
            var userInfo = dbCompany.cUserInfoes.FirstOrDefault(x => x.UserId.ToString().ToLower() == userId);

            var CMD = "0";

            if (Request.QueryString["CMD"] != null)
                CMD = Request.QueryString["CMD"].ToString();

            ViewBag.CMD = CMD;

            if (userInfo != null)
            {
                var reportId = "0";

                if (Url.RequestContext.RouteData.Values["id"] != null)
                    reportId = Url.RequestContext.RouteData.Values["id"].ToString();

                var report = dbJob.kReports.FirstOrDefault(x => x.Id.ToString() == reportId);

                if (report != null)
                {
                    var reporter = dbCompany.cUserInfoes.FirstOrDefault(x => x.UserId == report.UserId);
                    bool isReporter = report.UserId == userInfo.UserId;

                    bool isRoleMark = func.CheckRole(userInfo, report, "mark");
                    bool isRoleConfirm = func.CheckRole(userInfo, report, "confirm");
                    bool isBOD = func.CheckRole(userInfo, report, "bod");

                    //allJobAndDetail
                    var allJob = report.kJobs.ToList();
                    var allJobAndDetail = new List<JobAndDetailVM>();

                    for (int i = 0; i < allJob.Count; i++)
                    {
                        var job = allJob[i];
                        var allDetail = job.kJobDetails.ToList();

                        allJobAndDetail.Add(new JobAndDetailVM { Job = job, allDetail = allDetail });
                        ViewData["TotalAddedJobDetail_" + i] = allDetail.Count;
                    }

                    //allCompe
                    var allCompe = report.kCompes.ToList();

                    //allNextPlanAndDetail
                    var allNextPlan = report.kNextPlans.ToList();
                    var allNextPlanAndDetail = new List<NextPlanAndDetailVM>();

                    for (int i = 0; i < allNextPlan.Count; i++)
                    {
                        var nextPlan = allNextPlan[i];
                        var allDetail = nextPlan.kNextPlanDetails.ToList();

                        allNextPlanAndDetail.Add(new NextPlanAndDetailVM { NextPlan = nextPlan, allDetail = allDetail });
                        ViewData["TotalAddedNextPlanDetail_" + i] = allDetail.Count;
                    }

                    var VM = new IndividualVM
                    {
                        Report = report,
                        Reporter = reporter,

                        allJobAndDetail = allJobAndDetail,
                        TotalAddedJob = allJobAndDetail.Count,

                        allCompe = allCompe,

                        allNextPlanAndDetail = allNextPlanAndDetail,
                        TotalAddedNextPlan = allNextPlanAndDetail.Count,

                        StepId = report.StepId,
                        StateId = report.StateId,

                        IsReporter = isReporter,
                        IsRoleMark = isRoleMark,
                        IsRoleConfirm = isRoleConfirm,
                        IsBOD = isBOD
                    };

                    ViewData["TotalAddedJob"] = allJobAndDetail.Count;
                    ViewData["TotalAddedNextPlan"] = allNextPlanAndDetail.Count;

                    return View(VM);
                }
                else
                {
                    var reporter = userInfo;
                    bool isReporter = true;

                    bool isRoleMark = false;
                    bool isRoleConfirm = false;
                    bool isBOD = func.CheckRole(userInfo, report, "bod");

                    //allJobAndDetail
                    var allJobAndDetail = new List<JobAndDetailVM>();

                    for (int i = 0; i < 3; i++)
                    {
                        var job = new kJob();
                        var allDetail = new List<kJobDetail>();

                        for (int j = 0; j < 2; j++)
                        {
                            var jobDetail = new kJobDetail();
                            allDetail.Add(jobDetail);
                        }

                        allJobAndDetail.Add(new JobAndDetailVM { Job = job, allDetail = allDetail });
                        ViewData["TotalAddedJobDetail_" + i] = allDetail.Count;
                    }

                    //allCompe
                    var allCompe = new List<kCompe>();
                    var allCompeForSome = new List<kCompeForSome>();

                    var compeRelationType = dbJob.kRelationTypes.FirstOrDefault(x => x.UserId == userInfo.UserId);

                    long? compeRelationTypeId = compeRelationType != null
                        ? compeRelationType.CompeRelationTypeId
                        : dbJob.kRelationTypeForAlls.FirstOrDefault(x => x.IsDefaultCompeRelationType).Id;
                    //Neu ko co ? Thi doc Default. Va phai setup san Default tu dau.

                    //User
                    if (compeRelationTypeId == 7)
                        allCompeForSome = dbJob.kCompeForSomes.Where(x => x.CompeRelationTypeId == compeRelationTypeId && x.CompeRelationValue == userInfo.UserId.ToString().ToLower()).ToList();

                    //Rank
                    if (compeRelationTypeId == 6)
                        allCompeForSome = dbJob.kCompeForSomes.Where(x => x.CompeRelationTypeId == compeRelationTypeId && x.CompeRelationValue == userInfo.RankId.ToString()).ToList();

                    //Department
                    if (compeRelationTypeId == 5)
                        allCompeForSome = dbJob.kCompeForSomes.Where(x => x.CompeRelationTypeId == compeRelationTypeId && x.CompeRelationValue == userInfo.DepartmentId.ToString()).ToList();

                    //Branch
                    if (compeRelationTypeId == 4)
                        allCompeForSome = dbJob.kCompeForSomes.Where(x => x.CompeRelationTypeId == compeRelationTypeId && x.CompeRelationValue == userInfo.BranchId.ToString()).ToList();

                    //Company
                    if (compeRelationTypeId == 3)
                        allCompeForSome = dbJob.kCompeForSomes.Where(x => x.CompeRelationTypeId == compeRelationTypeId && x.CompeRelationValue == userInfo.CompanyId.ToString()).ToList();

                    //Corp
                    if (compeRelationTypeId == 2)
                        allCompeForSome = dbJob.kCompeForSomes.Where(x => x.CompeRelationTypeId == compeRelationTypeId && x.CompeRelationValue == userInfo.CorporationId.ToString()).ToList();

                    if (allCompeForSome.Count > 0)
                    {
                        foreach (kCompeForSome one in allCompeForSome)
                        {
                            allCompe.Add(new kCompe { kCompeForSome = one });
                        }
                    }

                    //allNextPlanAndDetail
                    var allNextPlanAndDetail = new List<NextPlanAndDetailVM>();

                    for (int i = 0; i < 3; i++)
                    {
                        var nextPlan = new kNextPlan();
                        var allDetail = new List<kNextPlanDetail>();

                        for (int j = 0; j < 2; j++)
                        {
                            var nextPlanDetail = new kNextPlanDetail();
                            allDetail.Add(nextPlanDetail);
                        }

                        allNextPlanAndDetail.Add(new NextPlanAndDetailVM { NextPlan = nextPlan, allDetail = allDetail });
                        ViewData["TotalAddedNextPlanDetail_" + i] = allDetail.Count;
                    }

                    var VM = new IndividualVM
                    {
                        Report = new kReport { Month = DateTime.Now.Month, Year = DateTime.Now.Year },
                        Reporter = reporter,

                        allJobAndDetail = allJobAndDetail,
                        TotalAddedJob = allJobAndDetail.Count,

                        allCompe = allCompe,

                        allNextPlanAndDetail = allNextPlanAndDetail,
                        TotalAddedNextPlan = allNextPlanAndDetail.Count,

                        StepId = 0,
                        StateId = 0,

                        IsReporter = isReporter,
                        IsRoleMark = isRoleMark,
                        IsRoleConfirm = isRoleConfirm,
                        IsBOD = isBOD
                    };

                    ViewData["TotalAddedJob"] = allJobAndDetail.Count;
                    ViewData["TotalAddedNextPlan"] = allNextPlanAndDetail.Count;

                    return View(VM);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Individual_AddJob(int TotalAddedJob)
        {
            var allJobAndDetail = new List<JobAndDetailVM>();

            for (int i = 0; i < TotalAddedJob + 1; i++)
            {
                var job = new kJob();
                var allDetail = new List<kJobDetail>();

                for (int j = 0; j < 2; j++)
                {
                    var jobDetail = new kJobDetail();
                    allDetail.Add(jobDetail);
                }

                allJobAndDetail.Add(new JobAndDetailVM { Job = job, allDetail = allDetail });
            }

            var VM = new IndividualVM
            {
                allJobAndDetail = allJobAndDetail,
                TotalAddedJob = TotalAddedJob
            };

            return PartialView(VM);
        }
        public ActionResult Individual_AddJobDetail(int JobNumber, int TotalAddedJobDetail)
        {
            var allJobAndDetail = new List<JobAndDetailVM>();

            for (int i = 0; i <= JobNumber; i++)
            {
                var job = new kJob();
                var allDetail = new List<kJobDetail>();

                for (int j = 0; j < TotalAddedJobDetail + 1; j++)
                {
                    var jobDetail = new kJobDetail();
                    allDetail.Add(jobDetail);
                }

                allJobAndDetail.Add(new JobAndDetailVM { Job = job, allDetail = allDetail });
            }

            var VM = new IndividualVM
            {
                allJobAndDetail = allJobAndDetail,
                JobNumber = JobNumber,
                TotalAddedJobDetail = TotalAddedJobDetail
            };

            return PartialView(VM);
        }

        public ActionResult Individual_AddNextPlan(int TotalAddedNextPlan)
        {
            var allNextPlanAndDetail = new List<NextPlanAndDetailVM>();

            for (int i = 0; i < TotalAddedNextPlan + 1; i++)
            {
                var nextPlan = new kNextPlan();
                var allDetail = new List<kNextPlanDetail>();

                for (int j = 0; j < 2; j++)
                {
                    var nextPlanDetail = new kNextPlanDetail();
                    allDetail.Add(nextPlanDetail);
                }

                allNextPlanAndDetail.Add(new NextPlanAndDetailVM { NextPlan = nextPlan, allDetail = allDetail });
            }

            var VM = new IndividualVM
            {
                allNextPlanAndDetail = allNextPlanAndDetail,
                TotalAddedNextPlan = TotalAddedNextPlan
            };

            return PartialView(VM);
        }
        public ActionResult Individual_AddNextPlanDetail(int NextPlanNumber, int TotalAddedNextPlanDetail)
        {
            var allNextPlanAndDetail = new List<NextPlanAndDetailVM>();

            for (int i = 0; i <= NextPlanNumber; i++)
            {
                var nextPlan = new kNextPlan();
                var allDetail = new List<kNextPlanDetail>();

                for (int j = 0; j < TotalAddedNextPlanDetail + 1; j++)
                {
                    var nextPlanDetail = new kNextPlanDetail();
                    allDetail.Add(nextPlanDetail);
                }

                allNextPlanAndDetail.Add(new NextPlanAndDetailVM { NextPlan = nextPlan, allDetail = allDetail });
            }

            var VM = new IndividualVM
            {
                allNextPlanAndDetail = allNextPlanAndDetail,
                NextPlanNumber = NextPlanNumber,
                TotalAddedNextPlanDetail = TotalAddedNextPlanDetail
            };

            return PartialView(VM);
        }

        [HttpPost]
        public ActionResult Individual_Submit(IndividualVM input)
        {
            var userId = User.Identity.GetUserId().ToLower();
            var userInfo = dbCompany.cUserInfoes.FirstOrDefault(x => x.UserId.ToString().ToLower() == userId);

            if (userInfo != null)
            {
                #region  Report
                //check valid role (can view, edit, mark...)
                var reportId = input.Report.Id;
                var report = dbJob.kReports.FirstOrDefault(x => x.Id == reportId);

                if (report == null)
                {
                    report = new kReport();
                    dbJob.kReports.Add(report);

                    report.Time = DateTime.Now;
                    report.UserId = userInfo.UserId;

                    report.Year = input.Report.Year;
                    report.Month = input.Report.Month;

                    var relationType = new Models.kRelationType();
                    relationType.StepSendTo(userInfo, 1);

                    report.StepId = relationType.StepId;
                    report.StateId = 1;
                    report.SendToRelationTypeId = relationType.SendToRelationTypeId;
                    report.SendToRelationValue = relationType.SendToRelationValue;

                    dbJob.SaveChanges();
                }
                else
                if (report.UserId == userInfo.UserId)
                {
                    report.Time = DateTime.Now;

                    report.Year = input.Report.Year;
                    report.Month = input.Report.Month;

                    report.StateId = 1;
                }

                //EMAIL - Reporter -> Manager
                if (report.UserId == userInfo.UserId)
                {
                    var sendFromStep1 = userInfo;
                    var sendToStep1 = dbCompany.cUserInfoes.FirstOrDefault(x => x.UserId.ToString() == report.SendToRelationValue); ;

                    string sendToEmailStep1 = dbCompany.vLoginUsers.FirstOrDefault(x => x.Id == sendToStep1.UserId.ToString()).Email;
                    string replyToEmailStep1 = dbCompany.vLoginUsers.FirstOrDefault(x => x.Id == sendFromStep1.UserId.ToString()).Email;

                    string emailTitleStep1 = "Đã nhận được: KPI tháng " + report.Month + "/" + report.Year + " của " + sendFromStep1.FullName;
                    string emailContentStep1 =
                        "Dear " + sendToStep1.FullName + "," + "<br/>"
                        + "Bạn đã nhận được: KPI tháng " + report.Month + "/" + report.Year + " của " + sendFromStep1.FullName + " (" + sendFromStep1.cDepartment.cDepartmentForAll.DepartmentName + ")." + "<br/><br/>"
                        + "Hãy truy cập web: <a href='http://Kpi.TheGarden.com.vn'>Kpi.TheGarden.com.vn</a>" + "<br/>"
                        + "Để xem nội dung chi tiết." + "<br/><br/>"
                        + "Thanks and best regards !"
                        ;

                    func.SendEmail(emailTitleStep1, emailContentStep1, sendToEmailStep1, replyToEmailStep1);
                }
                #endregion

                #region Check Role
                var reporter = dbCompany.cUserInfoes.FirstOrDefault(x => x.UserId == report.UserId);
                bool isReporter = report.UserId == userInfo.UserId;

                bool isRoleMark = func.CheckRole(userInfo, report, "mark");
                bool isRoleConfirm = func.CheckRole(userInfo, report, "confirm");
                bool isBOD = func.CheckRole(userInfo, report, "bod");

                bool isReject = Request.Form["IsReject"].ToLower() == "true";
                #endregion

                bool validSubmit = isReporter || isRoleMark || isRoleConfirm || isBOD;

                if (validSubmit)
                {
                    #region Job
                    if (input.allJobAndDetail != null)
                    {
                        #region isReporter
                        if (isReporter)
                        {
                            var allCurrentJob = new List<long>();

                            for (int i = 0; i < input.allJobAndDetail.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(input.allJobAndDetail[i].Job.JobName))
                                {
                                    var jobId = input.allJobAndDetail[i].Job.Id;

                                    if (jobId < 0)
                                    {
                                        var job = dbJob.kJobs.FirstOrDefault(x => x.Id == -jobId && x.ReportId == report.Id);

                                        if (job != null)
                                        {
                                            report.kJobs.Remove(job);
                                            dbJob.kJobs.Remove(job);
                                            dbJob.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        var job = dbJob.kJobs.FirstOrDefault(x => x.Id == jobId && x.ReportId == report.Id);

                                        if (job == null)
                                        {
                                            job = new kJob();
                                            report.kJobs.Add(job);

                                            job.ReportId = report.Id;
                                        }

                                        job.JobName = input.allJobAndDetail[i].Job.JobName;
                                        job.Seq = i + 1;

                                        job.Percent = input.allJobAndDetail[i].Job.Percent;
                                        job.SelfMark = input.allJobAndDetail[i].Job.SelfMark;

                                        dbJob.SaveChanges();

                                        #region JobDetail
                                        var allCurrentJobDetail = new List<long>();

                                        if (input.allJobAndDetail[i].allDetail != null)
                                        {
                                            for (int j = 0; j < input.allJobAndDetail[i].allDetail.Count; j++)
                                            {
                                                if (!string.IsNullOrEmpty(input.allJobAndDetail[i].allDetail[j].JobDetailName))
                                                {
                                                    var jobDetailId = input.allJobAndDetail[i].allDetail[j].Id;

                                                    if (jobDetailId < 0)
                                                    {
                                                        var jobDetail = dbJob.kJobDetails.FirstOrDefault(x => x.Id == -jobDetailId && x.ReportId == report.Id && x.JobId == job.Id);

                                                        if (jobDetail != null)
                                                        {
                                                            job.kJobDetails.Remove(jobDetail);
                                                            dbJob.kJobDetails.Remove(jobDetail);
                                                            dbJob.SaveChanges();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var jobDetail = dbJob.kJobDetails.FirstOrDefault(x => x.Id == jobDetailId && x.ReportId == report.Id && x.JobId == job.Id);

                                                        if (jobDetail == null)
                                                        {
                                                            jobDetail = new kJobDetail();
                                                            job.kJobDetails.Add(jobDetail);

                                                            jobDetail.ReportId = report.Id;
                                                            jobDetail.JobId = job.Id;
                                                        }

                                                        jobDetail.JobDetailName = input.allJobAndDetail[i].allDetail[j].JobDetailName;
                                                        jobDetail.Seq = j + 1;

                                                        jobDetail.Result = input.allJobAndDetail[i].allDetail[j].Result;
                                                        jobDetail.Status = input.allJobAndDetail[i].allDetail[j].Status;

                                                        dbJob.SaveChanges();
                                                        allCurrentJobDetail.Add(jobDetail.Id);
                                                    }
                                                }
                                            }

                                            //Remove if not in allCurrentJobDetail
                                            var removedJobDetail = job.kJobDetails.Where(x => !allCurrentJobDetail.Any(y => y == x.Id)).ToList();

                                            foreach (var jobDetail in removedJobDetail)
                                            {
                                                job.kJobDetails.Remove(jobDetail);
                                                dbJob.kJobDetails.Remove(jobDetail);
                                                dbJob.SaveChanges();
                                            }
                                        }

                                        if (job.kJobDetails.Count > 0)
                                            allCurrentJob.Add(job.Id);
                                        #endregion
                                    }
                                }
                            }

                            //Remove if not in allCurrentJob
                            var removedJob = report.kJobs.Where(x => !allCurrentJob.Any(y => y == x.Id)).ToList();

                            foreach (var job in removedJob)
                            {
                                report.kJobs.Remove(job);
                                dbJob.kJobs.Remove(job);
                                dbJob.SaveChanges();
                            }
                        }
                        #endregion

                        #region isRoleMark
                        else if (isRoleMark)
                        {
                            for (int i = 0; i < input.allJobAndDetail.Count; i++)
                            {
                                var jobId = input.allJobAndDetail[i].Job.Id;
                                var job = dbJob.kJobs.FirstOrDefault(x => x.Id == jobId && x.ReportId == report.Id);

                                if (job != null)
                                {
                                    job.ManagerMark = input.allJobAndDetail[i].Job.ManagerMark;
                                    job.ResultMark = (job.ManagerMark * job.Percent) / 100;

                                    dbJob.SaveChanges();
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region Compe
                    if (input.allCompe != null)
                    {
                        #region isReporter
                        if (isReporter)
                        {
                            for (int i = 0; i < input.allCompe.Count; i++)
                            {
                                var compeId = input.allCompe[i].Id;
                                var compe = dbJob.kCompes.FirstOrDefault(x => x.Id == compeId && x.ReportId == report.Id);

                                if (compe == null)
                                {
                                    compe = new kCompe();
                                    report.kCompes.Add(compe);

                                    compe.ReportId = report.Id;
                                    compe.Percent = input.allCompe[i].kCompeForSome.Percent;
                                }
                                else
                                {
                                    compe.Percent = input.allCompe[i].Percent;
                                }

                                compe.CompeForSomeId = input.allCompe[i].kCompeForSome.Id;
                                compe.Seq = i + 1;

                                compe.Result = input.allCompe[i].Result;
                                compe.SelfMark = input.allCompe[i].SelfMark;

                                dbJob.SaveChanges();
                            }
                        }
                        #endregion

                        #region isRoleMark
                        else if (isRoleMark)
                        {
                            for (int i = 0; i < input.allCompe.Count; i++)
                            {
                                var compeId = input.allCompe[i].Id;
                                var compe = dbJob.kCompes.FirstOrDefault(x => x.Id == compeId && x.ReportId == report.Id);

                                if (compe != null)
                                {
                                    compe.ManagerMark = input.allCompe[i].ManagerMark;
                                    compe.ResultMark = (compe.ManagerMark * compe.Percent) / 100;

                                    dbJob.SaveChanges();
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region NextPlan
                    if (input.allNextPlanAndDetail != null)
                    {
                        #region isReporter
                        if (isReporter)
                        {
                            var allCurrentNextPlan = new List<long>();

                            for (int i = 0; i < input.allNextPlanAndDetail.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(input.allNextPlanAndDetail[i].NextPlan.NextPlanName))
                                {
                                    var nextPlanId = input.allNextPlanAndDetail[i].NextPlan.Id;

                                    if (nextPlanId < 0)
                                    {
                                        var nextPlan = dbJob.kNextPlans.FirstOrDefault(x => x.Id == -nextPlanId && x.ReportId == report.Id);

                                        if (nextPlan != null)
                                        {
                                            report.kNextPlans.Remove(nextPlan);
                                            dbJob.kNextPlans.Remove(nextPlan);
                                            dbJob.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        var nextPlan = dbJob.kNextPlans.FirstOrDefault(x => x.Id == nextPlanId && x.ReportId == report.Id);

                                        if (nextPlan == null)
                                        {
                                            nextPlan = new kNextPlan();
                                            report.kNextPlans.Add(nextPlan);

                                            nextPlan.ReportId = report.Id;
                                        }

                                        nextPlan.NextPlanName = input.allNextPlanAndDetail[i].NextPlan.NextPlanName;
                                        nextPlan.Seq = i + 1;
                                        nextPlan.Percent = input.allNextPlanAndDetail[i].NextPlan.Percent;

                                        dbJob.SaveChanges();

                                        #region NextPlanDetail
                                        var allCurrentNextPlanDetail = new List<long>();

                                        if (input.allNextPlanAndDetail[i].allDetail != null)
                                        {
                                            for (int j = 0; j < input.allNextPlanAndDetail[i].allDetail.Count; j++)
                                            {
                                                if (!string.IsNullOrEmpty(input.allNextPlanAndDetail[i].allDetail[j].NextPlanDetailName))
                                                {
                                                    var nextPlanDetailId = input.allNextPlanAndDetail[i].allDetail[j].Id;

                                                    if (nextPlanDetailId < 0)
                                                    {
                                                        var nextPlanDetail = dbJob.kNextPlanDetails.FirstOrDefault(x => x.Id == -nextPlanDetailId && x.ReportId == report.Id && x.NextPlanId == nextPlan.Id);

                                                        if (nextPlanDetail != null)
                                                        {
                                                            nextPlan.kNextPlanDetails.Remove(nextPlanDetail);
                                                            dbJob.kNextPlanDetails.Remove(nextPlanDetail);
                                                            dbJob.SaveChanges();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var nextPlanDetail = dbJob.kNextPlanDetails.FirstOrDefault(x => x.Id == nextPlanDetailId && x.ReportId == report.Id && x.NextPlanId == nextPlan.Id);

                                                        if (nextPlanDetail == null)
                                                        {
                                                            nextPlanDetail = new kNextPlanDetail();
                                                            nextPlan.kNextPlanDetails.Add(nextPlanDetail);

                                                            nextPlanDetail.ReportId = report.Id;
                                                            nextPlanDetail.NextPlanId = nextPlan.Id;
                                                        }

                                                        nextPlanDetail.NextPlanDetailName = input.allNextPlanAndDetail[i].allDetail[j].NextPlanDetailName;
                                                        nextPlanDetail.Seq = j + 1;

                                                        dbJob.SaveChanges();
                                                        allCurrentNextPlanDetail.Add(nextPlanDetail.Id);
                                                    }
                                                }
                                            }

                                            //Remove if not in allCurrentNextPlanDetail
                                            var removedNextPlanDetail = nextPlan.kNextPlanDetails.Where(x => !allCurrentNextPlanDetail.Any(y => y == x.Id)).ToList();

                                            foreach (var nextPlanDetail in removedNextPlanDetail)
                                            {
                                                nextPlan.kNextPlanDetails.Remove(nextPlanDetail);
                                                dbJob.kNextPlanDetails.Remove(nextPlanDetail);
                                                dbJob.SaveChanges();
                                            }
                                        }

                                        if (nextPlan.kNextPlanDetails.Count > 0)
                                            allCurrentNextPlan.Add(nextPlan.Id);
                                        #endregion
                                    }
                                }
                            }

                            //Remove if not in allCurrentNextPlan
                            var removedNextPlan = report.kNextPlans.Where(x => !allCurrentNextPlan.Any(y => y == x.Id)).ToList();

                            foreach (var nextPlan in removedNextPlan)
                            {
                                report.kNextPlans.Remove(nextPlan);
                                dbJob.kNextPlans.Remove(nextPlan);
                                dbJob.SaveChanges();
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region Comment
                    if (isReporter)
                    {
                        report.Comment = input.Report.Comment;
                    }
                    else if (isRoleMark)
                    {
                        report.ManagerComment = input.Report.ManagerComment;
                    }
                    else if (isBOD)
                    {
                        report.BODcomment = input.Report.BODcomment;
                    }
                    #endregion

                    #region SendTo, State, History
                    if (isReporter || isRoleMark || isRoleConfirm)
                    {
                        if (isRoleMark || isRoleConfirm)
                        {
                            if (isReject)
                            {
                                report.StateId = 2;
                            }
                            else if (isRoleConfirm)
                            {
                                if (report.kStep.kStepForAll.Seq == 1 && isRoleMark)
                                {
                                    var autoHistory = new kReport_History();
                                    dbJob.kReport_History.Add(autoHistory);

                                    autoHistory.Time = DateTime.Now;
                                    autoHistory.UserId = userInfo.UserId;
                                    autoHistory.ReportId = report.Id;

                                    autoHistory.StepId = report.StepId;
                                    autoHistory.StateId = 3;
                                    autoHistory.SendToRelationTypeId = report.SendToRelationTypeId;
                                    autoHistory.SendToRelationValue = report.SendToRelationValue;
                                }

                                var relationType = new Models.kRelationType();
                                relationType.StepSendTo(reporter, 2);

                                report.StepId = relationType.StepId;
                                report.StateId = 3;
                                report.SendToRelationTypeId = relationType.SendToRelationTypeId;
                                report.SendToRelationValue = relationType.SendToRelationValue;

                                //EMAIL - Manager -> Done
                            }
                            else if (isRoleMark)
                            {
                                if (report.kStep.kStepForAll.Seq == 1)
                                {
                                    var autoHistory = new kReport_History();
                                    dbJob.kReport_History.Add(autoHistory);

                                    autoHistory.Time = DateTime.Now;
                                    autoHistory.UserId = userInfo.UserId;
                                    autoHistory.ReportId = report.Id;

                                    autoHistory.StepId = report.StepId;
                                    autoHistory.StateId = 3;
                                    autoHistory.SendToRelationTypeId = report.SendToRelationTypeId;
                                    autoHistory.SendToRelationValue = report.SendToRelationValue;

                                    var relationType = new Models.kRelationType();
                                    relationType.StepSendTo(reporter, 2);

                                    report.StepId = relationType.StepId;
                                    report.StateId = 1;
                                    report.SendToRelationTypeId = relationType.SendToRelationTypeId;
                                    report.SendToRelationValue = relationType.SendToRelationValue;

                                    //EMAIL - Manager -> HR
                                    /*
                                    var sendFromStep2 = reporter;
                                    var sendToStep2 = dbCompany.cUserInfoes.FirstOrDefault(x => x.UserId.ToString() == report.SendToRelationValue); ;

                                    string sendToEmailStep2 = dbCompany.vLoginUsers.FirstOrDefault(x => x.Id == sendToStep2.UserId.ToString()).Email;
                                    string replyToEmailStep2 = dbCompany.vLoginUsers.FirstOrDefault(x => x.Id == sendFromStep2.UserId.ToString()).Email;

                                    string emailTitleStep2 = "Đã nhận được: KPI tháng " + report.Month + "/" + report.Year + " của " + sendFromStep2.FullName;
                                    string emailContentStep2 =
                                        "Dear " + sendToStep2.FullName + "," + "<br/>"
                                        + "Bạn đã nhận được: KPI tháng " + report.Month + "/" + report.Year + " của " + sendFromStep2.FullName + " (" + sendFromStep2.cDepartment.cDepartmentForAll.DepartmentName + ")." + "<br/><br/>"
                                        + "Hãy truy cập web: <a href='http://Kpi.TheGarden.com.vn'>Kpi.TheGarden.com.vn</a>" + "<br/>"
                                        + "Để xem nội dung chi tiết." + "<br/><br/>"
                                        + "Thanks and best regards !"
                                        ;

                                    func.SendEmail(emailTitleStep2, emailContentStep2, sendToEmailStep2, replyToEmailStep2);
                                    */
                                }
                            }
                        }

                        var history = new kReport_History();
                        dbJob.kReport_History.Add(history);

                        history.Time = DateTime.Now;
                        history.UserId = userInfo.UserId;
                        history.ReportId = report.Id;

                        history.StepId = report.StepId;
                        history.StateId = report.StateId;
                        history.SendToRelationTypeId = report.SendToRelationTypeId;
                        history.SendToRelationValue = report.SendToRelationValue;
                    }
                    #endregion

                    #region Result
                    var xKpiJob = dbJob.vRanks.FirstOrDefault(x => x.Id == reporter.RankId).xKpiJob;
                    var xKpiCompe = dbJob.vRanks.FirstOrDefault(x => x.Id == reporter.RankId).xKpiCompe;

                    report.TotalMarkJob = report.kJobs.Sum(x => x.ResultMark);
                    report.TotalMarkCompe = report.kCompes.Sum(x => x.ResultMark);

                    report.FinalMarkJob = report.TotalMarkJob * xKpiJob;
                    report.FinalMarkCompe = report.TotalMarkCompe * xKpiCompe;
                    report.FinalMarkKpi = report.FinalMarkJob + report.FinalMarkCompe;

                    dbJob.SaveChanges();

                    var kpiAVG = dbJob.kReports.Where(x => x.UserId == report.UserId && x.Year == report.Year && x.StateId == 4).Average(x => x.FinalMarkKpi) ?? 0;
                    var rateId = dbJob.kRates.FirstOrDefault(x => x.Min <= kpiAVG && kpiAVG <= x.Max).Id;

                    var AverageMark = dbJob.kAverageMarks.FirstOrDefault(x => x.UserId == report.UserId && x.Year == report.Year);

                    if (AverageMark == null)
                    {
                        AverageMark = new kAverageMark();
                        dbJob.kAverageMarks.Add(AverageMark);

                        AverageMark.UserId = report.UserId;
                        AverageMark.Year = report.Year;
                    }

                    AverageMark.AverageValue = decimal.Parse(kpiAVG.ToString());
                    AverageMark.RateId = rateId;
                    #endregion

                    dbJob.SaveChanges();

                    #region SendEmail
                    string emailTitle = string.Empty;
                    string emailContent = string.Empty;

                    if (isReject)
                    {
                        emailTitle = "Đã từ chối: KPI tháng " + report.Month + "/" + report.Year + " của " + reporter.FullName;

                        emailContent =
                            "Dear " + reporter.FullName + "," + "<br/>"
                            + "KPI tháng " + report.Month + "/" + report.Year + " của bạn đã bị từ chối bởi: " + userInfo.FullName + " (" + userInfo.cDepartment.cDepartmentForAll.DepartmentName + ")." + "<br/><br/>"
                            + "Hãy truy cập web: <a href='http://Kpi.TheGarden.com.vn'>Kpi.TheGarden.com.vn</a>" + "<br/>"
                            + "Để chỉnh sửa và gửi lại KPI một lần nữa." + "<br/><br/>"
                            + "LƯU Ý: Đây là email tự động được gửi từ hệ thống." + "<br/>"
                            + "Tuy nhiên, nếu bạn Reply lại email này, thì người nhận sẽ là: " + userInfo.FullName + " (" + userInfo.cDepartment.cDepartmentForAll.DepartmentName + ")." + "<br/><br/>"
                            + "(Chỉ nên reply lại email nếu thực sự cần thiết, còn các chỉnh sửa KPI thì nên thao tác trên website)." + "<br/>"
                            + "Thanks and best regards !"
                            ;
                    }
                    else
                    {
                        if (isRoleConfirm)
                        {
                            emailTitle = "Đã xác nhận hoàn thành: KPI tháng " + report.Month + "/" + report.Year + " của " + reporter.FullName;

                            emailContent =
                                "Dear " + reporter.FullName + "," + "<br/>"
                                + "KPI tháng " + report.Month + "/" + report.Year + " của bạn đã được Xác nhận Hoàn thành bởi: " + userInfo.FullName + " (" + userInfo.cDepartment.cDepartmentForAll.DepartmentName + ")." + "<br/><br/>"
                                + "Bạn có thể truy cập web: <a href='http://Kpi.TheGarden.com.vn'>Kpi.TheGarden.com.vn</a>" + "<br/>"
                                + "Để xem điểm KPI tổng kết và các comment chi tiết." + "<br/><br/>"
                                + "LƯU Ý: Đây là email tự động được gửi từ hệ thống." + "<br/>"
                                + "Tuy nhiên, nếu bạn Reply lại email này, thì người nhận sẽ là: " + userInfo.FullName + " (" + userInfo.cDepartment.cDepartmentForAll.DepartmentName + ")." + "<br/><br/>"
                                + "(Chỉ nên reply lại email nếu thực sự cần thiết, còn các chỉnh sửa KPI thì nên thao tác trên website)." + "<br/>"
                                + "Thanks and best regards !"
                                ;
                        }
                        else if (isRoleMark)
                        {
                            emailTitle = "Đã chấm điểm và đánh giá: KPI tháng " + report.Month + "/" + report.Year + " của " + reporter.FullName;

                            emailContent =
                                "Dear " + reporter.FullName + "," + "<br/>"
                                + "KPI tháng " + report.Month + "/" + report.Year + " của bạn đã được Chấm điểm và Đánh giá bởi: " + userInfo.FullName + " (" + userInfo.cDepartment.cDepartmentForAll.DepartmentName + ")." + "<br/><br/>"
                                + "Báo cáo của bạn sẽ được chuyển tiếp tới Level cao hơn." + "<br/><br/>"
                                + "Bạn có thể truy cập web: <a href='http://Kpi.TheGarden.com.vn'>Kpi.TheGarden.com.vn</a>" + "<br/>"
                                + "Để xem điểm KPI và các comment chi tiết." + "<br/><br/>"
                                + "LƯU Ý: Đây là email tự động được gửi từ hệ thống." + "<br/>"
                                + "Tuy nhiên, nếu bạn Reply lại email này, thì người nhận sẽ là: " + userInfo.FullName + " (" + userInfo.cDepartment.cDepartmentForAll.DepartmentName + ")." + "<br/><br/>"
                                + "(Chỉ nên reply lại email nếu thực sự cần thiết, còn các chỉnh sửa KPI thì nên thao tác trên website)." + "<br/>"
                                + "Thanks and best regards !"
                                ;
                        }
                        else if (isBOD)
                        {
                            emailTitle = "BOD đã thêm Nhận Xét: KPI tháng " + report.Month + "/" + report.Year + " của " + reporter.FullName;

                            emailContent =
                                "Dear " + reporter.FullName + "," + "<br/>"
                                + "KPI tháng " + report.Month + "/" + report.Year + " của bạn đã được thêm Nhận xét bởi: " + userInfo.FullName + " (" + userInfo.cDepartment.cDepartmentForAll.DepartmentName + ")." + "<br/><br/>"
                                + "Bạn có thể truy cập web: <a href='http://Kpi.TheGarden.com.vn'>Kpi.TheGarden.com.vn</a>" + "<br/>"
                                + "Để xem điểm KPI và các comment chi tiết." + "<br/><br/>"
                                + "LƯU Ý: Đây là email tự động được gửi từ hệ thống." + "<br/>"
                                + "Tuy nhiên, nếu bạn Reply lại email này, thì người nhận sẽ là: " + userInfo.FullName + " (" + userInfo.cDepartment.cDepartmentForAll.DepartmentName + ")." + "<br/><br/>"
                                + "(Chỉ nên reply lại email nếu thực sự cần thiết, còn các chỉnh sửa KPI thì nên thao tác trên website)." + "<br/>"
                                + "Thanks and best regards !"
                                ;
                        }
                    }

                    //SendEmail
                    if (emailTitle != string.Empty && emailContent != string.Empty)
                    {
                        string sendToEmail = dbCompany.vLoginUsers.FirstOrDefault(x => x.Id == reporter.UserId.ToString()).Email;
                        string replyToEmail = dbCompany.vLoginUsers.FirstOrDefault(x => x.Id == userInfo.UserId.ToString()).Email;

                        func.SendEmail(emailTitle, emailContent, sendToEmail, replyToEmail);
                    }
                    #endregion

                    #region Done
                    string CMD = "Done";

                    if (isRoleConfirm)
                    {
                        CMD = isReject ? "DoneReject" : "DoneConfirm";
                    }
                    else
                    if (isRoleMark)
                    {
                        CMD = isReject ? "DoneReject" : "DoneMark";
                    }
                    else
                    if (isBOD)
                    {
                        CMD = "DoneComment";
                    }
                    #endregion

                    return RedirectToAction("Individual/" + report.Id, new { CMD = CMD });
                }
            }

            return RedirectToAction("Individual");
        }
        #endregion


        #region Review
        public List<kReport> SelectRoledReport(string _userId)
        {
            var allMyReport = dbJob.kReports.Where(x => x.UserId.ToString() == _userId).ToList();

            var allManagedUser = dbCompany.cUserInfoes.Where(x => x.ManagerId.ToString() == _userId).Select(x => x.UserId).ToList();
            var allManagedReport = dbJob.kReports.Where(x => allManagedUser.Any(y => y == x.UserId)).ToList();

            var allRoleForSome = dbCompany.aRoles.Where(x => x.RoleRelationTypeId == 7 && x.RoleRelationValue == _userId).Select(x => x.RoleForSomeId).ToList();
            var allRank = dbCompany.aRoleForSomeAllows.Where(x => x.RoleForAllId == 1 && allRoleForSome.Any(y => y == x.RoleForSomeId) && x.AllowRelationTypeId == 6).Select(x => x.AllowRelationValue).ToList();
            var allUserByRank = dbCompany.cUserInfoes.Where(x => allRank.Any(y => y == x.RankId.ToString())).Select(x => x.UserId).ToList();
            var allRoledReport = dbJob.kReports.Where(x => allUserByRank.Any(y => y == x.UserId)).ToList();

            var allReport = allMyReport;

            foreach (var one in allManagedReport)
                if (!allReport.Any(x => x.Id == one.Id))
                    allReport.Add(one);

            foreach (var one in allRoledReport)
                if (!allReport.Any(x => x.Id == one.Id))
                    allReport.Add(one);

            var allReportOrdered = allReport.OrderByDescending(x => x.Time).ToList();
            return allReportOrdered;
        }
        public ActionResult Review()
        {
            var allReport = SelectRoledReport(UserId).ToList();
            return View(allReport);
        }
        public ActionResult Review_ByYear(long Year)
        {
            var allMonth = SelectRoledReport(UserId).Where(x => (x.Year == Year || Year == 0)).OrderByDescending(x => x.Time).ToList();
            return PartialView("Review_Month", allMonth);
        }
        public ActionResult Review_ByMonth(long Year, long Month)
        {
            var allReport = SelectRoledReport(UserId).Where(x => (x.Year == Year || Year == 0) && (x.Month == Month || Month == 0)).Select(x => x.UserId).ToList();
            var allUserInfo = dbCompany.cUserInfoes.Where(x => allReport.Any(y => y == x.UserId)).Select(x => x.BranchId).ToList();

            var allBranch = dbCompany.cBranches.Where(x => allUserInfo.Any(y => y == x.Id)).ToList();
            return PartialView("Review_Branch", allBranch);
        }
        public ActionResult Review_ByBranch(long Year, long Month, long Branch)
        {
            var allReport = SelectRoledReport(UserId).Where(x => (x.Year == Year || Year == 0) && (x.Month == Month || Month == 0)).Select(x => x.UserId).ToList();
            var allUserInfo = dbCompany.cUserInfoes.Where(x => allReport.Any(y => y == x.UserId) && (x.BranchId == Branch || Branch == 0)).Select(x => x.DepartmentId).ToList();

            var allDepartment = dbCompany.cDepartments.Where(x => allUserInfo.Any(y => y == x.Id)).ToList();
            return PartialView("Review_Department", allDepartment);
        }
        public ActionResult Review_ByDepartment(long Year, long Month, long Branch, long Department)
        {
            var allReport = SelectRoledReport(UserId).Where(x => (x.Year == Year || Year == 0) && (x.Month == Month || Month == 0)).Select(x => x.UserId).ToList();
            var allUserInfo = dbCompany.cUserInfoes.Where(x => allReport.Any(y => y == x.UserId) && (x.BranchId == Branch || Branch == 0) && (x.DepartmentId == Department || Department == 0)).ToList();

            return PartialView("Review_Staff", allUserInfo);
        }
        public ActionResult Review_ByFilter(long Year, long Month, long Branch, long Department, string Staff, long State)
        {
            var allUserByDepartment = dbCompany.cUserInfoes.Where(x => (x.BranchId == Branch || Branch == 0) && (x.DepartmentId == Department || Department == 0)).Select(x => x.UserId).ToList();
            var allReport = SelectRoledReport(UserId).Where(x => (x.Year == Year || Year == 0) && (x.Month == Month || Month == 0) && (allUserByDepartment.Any(y => y == x.UserId)) && (x.UserId.ToString() == Staff || Staff == "0") && (x.StateId == State || State == 0)).ToList();

            return PartialView("Review_Body", allReport);
        }
        #endregion

        #region Report
        public ActionResult Report()
        {
            var allReport = SelectRoledReport(UserId).ToList();
            return View(allReport);
        }
        public ActionResult Report_ByYear(long Year)
        {
            var allReport = SelectRoledReport(UserId).Where(x => (x.Year == Year || Year == 0)).Select(x => x.UserId).ToList();
            var allUserInfo = dbCompany.cUserInfoes.Where(x => allReport.Any(y => y == x.UserId)).Select(x => x.BranchId).ToList();

            var allBranch = dbCompany.cBranches.Where(x => allUserInfo.Any(y => y == x.Id)).ToList();
            return PartialView("Report_Branch", allBranch);
        }
        public ActionResult Report_ByFilter(long Year)
        {
            ViewBag.Year = Year;

            var allReport = SelectRoledReport(UserId).Where(x => x.Year == Year && x.StepId == 2 && x.StateId == 3).Select(x => x.UserId).Distinct().ToList();
            return PartialView("Report_Body", allReport);
        }
        #endregion


        #region UserInfo
        public ActionResult UserInfo(long? Id)
        {
            try
            {
                if (Id == null)
                    Id = 1;

                ViewBag.Id = Id;

                var userId = Request.IsAuthenticated ? User.Identity.GetUserId().ToLower() : "";
                var isAdmin = dbCompany.cBranches.FirstOrDefault(x => x.Id == Id && x.AdminId.ToString() == userId) != null;

                if (isAdmin)
                {
                    var queryRows =
                    from u in dbCompany.cUserInfoes

                    join lu in dbCompany.vLoginUsers on u.UserId.ToString().ToLower() equals lu.Id into um
                    from lu in um.ToList()

                    where u.BranchId == Id
                    orderby u.cDepartment.Seq, u.cRank.Seq, u.FullName

                    select new UserInfoVM
                    {
                        UserId = u.UserId,
                        IsNew = false,

                        UserInfo = u,
                        LoginUser = lu
                    }
                    ;

                    //listRows
                    var listRows = queryRows.ToList();

                    for (int i1 = 1; i1 <= 5; i1++)
                    {
                        listRows.Insert(0, new UserInfoVM
                        {
                            UserId = Guid.NewGuid(),
                            IsNew = true,

                            UserInfo = new cUserInfo(),
                            LoginUser = new vLoginUser()
                        });
                    }

                    //allDepartment
                    var allDepartment =
                        from d in dbCompany.cDepartments

                        join da in dbCompany.cDepartmentForAlls on d.DepartmentForAllId equals da.Id into dda
                        from da in dda.ToList()

                        where d.BranchId == Id
                        orderby da.Seq

                        select new DepartmentVM
                        {
                            Id = d.Id,
                            DepartmentName = da.DepartmentName
                        };
                    ViewData["allDepartment"] = allDepartment.ToList();

                    //allRank
                    var allRank =
                        from d in dbCompany.cRanks

                        join da in dbCompany.cRankForAlls on d.RankForAllId equals da.Id into dda
                        from da in dda.ToList()

                        where d.BranchId == Id
                        orderby d.Seq

                        select new RankVM
                        {
                            Id = d.Id,
                            RankName = da.RankName
                        };
                    ViewData["allRank"] = allRank.ToList();

                    //allGrade
                    var allGrade =
                        from g in dbCompany.cGrades

                        join ga in dbCompany.cGradeForAlls on g.GradeForAllId equals ga.Id into gga
                        from ga in gga.ToList()

                        where g.BranchId == Id
                        orderby ga.Seq

                        select new GradeVM
                        {
                            Id = g.Id,
                            GradeName = ga.GradeName
                        };
                    ViewData["allGrade"] = allGrade.ToList();

                    //allManager
                    var allManager =
                        from u in dbCompany.cUserInfoes
                        join r in dbCompany.cRanks on u.RankId equals r.Id into ur
                        from r in ur.ToList()

                        where u.BranchId == Id
                        && r.CanBeManager

                        orderby r.Seq

                        select new ManagerVM
                        {
                            UserId = u.UserId,
                            FullNameAndRank = "(" + r.cRankForAll.RankName + ") " + u.FullName
                        };
                    ViewData["allManager"] = allManager.ToList();

                    return View(listRows);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                sys.Write(@"D:\Error.log", ex);
                return Content(ex.ToString());
            }
        }
        public ActionResult UserInfo_Submit(List<UserInfoVM> allInput)
        {
            try
            {
                var BranchId = System.Convert.ToInt64(Request.Form["Id"]);
                var CompanyId = dbCompany.cBranches.FirstOrDefault(x => x.Id == BranchId).CompanyId;

                var userId = Request.IsAuthenticated ? User.Identity.GetUserId().ToLower() : "";
                var isAdmin = dbCompany.cBranches.FirstOrDefault(x => x.Id == BranchId && x.AdminId.ToString() == userId) != null;

                if (isAdmin)
                {
                    foreach (var input in allInput)
                    {
                        var userInfo = dbCompany.cUserInfoes.FirstOrDefault(x => x.UserId == input.UserId);

                        if (userInfo != null)
                        {
                            userInfo.FirstName = input.UserInfo.FirstName;
                            userInfo.LastName = input.UserInfo.LastName;
                            userInfo.FullName = userInfo.LastName + " " + userInfo.FirstName;

                            userInfo.CountryId = 1;// input.UserInfo.CountryId;
                            userInfo.CorporationId = 1;//input.UserInfo.CorporationId;
                            userInfo.CompanyId = CompanyId;//input.UserInfo.CompanyId;
                            userInfo.BranchId = BranchId;//input.UserInfo.BranchId;

                            userInfo.DepartmentId = input.UserInfo.DepartmentId;
                            userInfo.RankId = input.UserInfo.RankId;
                            userInfo.GradeId = input.UserInfo.GradeId;
                            userInfo.ManagerId = input.UserInfo.ManagerId;

                            userInfo.StartWorkingDate = input.UserInfo.StartWorkingDate;
                            userInfo.StartCurrentJobDate = input.UserInfo.StartCurrentJobDate;

                            if (dbCompany.Entry(userInfo).State == System.Data.Entity.EntityState.Modified)
                                dbCompany.SaveChanges();
                        }
                        else
                        {
                            //New user
                            string newEmail = input.LoginUser.UserName;
                            string newPassword = sys.Application_Name.ToLower();

                            if (newEmail != null && !newEmail.Contains("@"))
                                newEmail += "@bitexco.com.vn";

                            if (newEmail != string.Empty
                                && input.UserInfo.FirstName != string.Empty
                                && input.UserInfo.LastName != string.Empty
                                && input.UserInfo.DepartmentId != 0
                                && input.UserInfo.RankId != 0
                                && input.UserInfo.GradeId != 0
                                //&& input.UserInfo.ManagerId != null
                                )
                            {
                                var newUser = new ApplicationUser { UserName = newEmail, Email = newEmail };
                                var registered = UserManager.Create(newUser, newPassword);

                                if (registered.Succeeded)
                                {
                                    userInfo = new cUserInfo();

                                    userInfo.UserId = Guid.Parse(newUser.Id);
                                    userInfo.UserIdd = Guid.NewGuid();

                                    userInfo.FirstName = input.UserInfo.FirstName;
                                    userInfo.LastName = input.UserInfo.LastName;
                                    userInfo.FullName = userInfo.LastName + " " + userInfo.FirstName;

                                    userInfo.CountryId = 1;// input.UserInfo.CountryId;
                                    userInfo.CorporationId = 1;//input.UserInfo.CorporationId;
                                    userInfo.CompanyId = CompanyId;//input.UserInfo.CompanyId;
                                    userInfo.BranchId = BranchId;//input.UserInfo.BranchId;

                                    userInfo.DepartmentId = input.UserInfo.DepartmentId;
                                    userInfo.RankId = input.UserInfo.RankId;
                                    userInfo.GradeId = input.UserInfo.GradeId;
                                    userInfo.ManagerId = input.UserInfo.ManagerId;

                                    userInfo.StartWorkingDate = input.UserInfo.StartWorkingDate;
                                    userInfo.StartCurrentJobDate = input.UserInfo.StartCurrentJobDate;

                                    dbCompany.cUserInfoes.Add(userInfo);
                                    dbCompany.SaveChanges();
                                }
                            }
                        }
                    }
                }

                return RedirectToAction("UserInfo/" + BranchId);
            }
            catch (Exception ex)
            {
                sys.Write(@"D:\Error.log", ex);
                return Content(ex.ToString());
            }
        }
        #endregion

        #region Department
        public ActionResult Department(long? Id)
        {
            try
            {
                if (Id == null)
                    Id = 1;

                ViewBag.Id = Id;

                var userId = Request.IsAuthenticated ? User.Identity.GetUserId().ToLower() : "";
                var isAdmin = dbCompany.cBranches.FirstOrDefault(x => x.Id == Id && x.AdminId.ToString() == userId) != null;

                if (isAdmin)
                {
                    var VM = new List<cDepartment>();

                    for (int i = 1; i <= 3; i++)
                    {
                        VM.Add(new cDepartment { Id = 0, cDepartmentForAll = new cDepartmentForAll { DepartmentName = "" } });
                    }

                    VM.AddRange(dbCompany.cDepartments.Where(x => x.BranchId == Id).OrderBy(x => x.Seq).ToList());
                    return View(VM);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                sys.Write(@"D:\Error.log", ex);
                return Content(ex.ToString());
            }
        }
        public ActionResult Department_Submit(List<cDepartment> allInput)
        {
            try
            {
                var BranchId = System.Convert.ToInt64(Request.Form["Id"]);
                var CompanyId = dbCompany.cBranches.FirstOrDefault(x => x.Id == BranchId).CompanyId;

                foreach (var input in allInput)
                {
                    if (!string.IsNullOrEmpty(input.cDepartmentForAll.DepartmentName))
                    {
                        var department = dbCompany.cDepartments.FirstOrDefault(x => x.Id == input.Id);

                        if (department == null)
                        {
                            department = new cDepartment();
                            department.CorporationId = 1;
                            department.CompanyId = CompanyId;
                            department.BranchId = BranchId;

                            var departmentForAll = dbCompany.cDepartmentForAlls.FirstOrDefault(x => x.DepartmentName == input.cDepartmentForAll.DepartmentName);

                            if (departmentForAll == null)
                            {
                                departmentForAll = new cDepartmentForAll();
                                departmentForAll.DepartmentName = input.cDepartmentForAll.DepartmentName;
                                departmentForAll.AutoCreate = false;

                                dbCompany.cDepartmentForAlls.Add(departmentForAll);
                                dbCompany.SaveChanges();
                            }

                            department.DepartmentForAllId = departmentForAll.Id;
                            department.Seq = input.Seq;

                            dbCompany.cDepartments.Add(department);
                            dbCompany.SaveChanges();
                        }
                        else
                        {
                            var departmentForAll = dbCompany.cDepartmentForAlls.FirstOrDefault(x => x.DepartmentName == input.cDepartmentForAll.DepartmentName);

                            if (departmentForAll == null)
                            {
                                departmentForAll = new cDepartmentForAll();
                                departmentForAll.DepartmentName = input.cDepartmentForAll.DepartmentName;
                                departmentForAll.AutoCreate = false;

                                dbCompany.cDepartmentForAlls.Add(departmentForAll);
                                dbCompany.SaveChanges();
                            }

                            department.DepartmentForAllId = departmentForAll.Id;
                            department.Seq = input.Seq;

                            dbCompany.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("Department/" + BranchId);
            }
            catch (Exception ex)
            {
                sys.Write(@"D:\Error.log", ex);
                return Content(ex.ToString());
            }
        }
        #endregion

        #region Rank
        public ActionResult Rank(long? Id)
        {
            try
            {
                if (Id == null)
                    Id = 1;

                ViewBag.Id = Id;

                var userId = Request.IsAuthenticated ? User.Identity.GetUserId().ToLower() : "";
                var isAdmin = dbCompany.cBranches.FirstOrDefault(x => x.Id == Id && x.AdminId.ToString() == userId) != null;

                if (isAdmin)
                {
                    var VM = new List<cRank>();

                    for (int i = 1; i <= 3; i++)
                    {
                        VM.Add(new cRank { Id = 0, cRankForAll = new cRankForAll { RankName = "" } });
                    }

                    VM.AddRange(dbCompany.cRanks.Where(x => x.BranchId == Id).OrderBy(x => x.Seq).ToList());
                    return View(VM);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                sys.Write(@"D:\Error.log", ex);
                return Content(ex.ToString());
            }
        }
        public ActionResult Rank_Submit(List<cRank> allInput)
        {
            try
            {
                var BranchId = System.Convert.ToInt64(Request.Form["Id"]);
                var CompanyId = dbCompany.cBranches.FirstOrDefault(x => x.Id == BranchId).CompanyId;

                var userId = Request.IsAuthenticated ? User.Identity.GetUserId().ToLower() : "";
                var isAdmin = dbCompany.cBranches.FirstOrDefault(x => x.Id == BranchId && x.AdminId.ToString() == userId) != null;

                if (isAdmin)
                {
                    foreach (var input in allInput)
                    {
                        if (!string.IsNullOrEmpty(input.cRankForAll.RankName))
                        {
                            var rank = dbCompany.cRanks.FirstOrDefault(x => x.Id == input.Id);

                            if (rank == null)
                            {
                                rank = new cRank();
                                rank.CorporationId = 1;
                                rank.CompanyId = CompanyId;
                                rank.BranchId = BranchId;

                                var rankForAll = dbCompany.cRankForAlls.FirstOrDefault(x => x.RankName == input.cRankForAll.RankName);

                                if (rankForAll == null)
                                {
                                    rankForAll = new cRankForAll();
                                    rankForAll.RankName = input.cRankForAll.RankName;
                                    rankForAll.AutoCreate = false;

                                    dbCompany.cRankForAlls.Add(rankForAll);
                                    dbCompany.SaveChanges();
                                }

                                rank.RankForAllId = rankForAll.Id;
                                rank.Seq = input.Seq;

                                rank.xKpiJob = input.xKpiJob;
                                rank.xKpiCompe = input.xKpiCompe;

                                dbCompany.cRanks.Add(rank);
                                dbCompany.SaveChanges();
                            }
                            else
                            {
                                var rankForAll = dbCompany.cRankForAlls.FirstOrDefault(x => x.RankName == input.cRankForAll.RankName);

                                if (rankForAll == null)
                                {
                                    rankForAll = new cRankForAll();
                                    rankForAll.RankName = input.cRankForAll.RankName;
                                    rankForAll.AutoCreate = false;

                                    dbCompany.cRankForAlls.Add(rankForAll);
                                    dbCompany.SaveChanges();
                                }

                                rank.RankForAllId = rankForAll.Id;
                                rank.Seq = input.Seq;

                                rank.xKpiJob = input.xKpiJob;
                                rank.xKpiCompe = input.xKpiCompe;

                                dbCompany.SaveChanges();
                            }
                        }
                    }
                }

                return RedirectToAction("Rank/" + BranchId);
            }
            catch (Exception ex)
            {
                sys.Write(@"D:\Error.log", ex);
                return Content(ex.ToString());
            }
        }
        #endregion


        #region RoleForSome
        public ActionResult RoleForSome(long? Id)
        {
            try
            {
                if (Id == null)
                    Id = 1;

                ViewBag.Id = Id;

                var userId = Request.IsAuthenticated ? User.Identity.GetUserId().ToLower() : "";
                var isAdmin = dbCompany.cBranches.FirstOrDefault(x => x.Id == Id && x.AdminId.ToString() == userId) != null;

                if (isAdmin)
                {
                    var VM = dbCompany.aRoleForSomes.Where(x => x.BranchId == Id).ToList();
                    return View(VM);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                sys.Write(@"D:\Error.log", ex);
                return Content(ex.ToString());
            }
        }
        #endregion

        #region RoleForSome_Config
        public ActionResult RoleForSome_Config(long id)
        {
            try
            {
                var roleForSome = dbCompany.aRoleForSomes.FirstOrDefault(x => x.Id == id);
                if (roleForSome != null)
                {
                    ViewData["RoleForSomeName"] = roleForSome.RoleName;
                    ViewData["RoleForSomeId"] = roleForSome.Id;

                    List<RoleForSome_ConfigVM> allVM = new List<RoleForSome_ConfigVM>();

                    var allRoleForAll = dbCompany.aRoleForAlls;
                    var allRoleForSomeAllow = dbCompany.aRoleForSomeAllows;
                    //var allRole = dbCompany.aRoles.Where(x => x.CorporationId == roleForSome.CorporationId && x.CompanyId == roleForSome.CompanyId && x.BranchId == roleForSome.BranchId);

                    /*
                    var allRank = dbCompany.cRanks.Where(x => x.CorporationId == roleForSome.CorporationId && x.CompanyId == roleForSome.CompanyId && x.BranchId == roleForSome.BranchId);
                    var allDepartment = dbCompany.cDepartments.Where(x => x.CorporationId == roleForSome.CorporationId && x.CompanyId == roleForSome.CompanyId && x.BranchId == roleForSome.BranchId);
                    var allBranch = dbCompany.cBranches.Where(x => x.CompanyId == roleForSome.CompanyId);
                    var allCompany = dbCompany.cCompanies.Where(x => x.CorporationId == roleForSome.CorporationId);
                    var allCorporation = dbCompany.cCorporations;
                    */

                    //Loc theo kha nang cua Admin
                    var allRank = dbCompany.cRanks;
                    var allDepartment = dbCompany.cDepartments;
                    var allBranch = dbCompany.cBranches;
                    var allCompany = dbCompany.cCompanies;
                    var allCorporation = dbCompany.cCorporations;

                    foreach (aRoleForAll roleForAll in allRoleForAll)
                    {
                        var allCheckedRank =
                            from x in allRoleForSomeAllow
                            where
                            x.RoleForSomeId == roleForSome.Id && x.RoleForAllId == roleForAll.Id
                            && x.AllowRelationTypeId == 6
                            select x;

                        var allCheckedDepartment =
                            from x in allRoleForSomeAllow
                            where
                            x.RoleForSomeId == roleForSome.Id && x.RoleForAllId == roleForAll.Id
                            && x.AllowRelationTypeId == 5
                            select x;

                        var allCheckedBranch =
                            from x in allRoleForSomeAllow
                            where
                            x.RoleForSomeId == roleForSome.Id && x.RoleForAllId == roleForAll.Id
                            && x.AllowRelationTypeId == 4
                            select x;

                        var allCheckedCompany =
                            from x in allRoleForSomeAllow
                            where
                            x.RoleForSomeId == roleForSome.Id && x.RoleForAllId == roleForAll.Id
                            && x.AllowRelationTypeId == 3
                            select x;

                        var allCheckedCorporation =
                            from x in allRoleForSomeAllow
                            where
                            x.RoleForSomeId == roleForSome.Id && x.RoleForAllId == roleForAll.Id
                            && x.AllowRelationTypeId == 2
                            select x;

                        //Rank
                        List<List<RankAndChecked>> allRankAndChecked = new List<List<RankAndChecked>>();

                        foreach (cBranch Branch in allBranch.OrderBy(x => x.cCompany.cCorporation.Seq).ThenBy(x => x.cCompany.Seq).ThenBy(x => x.Seq))
                        {
                            List<RankAndChecked> someRankAndChecked = new List<RankAndChecked>();

                            foreach (cRank Rank in allRank.Where(x => x.BranchId == Branch.Id).OrderBy(x => x.Seq))
                            {
                                bool checkedRank = allCheckedRank.Any(x => x.AllowRelationValue == Rank.Id.ToString());
                                someRankAndChecked.Add(new RankAndChecked { Branch = Branch, Rank = Rank, Checked = checkedRank });
                            }

                            allRankAndChecked.Add(someRankAndChecked);
                        }

                        //Department
                        List<List<DepartmentAndChecked>> allDepartmentAndChecked = new List<List<DepartmentAndChecked>>();

                        foreach (cBranch Branch in allBranch.OrderBy(x => x.cCompany.cCorporation.Seq).ThenBy(x => x.cCompany.Seq).ThenBy(x => x.Seq))
                        {
                            List<DepartmentAndChecked> someDepartmentAndChecked = new List<DepartmentAndChecked>();

                            foreach (cDepartment Department in allDepartment.Where(x => x.BranchId == Branch.Id).OrderBy(x => x.Seq))
                            {
                                bool checkedDepartment = allCheckedDepartment.Any(x => x.AllowRelationValue == Department.Id.ToString());
                                someDepartmentAndChecked.Add(new DepartmentAndChecked { Branch = Branch, Department = Department, Checked = checkedDepartment });
                            }

                            allDepartmentAndChecked.Add(someDepartmentAndChecked);
                        }

                        //Branch
                        List<List<BranchAndChecked>> allBranchAndChecked = new List<List<BranchAndChecked>>();

                        foreach (cCompany Company in allCompany.OrderBy(x => x.cCorporation.Seq).ThenBy(x => x.Seq))
                        {
                            List<BranchAndChecked> someBranchAndChecked = new List<BranchAndChecked>();

                            foreach (cBranch Branch in allBranch.Where(x => x.CompanyId == Company.Id).OrderBy(x => x.Seq))
                            {
                                bool checkedBranch = allCheckedBranch.Any(x => x.AllowRelationValue == Branch.Id.ToString());
                                someBranchAndChecked.Add(new BranchAndChecked { Company = Company, Branch = Branch, Checked = checkedBranch });
                            }

                            allBranchAndChecked.Add(someBranchAndChecked);
                        }

                        //Company
                        List<List<CompanyAndChecked>> allCompanyAndChecked = new List<List<CompanyAndChecked>>();

                        foreach (cCorporation Corporation in allCorporation.OrderBy(x => x.Seq))
                        {
                            List<CompanyAndChecked> someCompanyAndChecked = new List<CompanyAndChecked>();

                            foreach (cCompany Company in allCompany.Where(x => x.CorporationId == Corporation.Id).OrderBy(x => x.Seq))
                            {
                                bool checkedCompany = allCheckedCompany.Any(x => x.AllowRelationValue == Company.Id.ToString());
                                someCompanyAndChecked.Add(new CompanyAndChecked { Corporation = Corporation, Company = Company, Checked = checkedCompany });
                            }

                            allCompanyAndChecked.Add(someCompanyAndChecked);
                        }

                        //Corporation
                        List<CorporationAndChecked> allCorporationAndChecked = new List<CorporationAndChecked>();

                        foreach (cCorporation Corporation in allCorporation.OrderBy(x => x.Seq))
                        {
                            bool checkedCorporation = allCheckedCorporation.Any(x => x.AllowRelationValue == Corporation.Id.ToString());
                            allCorporationAndChecked.Add(new CorporationAndChecked { Corporation = Corporation, Checked = checkedCorporation });
                        }

                        //allVM
                        allVM.Add(new RoleForSome_ConfigVM
                        {
                            RoleForAll = roleForAll,

                            allRankAndChecked = allRankAndChecked,
                            allDepartmentAndChecked = allDepartmentAndChecked,
                            allBranchAndChecked = allBranchAndChecked,
                            allCompanyAndChecked = allCompanyAndChecked,
                            allCorporationAndChecked = allCorporationAndChecked
                        });
                    }

                    return PartialView(allVM);
                }

                return Content("Error");
            }
            catch (Exception ex)
            {
                sys.Write(@"D:\Error.log", ex);
                return Content(ex.ToString());
            }
        }

        [ValidateAntiForgeryToken]
        public ActionResult RoleForSome_Config_Submit(List<RoleForSome_ConfigVM> allInput)
        {
            try
            {
                long roleForSomeId = 0;

                if (Url.RequestContext.RouteData.Values["id"] != null)
                    roleForSomeId = Convert.ToInt64(Url.RequestContext.RouteData.Values["id"].ToString());

                var roleForSome = dbCompany.aRoleForSomes.FirstOrDefault(x => x.Id == roleForSomeId);

                var allRoleForAll = dbCompany.aRoleForAlls;
                var allRoleForSomeAllow = dbCompany.aRoleForSomeAllows;

                foreach (var input in allInput)
                {
                    var roleForAll = dbCompany.aRoleForAlls.FirstOrDefault(x => x.Id == input.RoleForAll.Id);

                    if (roleForAll != null)
                    {
                        #region allRankAndChecked
                        var allRankAndChecked = input.allRankAndChecked;

                        var allLastCheckedRank =
                            from x in allRoleForSomeAllow
                            where
                            x.RoleForSomeId == roleForSome.Id && x.RoleForAllId == roleForAll.Id
                            && x.AllowRelationTypeId == 6
                            select x;

                        var allCurrentCheckedRank = new List<aRoleForSomeAllow>();

                        foreach (var someRankAndChecked in allRankAndChecked)
                        {
                            foreach (var rankAndChecked in someRankAndChecked)
                            {
                                if (rankAndChecked.Checked)
                                {
                                    var roleForSomeAllow = allRoleForSomeAllow.FirstOrDefault(x =>
                                        x.RoleForSomeId == roleForSome.Id && x.RoleForAllId == roleForAll.Id
                                        && x.AllowRelationTypeId == 6 && x.AllowRelationValue == rankAndChecked.Rank.Id.ToString()
                                        );

                                    if (roleForSomeAllow == null)
                                    {
                                        roleForSomeAllow = new aRoleForSomeAllow();

                                        roleForSomeAllow.RoleForSomeId = roleForSome.Id;
                                        roleForSomeAllow.RoleForAllId = roleForAll.Id;

                                        roleForSomeAllow.AllowRelationTypeId = 6;
                                        roleForSomeAllow.AllowRelationValue = rankAndChecked.Rank.Id.ToString();

                                        allRoleForSomeAllow.Add(roleForSomeAllow);
                                        dbCompany.SaveChanges();
                                    }

                                    allCurrentCheckedRank.Add(roleForSomeAllow);
                                }
                            }
                        }

                        //Remove if not in allCurrentCheckedRank
                        var removedCheckedRank = allLastCheckedRank.ToList().Where(x => !allCurrentCheckedRank.Any(y => y.Id == x.Id)).ToList();

                        foreach (var one in removedCheckedRank)
                        {
                            allRoleForSomeAllow.Remove(one);
                            dbCompany.SaveChanges();
                        }
                        #endregion
                    }
                }

                return RedirectToAction("RoleForSome");
            }
            catch (Exception ex)
            {
                sys.Write(@"D:\Error.log", ex);
                return Content(ex.ToString());
            }
        }
        #endregion

        #region RoleForSome_User
        public ActionResult RoleForSome_User(long id)
        {
            try
            {
                var roleForSome = dbCompany.aRoleForSomes.FirstOrDefault(x => x.Id == id);
                if (roleForSome != null)
                {
                    ViewData["RoleForSomeName"] = roleForSome.RoleName;
                    ViewData["RoleForSomeId"] = roleForSome.Id;

                    var allUserId = dbCompany.aRoles.Where(x => x.RoleForSomeId == roleForSome.Id && x.RoleRelationTypeId == 7).Select(x => x.RoleRelationValue.ToLower());
                    return PartialView(allUserId.ToList());
                }

                return Content("Error");
            }
            catch (Exception ex)
            {
                sys.Write(@"D:\Error.log", ex);
                return Content(ex.ToString());
            }
        }
        public ActionResult RoleForSome_User_Submit(long RoleForSomeId, string allUserTreeViewChecked)
        {
            try
            {
                var allCurrentRole = new List<string>();
                var allJson = JsonConvert.DeserializeObject<List<UserTreeViewChecked>>(allUserTreeViewChecked);

                foreach (var json in allJson)
                {
                    if (json.Type == "User")
                    {
                        string _userId = json.Id.ToLower();

                        var role = dbCompany.aRoles.FirstOrDefault(x => x.RoleForSomeId == RoleForSomeId && x.RoleRelationTypeId == 7 && x.RoleRelationValue == _userId);

                        if (role == null)
                        {
                            role = new aRole();

                            role.RoleRelationTypeId = 7;
                            role.RoleRelationValue = _userId;
                            role.RoleForSomeId = RoleForSomeId;

                            dbCompany.aRoles.Add(role);
                            dbCompany.SaveChanges();
                        }

                        allCurrentRole.Add(_userId);
                    }
                }

                //Remove if not in allCurrentRole
                var removedRole = dbCompany.aRoles.Where(x => x.RoleForSomeId == RoleForSomeId && x.RoleRelationTypeId == 7 && !allCurrentRole.Any(y => y == x.RoleRelationValue)).ToList();

                foreach (var Role in removedRole)
                {
                    dbCompany.aRoles.Remove(Role);
                    dbCompany.SaveChanges();
                }

                return RedirectToAction("RoleForSome");
            }
            catch (Exception ex)
            {
                sys.Write(@"D:\Error.log", ex);
                return Content(ex.ToString());
            }
        }
        #endregion
    }
}

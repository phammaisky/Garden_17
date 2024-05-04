using BtcKpi.Data.Infrastructure;
using BtcKpi.Data.Repositories;
using BtcKpi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BtcKpi.Service.Common;

namespace BtcKpi.Service
{
    // operations you want to expose
    public interface IDepartmentService
    {
        List<UpfSchedule> DepartSchedulesByDeparment(int departmentId, int companyID);
        List<DepartmentInfo> GetDepartByConditions(string companies, string departments, string scheduleTypes, string years, string scheduleIds, string statusId);
        bool InsertDepartment(int userId, int? departmentID, Upf upf, List<UpfPersRewProposal> persRewPropDetails, List<UpfNameDetail> details, ref string insertMsg);
        bool UpdateDepartment(int userId, Upf upf, List<UpfPersRewProposal> persRewPropDetails, List<UpfNameDetail> details, ref string insertMsg);
        Upf GetDepartmentInfo(int id, ref List<UpfNameDetail> nameDetails, ref List<UpfPersRewProposal> persRewPropDetail, ref List<UpfComment> comments);
        bool deleteDepartment(int departmentID, int userID);
        bool ApprovedOrNotApprovedUpf(int userId, int iD, bool isApprove, string comment, List<UpfNameDetail> nameDetails, decimal? totalManagePoint, ref string updateMsg);
        bool ApprovedOrNotBODApprovedUpf(int userId, int ID, bool isBODApprove, string comment, List<UpfNameDetail> nameDetails, decimal? totalBODPoint, ref string updateMsg);
        bool InsertComment(int userId, int iD, string comment, ref string updateMsg);
        bool CheckCreateDepart(Upf upf, int userId);
        List<CompleteWorkTitle> CompleteWorkTitleByDepartment(int departmentId);
        List<DepartmentInfo> GetReportDepartment(string companies, string departments, string years);
        UpfSchedule GetScheduleById(int? id);
        void InsertUpfSummary(UpfSummary upfSum, int? departmentId, int? year, int userId);
        void UpdateUpfSummary(UpfSummary upfSum, int userId);
        UpfSummary GetUpfSummaryByDepartYear(int? departmentId, int? year);
        UpfCrossSummary GetUpfCrossSummary(int? departmentId, int? month, int? year);
        void InsertUpfCrossSummary(UpfCrossSummary crossSummary);
        void UpdateUpfCrossSummary(UpfCrossSummary crossSummary);
        UpfCrossSummary GetUpfCrossSummaryByYear(int? departmentId, int? year);
    }

    public class DepartmentService : IDepartmentService
    {
        private readonly IUpfRepository departRepository;
        private readonly IDepartScheduleRepository departScheduleRepository;
        private readonly IDepartNameDetailRepository departNameDetailRepository;
        private readonly IDepartJobDetailRepository departJobDetailRepository;
        private readonly IDepartPersRewPropRepository departPersRewPropRepository;
        private readonly IDepartCommentRepository commentRepository;
        private readonly IUpfHisRepository upfHisRepository;
        private readonly IUpfNameDetailHisRepository nameDetailHisRepository;
        private readonly IUpfJobDetailHisRepository jobDetailHisRepository;
        private readonly IUpfPersRewPropHisRepository persRewPropHisRepository;
        private readonly IUpfRateRepository upfRateRepository;
        private readonly ICompleteWorkTitleRepository completeWorkTitleRepository;
        private readonly IUpfSummaryRepository upfSummaryRepository;
        private readonly IUpfCrossSummaryRepository upfCrossSummaryRepository;

        private readonly IUnitOfWork unitOfWork;

        public DepartmentService(IUpfRepository departRepository, IDepartScheduleRepository departScheduleRepository, IIpfDetailRepository departDetailRepository, 
            IIpfCommentRepository departCommentRepository, IzIpfRepository zDepartRepository, IzIpfDetailRepository zDepartDetailRepository, IDepartNameDetailRepository departNameDetailRepository, 
            IDepartJobDetailRepository departJobDetailRepository, IDepartPersRewPropRepository departPersRewPropRepository, IDepartCommentRepository commentRepository, 
            IUpfHisRepository upfHisRepository, IUpfNameDetailHisRepository nameDetailHisRepository,IUpfJobDetailHisRepository jobDetailHisRepository, IUpfPersRewPropHisRepository persRewPropHisRepository, 
            IUpfRateRepository upfRateRepository, ICompleteWorkTitleRepository completeWorkTitleRepository, IUpfSummaryRepository upfSummaryRepository, IUpfCrossSummaryRepository upfCrossSummaryRepository, IUnitOfWork unitOfWork)
        {
            this.departRepository = departRepository;
            this.departScheduleRepository = departScheduleRepository;
            this.departNameDetailRepository = departNameDetailRepository;
            this.departJobDetailRepository = departJobDetailRepository;
            this.departPersRewPropRepository = departPersRewPropRepository;
            this.commentRepository = commentRepository;
            this.upfHisRepository = upfHisRepository;
            this.nameDetailHisRepository = nameDetailHisRepository;
            this.jobDetailHisRepository = jobDetailHisRepository;
            this.persRewPropHisRepository = persRewPropHisRepository;
            this.upfRateRepository = upfRateRepository;
            this.completeWorkTitleRepository = completeWorkTitleRepository;
            this.upfSummaryRepository = upfSummaryRepository;
            this.upfCrossSummaryRepository = upfCrossSummaryRepository;
            this.unitOfWork = unitOfWork;
        }

        public List<CompleteWorkTitle> CompleteWorkTitleByDepartment(int departmentId)
        {
            return completeWorkTitleRepository.GetByDeparmentId(departmentId);
        }

        public List<UpfSchedule> DepartSchedulesByDeparment(int departmentId, int companyId)
        {
            return departScheduleRepository.GetByDepartmentId(departmentId, companyId);
        }

        public bool InsertDepartment(int userId, int? departmentID, Upf upf, List<UpfPersRewProposal> persRewPropDetails, List<UpfNameDetail> details, ref string errorMessage)
        {
            if (upf.WeightTotal != 100)
            {
                errorMessage = "Tổng tỷ trọng mục tiêu hoàn thành công việc trong năm/kỳ phải bằng 100%.";
                return false;
            }
            if (details == null || details.Count <= 0)
            {
                errorMessage = "Mục tiêu công việc không được để trống !";
                return false;
            }
            else
            {
                bool checkJobDetail = false;
                foreach (var itemCheck in details)
                {
                    if (itemCheck.JobDetails != null && itemCheck.JobDetails.Count > 0)
                    {
                        checkJobDetail = true;
                        break;
                    }
                    if (itemCheck.JobDetails == null || itemCheck.JobDetails.Count <= 0)
                    {
                        errorMessage = "Chi tiết mục tiêu công việc không được để trống !";
                        checkJobDetail = false;
                    }
                }
                if(checkJobDetail == false)
                {
                    return checkJobDetail;
                }
            }
            var insertTime = DateTime.Now;
            byte? delFlg = 0;
            int seqID = 0;

            upf.DeleteFlg = delFlg;
            upf.Created = insertTime;
            upf.CreatedBy = userId;
            upf.PersChargID = userId;
            upf.DepartmentID = departmentID;
            departRepository.Add(upf);
            upf.ID = departRepository.GetDepartmentID();
            int upfHisID = InsertUpfHistory(upf, 0, "Tạo mới");
            foreach (var item in details)
            {
                if(item.JobDetails != null && item.JobDetails.Count > 0)
                {
                    item.DeleteFlg = delFlg;
                    item.CreatedDate = insertTime;
                    item.CreatedBy = userId;
                    item.UpfID = upf.ID;
                    departNameDetailRepository.Add(item);
                    InsertNameDetailHistory(item, 0, "Tạo mới", upfHisID);
                    int nameDetailHisID = 0;
                    if (seqID == 0)
                    {
                        item.ID = departNameDetailRepository.GetNameDetailID();
                        nameDetailHisID = nameDetailHisRepository.GetNameDetailHisID();
                    }
                    else
                    {
                        item.ID = departNameDetailRepository.GetNameDetailID() + seqID;
                        nameDetailHisID = nameDetailHisRepository.GetNameDetailHisID() + seqID;
                    }
                    seqID++;
                    foreach (var itemDetail in item.JobDetails)
                    {
                        itemDetail.DeleteFlg = delFlg;
                        itemDetail.CreatedDate = insertTime;
                        itemDetail.CreatedBy = userId;
                        itemDetail.UpfNameDetailID = item.ID;
                        itemDetail.JobName = itemDetail.JobName.Trim();
                        itemDetail.NumberPlan = itemDetail.NumberPlan.Trim();
                        itemDetail.PerformResults = itemDetail.PerformResults.Trim();
                        departJobDetailRepository.Add(itemDetail);
                        InsertJobDetailHis(itemDetail, 0, "Tạo mới", nameDetailHisID);
                    }
                }
            }
            foreach (var itemPers in persRewPropDetails)
            {
                itemPers.DeleteFlg = delFlg;
                itemPers.CreatedDate = insertTime;
                itemPers.CreatedBy = userId;
                itemPers.UpfID = upf.ID;
                itemPers.EmployeeName = itemPers.EmployeeName.Trim();
                departPersRewPropRepository.Add(itemPers);
                InsertPersRewPropHistory(itemPers, 0, "Tạo mới", upfHisID);
            }

            unitOfWork.Commit();

            return true;

        }

        public List<DepartmentInfo> GetDepartByConditions(string companies, string departments, string scheduleTypes, string years, string scheduleIds, string statusId)
        {
            return departRepository.GetDepartByConditions(BtcHelper.RemoveComman(companies), BtcHelper.RemoveComman(departments), BtcHelper.RemoveComman(scheduleTypes), BtcHelper.RemoveComman(years), BtcHelper.RemoveComman(scheduleIds), BtcHelper.RemoveComman(statusId));
        }

        public Upf GetDepartmentInfo(int id, ref List<UpfNameDetail> nameDetails, ref List<UpfPersRewProposal> persRewPropDetail, ref List<UpfComment> comments)
        {
            Upf upf = departRepository.GetById(id);
            UpfRate upfRate = upfRateRepository.GetById(Int32.Parse(upf.SelfRating.ToString()));
            if(upfRate != null)
            {
                upf.SelfRatingName = upfRate.Name;
            }
            nameDetails = departNameDetailRepository.GetNameDetailByUpfId(id);
            foreach (var item in nameDetails)
            {
                if (item.JobDetails == null) {
                    item.JobDetails = new List<UpfJobDetail>();
                }
                item.JobDetails = departJobDetailRepository.GetJobDetailByNameDetailId(item.ID);
                foreach (var jobBO in item.JobDetails)
                {
                    jobBO.NameDetailID = item.Order;
                    jobBO.NameKPIEdit = item.NameKPI;
                }
                item.JobDetails.OrderByDescending(t => t.UpfNameDetailID);
            }

            persRewPropDetail = departPersRewPropRepository.GetPersRewPropByUpfId(id);
            comments = commentRepository.GetByUpfId(id);
            return upf;
        }

        public bool UpdateDepartment(int userId, Upf upf, List<UpfPersRewProposal> persRewPropDetails, List<UpfNameDetail> details, ref string errorMessage)
        {
            var insertTime = DateTime.Now;
            byte? delFlg = 0;
            departRepository.Update(upf);
            int upfHisID = InsertUpfHistory(upf, 1, "Chỉnh sửa");
            foreach (var item in details)
            {
                UpdateOrInsertNameDetail(item, insertTime, delFlg, userId, upf.ID);
                InsertNameDetailHistory(item, 1, "Chỉnh sửa", upfHisID);
                if(item.JobDetails != null && item.JobDetails.Count > 0)
                {
                    UpdateOrInsertJobDetail(item.JobDetails, insertTime, delFlg, userId, item.ID);
                }
            }
            foreach (var itemPers in persRewPropDetails)
            {
                UpdateOrInsertPersRewProp(itemPers, insertTime, delFlg, userId, upf.ID);
                InsertPersRewPropHistory(itemPers, 1, "Chỉnh sửa", upfHisID);
            }

            unitOfWork.Commit();

            return true;

        }

        public void UpdateOrInsertNameDetail(UpfNameDetail item, DateTime insertTime, byte? delFlg, int userId, int upfID)
        {
            if (item.ID != 0)
            {
                departNameDetailRepository.Update(item);
            }
            else
            {
                item.DeleteFlg = delFlg;
                item.CreatedDate = insertTime;
                item.CreatedBy = userId;
                item.UpfID = upfID;
                departNameDetailRepository.Add(item);
                item.ID = departNameDetailRepository.GetNameDetailID();
            }
        }

        public void UpdateOrInsertJobDetail(List<UpfJobDetail> itemDetails, DateTime insertTime, byte? delFlg, int userId, int nameDetailID)
        {
            foreach (var itemDetail in itemDetails)
            {
                if (itemDetail.ID != 0)
                {
                    List<UpfJobDetail> lstJobDetails = departJobDetailRepository.GetJobDetailByNameDetailId(nameDetailID);
                    foreach (var jobDetailOld in lstJobDetails)
                    {
                        UpfJobDetail jobDetail = itemDetails.Where(t => t.ID == jobDetailOld.ID).OrderByDescending(t => t.Order).FirstOrDefault();
                        if (jobDetail == null)
                        {
                            jobDetailOld.DeleteFlg = 1;
                            jobDetailOld.Deleted = insertTime;
                            jobDetailOld.DeletedBy = userId;
                            departJobDetailRepository.Update(jobDetailOld);
                            InsertJobDetailHis(jobDetailOld, 1, "Chỉnh sửa", jobDetailOld.UpfNameDetailID);
                        } else if(itemDetail.ID == jobDetailOld.ID && itemDetail.Order == jobDetailOld.Order)
                        {
                            jobDetailOld.JobName = itemDetail.JobName;
                            jobDetailOld.NumberPlan = itemDetail.NumberPlan;
                            jobDetailOld.PerformResults = itemDetail.PerformResults;
                            jobDetailOld.Weight = itemDetail.Weight;
                            jobDetailOld.Point = itemDetail.Point;
                            jobDetailOld.Order = itemDetail.Order;
                            jobDetailOld.ScheduledTime = itemDetail.ScheduledTime;
                            departJobDetailRepository.Update(jobDetailOld);
                            InsertJobDetailHis(jobDetailOld, 1, "Chỉnh sửa", jobDetailOld.UpfNameDetailID);
                        }
                    }

                }
                else
                {
                    itemDetail.DeleteFlg = delFlg;
                    itemDetail.CreatedDate = insertTime;
                    itemDetail.CreatedBy = userId;
                    itemDetail.UpfNameDetailID = nameDetailID;
                    departJobDetailRepository.Add(itemDetail);
                    InsertJobDetailHis(itemDetail, 1, "Chỉnh sửa", nameDetailID);
                }
            }
        }

        public void UpdateOrInsertPersRewProp(UpfPersRewProposal itemPers, DateTime insertTime, byte? delFlg, int userId, int upfID)
        {
            if (itemPers.ID != 0)
            {
                departPersRewPropRepository.Update(itemPers);
            } else
            {
                itemPers.DeleteFlg = delFlg;
                itemPers.CreatedDate = insertTime;
                itemPers.CreatedBy = userId;
                itemPers.UpfID = upfID;
                departPersRewPropRepository.Add(itemPers);
            }
        }

        public bool deleteDepartment(int departmentID, int userID)
        {
            var DelTime = DateTime.Now;
            //Xoa UPF
            Upf upf = departRepository.GetById(departmentID);
            upf.DeleteFlg = 1;
            upf.Deleted = DelTime;
            upf.DeletedBy = userID;
            departRepository.Update(upf);
            int upfHisID = InsertUpfHistory(upf, 2, "Xóa");
            List<UpfNameDetail> nameDetails = departNameDetailRepository.GetNameDetailByUpfId(departmentID);
            foreach (var item in nameDetails)
            {
                //Xoa ten cong viec
                item.DeleteFlg = 1;
                item.Deleted = DelTime;
                item.DeletedBy = userID;
                departNameDetailRepository.Update(item);
                InsertNameDetailHistory(item, 2, "Xóa", upfHisID);
                //Xoa chi tiet cong viec
                List<UpfJobDetail> lstJobDetails = departJobDetailRepository.GetJobDetailByNameDetailId(item.ID);
                foreach (var itemJob in lstJobDetails)
                {
                    itemJob.DeleteFlg = 1;
                    itemJob.Deleted = DelTime;
                    itemJob.DeletedBy = userID;
                    departJobDetailRepository.Update(itemJob);
                    InsertJobDetailHis(itemJob, 2, "Xóa", itemJob.UpfNameDetailID);
                }
            }
            //Xoa ca nhan de xuat khen thuong
            List<UpfPersRewProposal> lstPersRewProp = departPersRewPropRepository.GetPersRewPropByUpfId(departmentID);
            foreach (var itemPers in lstPersRewProp)
            {
                itemPers.DeleteFlg = 1;
                itemPers.Deleted = DelTime;
                itemPers.DeletedBy = userID;
                departPersRewPropRepository.Update(itemPers);
                InsertPersRewPropHistory(itemPers, 2, "Xóa", upfHisID);
            }

            unitOfWork.Commit();

            return true;

        }

        public bool ApprovedOrNotApprovedUpf(int userId, int ID, bool isApprove, string comment, List<UpfNameDetail> nameDetails, decimal? totalManagePoint, ref string updateMsg)
        {
            var approvedTime = DateTime.Now;
            List<UpfNameDetail> nameDetailsNew = new List<UpfNameDetail>();
            List<UpfPersRewProposal> persRewPropDetail = new List<UpfPersRewProposal>();
            List<UpfComment> comments = new List<UpfComment>();

            var curentUpf = GetDepartmentInfo(ID, ref nameDetailsNew, ref persRewPropDetail, ref comments);

            if (isApprove)
            {
                curentUpf.StatusID = 2;
                curentUpf.Approved = approvedTime;
                curentUpf.TotalManagePoint = totalManagePoint;
                departRepository.Update(curentUpf);
                int upfHisID = InsertUpfHistory(curentUpf, 3, "Phê duyệt");

                foreach (var nameDetail in nameDetails)
                {
                    if(nameDetail.JobDetails != null && nameDetail.JobDetails.Count > 0)
                    {
                        foreach (var jobDetail in nameDetail.JobDetails)
                        {
                            foreach (var nameDetailNew in nameDetailsNew)
                            {
                                InsertNameDetailHistory(nameDetailNew, 3, "Phê duyệt", upfHisID);
                                foreach (var jobDetailNew in nameDetailNew.JobDetails)
                                {
                                    if(jobDetail.ID == jobDetailNew.ID)
                                    {
                                        jobDetailNew.ManagePoint = jobDetail.ManagePoint;
                                        departJobDetailRepository.Update(jobDetailNew);
                                        InsertJobDetailHis(jobDetailNew, 0, "Phê duyệt", jobDetail.UpfNameDetailID);
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (var itemPers in persRewPropDetail)
                {
                    InsertPersRewPropHistory(itemPers, 3, "Phê duyệt", upfHisID);
                }
            } else
            {
                curentUpf.StatusID = 4;
                departRepository.Update(curentUpf);
            }

            if (!string.IsNullOrEmpty(comment))
            {
                UpfComment newComment = new UpfComment() { UpfID = ID, Comment = comment, DeleteFlg = 0, CreatedDate = approvedTime, CreatedBy = userId };
                commentRepository.Add(newComment);
            }

            unitOfWork.Commit();

            return true;
        }

        public bool ApprovedOrNotBODApprovedUpf(int userId, int ID, bool isBODApprove, string comment, List<UpfNameDetail> nameDetails, decimal? totalBODPoint, ref string updateMsg)
        {
            var approvedTime = DateTime.Now;
            List<UpfNameDetail> nameDetailsNew = new List<UpfNameDetail>();
            List<UpfPersRewProposal> persRewPropDetail = new List<UpfPersRewProposal>();
            List<UpfComment> comments = new List<UpfComment>();

            var curentUpf = GetDepartmentInfo(ID, ref nameDetailsNew, ref persRewPropDetail, ref comments);

            if (isBODApprove)
            {
                curentUpf.StatusID = 3;
                curentUpf.Approved = approvedTime;
                curentUpf.TotalBODPoint = totalBODPoint;
                departRepository.Update(curentUpf);
                int upfHisID = InsertUpfHistory(curentUpf, 3, "Phê duyệt");

                foreach (var nameDetail in nameDetails)
                {
                    if (nameDetail.JobDetails != null && nameDetail.JobDetails.Count > 0)
                    {
                        foreach (var jobDetail in nameDetail.JobDetails)
                        {
                            foreach (var nameDetailNew in nameDetailsNew)
                            {
                                InsertNameDetailHistory(nameDetailNew, 3, "Phê duyệt", upfHisID);
                                foreach (var jobDetailNew in nameDetailNew.JobDetails)
                                {
                                    if(jobDetail.ID == jobDetailNew.ID)
                                    {
                                        jobDetailNew.BodPoint = jobDetail.BodPoint;
                                        departJobDetailRepository.Update(jobDetailNew);
                                        InsertJobDetailHis(jobDetailNew, 0, "Phê duyệt", jobDetail.UpfNameDetailID);
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (var itemPers in persRewPropDetail)
                {
                    InsertPersRewPropHistory(itemPers, 3, "Phê duyệt", upfHisID);
                }
            }
            else
            {
                curentUpf.StatusID = 4;
                departRepository.Update(curentUpf);
            }

            if (!string.IsNullOrEmpty(comment))
            {
                UpfComment newComment = new UpfComment() { UpfID = ID, Comment = comment, DeleteFlg = 0, CreatedDate = approvedTime, CreatedBy = userId };
                commentRepository.Add(newComment);
            }

            unitOfWork.Commit();

            return true;
        }

        public bool InsertComment(int userId, int ID, string comment, ref string updateMsg)
        {
            var insertTime = DateTime.Now;
            if (!string.IsNullOrEmpty(comment))
            {
                UpfComment newComment = new UpfComment() { UpfID = ID, Comment = comment, DeleteFlg = 0, CreatedDate = insertTime, CreatedBy = userId };
                commentRepository.Add(newComment);
            }
            unitOfWork.Commit();

            return true;
        }

        public int InsertUpfHistory(Upf upf, byte? action, string descriptions)
        {
            UpfHis upfHis = new UpfHis();
            upfHis.Action = action;
            upfHis.Descriptions = descriptions;
            upfHis.OutsAchiev = upf.OutsAchiev;
            upfHis.PersChargID = upf.PersChargID;
            upfHis.ScheduleID = upf.ScheduleID;
            upfHis.ScheduleType = upf.ScheduleType;
            upfHis.SelfRating = upf.SelfRating;
            upfHis.StatusID = upf.StatusID;
            upfHis.TotalManagePoint = upf.TotalManagePoint;
            upfHis.TotalPoint = upf.TotalPoint;
            upfHis.Year = upf.Year;
            upfHis.upfID = upf.ID;
            upfHis.upfID = upf.ID;
            upfHis.UpdateBy = upf.CreatedBy;
            upfHis.Updated = upf.Created;
            upfHisRepository.Add(upfHis);
            return upfHisRepository.GetUpfHisID();
        }

        public void InsertNameDetailHistory(UpfNameDetail nameDetail, byte? action, string descriptions, int upfHisID)
        {
            UpfNameDetailHis detailHis = new UpfNameDetailHis();
            detailHis.Action = action;
            detailHis.Descriptions = descriptions;
            detailHis.NameKPI = nameDetail.NameKPI;
            detailHis.Order = nameDetail.Order;
            detailHis.UpdateBy = nameDetail.CreatedBy;
            detailHis.Updated = nameDetail.CreatedDate;
            detailHis.UpfHisID = upfHisID;
            nameDetailHisRepository.Add(detailHis);
        }

        public void InsertJobDetailHis(UpfJobDetail jobDetail, byte? action, string descriptions, int upfNameDetailID)
        {
            UpfJobDetailHis jobDetailHis = new UpfJobDetailHis();
            jobDetailHis.Action = action;
            jobDetailHis.Descriptions = descriptions;
            jobDetailHis.JobName = jobDetail.JobName;
            jobDetailHis.ManagePoint = jobDetail.ManagePoint;
            jobDetailHis.NumberPlan = jobDetail.NumberPlan;
            jobDetailHis.PerformResults = jobDetail.PerformResults;
            jobDetailHis.Point = jobDetail.Point;
            jobDetailHis.ScheduledTime = jobDetail.ScheduledTime;
            jobDetailHis.Weight = jobDetail.Weight;
            jobDetailHis.Order = jobDetail.Order;
            jobDetailHis.UpdateBy = jobDetail.CreatedBy;
            jobDetailHis.Updated = jobDetail.CreatedDate;
            jobDetailHis.UpfNameDetailHisID = upfNameDetailID;
            jobDetailHisRepository.Add(jobDetailHis);
        }

        public void InsertPersRewPropHistory(UpfPersRewProposal persRewProp, byte? action, string descriptions, int upfHisID)
        {
            UpfPersRewProposalHis persRewPropHis = new UpfPersRewProposalHis();
            persRewPropHis.Action = action;
            persRewPropHis.Descriptions = descriptions;
            persRewPropHis.EmployeeName = persRewProp.EmployeeName;
            persRewPropHis.PersOutsAchiev = persRewProp.PersOutsAchiev;
            persRewPropHis.Order = persRewProp.Order;
            persRewPropHis.UpdateBy = persRewProp.CreatedBy;
            persRewPropHis.Updated = persRewProp.CreatedDate;
            persRewPropHis.UpfHisID = upfHisID;
            persRewPropHisRepository.Add(persRewPropHis);
        }

        public bool CheckCreateDepart(Upf upf, int userId)
        {
            return departRepository.CheckCreateDepart(upf, userId);
        }

        public List<DepartmentInfo> GetReportDepartment(string companies, string departments, string years)
        {
            return departRepository.GetReportDepartment(BtcHelper.RemoveComman(companies), BtcHelper.RemoveComman(departments), BtcHelper.RemoveComman(years));
        }

        public UpfSchedule GetScheduleById(int? id)
        {
            return departScheduleRepository.GetScheduleById(id);
        }

        public void InsertUpfSummary(UpfSummary upfSum, int? departmentId, int? year, int userId)
        {
            upfSum.DepartmentID = departmentId;
            upfSum.Year = year;
            upfSum.Created = DateTime.Now;
            upfSum.CreatedBy = userId;
            upfSum.Active = 1;
            upfSummaryRepository.Add(upfSum);

            unitOfWork.Commit();
        }
        public void UpdateUpfSummary(UpfSummary upfSum, int userId)
        {
            upfSum.Updated = DateTime.Now;
            upfSum.UpdateBy = userId;
            upfSummaryRepository.Update(upfSum);

            unitOfWork.Commit();
        }

        public UpfSummary GetUpfSummaryByDepartYear(int? departmentId, int? year)
        {
            return upfSummaryRepository.GetUpfSummaryByDepartYear(departmentId, year);
        }

        public UpfCrossSummary GetUpfCrossSummary(int? departmentId, int? month, int? year)
        {
            return upfCrossSummaryRepository.GetUpfCrossSummary(departmentId, month, year);
        }

        public void InsertUpfCrossSummary(UpfCrossSummary crossSummary)
        {
            upfCrossSummaryRepository.Add(crossSummary);

            unitOfWork.Commit();
        }

        public void UpdateUpfCrossSummary(UpfCrossSummary crossSummary)
        {
            upfCrossSummaryRepository.Update(crossSummary);

            unitOfWork.Commit();
        }

        public UpfCrossSummary GetUpfCrossSummaryByYear(int? departmentId, int? year)
        {
            return upfCrossSummaryRepository.GetUpfCrossSummaryByYear(departmentId, year);
        }
    }
}

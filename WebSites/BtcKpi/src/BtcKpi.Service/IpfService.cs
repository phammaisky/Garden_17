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
    public interface IIpfService
    {
        //bool ValidateIpf(string salt, string ipfName, string password, out string errorMessage);
        //bool GetIpf(string ipfName, ref Ipf ipf, out string errorMessage);
        //IEnumerable<Ipf> GetIpfs();
        //IEnumerable<Ipf> GetCategoryIpfs(string categoryName, string ipfName = null);
        //Ipf GetIpf(int id);
        //void CreateIpf(Ipf ipf);
        //void SaveIpf();
        //IEnumerable<Test> GetTests();
        List<CompleteWorkTitle> CompleteWorkTitleByDepartment(int departmentId);
        List<IpfSchedule> IpfSchedulesByDeparment(int departmentId);
        List<IpfInfo> GetIpfByConditions(string status, string companies, string departments, string scheduleTypes, string years, string scheduleIds, string fullName);
        bool AddIpf(int userId, int approveId, int personalType, Ipf ipf, List<IpfDetail> details, List<PersonalPlan> personalPlans, ref string insertMsg);

        Ipf GetIpfInfo(int id, ref List<IpfDetail> ipfDetails, ref List<PersonalPlan> personalPlans, ref List<IpfComment> comments);
        bool UpdateIpf(int userId, Ipf ipf, List<IpfDetail> details, List<PersonalPlan> personalPlans, ref string updateMsg);
        bool ApproveIpf(int userId, int administratorShipId, int iD, bool isApprove, string managerComment, string comment, List<IpfDetail> details, List<PersonalPlan> personalPlans, ref string updateMsg);

        bool BodApproveIpf(int userId, int administratorShipId, int iD, bool isApprove, string managerComment, string comment, List<IpfDetail> details, List<PersonalPlan> personalPlans, ref string updateMsg);

        bool DeleteIpf(int userId, int iD, string comment, ref string updateMsg);
        bool AssessmentIpf(string action, int userId, int administratorShipId, Ipf ipf, List<IpfDetail> details, ref string updateMsg);
        IpfDetail GetIpfDetailById(int ipfDetailID);
        bool IsExist(int userId, int year, int scheduleType, int scheduleId);
        Ipf GetById(int id);
    }

    public class IpfService : IIpfService
    {
        private readonly ICompleteWorkTitleRepository completeWorkTitleRepository;
        private readonly IIpfRepository ipfRepository;
        private readonly IIpfScheduleRepository ipfScheduleRepository;
        private readonly IPersonalPlanRepository personalPlanRepository;
        private readonly IIpfCommentRepository ipfCommentRepository;

        private readonly IIpfDetailRepository ipfDetailRepository;
        private readonly IzIpfRepository zIpfRepository;
        private readonly IzIpfDetailRepository zIpfDetailRepository;
        private readonly IzPersonalPlanRepository zPersonalPlanRepository;

        private readonly IUnitOfWork unitOfWork;

        public IpfService(ICompleteWorkTitleRepository completeWorkTitleRepository, IIpfRepository ipfRepository, IIpfScheduleRepository ipfScheduleRepository, IPersonalPlanRepository personalPlanRepository, IIpfDetailRepository ipfDetailRepository, IIpfCommentRepository ipfCommentRepository, IzIpfRepository zIpfRepository, IzIpfDetailRepository zIpfDetailRepository, IzPersonalPlanRepository zPersonalPlanRepository, IUnitOfWork unitOfWork)
        {
            this.completeWorkTitleRepository = completeWorkTitleRepository;
            this.ipfRepository = ipfRepository;
            this.ipfScheduleRepository = ipfScheduleRepository;
            this.personalPlanRepository = personalPlanRepository;
            this.ipfDetailRepository = ipfDetailRepository;
            this.ipfCommentRepository = ipfCommentRepository;
            this.zIpfRepository = zIpfRepository;
            this.zIpfDetailRepository = zIpfDetailRepository;
            this.zPersonalPlanRepository = zPersonalPlanRepository;

            this.unitOfWork = unitOfWork;
        }

        public List<CompleteWorkTitle> CompleteWorkTitleByDepartment(int departmentId)
        {
            return completeWorkTitleRepository.GetByDeparmentId(departmentId);
        }

        public List<IpfSchedule> IpfSchedulesByDeparment(int departmentId)
        {
            return ipfScheduleRepository.GetByDepartmentId(departmentId);
        }

        public bool AddIpf(int userId, int approveId, int personalType, Ipf ipf, List<IpfDetail> details, List<PersonalPlan> personalPlans, ref string errorMessage)
        {
            var insertTime = DateTime.Now;
            byte? delFlg = 0;

            ipf.DeleteFlg = delFlg;
            ipf.Created = insertTime;
            ipf.CreatedBy = userId;
            ipf.ApproveBy = approveId;
            ipf.PersonType = (byte?)personalType;
            ipfRepository.Add(ipf);
            ipf.ID = ipfRepository.GetNextID();
            foreach (var item in details)
            {
                item.DeleteFlg = delFlg;
                item.Created = insertTime;
                item.CreatedBy = userId;
                item.IpfID = ipf.ID;
                ipfDetailRepository.Add(item);
            }
            foreach (var item in personalPlans)
            {
                item.DeleteFlg = delFlg;
                item.Created = insertTime;
                item.CreatedBy = userId;
                item.IpfID = ipf.ID;
                personalPlanRepository.Add(item);
            }

            unitOfWork.Commit();
            
            return true;

        }

        public Ipf GetIpfInfo(int id, ref List<IpfDetail> ipfDetails, ref List<PersonalPlan> personalPlans, ref List<IpfComment> comments)
        {
            Ipf ipf = ipfRepository.GetById(id);
            ipfDetails = ipfDetailRepository.GetByIpfId(id);
            personalPlans = personalPlanRepository.GetByIpfId(id);
            comments = ipfCommentRepository.GetByIpfId(id);
            return ipf;
        }

        public bool UpdateIpf(int userId, Ipf ipf, List<IpfDetail> details, List<PersonalPlan> personalPlans, ref string updateMsg)
        {
            var insertTime = DateTime.Now;

            List<IpfDetail> currentIpfDetails = new List<IpfDetail>();
            List<PersonalPlan> currentPersonalPlans = new List<PersonalPlan>();
            List<IpfComment> comments = new List<IpfComment>();

            List<IpfDetail> deleteIpfDetails = new List<IpfDetail>();
            List<PersonalPlan> deletePersonalPlans = new List<PersonalPlan>();

            
            var curentIpf = GetIpfInfo(ipf.ID, ref currentIpfDetails, ref currentPersonalPlans, ref comments);
            curentIpf.SelfComment = ipf.SelfComment;
            curentIpf.Status = ipf.Status;
            ipfRepository.Update(curentIpf);

            //Find deleted items -> Update DeleteFlg = 1
            foreach (var item in currentIpfDetails)
            {
                if (details.FirstOrDefault(t => t.ID == item.ID) == null)
                {
                    item.DeleteFlg = 1;
                    item.Deleted = insertTime;
                    item.DeletedBy = userId;
                    ipfDetailRepository.Update(item);
                }
            }
            foreach (var item in currentPersonalPlans)
            {
                if (personalPlans.FirstOrDefault(t => t.ID == item.ID) == null)
                {
                    item.DeleteFlg = 1;
                    item.Deleted = insertTime;
                    item.DeletedBy = userId;
                    personalPlanRepository.Update(item);
                }
            }

            //Insert or update details
            //Detail
            foreach (var item in details)
            {
                //Add new
                if (item.ID == 0)
                {
                    item.DeleteFlg = 0;
                    item.Created = insertTime;
                    item.CreatedBy = userId;
                    item.IpfID = ipf.ID;
                    ipfDetailRepository.Add(item);
                }
                else
                {
                    //Update
                    IpfDetail updateDetail = currentIpfDetails.FirstOrDefault(t => t.ID == item.ID);
                    updateDetail.Seq = item.Seq;
                    updateDetail.IsNextYear = item.IsNextYear;
                    updateDetail.ManagerScore = item.ManagerScore;
                    updateDetail.NextYearFlg = item.NextYearFlg;
                    updateDetail.Objective = item.Objective;
                    updateDetail.Result = item.Result;
                    updateDetail.SelfScore = item.SelfScore;
                    updateDetail.Target = item.Target;
                    updateDetail.TotalScore = item.TotalScore;
                    updateDetail.Weight = item.Weight;
                    updateDetail.WorkCompleteID = item.WorkCompleteID;
                    updateDetail.WorkType = item.WorkType;
                    ipfDetailRepository.Update(updateDetail);
                }
                
            }
            //Personal Plan
            foreach (var item in personalPlans)
            {
                //Add new
                if (item.ID == 0)
                {
                    item.DeleteFlg = 0;
                    item.Created = insertTime;
                    item.CreatedBy = userId;
                    item.IpfID = ipf.ID;
                    personalPlanRepository.Add(item);
                }
                //Update
                else
                {
                    PersonalPlan updatePersonalPlan = currentPersonalPlans.FirstOrDefault(t => t.ID == item.ID);
                    updatePersonalPlan.Seq = item.Seq;
                    updatePersonalPlan.Term = item.Term;
                    updatePersonalPlan.Activity = item.Activity;
                    updatePersonalPlan.CompleteDate = item.CompleteDate;
                    updatePersonalPlan.Remark = item.Remark;
                    updatePersonalPlan.RequestOfManager = item.RequestOfManager;
                    updatePersonalPlan.Type = item.Type;
                    updatePersonalPlan.WishesOfStaff = item.WishesOfStaff;
                    personalPlanRepository.Update(updatePersonalPlan);
                }
            }

            unitOfWork.Commit();

            return true;
        }

        public bool ApproveIpf(int userId, int administratorShipId, int iD, bool isApprove, string managerComment, string comment, List<IpfDetail> details, List<PersonalPlan> personalPlans, ref string updateMsg)
        {
            var insertTime = DateTime.Now;

            List<IpfDetail> currentIpfDetails = new List<IpfDetail>();
            List<PersonalPlan> currentPersonalPlans = new List<PersonalPlan>();
            List<IpfComment> comments = new List<IpfComment>();

            var currentIpf = GetIpfInfo(iD, ref currentIpfDetails, ref currentPersonalPlans, ref comments);

            decimal workScore = 0;
            decimal competencyScore = 0;
            currentIpf.WorkScore = 0;
            currentIpf.CompetencyScore = 0;
            currentIpf.TotalScore = 0;

            //Insert comment
            if (!string.IsNullOrEmpty(comment))
            {
                IpfComment newComment = new IpfComment() {IpfID = iD, Comment = comment, DeleteFlg = 0, Created = insertTime, CreatedBy = userId};
                ipfCommentRepository.Add(newComment);
            }

            //Update details
            //Detail
            foreach (var item in details)
            {
                IpfDetail updateDetail = currentIpfDetails.FirstOrDefault(t => t.ID == item.ID);
                if (updateDetail != null)
                {
                    updateDetail.ManagerScore = item.ManagerScore;
                    updateDetail.TotalScore = item.ManagerScore;
                    //Tính điểm
                    //Hoàn thành công việc
                    if (item.WorkType == 0 && item.IsNextYear == 0)
                    {
                        if (updateDetail.ManagerScore != null && item.Weight != null)
                            workScore = ((decimal) updateDetail.ManagerScore * (decimal) item.Weight) / 100;
                        currentIpf.WorkScore += workScore;
                    }
                    //Năng lực
                    if (item.WorkType == 1 && item.IsNextYear == 0)
                    {
                        if (updateDetail.ManagerScore != null && item.Weight != null)
                            competencyScore = ((decimal)updateDetail.ManagerScore * (decimal)item.Weight) / 100;
                        currentIpf.CompetencyScore += competencyScore;
                    }
                    ipfDetailRepository.Update(updateDetail);
                }

            }
            //Personal Plan
            foreach (var item in personalPlans)
            {
                PersonalPlan updatePersonalPlan = currentPersonalPlans.FirstOrDefault(t => t.ID == item.ID);
                if (updatePersonalPlan != null)
                {
                    updatePersonalPlan.RequestOfManager = item.RequestOfManager;
                    personalPlanRepository.Update(updatePersonalPlan);
                }
            }

            //Tổng điểm
            //Cấp nhân viên -> Điểm = Hoàn thành công việc * 0.8 + Năng lực * 0.2
            if (administratorShipId >= 7)
            {
                currentIpf.TotalScore = 2 * (currentIpf.WorkScore * (decimal)0.8 + currentIpf.CompetencyScore * (decimal)0.2);
            }
            //Cấp quản lý -> Điểm = Hoàn thành công việc * 0.7 + Năng lực * 0.3
            if (administratorShipId < 7)
            {
                currentIpf.TotalScore = 2 * (currentIpf.WorkScore * (decimal)0.7 + currentIpf.CompetencyScore * (decimal)0.3);
            }

            //Update Ipf
            currentIpf.Status = isApprove ? 2 : 1;
            currentIpf.Approved = insertTime;
            currentIpf.ManagerComment = managerComment;

            ipfRepository.Update(currentIpf);

            unitOfWork.Commit();

            return true;
        }

        public bool BodApproveIpf(int userId, int administratorShipId, int iD, bool isApprove, string managerComment, string comment, List<IpfDetail> details, List<PersonalPlan> personalPlans, ref string updateMsg)
        {
            var insertTime = DateTime.Now;

            List<IpfDetail> currentIpfDetails = new List<IpfDetail>();
            List<PersonalPlan> currentPersonalPlans = new List<PersonalPlan>();
            List<IpfComment> comments = new List<IpfComment>();

            var currentIpf = GetIpfInfo(iD, ref currentIpfDetails, ref currentPersonalPlans, ref comments);

            decimal workScore = 0;
            decimal competencyScore = 0;
            decimal totalWorkScore = 0;
            decimal totalCompetencyScore = 0;

            //Update details
            //Detail
            foreach (var item in details)
            {
                IpfDetail updateDetail = currentIpfDetails.FirstOrDefault(t => t.ID == item.ID);
                if (updateDetail != null)
                {
                    updateDetail.BodScore = item.BodScore;
                    //Tính điểm
                    //Hoàn thành công việc
                    if (item.WorkType == 0 && item.IsNextYear == 0)
                    {
                        if (updateDetail.BodScore != null && item.Weight != null)
                            workScore = ((decimal)updateDetail.BodScore * (decimal)item.Weight) / 100;
                        totalWorkScore += workScore;
                    }
                    //Năng lực
                    if (item.WorkType == 1 && item.IsNextYear == 0)
                    {
                        if (updateDetail.BodScore != null && item.Weight != null)
                            competencyScore = ((decimal)updateDetail.BodScore * (decimal)item.Weight) / 100;
                        totalCompetencyScore += competencyScore;
                    }
                    ipfDetailRepository.Update(updateDetail);
                }

            }

            //Tổng điểm Bod đánh giá
            //Cấp nhân viên -> Điểm = Hoàn thành công việc * 0.8 + Năng lực * 0.2
            if (administratorShipId >= 7)
            {
                currentIpf.BodScore = 2 * (totalWorkScore * (decimal)0.8 + totalCompetencyScore * (decimal)0.2);
            }
            //Cấp quản lý -> Điểm = Hoàn thành công việc * 0.7 + Năng lực * 0.3
            if (administratorShipId < 7)
            {
                currentIpf.BodScore = 2 * (totalWorkScore * (decimal)0.7 + totalCompetencyScore * (decimal)0.3);
            }

            //Update Ipf
            currentIpf.Approved = insertTime;
            currentIpf.BodComment = managerComment;

            ipfRepository.Update(currentIpf);

            unitOfWork.Commit();

            return true;
        }

        public bool DeleteIpf(int userId, int iD, string comment, ref string updateMsg)
        {
            var insertTime = DateTime.Now;

            var curentIpf = ipfRepository.GetById(iD);

            curentIpf.DeleteFlg = 1;
            curentIpf.Deleted = insertTime;
            curentIpf.DeletedBy = userId;
            ipfRepository.Update(curentIpf);

            if (!string.IsNullOrEmpty(comment))
            {
                IpfComment newComment = new IpfComment() { IpfID = iD, Comment = comment, DeleteFlg = 0, Created = insertTime, CreatedBy = userId };
                ipfCommentRepository.Add(newComment);
            }

            unitOfWork.Commit();

            return true;
        }

        public List<IpfInfo> GetIpfByConditions(string status, string companies, string departments, string scheduleTypes, string years, string scheduleIds, string fullName)
        {
            return ipfRepository.GetIpfByConditions(status, BtcHelper.RemoveComman(companies), BtcHelper.RemoveComman(departments), BtcHelper.RemoveComman(scheduleTypes), BtcHelper.RemoveComman(years), BtcHelper.RemoveComman(scheduleIds), string.IsNullOrEmpty(fullName) ? "" : fullName.Trim());
        }

        public bool AssessmentIpf(string action, int userId, int administratorShipId, Ipf ipf, List<IpfDetail> details, ref string updateMsg)
        {
            var insertTime = DateTime.Now;
            //Comment: chỉ thêm vào bảng IpfComment
            if (action == "Comment")
            {
                IpfComment comment = new IpfComment() {Comment = ipf.ManagerComment, IpfID = ipf.ID, Created = insertTime, CreatedBy = userId, DeleteFlg = 0};
                ipfCommentRepository.Add(comment);
            }
            //Trường hợp khác, thêm nhiều bảng
            else
            {
                List<IpfDetail> currentIpfDetails = new List<IpfDetail>();
                List<PersonalPlan> currentPersonalPlans = new List<PersonalPlan>();
                List<IpfComment> comments = new List<IpfComment>();

                List<IpfDetail> deleteIpfDetails = new List<IpfDetail>();
                List<PersonalPlan> deletePersonalPlans = new List<PersonalPlan>();


                var curentIpfs = GetIpfInfo(ipf.ID, ref currentIpfDetails, ref currentPersonalPlans, ref comments);
                curentIpfs.WorkScore = 0;
                curentIpfs.CompetencyScore = 0;
                curentIpfs.TotalScore = 0;

                decimal workScore;
                decimal competencyScore;
                IpfDetail detailItem;

                //Update Self Score
                foreach (var item in currentIpfDetails.Where(t => t.IsNextYear == 0))
                {
                    detailItem = details.FirstOrDefault(t => t.ID == item.ID);
                    if (detailItem != null)
                    {
                        // Nhân viên tự đánh giá
                        if (action == "Edit")
                        {
                            item.SelfScore = detailItem.SelfScore;
                        }
                        //Quản lý đánh giá
                        else if (action == "Approve")
                        {
                            item.ManagerScore = detailItem.ManagerScore;
                            //Tính điểm
                            //Hoàn thành công việc
                            if (item.WorkType == 0 && item.IsNextYear == 0)
                            {
                                workScore = ((decimal)detailItem.ManagerScore * (decimal)item.Weight) / 100;
                                curentIpfs.WorkScore += workScore;
                            }
                            //Năng lực
                            if (item.WorkType == 1 && item.IsNextYear == 0)
                            {
                                competencyScore = ((decimal)detailItem.ManagerScore * (decimal)item.Weight) / 100;
                                curentIpfs.CompetencyScore += competencyScore;
                            }
                        }
                        ipfDetailRepository.Update(item);
                    }
                }
                // Nhân viên tự đánh giá
                if (action == "Edit")
                {
                    curentIpfs.SelfComment = ipf.SelfComment;

                }
                //Quản lý đánh giá
                else if (action == "Approve")
                {
                    curentIpfs.ManagerComment = ipf.ManagerComment;
                    //Tổng điểm
                    //Cấp nhân viên -> Điểm = Hoàn thành công việc * 0.8 + Năng lực * 0.2
                    if (administratorShipId >= 7)
                    {
                        curentIpfs.TotalScore = curentIpfs.WorkScore * (decimal)0.8 + curentIpfs.CompetencyScore * (decimal)0.2;
                    }
                    //Cấp quản lý -> Điểm = Hoàn thành công việc * 0.7 + Năng lực * 0.3
                    if (administratorShipId < 7)
                    {
                        curentIpfs.TotalScore = curentIpfs.WorkScore * (decimal)0.7 + curentIpfs.CompetencyScore * (decimal)0.3;
                    }
                }
                ipfRepository.Update(curentIpfs);
            }
            

            unitOfWork.Commit();

            return true;
        }

        public IpfDetail GetIpfDetailById(int ipfDetailID)
        {
            return ipfDetailRepository.GetById(ipfDetailID);
        }

        public bool IsExist(int userId, int year, int scheduleType, int scheduleId)
        {
            Ipf ipf = ipfRepository.GetByUserYearMonth(userId, year, scheduleType, scheduleId);
            return ipf != null;
        }

        public Ipf GetById(int id)
        {
            return ipfRepository.GetById(id);
        }
    }
}

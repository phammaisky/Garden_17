using BtcKpi.Data.Infrastructure;
using BtcKpi.Data.Repositories;
using BtcKpi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BtcKpi.Model.Enum;
using BtcKpi.Service.Common;

namespace BtcKpi.Service
{
    // operations you want to expose
    public interface IUpfCrossService
    {
        List<UpfCrossInfo> GetUpfCrossByConditions(int userId, int userDepartmentID, string status, string companies, string fromDepartments, string toDepartments, string years, string months);
        bool AddUpfCross(int userId, UpfCross upfCross, List<UpfCrossDetail> details, ref string insertMsg);

        UpfCross GetUpfCrossInfo(int id, ref List<UpfCrossDetail> upfCrossDetails);
        bool UpdateUpfCross(int userId, UpfCross upfCross, List<UpfCrossDetail> details, ref string updateMsg);
        bool CreateUpfCrossDetail(int userId, UpfCross upfCross, UpfCrossDetail upfCrossDetail, ref string createMsg);
        bool IsDupllicateUpfCrossDetail(int? year, byte? month, int? fromDepartment, int? toDepartment);
        bool UpdateUpfCrossDetail(int userId, UpfCrossDetail upfCrossDetail, ref string updateMsg);
        bool DeleteUpfCrossDetail(int userId, int detailId, ref string updateMsg);
        bool ApproveUpfCross(int userId, int iD, bool isApprove, string comment, ref string updateMsg);
        bool DeleteUpfCross(int userId, int iD, string comment, ref string updateMsg);
        bool AssessmentUpfCross(string action, int userId, int administratorShipId, UpfCross upfCross, List<UpfCrossDetail> details, ref string updateMsg);
        UpfCross GetUpfCrossById(int id, ref List<UpfCrossDetail> details);
        UpfCrossDetail GetUpfCrossDetailById(int id);

        List<UpfCrossInfo> GetUpfCrossReport(int userId, int userDepartmentId, string companies, string departmentId, string years, string months);
        List<UpfCrossInfo> GetUpfCrossDetailByUpfCrossId(string companies, string departmentId, string years, string months);
    }

    public class UpfCrossService : IUpfCrossService
    {
        private readonly IUpfCrossRepository upfCrossRepository;
        private readonly IUpfCrossDetailRepository upfCrossDetailRepository;
        private readonly IzUpfCrossRepository zUpfCrossRepository;
        private readonly IzUpfCrossDetailRepository zUpfCrossDetailRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly ISysConfigRepository sysConfigRepository;

        private readonly IUnitOfWork unitOfWork;

        public UpfCrossService(IUpfCrossRepository upfCrossRepository, IUpfCrossDetailRepository upfCrossDetailRepository, IzUpfCrossRepository zUpfCrossRepository, IzUpfCrossDetailRepository zUpfCrossDetailRepository, IDepartmentRepository departmentRepository, ISysConfigRepository sysConfigRepository, IUnitOfWork unitOfWork)
        {
            this.upfCrossRepository = upfCrossRepository;
            this.upfCrossDetailRepository = upfCrossDetailRepository;
            this.zUpfCrossRepository = zUpfCrossRepository;
            this.zUpfCrossDetailRepository = zUpfCrossDetailRepository;
            this.departmentRepository = departmentRepository;
            this.sysConfigRepository = sysConfigRepository;

            this.unitOfWork = unitOfWork;
        }

        public bool AddUpfCross(int userId, UpfCross upfCross, List<UpfCrossDetail> details, ref string insertMsg)
        {
            var insertTime = DateTime.Now;
            byte? delFlg = 0;
            if(upfCross == null) upfCross = new UpfCross();

            upfCross.DeleteFlg = delFlg;
            upfCross.Created = insertTime;
            upfCross.CreatedBy = userId;
            upfCross.Status = 0;
            upfCrossRepository.Add(upfCross);
            upfCross.ID = upfCrossRepository.GetNextID();

            foreach (var item in details)
            {
                item.DeleteFlg = delFlg;
                item.Created = insertTime;
                item.CreatedBy = userId;
                item.UpfCrossID = upfCross.ID;
                item.Status = 0;
                upfCrossDetailRepository.Add(item);
            }

            unitOfWork.Commit();
            
            return true;

        }

        public UpfCross GetUpfCrossInfo(int id, ref List<UpfCrossDetail> upfCrossDetails)
        {
            UpfCross upfCross = upfCrossRepository.GetById(id);
            upfCrossDetails = upfCrossDetailRepository.GetByUpfCrossId(id);
            return upfCross;
        }

        public bool UpdateUpfCross(int userId, UpfCross upfCross, List<UpfCrossDetail> details, ref string updateMsg)
        {
            var insertTime = DateTime.Now;

            List<UpfCrossDetail> currentUpfCrossDetails = new List<UpfCrossDetail>();

            //Find deleted items -> Update DeleteFlg = 1
            foreach (var item in currentUpfCrossDetails)
            {
                if (details.FirstOrDefault(t => t.ID == item.ID) == null)
                {
                    item.DeleteFlg = 1;
                    item.Deleted = insertTime;
                    item.DeletedBy = userId;
                    upfCrossDetailRepository.Update(item);
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
                    item.UpfCrossID = upfCross.ID;
                    upfCrossDetailRepository.Add(item);
                }
                else
                {
                    //Update
                    UpfCrossDetail updateDetail = currentUpfCrossDetails.FirstOrDefault(t => t.ID == item.ID);
                    updateDetail.ContentsRequested = item.ContentsRequested;
                    updateDetail.ExpectedTimeOfCompletion = item.ExpectedTimeOfCompletion;
                    updateDetail.ExpectedResult = item.ExpectedResult;
                    updateDetail.FromWeight = item.FromWeight;
                    updateDetail.TimeOfCompletion = item.TimeOfCompletion;
                    updateDetail.Result = item.Result;
                    updateDetail.FromScore = item.FromScore;
                    updateDetail.PlanToDo = item.PlanToDo;
                    updateDetail.ExplainationForResults = item.ExplainationForResults;
                    updateDetail.Solutions = item.Solutions;
                    updateDetail.Timeline = item.Timeline;
                    updateDetail.ToWeight = item.ToWeight;
                    updateDetail.ToScore = item.ToScore;
                    updateDetail.AssessmentByCouncil = item.AssessmentByCouncil;
                    updateDetail.TotalScore = item.TotalScore;
                    updateDetail.Status = item.Status;

                    upfCrossDetailRepository.Update(updateDetail);
                }
                
            }
            
            unitOfWork.Commit();

            return true;
        }

        public bool CreateUpfCrossDetail(int userId, UpfCross upfCross , UpfCrossDetail upfCrossDetail, ref string createMsg)
        {
            DateTime insertTime = DateTime.Now;
            byte? delFlg = 0;
            int? status = 0;
            bool newUpfCross = false;

            UpfCross currentUpfCross =
                upfCrossRepository.GetByDepartmentYearMonth(upfCross.DepartmentID, upfCross.Year, upfCross.Month);
            UpfCrossDetail newDetail = new UpfCrossDetail();
            newDetail.UpfCrossID = upfCrossRepository.GetNextID();

            if (currentUpfCross != null)
            {
                newDetail.UpfCrossID = currentUpfCross.ID;
                upfCrossRepository.Update(currentUpfCross);
            }
            else
            {
                //UpfCross
                upfCross.CreatedBy = userId;
                upfCross.Created = insertTime;
                upfCross.DeleteFlg = delFlg;
                upfCross.Status = status;
                upfCross.DependWeight = upfCrossDetail.FromWeight;
                upfCrossRepository.Add(upfCross);
                newUpfCross = true;
            }

            //Detail
            newDetail.Objective = upfCrossDetail.Objective;
            newDetail.FromDepartment = upfCrossDetail.FromDepartment;
            newDetail.ToDepartment = upfCrossDetail.ToDepartment;
            newDetail.ContentsRequested = upfCrossDetail.ContentsRequested;
            newDetail.ExpectedTimeOfCompletion = upfCrossDetail.ExpectedTimeOfCompletion;
            newDetail.ExpectedResult = upfCrossDetail.ExpectedResult;
            newDetail.FromWeight = upfCrossDetail.FromWeight;
            newDetail.TimeOfCompletion = upfCrossDetail.TimeOfCompletion;
            newDetail.Result = upfCrossDetail.Result;
            newDetail.FromScore = upfCrossDetail.FromScore;
            newDetail.Status = status;

            newDetail.Created = insertTime;
            newDetail.CreatedBy = userId;
            newDetail.DeleteFlg = delFlg;

            upfCrossDetailRepository.Add(newDetail);

            unitOfWork.Commit();

            //Tính điểm sau khi tạo mới
            UpdateScore(newDetail.UpfCrossID);

            //Insert History
            int rowOfUpfCrossHis = 0;
            int rowOfUpfCrossDetailHis = 0;
            if (newUpfCross)
            {
                rowOfUpfCrossHis = zUpfCrossRepository.InsertHistory(upfCross.ID, 0, "Tạo mới", insertTime, userId);
            }

            rowOfUpfCrossDetailHis =
                zUpfCrossDetailRepository.InsertHistory(newDetail.ID, 0, "Tạo mới", insertTime, userId);
            if (rowOfUpfCrossDetailHis == 0 | (newUpfCross && rowOfUpfCrossHis == 0))
            {
                createMsg = "Lỗi thêm dữ liệu lịch sử!";
                return false;
            }

            return true;
        }

        public bool IsDupllicateUpfCrossDetail(int? year, byte? month, int? fromDepartment, int? toDepartment)
        {
            var upfCross = upfCrossRepository.GetByDepartmentYearMonth(fromDepartment, year, month);
            if (upfCross == null)
            {
                return false;
            }
            else
            {
                var details = upfCrossDetailRepository.GetByUpfCrossId(upfCross.ID);
                if (!details.Any())
                {
                    return false;
                }
                else
                {
                    return details.Any(t => t.ToDepartment == toDepartment);
                }
            }
        }

        public bool UpdateUpfCrossDetail(int userId, UpfCrossDetail upfCrossDetail, ref string updateMsg)
        {
            //Update
            var currentUpfCross =
                upfCrossRepository.GetById(upfCrossDetail.UpfCrossID);
            if (currentUpfCross != null)
            {
                UpfCrossDetail updateDetail = upfCrossDetailRepository.GetById(upfCrossDetail.ID);
                
                upfCrossRepository.Update(currentUpfCross);

                

                updateDetail.FromDepartment = upfCrossDetail.FromDepartment;
                updateDetail.ContentsRequested = upfCrossDetail.ContentsRequested;
                updateDetail.ExpectedTimeOfCompletion = upfCrossDetail.ExpectedTimeOfCompletion;
                updateDetail.ExpectedResult = upfCrossDetail.ExpectedResult;
                updateDetail.FromWeight = upfCrossDetail.FromWeight;
                updateDetail.TimeOfCompletion = upfCrossDetail.TimeOfCompletion;
                updateDetail.Result = upfCrossDetail.Result;
                updateDetail.FromScore = upfCrossDetail.FromScore;
                updateDetail.ToDepartment = upfCrossDetail.ToDepartment;
                updateDetail.PlanToDo = upfCrossDetail.PlanToDo;
                updateDetail.ExplainationForResults = upfCrossDetail.ExplainationForResults;
                updateDetail.Solutions = upfCrossDetail.Solutions;
                updateDetail.Timeline = upfCrossDetail.Timeline;
                updateDetail.ToWeight = upfCrossDetail.ToWeight;
                updateDetail.ToScore = upfCrossDetail.ToScore;
                updateDetail.AssessmentByCouncil = upfCrossDetail.AssessmentByCouncil;
                updateDetail.TotalScore = upfCrossDetail.TotalScore;
                updateDetail.Status = upfCrossDetail.Status;
                if (updateDetail.ToWeight != null & updateDetail.ToScore != null)
                {
                    updateDetail.Status = 1; // Đã có tỷ trọng và điểm tự đánh giá = đã phản hồi
                }

                upfCrossDetailRepository.Update(updateDetail);

                unitOfWork.Commit();

                //Tính điểm
                UpdateScore(updateDetail.UpfCrossID);
                
                //Insert History
                DateTime insertTime = DateTime.Now;
                int rowOfUpfCrossHis = 0;
                int rowOfUpfCrossDetailHis = 0;
                rowOfUpfCrossHis = zUpfCrossRepository.InsertHistory(updateDetail.UpfCrossID, 1, "Chỉnh sửa", insertTime, userId);

                rowOfUpfCrossDetailHis =
                    zUpfCrossDetailRepository.InsertHistory(updateDetail.ID, 1, "Chỉnh sửa", insertTime, userId);
                if (rowOfUpfCrossHis == 0 | rowOfUpfCrossDetailHis == 0)
                {
                    updateMsg = "Lỗi thêm dữ liệu lịch sử!";
                    return false;
                }

                return true;
            }

            updateMsg = "Không tìm thấy dữ liệu!";
            return false;
        }

        private void UpdateScore(int id)
        {
            var upfCross = upfCrossRepository.GetById(id);
            if (upfCross != null)
            {
                byte totalWeight = 0;
                decimal totalScore = 0;

                List<UpfCrossDetail> details = upfCrossDetailRepository.GetByUpfCrossId(id);
                if (details.Any())
                {
                    foreach (var detail in details)
                    {
                        if (detail.ToScore != null & detail.FromScore == null)
                        {
                            detail.TotalScore = detail.ToScore;
                        }
                        else if (detail.ToScore == null & detail.FromScore != null)
                        {
                            detail.TotalScore = detail.FromScore;

                        }
                        else if (detail.ToScore != null & detail.FromScore != null)
                        {
                            detail.TotalScore = ((decimal)detail.FromScore + (decimal)detail.ToScore) / 2;
                        }
                        else
                        {
                            detail.TotalScore = 0;
                        }

                        totalScore += ((decimal)detail.TotalScore * (detail.ToWeight == null ? 0 : (int)detail.ToWeight)) / 100;
                        totalWeight += detail.ToWeight ?? (byte)0;

                        upfCrossDetailRepository.Update(detail);
                    }

                    if (totalWeight != 0)
                    {
                        upfCross.DependScore = totalScore * 100 / totalWeight;
                        upfCross.DependWeight = totalWeight;

                        upfCrossRepository.Update(upfCross);
                        unitOfWork.Commit();
                    }
                    
                }
            }
        }

        public bool DeleteUpfCrossDetail(int userId, int detailId, ref string updateMsg)
        {
            //Update
            UpfCrossDetail updateDetail = upfCrossDetailRepository.GetById(detailId);

            updateDetail.DeleteFlg = 1;
            updateDetail.Deleted = DateTime.Now;
            updateDetail.DeletedBy = userId;

            upfCrossDetailRepository.Update(updateDetail);

            unitOfWork.Commit();

            //Tính điểm
            UpdateScore(updateDetail.UpfCrossID);

            //Insert History
            DateTime insertTime = DateTime.Now;
            int rowOfUpfCrossHis = 0;
            int rowOfUpfCrossDetailHis = 0;
            rowOfUpfCrossHis = zUpfCrossRepository.InsertHistory(updateDetail.UpfCrossID, 2, "Xóa", insertTime, userId);

            rowOfUpfCrossDetailHis =
                zUpfCrossDetailRepository.InsertHistory(updateDetail.ID, 2, "Xóa", insertTime, userId);
            if (rowOfUpfCrossHis == 0 | rowOfUpfCrossDetailHis == 0)
            {
                updateMsg = "Lỗi thêm dữ liệu lịch sử!";
                return false;
            }

            return true;
        }

        public bool ApproveUpfCross(int userId, int iD, bool isApprove, string comment, ref string updateMsg)
        {
            var insertTime = DateTime.Now;

            return true;
        }

        public bool DeleteUpfCross(int userId, int iD, string comment, ref string updateMsg)
        {
            var insertTime = DateTime.Now;

            var curentUpfCross = upfCrossRepository.GetById(iD);

            curentUpfCross.DeleteFlg = 1;
            curentUpfCross.Deleted = insertTime;
            curentUpfCross.DeletedBy = userId;
            upfCrossRepository.Update(curentUpfCross);

            unitOfWork.Commit();

            return true;
        }

        public List<UpfCrossInfo> GetUpfCrossByConditions(int userId, int userDepartmentID, string status, string companies, string fromDepartments, string toDepartments, string years, string months)
        {
            bool isSpecialUser = false;
            var sysconfig = sysConfigRepository.GetByCode(BtcConst.UpfCrossSpecialUsersCode);
            if (sysconfig != null)
            {
                if (!string.IsNullOrEmpty(sysconfig.Values))
                {
                    if (sysconfig.Values.Contains(","))
                    {
                        var users = sysconfig.Values.Split(',').ToList();
                        isSpecialUser = users.Any(t => t == userId.ToString());
                    }
                    else
                    {
                        isSpecialUser = sysconfig.Values.Trim() == userId.ToString();
                    }
                }
            }
            return upfCrossRepository.GetUpfCrossByConditions(isSpecialUser, userDepartmentID, status, BtcHelper.RemoveComman(companies), BtcHelper.RemoveComman(fromDepartments), BtcHelper.RemoveComman(toDepartments), BtcHelper.RemoveComman(years), BtcHelper.RemoveComman(months));
        }

        public bool AssessmentUpfCross(string action, int userId, int administratorShipId, UpfCross upfCross, List<UpfCrossDetail> details, ref string updateMsg)
        {
            var insertTime = DateTime.Now;
            
            return true;
        }

        public UpfCross GetUpfCrossById(int id, ref List<UpfCrossDetail> details)
        {
            UpfCross upfCross = upfCrossRepository.GetById(id);
            if (upfCross != null)
            {
                details = upfCrossDetailRepository.GetByUpfCrossId(id);
            }

            return upfCross;
        }

        public UpfCrossDetail GetUpfCrossDetailById(int id)
        {
            UpfCrossDetail detail = upfCrossDetailRepository.GetById(id);
            if (detail != null)
            {
                if (detail.FromDepartment != null)
                {
                    Department department = departmentRepository.GetById((int)detail.FromDepartment);
                    if (department != null)
                    {
                        detail.FromName = department.Name;
                    }
                }
                if (detail.ToDepartment != null)
                {
                    Department toDepartment = departmentRepository.GetById((int)detail.ToDepartment);
                    if (toDepartment != null)
                    {
                        detail.ToName = toDepartment.Name;
                    }
                }

            }

            return detail;
        }

        public List<UpfCrossInfo> GetUpfCrossReport(int userId, int userDepartmentId, string companies, string departmentId, string years, string months)
        {
            bool isSpecialUser = false;
            var sysconfig = sysConfigRepository.GetByCode(BtcConst.UpfCrossSpecialUsersCode);
            if (sysconfig != null)
            {
                if (!string.IsNullOrEmpty(sysconfig.Values))
                {
                    if (sysconfig.Values.Contains(","))
                    {
                        var users = sysconfig.Values.Split(',').ToList();
                        isSpecialUser = users.Any(t => t == userId.ToString());
                    }
                    else
                    {
                        isSpecialUser = sysconfig.Values.Trim() == userId.ToString();
                    }
                }
            }
            return upfCrossRepository.GetUpfCrossReport(isSpecialUser, userDepartmentId, BtcHelper.RemoveComman(companies), BtcHelper.RemoveComman(departmentId), BtcHelper.RemoveComman(years), BtcHelper.RemoveComman(months));
        }

        public List<UpfCrossInfo> GetUpfCrossDetailByUpfCrossId(string companies, string departmentId, string years, string months)
        {
            return upfCrossRepository.GetUpfCrossDetailByUpfCrossId(BtcHelper.RemoveComman(companies), BtcHelper.RemoveComman(departmentId), years, months);
        }
    }
}

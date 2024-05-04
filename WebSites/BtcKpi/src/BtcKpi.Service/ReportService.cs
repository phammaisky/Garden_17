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
    public interface IReportService
    {
        List<UpfReport> GetUpfMonthByConditions(int userId, string companies, string departments, string years, string schedules, ref string errorMsg);

        List<UpfMonthItem> GetReportMonth(int userId, int departmentId, int company, int years, ref string errorMsg, ref List<Department> departments, ref List<UpfSchedule> upfSchedules);
        List<IpfReportInfo> GetIpfReportInfos(string year, string companies, string departments);
        bool UpdateIpfReport(int userId, List<IpfReportInfo> ipfReportInfos, ref string updateMsg);

    }

    public class ReportService : IReportService
    {
        private readonly IUpfCrossRepository upfCrossRepository;
        private readonly IUpfCrossDetailRepository upfCrossDetailRepository;
        private readonly IzUpfCrossRepository zUpfCrossRepository;
        private readonly IzUpfCrossDetailRepository zUpfCrossDetailRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly ISysConfigRepository sysConfigRepository;
        private readonly IDepartScheduleRepository departScheduleRepository;
        private readonly IReportRepository reportRepository;
        private readonly IIpfRepository ipfRepository;
        private readonly IIpfReportRepository ipfReportRepository;
        private readonly IIpfScheduleRepository ipfScheduleRepository;

        private readonly IUnitOfWork unitOfWork;

        public ReportService(IUpfCrossRepository upfCrossRepository, IUpfCrossDetailRepository upfCrossDetailRepository, IzUpfCrossRepository zUpfCrossRepository, IzUpfCrossDetailRepository zUpfCrossDetailRepository, IDepartmentRepository departmentRepository, ISysConfigRepository sysConfigRepository, IDepartScheduleRepository departScheduleRepository, IReportRepository reportRepository, IIpfRepository ipfRepository, IIpfReportRepository ipfReportRepository, IIpfScheduleRepository ipfScheduleRepository, IUnitOfWork unitOfWork)
        {
            this.upfCrossRepository = upfCrossRepository;
            this.upfCrossDetailRepository = upfCrossDetailRepository;
            this.zUpfCrossRepository = zUpfCrossRepository;
            this.zUpfCrossDetailRepository = zUpfCrossDetailRepository;
            this.departmentRepository = departmentRepository;
            this.sysConfigRepository = sysConfigRepository;
            this.departScheduleRepository = departScheduleRepository;
            this.reportRepository = reportRepository;
            this.ipfRepository = ipfRepository;
            this.ipfReportRepository = ipfReportRepository;
            this.ipfScheduleRepository = ipfScheduleRepository;

            this.unitOfWork = unitOfWork;
        }


        public List<UpfReport> GetUpfMonthByConditions(int userId, string companies, string departments, string years, string schedules, ref string errorMsg)
        {
            return reportRepository.GetUpfMonthByConditions(userId, BtcHelper.RemoveComman(companies), BtcHelper.RemoveComman(departments), BtcHelper.RemoveComman(years), BtcHelper.RemoveComman(schedules), ref errorMsg);
        }

        public List<UpfMonthItem> GetReportMonth(int userId, int departmentId, int company, int years, ref string errorMsg, ref List<Department> departments, ref List<UpfSchedule> upfSchedules)
        {
            departments = departmentRepository.GetDepartmentByCompanyId(company);
            upfSchedules = departScheduleRepository.GetByDepartmentId(departmentId, company);
            List<UpfReport> upfReports = GetUpfMonthByConditions(userId, company.ToString(), string.Empty,
                years.ToString(), string.Empty, ref errorMsg);
            List<UpfMonthItem> items = new List<UpfMonthItem>();
            if (upfReports.Any())
            {
                foreach (var department in departments)
                {
                    UpfMonthItem upfMonths = new UpfMonthItem();
                    upfMonths.DepartmentName = department.Name;
                    upfMonths.Details = new List<UpfMonth>();
                    foreach (var upfSchedule in upfSchedules)
                    {
                        UpfMonth upfMonth = new UpfMonth() {DepartmentID = department.Id, Rank = "", ScheduleID = upfSchedule.ID, Score = null};
                        var report = upfReports.FirstOrDefault(t => t.DepartmentID == department.Id & t.ScheduleID == upfSchedule.ID);
                        if (report != null)
                        {
                            if ((report.SelfScore == null || report.SelfScore == 0) & (report.DependWeight == null || report.DependWeight == 0))
                            {
                                upfMonth.Score = 0;
                            }
                            else if (report.SelfScore == null || report.SelfScore == 0)
                            {
                                upfMonth.Score = report.DependScore;
                            }
                            else if (report.DependWeight == null || report.DependWeight == 0)
                            {
                                upfMonth.Score = report.SelfScore;
                            }
                            else
                            {
                                upfMonth.Score = ((report.SelfScore ?? 0) * (100 - report.DependWeight ?? 0) +
                                                 (report.DependScore ?? 0) * (report.DependWeight ?? 0))/100;
                            }
                            
                            upfMonth.Rank = BtcHelper.ConvertSroreToRank(upfMonth.Score);
                            
                        }
                        upfMonths.Details.Add(upfMonth);
                    }

                    //Trung bình theo năm
                    var uAverageScores =
                        from u in upfMonths.Details.Where(t => t.Score != null)
                        group u by u.DepartmentID into uGroup
                        select new
                        {
                            DepartmentID = uGroup.Key,
                            AverageScore = uGroup.Where(x => x.Score != null).Average(x => x.Score)
                        };
                    var uAverageScore = uAverageScores.FirstOrDefault();
                    upfMonths.Details.Add(new UpfMonth() { DepartmentID = department.Id, Rank = BtcHelper.ConvertSroreToRank(uAverageScore?.AverageScore), ScheduleID = 0, Score = uAverageScore?.AverageScore});

                    //YTD
                    upfMonths.Details.Add(new UpfMonth() { DepartmentID = department.Id, Rank = "", ScheduleID = -1, Score = null });

                    //BOD
                    upfMonths.Details.Add(new UpfMonth() { DepartmentID = department.Id, Rank = "", ScheduleID = -2, Score = null });

                    items.Add(upfMonths);
                }
            }
            return items;
        }

        public List<IpfReportInfo> GetIpfReportInfos(string year, string companies, string departments)
        {
            List<IpfReportInfo> results = new List<IpfReportInfo>();

            //Lấy danh sách IPF định kỳ: lấy mặc định là 12 tháng
            List<IpfSchedule> ipfSchedules = ipfScheduleRepository.GetByDepartmentId(0);
            if (ipfSchedules == null || !ipfSchedules.Any())
            {
                ipfSchedules = new List<IpfSchedule>();
            }
            //Thêm năm: ID = -1
            ipfSchedules.Add(new IpfSchedule(){ CompanyID = 0, DepartmentID = 0, ID = -1, Name = BtcConst.IpfScheduleTypeYear});

            /*
             *Lấy danh sách IPF
             */
            List<IpfInfo> ipfs = ipfRepository.GetIpfByConditions("", companies, departments, "", year, "", "");
            if (ipfs != null && ipfs.Any())
            {
                /*
                 * Lấy danh sách báo cáo đã có
                 */
                List<IpfReport> ipfReports = ipfReportRepository.GetInfoByCondition(companies, departments, year);
                if (ipfReports == null)
                {
                    ipfReports = new List<IpfReport>();
                }

                /*
                 * Bắt đầu xử lý dữ liệu
                 * Danh sách = ipfs nhóm theo ID người dùng và năm
                 */
                var gList = ipfs.GroupBy(i => new
                {
                    i.UserID,
                    i.Year
                }).Select(gi => new IpfReportInfo()
                {
                    UserId = gi.Key.UserID,
                    Year = gi.Key.Year,
                    IpfScores = new List<IpfScore>(),
                    FullName = gi.Select(g => g.FullName).FirstOrDefault(),
                    DepartmentID = gi.Select(g => g.DepartmentID).FirstOrDefault(),
                    DepartmentName = gi.Select(g => g.DepartmentName).FirstOrDefault(),
                    CompanyID = gi.Select(g => g.CompanyID).FirstOrDefault(),
                    CompanyName = gi.Select(g => g.CompanyName).FirstOrDefault(),
                    AdministratorshipID = gi.Select(g => g.AdministratorshipID).FirstOrDefault(),
                    AdministratorshipName = gi.Select(g => g.AdministratorshipName).FirstOrDefault(),
                    IsDeparmentRow = false

                }).OrderBy(p => p.Year).ThenBy(p => p.DepartmentID).ThenBy(p => p.UserId).ToList();

                IpfReportInfo resultItem;
                IpfScore scoreItem;
                IpfReport reportItem;
                for (int j = 0; j < gList.Count(); j++)
                {
                    //Dòng
                    resultItem = gList[j];

                    //Thêm dòng dữ liệu phòng ban: khi bắt đầu hoặc có sự thay đổi phòng ban
                    if (j == 0 | (j > 0 && gList[j - 1].DepartmentID != resultItem.DepartmentID))
                    {
                        results.Add(new IpfReportInfo()
                        {
                            IsDeparmentRow = true,
                            DepartmentID = resultItem.DepartmentID,
                            DepartmentName = resultItem.DepartmentName
                        });
                    }

                    //Điểm cho từng tháng
                    foreach (var s in ipfSchedules)
                    {
                        scoreItem = ipfs.Where(p =>
                            p.Year == resultItem.Year & p.UserID == resultItem.UserId & p.ScheduleID == s.ID).Select(p => new IpfScore()
                        {
                            ScheduleID = p.ScheduleID,
                            ScheduleName = p.ScheduleName,
                            ScheduleType = p.ScheduleType,
                            Score = p.TotalScore,
                            BodScore = p.BodScore
                        }).FirstOrDefault();
                        //Nếu chưa có điểm khởi tạo rỗng
                        if (scoreItem == null)
                        {
                            scoreItem = new IpfScore(){ScheduleType = 1, ScheduleID = s.ID, ScheduleName = s.Name};
                        }
                        resultItem.IpfScores.Add(scoreItem);
                    }
                    //Tính điểm trung bình và xếp loại
                    resultItem.AverageScore = resultItem.IpfScores.Where(p => p.ScheduleType == 1 & p.Score != null).Select(p => p.Score).Average();
                    resultItem.AverageRank = BtcHelper.ConvertScoreToRankScheme10(resultItem.AverageScore);

                    //Điểm của BOD
                    reportItem =
                        ipfReports.FirstOrDefault(p => p.Year == resultItem.Year & p.UserId == resultItem.UserId);
                    if (reportItem != null)
                    {
                        resultItem.BodComment = reportItem.BodComment;
                        resultItem.Remark = reportItem.Remark;

                        //Nếu đã có điểm ở bảng IpfReport tức là đã từng có đánh giá thì lấy điểm ở đây
                        if (reportItem.BodScore != null)
                        {
                            resultItem.BodScore = reportItem.BodScore;
                        }
                        //Nếu không tính điểm trung bình
                        else
                        {
                            resultItem.BodScore = resultItem.IpfScores.Where(p => p.BodScore != null).Select(p => p.BodScore).Average();
                        }
                    }
                    //Nếu không có dữ liệu thì tính điểm trung bình
                    else
                    {
                        resultItem.BodScore = resultItem.IpfScores.Where(p => p.BodScore != null).Select(p => p.BodScore).Average();
                    }

                    //Trả về dữ liệu
                    results.Add(resultItem);
                }

                
                
            }

            

            return results;
        }

        public bool UpdateIpfReport(int userId, List<IpfReportInfo> ipfReportInfos, ref string updateMsg)
        {
            updateMsg = "";
            bool result = true;
            if (ipfReportInfos != null && ipfReportInfos.Any())
            {
                var listUpdate = ipfReportInfos.Where(p => p.IsDeparmentRow == false).ToList();
                if (listUpdate.Any())
                {
                    
                    foreach (var item in listUpdate)
                    {
                        var ipfReport = ipfReportRepository.GetByUserIdAndYear(item.UserId, item.Year);
                        if (ipfReport != null)
                        {
                            ipfReport.BodScore = item.BodScore;
                            ipfReport.Remark = item.Remark;
                            ipfReport.DeleteFlg = 0;
                            ipfReportRepository.Update(ipfReport);
                        }
                        else
                        {
                            IpfReport newItem = new IpfReport(){UserId = item.UserId, Year = item.Year, BodScore = item.BodScore, Remark = item.Remark, DeleteFlg = 0, CreatedBy = userId, Created = DateTime.Now};
                            ipfReportRepository.Add(newItem);
                        }
                    }

                    try
                    {
                        unitOfWork.Commit();
                    }
                    catch (Exception ex)
                    {
                        updateMsg = ex.Message;
                        result = false;
                    }
                }
            }

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class IpfRepository : RepositoryBase<Ipf>, IIpfRepository
    {
        public IpfRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public int GetNextID()
        {
            int identCurrent = (int)DbContext.Database.SqlQuery<decimal>("SELECT IDENT_CURRENT('kpi.Ipf')").FirstOrDefault();
            return (identCurrent + 1);
        }

        public List<IpfInfo> GetIpfByConditions(string status, string companies, string departments, string scheduleTypes, string years, string scheduleIds, string fullName)
        {
            string sql =
                @"SELECT ui.*, i.ID, i.ScheduleType, i.Year, CASE WHEN i.ScheduleID IS NULL THEN -1 ELSE i.ScheduleID END AS ScheduleID, s.Name AS ScheduleName, i.WorkScore, i.CompetencyScore, i.TotalScore, i.Status, i.Created, i.CreatedBy, i.Approved, i.ApproveBy, i.BodId, i.BodScore
                            FROM kpi.Ipf i
	                            LEFT JOIN ( SELECT u.ID AS UserID, u.FullName, u.DepartmentID, d.Name AS DepartmentName, c.Id AS CompanyID, c.Name AS CompanyName, u.AdministratorshipID, a.Name AS AdministratorshipName
				                            FROM dbo.Users u 
					                            LEFT JOIN dbo.Departments d ON d.Id = u.DepartmentID
					                            LEFT JOIN dbo.Companies c ON c.Id = d.CompanyId
					                            LEFT JOIN dbo.AdministratorShips a ON a.ID = u.AdministratorshipID
				                            ) ui ON ui.UserID = i.CreatedBy
	                            LEFT JOIN kpi.IpfSchedule s ON s.ID = i.ScheduleID
                            WHERE i.DeleteFlg = 0 ";

            //Conditions
            if (!string.IsNullOrEmpty(status))
            {
                //Mới tạo
                if (status == "0")
                {
                    sql += string.Format(" AND i.Status = {0} ", status);
                }
                //Đã duyệt tạo mới
                else
                {
                    sql += string.Format(" AND i.Status >= {0} ", status);
                }
            }
            if (!string.IsNullOrEmpty(companies))
            {
                sql += string.Format(" AND ui.CompanyID IN ({0})", companies);
            }
            if (!string.IsNullOrEmpty(departments))
            {
                sql += string.Format(" AND ui.DepartmentID IN ({0})", departments);
            }
            if (!string.IsNullOrEmpty(scheduleTypes))
            {
                sql += string.Format(" AND i.ScheduleType IN ({0})", scheduleTypes);
            }
            if (!string.IsNullOrEmpty(years))
            {
                sql += string.Format(" AND i.Year IN ({0})", years);
            }
            if (scheduleTypes == "1" && !string.IsNullOrEmpty(scheduleIds))
            {
                sql += string.Format(" AND i.ScheduleID IN ({0})", scheduleIds);
            }
            if (!string.IsNullOrEmpty(fullName))
            {
                sql += string.Format(" AND ui.FullName LIKE '%{0}%' ", fullName);
            }

            var items = DbContext.Database.SqlQuery<IpfInfo>(sql).ToList<IpfInfo>();
            return items;
        }

        public Ipf GetByUserYearMonth(int userId, int year, int scheduleType, int scheduleId)
        {
            if (scheduleType == 0)
            {
                return DbContext.Ipfs.FirstOrDefault(t => t.DeleteFlg == 0 & t.CreatedBy == userId & t.Year == year & t.ScheduleType == scheduleType);
            }
            else
            {
                return DbContext.Ipfs.FirstOrDefault(t =>
                    t.DeleteFlg == 0 & t.CreatedBy == userId & t.Year == year & t.ScheduleID == scheduleId & t.ScheduleType == scheduleType);
            }
        }
    }

    public interface IIpfRepository : IRepository<Ipf>
    {
        int GetNextID();
        List<IpfInfo> GetIpfByConditions(string status, string companies, string departments,string scheduleTypes, string years, string scheduleIds, string fullName);
        Ipf GetByUserYearMonth(int userId, int year, int scheduleType, int scheduleId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;
using BtcKpi.Model.Enum;

namespace BtcKpi.Data.Repositories
{
    public class UpfRepository : RepositoryBase<Upf>, IUpfRepository
    {
        public UpfRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public int GetDepartmentID()
        {
            int identCurrent = (int)DbContext.Database.SqlQuery<decimal>("SELECT IDENT_CURRENT('kpi.Upf')").FirstOrDefault();
            return (identCurrent + 1);
        }

        public List<DepartmentInfo> GetDepartByConditions(string companies, string departments, string scheduleTypes, string years, string scheduleIds, string statusId)
        {
            StringBuilder sql = new StringBuilder(@"SELECT ui.*, i.ID, i.ScheduleType, i.Year, i.ScheduleID, s.Name AS ScheduleName, i.StatusID, i.TotalPoint as SelfAssessScore, 
                                                    i.TotalManagePoint as AssessedScore, i.CreatedBy, i.Created, i.Approved, i.ApproveBy, i.BodApproved, i.TotalBODPoint
                                                    FROM kpi.Upf i
                                                    LEFT JOIN ( SELECT u.ID AS UserID, u.FullName as PersonInCharge, u.DepartmentID, d.Name AS DepartmentName, c.Id AS CompanyID, 
                                                                c.Name AS CompanyName, u.AdministratorshipID, a.Name AS AdministratorshipName
			                                                    FROM dbo.Users u 
				                                                    LEFT JOIN dbo.Departments d ON d.Id = u.DepartmentID
				                                                    LEFT JOIN dbo.Companies c ON c.Id = d.CompanyId
				                                                    LEFT JOIN dbo.AdministratorShips a ON a.ID = u.AdministratorshipID
			                                                    ) ui ON ui.UserID = i.CreatedBy
                                                    LEFT JOIN kpi.UpfSchedule s ON s.ID = i.ScheduleID
                                                    WHERE i.DeleteFlg = 0 ");
            //Conditions
            if (!string.IsNullOrEmpty(companies))
            {
                sql.Append(string.Format(" AND ui.CompanyID IN ({0})", companies));
            }
            if (!string.IsNullOrEmpty(departments))
            {
                sql.Append(string.Format(" AND ui.DepartmentID IN ({0})", departments));
            }
            if (!string.IsNullOrEmpty(scheduleTypes))
            {
                sql.Append(string.Format(" AND i.ScheduleType = ({0})", scheduleTypes));
            }
            if (!string.IsNullOrEmpty(years))
            {
                sql.Append(string.Format(" AND i.Year = ({0})", years));
            }
            if (scheduleTypes == "1" && !string.IsNullOrEmpty(scheduleIds))
            {
                sql.Append(string.Format(" AND i.ScheduleID = ({0})", scheduleIds));
            }
            if (!string.IsNullOrEmpty(statusId))
            {
                sql.Append(string.Format(" AND i.StatusID = {0} ", statusId));
            }

            var items = DbContext.Database.SqlQuery<DepartmentInfo>(sql.ToString()).ToList<DepartmentInfo>();
            return items;
        }

        public bool CheckCreateDepart(Upf upf, int userId)
        {
            List<Upf> items = new List<Upf>();
            if (upf.ScheduleType == 0)
            {
                items = this.DbContext.Upfs.Where(t => t.ScheduleType == upf.ScheduleType && t.Year == upf.Year && t.CreatedBy == userId && t.DeleteFlg == 0).ToList();
            } else if (upf.ScheduleType == 1)
            {
                items = this.DbContext.Upfs.Where(t => t.ScheduleType == upf.ScheduleType && t.Year == upf.Year && t.ScheduleID == upf.ScheduleID && t.CreatedBy == userId && t.DeleteFlg == 0).ToList();
            }
            return items.Count > 0 ? false : true;
        }

        public List<DepartmentInfo> GetReportDepartment(string companies, string departments, string years)
        {
            StringBuilder sql = new StringBuilder(@"SELECT ui.*, i.ID, i.ScheduleType, i.Year, i.ScheduleID, s.Name AS ScheduleName, i.StatusID, i.TotalPoint as SelfAssessScore, 
                                                    i.TotalManagePoint as AssessedScore, i.CreatedBy, i.Created, i.Approved, i.ApproveBy, i.BodApproved, i.TotalBODPoint
                                                    FROM kpi.Upf i
                                                    LEFT JOIN ( SELECT u.ID AS UserID, u.FullName as PersonInCharge, u.DepartmentID, d.Name AS DepartmentName, c.Id AS CompanyID, 
                                                                c.Name AS CompanyName, u.AdministratorshipID, a.Name AS AdministratorshipName
			                                                    FROM dbo.Users u 
				                                                    LEFT JOIN dbo.Departments d ON d.Id = u.DepartmentID
				                                                    LEFT JOIN dbo.Companies c ON c.Id = d.CompanyId
				                                                    LEFT JOIN dbo.AdministratorShips a ON a.ID = u.AdministratorshipID
			                                                    ) ui ON ui.UserID = i.CreatedBy
                                                    LEFT JOIN kpi.UpfSchedule s ON s.ID = i.ScheduleID
                                                    WHERE i.DeleteFlg = 0 ");
            //Conditions
            if (!string.IsNullOrEmpty(companies))
            {
                sql.Append(string.Format(" AND ui.CompanyID IN ({0})", companies));
            }
            if (!string.IsNullOrEmpty(departments))
            {
                sql.Append(string.Format(" AND ui.DepartmentID IN ({0})", departments));
            }
            if (!string.IsNullOrEmpty(years))
            {
                sql.Append(string.Format(" AND i.Year = ({0})", years));
            }
            sql.Append(@" AND (i.StatusID = 2 OR i.StatusID = 3) ");

            var items = DbContext.Database.SqlQuery<DepartmentInfo>(sql.ToString()).ToList<DepartmentInfo>();
            return items;
        }

    }

    public interface IUpfRepository : IRepository<Upf>
    {
        List<DepartmentInfo> GetDepartByConditions(string companies, string departments, string scheduleTypes, string years, string scheduleIds, string statusId);
        List<DepartmentInfo> GetReportDepartment(string companies, string departments, string years);
        int GetDepartmentID();

        bool CheckCreateDepart(Upf upf, int userId);
    }
}

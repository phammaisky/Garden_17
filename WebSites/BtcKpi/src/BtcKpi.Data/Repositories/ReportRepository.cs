using System;
using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class ReportRepository : RepositoryBase<UpfCross>, IReportRepository
    {
        public ReportRepository(IDbFactory dbFactory)
            : base(dbFactory) { }


        public List<UpfReport> GetUpfMonthByConditions(int userId, string companies, string departments, string years, string schedules, ref string errorMsg)
        {
            errorMsg = "";
            string sql = @"SELECT d.CompanyId, c.Name AS CompanyName, d.Id AS DepartmentID, d.Name AS DepartmentName
		                        , usr.UserID, usr.FullName, usr.AdministratorshipID, usr.AdministratorshipName
		                        , cr.Year, cr.ScheduleID, cr.DependWeight, cr.DependScore
		                        , upf.SelfWeight, upf.SelfScore
                        FROM dbo.Departments d
                        LEFT JOIN dbo.Companies c ON c.Id = d.CompanyId
                        LEFT JOIN (
	                        SELECT ucd.ToDepartment AS DeparmentID,  uc.Year, uc.Month AS ScheduleID, SUM(ucd.FromWeight) AS DependWeight, AVG(ucd.TotalScore) AS DependScore
	                        FROM kpi.UpfCross uc 
	                        LEFT JOIN kpi.UpfCrossDetail ucd ON ucd.UpfCrossID = uc.ID
	                        WHERE uc.DeleteFlg = 0
	                        GROUP BY ucd.ToDepartment, uc.Year, uc.Month) cr ON cr.DeparmentID = d.Id
                        LEFT JOIN (
	                        SELECT ID, DepartmentID, Year, ScheduleID, 0 AS SelfWeight, TotalPoint AS SelfScore FROM kpi.Upf 
	                        WHERE DeleteFlg = 0 AND ScheduleType = 1) upf ON upf.DepartmentID = d.Id
                        LEFT JOIN (
	                        SELECT MIN(u.ID) AS UserID, MIN(u.FullName) AS FullName, u.DepartmentID, MIN(u.AdministratorshipID) AS AdministratorshipID, CASE WHEN MIN(u.AdministratorshipName) IS NULL THEN MIN(a.Name) ELSE MIN(u.AdministratorshipName) END AS AdministratorshipName
	                        FROM dbo.Users u 
	                        LEFT JOIN dbo.AdministratorShips a ON a.ID = u.AdministratorshipID
	                        WHERE u.AdministratorshipID = 5
	                        GROUP BY u.DepartmentID) usr ON usr.DepartmentID = d.Id
                        WHERE d.DeleteFlg = 0 ";
            if (!string.IsNullOrEmpty(companies))
            {
                sql += string.Format(" AND d.CompanyId IN ({0}) ", companies);
            }
            if (!string.IsNullOrEmpty(departments))
            {
                sql += string.Format(" AND d.Id IN ({0}) ", departments);
            }
            if (!string.IsNullOrEmpty(years))
            {
                sql += string.Format(" AND (cr.Year IN ({0}) OR upf.Year IN ({0})) ", years);
            }
            if (!string.IsNullOrEmpty(schedules))
            {
                sql += string.Format(" AND (cr.ScheduleID IN ({0}) OR upf.ScheduleID IN ({0})) ", schedules);
            }

            List<UpfReport> items = new List<UpfReport>();

            try
            {
                items = DbContext.Database.SqlQuery<UpfReport>(sql).ToList();
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }
            
            return items;
        }
    }

    public interface IReportRepository : IRepository<UpfCross>
    {
        List<UpfReport> GetUpfMonthByConditions(int userId, string companies, string departments, string years,
            string schedules, ref string errorMsg);
    }
}

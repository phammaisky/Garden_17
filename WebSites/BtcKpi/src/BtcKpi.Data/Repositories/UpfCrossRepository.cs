using System;
using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class UpfCrossRepository : RepositoryBase<UpfCross>, IUpfCrossRepository
    {
        public UpfCrossRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public int GetNextID()
        {
            int identCurrent = (int)DbContext.Database.SqlQuery<decimal>("SELECT IDENT_CURRENT('kpi.UpfCross')").FirstOrDefault();
            return (identCurrent + 1);
        }

        public List<UpfCross> GetByYearMonth(int? year, int? month)
        {
            var items = DbContext.UpfCrosses.Where(t => t.Year == year & t.Month == month).Distinct();
            if (items != null && items.Any())
                return items.ToList();
            return new List<UpfCross>();
        }

        public UpfCross GetByDepartmentYearMonth(int? department, int? year, int? month)
        {
            return DbContext.UpfCrosses.FirstOrDefault(t => t.DepartmentID == department & t.Year == year & t.Month == month);
        }

        public List<UpfCrossInfo> GetUpfCrossByConditions(bool isSpecialUser, int userDepartmentID, string status, string companies, string fromDepartments, string toDepartments, string years, string months)
        {
            string sql = @"SELECT ud.UpfCrossID, ud.ID, ud.Status, u.[Year], u.[Month], ud.FromScore, ud.ToScore, ud.Objective, ud.CreatedBy, fd.CompanyName, fd.FromDepartment, fd.FromName, td.Name AS ToName, tu.ID AS ToUser
                            FROM kpi.UpfCross u
		                        LEFT JOIN kpi.UpfCrossDetail ud ON ud.UpfCrossID = u.ID						
	                            LEFT JOIN ( SELECT d.Id AS FromDepartment, d.Name AS FromName, c.Id AS CompanyID, c.Name AS CompanyName
				                            FROM dbo.Departments d 
					                            LEFT JOIN dbo.Companies c ON c.Id = d.CompanyId
				                            ) fd ON fd.FromDepartment = ud.FromDepartment
	                            LEFT JOIN dbo.Departments td ON td.Id = ud.ToDepartment
		                        LEFT JOIN (SELECT ID, DepartmentID FROM dbo.Users WHERE AdministratorshipID = 5 AND IsActive = 1 AND DeleteFlg = 0) tu ON tu.DepartmentID = ud.ToDepartment
                            WHERE ud.DeleteFlg = 0 ";

            //Conditions
            //Không là user đặc biệt thì chỉ nhìn những KPIs do phòng mình tạo ra hoặc đánh giá phòng mình
            if (!isSpecialUser)
            {
                string departmentCondition = string.Format(" AND (ud.FromDepartment = {0} OR ud.ToDepartment = {0}) ",
                    userDepartmentID);
                sql += departmentCondition;
            }
            if (!string.IsNullOrEmpty(status))
            {
                sql += string.Format(" AND ud.Status = {0} ", status);
            }
            if (!string.IsNullOrEmpty(companies))
            {
                sql += string.Format(" AND fd.CompanyID IN ({0})", companies);
            }
            if (!string.IsNullOrEmpty(fromDepartments))
            {
                sql += string.Format(" AND fd.FromDepartment IN ({0})", fromDepartments);
            }
            if (!string.IsNullOrEmpty(toDepartments))
            {
                sql += string.Format(" AND td.ID IN ({0})", toDepartments);
            }
            if (!string.IsNullOrEmpty(years))
            {
                sql += string.Format(" AND u.Year IN ({0})", years);
            }
            if (!string.IsNullOrEmpty(months))
            {
                sql += string.Format(" AND u.Month IN ({0})", months);
            }

            //Order
            sql += " ORDER BY u.Year, u.Month, fd.FromDepartment ";

            var items = DbContext.Database.SqlQuery<UpfCrossInfo>(sql).ToList<UpfCrossInfo>();
            return items;
        }

        public List<UpfCrossInfo> GetUpfCrossReport(bool isSpecialUser, int userDepartmentId, string companies, string departmentId, string years, string months)
        {
            string sql = @"SELECT ud.UpfCrossID, ud.ID, ud.Status, u.[Year], u.[Month], ud.FromScore, ud.ToScore, ud.Objective, ud.CreatedBy, fd.CompanyName, fd.FromDepartment, fd.FromName, td.Name AS ToName,
                            u.[DependWeight], u.[DependScore], ud.FromWeight, ud.ToDepartment
                            FROM kpi.UpfCross u
		                        LEFT JOIN kpi.UpfCrossDetail ud ON ud.UpfCrossID = u.ID						
	                            LEFT JOIN ( SELECT d.Id AS FromDepartment, d.Name AS FromName, c.Id AS CompanyID, c.Name AS CompanyName
				                            FROM dbo.Departments d 
					                            LEFT JOIN dbo.Companies c ON c.Id = d.CompanyId
				                            ) fd ON fd.FromDepartment = ud.FromDepartment
	                            LEFT JOIN dbo.Departments td ON td.Id = ud.ToDepartment
                            WHERE u.DeleteFlg = 0 AND ud.DeleteFlg = 0 ";

            //Conditions
            //Không là user đặc biệt thì chỉ nhìn những KPIs do phòng mình tạo ra hoặc đánh giá phòng mình
            if (!isSpecialUser)
            {
                string departmentCondition = string.Format(" AND (ud.FromDepartment = {0} OR ud.ToDepartment = {0}) ",
                    userDepartmentId);
                sql += departmentCondition;
            }
            sql += @" AND (u.Status = 2 OR u.Status = 3 OR ud.Status = 1) ";
            if (!string.IsNullOrEmpty(companies))
            {
                sql += string.Format(" AND fd.CompanyID IN ({0})", companies);
            }
            if (!string.IsNullOrEmpty(departmentId))
            {
                sql += string.Format(" AND u.DepartmentID IN ({0})", departmentId);
            }
            if (!string.IsNullOrEmpty(years))
            {
                sql += string.Format(" AND u.Year IN ({0})", years);
            }
            if (!string.IsNullOrEmpty(months))
            {
                sql += string.Format(" AND u.Month IN ({0})", months);
            }

            //Order
            sql += " ORDER BY u.Month, u.Year, u.DepartmentID ";

            var items = DbContext.Database.SqlQuery<UpfCrossInfo>(sql).ToList<UpfCrossInfo>();
            return items;
        }

        public List<UpfCrossInfo> GetUpfCrossDetailByUpfCrossId(string companies, string departmentId, string years, string months)
        {
            string sql = @"SELECT ud.UpfCrossID, ud.ID, ud.Status, fd.FromDepartment, fd.FromName, ud.ContentsRequested, ud.ExpectedTimeOfCompletion, 
                            ud.ExpectedResult, ud.FromWeight, ud.ToDepartment, td.Name AS ToName, ud.TimeOfCompletion, ud.Result, ud.FromScore, ud.PlanToDo, 
                            ud.ExplainationForResults, ud.Solutions, ud.Timeline, ud.ToWeight, ud.ToScore, ud.AssessmentByCouncil, ud.TotalScore, ud.CreatedBy, 
                            ud.Objective, fd.CompanyName
                            FROM kpi.UpfCross u
                            LEFT JOIN kpi.UpfCrossDetail ud on u.ID =  ud.UpfCrossID				
	                        LEFT JOIN ( SELECT d.Id AS FromDepartment, d.Name AS FromName, c.Id AS CompanyID, c.Name AS CompanyName
				                        FROM dbo.Departments d 
					                        LEFT JOIN dbo.Companies c ON c.Id = d.CompanyId
				                        ) fd ON fd.FromDepartment = ud.FromDepartment
	                        LEFT JOIN dbo.Departments td ON td.Id = ud.ToDepartment
                            WHERE u.DeleteFlg = 0 AND ud.DeleteFlg = 0 ";
            if (!string.IsNullOrEmpty(companies))
            {
                sql += string.Format(" AND fd.CompanyID IN ({0})", companies);
            }
            if (!string.IsNullOrEmpty(departmentId))
            {
                sql += string.Format(" AND u.DepartmentID IN ({0})", departmentId);
            }
            if (!string.IsNullOrEmpty(years))
            {
                sql += string.Format(" AND u.Year IN ({0})", years);
            }
            if (!string.IsNullOrEmpty(months))
            {
                sql += string.Format(" AND u.Month IN ({0})", months);
            }
            //Order
            sql += " ORDER BY u.DepartmentID ";

            var items = DbContext.Database.SqlQuery<UpfCrossInfo>(sql).ToList();
            return items;
        }
    }

    public interface IUpfCrossRepository : IRepository<UpfCross>
    {
        int GetNextID();
        List<UpfCross> GetByYearMonth(int? year, int? month);
        UpfCross GetByDepartmentYearMonth(int? department, int? year, int? month);
        List<UpfCrossInfo> GetUpfCrossByConditions(bool isSpecialUser, int userDepartmentID, string status, string companies, string fromDepartments, string toDepartments, string years, string months);
        List<UpfCrossInfo> GetUpfCrossReport(bool isSpecialUser, int userDepartmentId, string companies, string departmentId, string years, string months);
        List<UpfCrossInfo> GetUpfCrossDetailByUpfCrossId(string companies, string departmentId, string years, string months);
    }
}

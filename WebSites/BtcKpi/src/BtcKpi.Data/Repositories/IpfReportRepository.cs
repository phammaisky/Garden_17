using System;
using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class IpfReportRepository : RepositoryBase<IpfReport>, IIpfReportRepository
    {
        public IpfReportRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public int GetNextID()
        {
            int identCurrent = (int)DbContext.Database.SqlQuery<decimal>("SELECT IDENT_CURRENT('kpi.IpfReport')").FirstOrDefault();
            return (identCurrent + 1);
        }

        public List<IpfReport> GetInfoByCondition(string companies, string departments, string years)
        {
            string sql =
                @"SELECT i.[ID] ,i.[UserId] ,i.[Year] ,[BodScore] ,[BodComment] ,[Created] ,[CreatedBy] ,[DeleteFlg] ,[Deleted] ,[DeletedBy] ,[Remark]
                      FROM [BtcKpi].[kpi].[IpfReport] i
	                                                LEFT JOIN ( SELECT u.ID AS UserID, u.FullName, u.DepartmentID, d.CompanyId
				                                                FROM dbo.Users u 
					                                                LEFT JOIN dbo.Departments d ON d.Id = u.DepartmentID
				                                                ) ui ON ui.UserID = i.UserId
                                                WHERE i.DeleteFlg = 0 ";

            //Conditions
            if (!string.IsNullOrEmpty(companies))
            {
                sql += string.Format(" AND ui.CompanyID IN ({0})", companies);
            }
            if (!string.IsNullOrEmpty(departments))
            {
                sql += string.Format(" AND ui.DepartmentID IN ({0})", departments);
            }
            if (!string.IsNullOrEmpty(years))
            {
                sql += string.Format(" AND i.Year IN ({0})", years);
            }
            var items = DbContext.Database.SqlQuery<IpfReport>(sql).ToList<IpfReport>();
            return items;
        }

        public IpfReport GetByUserIdAndYear(int? userId, int? year)
        {
            return DbContext.IpfReports.FirstOrDefault(t => t.UserId == userId & t.Year == year);
        }
    }

    public interface IIpfReportRepository : IRepository<IpfReport>
    {
        int GetNextID();
        List<IpfReport> GetInfoByCondition(string companies, string departments, string years);
        IpfReport GetByUserIdAndYear(int? userId, int? year);
    }
}

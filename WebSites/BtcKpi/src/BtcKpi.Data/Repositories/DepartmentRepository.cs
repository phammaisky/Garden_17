using System;
using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;
using BtcKpi.Model.Enum;

namespace BtcKpi.Data.Repositories
{
    public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<Department> GetDepartmentByRoleId(int roleId)
        {
            var queryItems = (from rd in this.DbContext.RoleDepartments.Where(t =>
                                    t.RoleID == roleId & t.DeleteFlg == (byte?)BtcKpiDeleteFlag.NotDelete)
                              join d in this.DbContext.Departments.Where(r =>
                                  r.DeleteFlg == (byte?)BtcKpiDeleteFlag.NotDelete) on rd.DepartmentID equals d.Id
                              join c in this.DbContext.Companies.Where(f =>
                                      f.DeleteFlg == (byte?)BtcKpiDeleteFlag.NotDelete) on d.CompanyId equals c.Id
                              select new
                              {
                                  DeleteFlg = d.DeleteFlg,
                                  Id = d.Id,
                                  Code = d.Code,
                                  CompanyId = d.CompanyId,
                                  CompanyName = c.Name, //
                                  Name = d.Name,
                                  NameEn = d.NameEn,
                                  ShortName = d.ShortName

                              }).ToList();
            var items = from d in queryItems
                                 select new Department()
                                 {
                                     DeleteFlg = d.DeleteFlg,
                                     Id = d.Id,
                                     Code = d.Code,
                                     CompanyId = d.CompanyId,
                                     CompanyName = d.CompanyName, //
                                     Name = d.Name,
                                     NameEn = d.NameEn,
                                     ShortName = d.ShortName
                                 };
            if (items != null && items.Any())
            {
                return items.ToList();
            }
            return new List<Department>();
        }

        public List<Department> GetDepartmentByCompanyId(int companyId)
        {
            var queryItems = (from c in this.DbContext.Companies.Where(f =>
                            f.DeleteFlg == (byte?)BtcKpiDeleteFlag.NotDelete & f.Id == companyId) 
                              join d in this.DbContext.Departments.Where(r =>
                                  r.DeleteFlg == (byte?)BtcKpiDeleteFlag.NotDelete) on c.Id equals d.CompanyId
                              select new
                              {
                                  DeleteFlg = d.DeleteFlg,
                                  Id = d.Id,
                                  Code = d.Code,
                                  CompanyId = d.CompanyId,
                                  CompanyName = c.Name, //
                                  Name = d.Name,
                                  NameEn = d.NameEn,
                                  ShortName = d.ShortName
                              }).ToList();
            var items = from d in queryItems
                        select new Department()
                        {
                            DeleteFlg = d.DeleteFlg,
                            Id = d.Id,
                            Code = d.Code,
                            CompanyId = d.CompanyId,
                            CompanyName = d.CompanyName, //
                            Name = d.Name,
                            NameEn = d.NameEn,
                            ShortName = d.ShortName
                        };
            if (items != null && items.Any())
            {
                return items.ToList();
            }
            return new List<Department>();
        }

        public List<Department> GetDepartmentCrossByRoleId(int roleId)
        {
            string sql = string.Format(
                @"SELECT d.DeleteFlg, d.Id, d.Code, d.CompanyId, c.Name AS CompanyName, d.Name, d.NameEn, d.ShortName, d.CreatedBy, d.CreatedDate, d.DeleteFlg, d.DeletedBy, d.DeletedDate
                                            FROM dbo.Companies c
	                                            LEFT JOIN dbo.Departments d ON d.CompanyId = c.Id
	                                            WHERE c.Id IN
	                                            (SELECT DISTINCT dp.CompanyId FROM dbo.Departments dp LEFT JOIN dbo.RoleDepartment rd ON rd.DepartmentID = dp.Id WHERE rd.RoleID = {0})
                                                    AND d.DeleteFlg = 0 ",
                                            roleId);
            var items = DbContext.Database.SqlQuery<Department>(sql).ToList<Department>();
            return items;
        }

        public Department GetDepartmentById(int? departmentId)
        {
            var item = DbContext.Departments.FirstOrDefault(w => w.DeleteFlg == 0 && w.Id == departmentId);
            return item;
        }

    }

    public interface IDepartmentRepository : IRepository<Department>
    {
        List<Department> GetDepartmentByRoleId(int roleId);
        List<Department> GetDepartmentByCompanyId(int companyId);
        List<Department> GetDepartmentCrossByRoleId(int roleId);
        Department GetDepartmentById(int? departmentId);
    }
}

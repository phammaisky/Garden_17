using System;
using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;
using BtcKpi.Model.Enum;

namespace BtcKpi.Data.Repositories
{
    public class RoleDepartmentRepository : RepositoryBase<RoleDepartment>, IRoleDepartmentRepository
    {
        public RoleDepartmentRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<RoleDepartment> GetByRoleId(int roleId)
        {
            var queryItems = (from roleDept in this.DbContext.RoleDepartments.Where(t =>
                    t.RoleID == roleId & t.DeleteFlg == 0)
                join department in this.DbContext.Departments.Where(f =>
                        f.DeleteFlg == 0) on roleDept.DepartmentID equals
                    department.Id
                join company in this.DbContext.Companies.Where(f =>
                        f.DeleteFlg == 0) on department.CompanyId equals
                    company.Id
                select new
                {
                    RoleID = roleDept.RoleID,
                    DepartmentID = roleDept.DepartmentID,
                    DeleteFlg = roleDept.DeleteFlg,
                    DepartmentName = department.Name,
                    CompanyID = department.CompanyId,
                    CompanyName = company.Name
                }).ToList();
            var rolesFunctions = from r in queryItems
                                 select new RoleDepartment()
                {
                    RoleID = r.RoleID,
                    DepartmentID = r.DepartmentID,
                    DeleteFlg = r.DeleteFlg,
                    DepartmentName = r.DepartmentName,
                    CompanyID = r.CompanyID,
                    CompanyName = r.CompanyName
                                 };
            if (rolesFunctions != null && rolesFunctions.Any())
            {
                return rolesFunctions.ToList();
            }
            return new List<RoleDepartment>();
        }
    }

    public interface IRoleDepartmentRepository : IRepository<RoleDepartment>
    {
        List<RoleDepartment> GetByRoleId(int roleId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;
using BtcKpi.Model.Enum;

namespace BtcKpi.Data.Repositories
{
    public class RolesFunctionRepository : RepositoryBase<RolesFunction>, IRolesFunctionRepository
    {
        public RolesFunctionRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<RolesFunction> GetByRoleId(int roleId)
        {
            var queryItems = (from roleFunc in this.DbContext.RolesFunctions.Where(t =>
                    t.RoleID == roleId & t.DeleteFlg == (byte?)BtcKpiDeleteFlag.NotDelete) 
                join role in this.DbContext.Roles.Where(r =>
                    r.DeleteFlg == (byte?)BtcKpiDeleteFlag.NotDelete) on roleFunc.RoleID equals role.ID
                join function in this.DbContext.Functions.Where(f =>
                        f.DeleteFlg == (byte?)BtcKpiDeleteFlag.NotDelete) on roleFunc.FunctionID equals
                    function.ID
                select new 
                {
                    CanAdd = roleFunc.CanAdd,
                    CanApprove = roleFunc.CanApprove,
                    CanComment = roleFunc.CanComment,
                    CanDelete = roleFunc.CanDelete,
                    CanEdit = roleFunc.CanEdit,
                    CanView = roleFunc.CanView,
                    RoleID = roleFunc.RoleID,
                    RoleName = role.Name,
                    FunctionID = roleFunc.FunctionID,
                    FunctionName = function.Name,
                    DeleteFlg = roleFunc.DeleteFlg,
                    FuncController = function.FunController,
                    FuncAction = function.FunAction

                }).ToList();
            var rolesFunctions = from rl in queryItems
                select new RolesFunction()
                {
                    CanAdd = rl.CanAdd,
                    CanApprove = rl.CanApprove,
                    CanComment = rl.CanComment,
                    CanDelete = rl.CanDelete,
                    CanEdit = rl.CanEdit,
                    CanView = rl.CanView,
                    RoleID = rl.RoleID,
                    RoleName = rl.RoleName,
                    FunctionID = rl.FunctionID,
                    FunctionName = rl.FunctionName,
                    DeleteFlg = rl.DeleteFlg,
                    FuncController = rl.FuncController,
                    FuncAction = rl.FuncAction
                };
            if (rolesFunctions != null && rolesFunctions.Any())
            {
                return rolesFunctions.ToList();
            }
            return new List<RolesFunction>();
        }
    }

    public interface IRolesFunctionRepository : IRepository<RolesFunction>
    {
        List<RolesFunction> GetByRoleId(int roleId);
    }
}

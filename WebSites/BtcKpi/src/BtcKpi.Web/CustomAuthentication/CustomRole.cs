using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BtcKpi.Model;
using BtcKpi.Service;

namespace BtcKpi.Web
{
    public class CustomRole : RoleProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            var userRoles = GetRolesForUser(username);
            return userRoles.Where(r => r.Contains(roleName)).Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public override string[] GetRolesForUser(string username)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userRoles = new string[] { };

            //HttpContext.Current.User


            //using (AuthenticationDB dbContext = new AuthenticationDB())
            //{
            //    var selectedUser = (from us in dbContext.Users.Include("Roles")
            //                        where string.Compare(us.Username, username, StringComparison.OrdinalIgnoreCase) == 0
            //                        select us).FirstOrDefault();


            //    if(selectedUser != null)
            //    {
            //        userRoles = new[] { selectedUser.Roles.Select(r=>r.RoleName).ToString() };
            //    }

            //    return userRoles.ToArray();
            //}
            var userService = DependencyResolver.Current.GetService<IUserService>();
            string errorMsg = "";
            User currentUser = new User();
            var user = userService.GetUser(username, ref currentUser, out errorMsg);
            if (currentUser != null)
            {
                userRoles = currentUser.RolesFunctions.Select(r => r.FuncController + "/" + r.FuncAction + "-" + Convert.ToInt32(r.CanAdd) + "-" + Convert.ToInt32(r.CanView) + "-" + Convert.ToInt32(r.CanEdit) + "-" + Convert.ToInt32(r.CanDelete) + "-" + Convert.ToInt32(r.CanComment) + "-" + Convert.ToInt32(r.CanApprove)).ToArray();
            }

            return userRoles.ToArray();
        }



        #region Overrides of Role Provider

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }


        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
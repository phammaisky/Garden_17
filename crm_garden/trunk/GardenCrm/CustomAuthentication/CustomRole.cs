using GardenCrm.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Security;

namespace GardenCrm.CustomAuthentication
{
    public class CustomRole : RoleProvider
    {
        #region Properties

        private int _cacheTimeoutInMinutes = 30;

        #endregion

        #region Overrides of RoleProvider

        /// <summary>
        /// Initialize values from web.config.
        /// </summary>
        /// <param name="name">The friendly name of the provider.</param>
        /// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            // Set Properties
            int val;
            if (!string.IsNullOrEmpty(config["cacheTimeoutInMinutes"]) && Int32.TryParse(config["cacheTimeoutInMinutes"], out val))
                _cacheTimeoutInMinutes = val;

            // Call base method
            base.Initialize(name, config);
        }

        /// <summary>
        /// Gets a value indicating whether the specified user is in the specified role for the configured applicationName.
        /// </summary>
        /// <returns>
        /// true if the specified user is in the specified role for the configured applicationName; otherwise, false.
        /// </returns>
        /// <param name="username">The user name to search for.</param><param name="roleName">The role to search in.</param>
        public override bool IsUserInRole(string username, string roleName)
        {
            var userRoles = GetRolesForUser(username);
            return userRoles.Contains(roleName);
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

            //Return if present in Cache
            var cacheKey = string.Format("UserRoles_{0}", username);
            if (HttpRuntime.Cache[cacheKey] != null)
                return (string[])HttpRuntime.Cache[cacheKey];

            var userRoles = new string[] { };

            using (PortalEntities dbContext = new PortalEntities())
            {
                var user = (CustomMembershipUser)Membership.GetUser(username);
                string sqlQuery = string.Format(
                    @"SELECT ucf.UserId, m.Id AS MenuId, ucf.CompanyId AS CompID, NULL AS DeptId, ucf.FunctionId, f.Operator, f.UrlControlAction, f.HasExtend  FROM dbo.Menu m
                                                    LEFT JOIN dbo.[Function] f ON f.MenuId = m.Id
                                                    LEFT JOIN dbo.User_Com_Function ucf ON ucf.FunctionId = f.Id
                                                    WHERE m.AppId IN (SELECT TOP 1 Id FROM dbo.Application WHERE AppName = 'CrmGarden')
                                                    AND ucf.UserId = {0}

                                                    UNION

                                                    SELECT udf.UserId, m.Id AS MenuId, NULL AS CompID, udf.DeptId AS DeptId, udf.FunctionId, f.Operator, f.UrlControlAction, f.HasExtend  FROM dbo.Menu m
                                                    LEFT JOIN dbo.[Function] f ON f.MenuId = m.Id
                                                    LEFT JOIN dbo.User_Dept_Function udf ON udf.FunctionId = f.Id
                                                    WHERE m.AppId IN (SELECT TOP 1 Id FROM dbo.Application WHERE AppName = 'CrmGarden')
                                                    AND udf.UserId = {0}", user.UserId);
                var userFunctions = dbContext.Database.SqlQuery<UserFunction>(sqlQuery).ToList();
                //var deptFunctions =
                //    (from df in dbContext.User_Dept_Function.Where(udf =>
                //            udf.UserId == user.UserId)
                //        select new {Id = df.UserId, Name = df.Function.UrlControlAction}).ToList();
                //var comFunctions =
                //    (from df in dbContext.User_Com_Function.Where(udf =>
                //            udf.UserId == user.UserId)
                //        select new { Id = df.UserId, Name = df.Function.UrlControlAction }).ToList();
                //var userFunctions = deptFunctions.Concat(comFunctions).Distinct();
                if (userFunctions.Any())
                {
                    userRoles = userFunctions.Select(r => r.UrlControlAction).ToArray();
                }
            }

            //Store in cache
            //Store here must be clear while signout
            HttpRuntime.Cache.Insert(cacheKey, userRoles, null, DateTime.Now.AddMinutes(_cacheTimeoutInMinutes), Cache.NoSlidingExpiration);

            // Return
            return userRoles.ToArray();

        }
        #endregion


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
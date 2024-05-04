using System;
using System.Collections.Generic;
using System.Web.Security;
using BtcKpi.Model;

namespace BtcKpi.Web
{
    public class CustomMembershipUser : MembershipUser
    {
        #region User Properties

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string LastName { get; set; }
        public int? AdministratorshipID { get; set; }
        public List<RolesFunction> RolesFunctions { get; set; }
        public List<RoleDepartment> RoleDepartments { get; set; }

        #endregion

        public CustomMembershipUser(User user):base("CustomMembership", user.UserName, user.ID, user.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            UserId = user.ID;
            UserName = user.UserName;
            FullName = user.FullName;
            RolesFunctions = user.RolesFunctions;
            RoleDepartments = user.RolesDepartments;
            AdministratorshipID = user.AdministratorshipID;
        }
    }
}
using System;
using GardenCrm.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace GardenCrm.CustomAuthentication
{
    public class CustomMembershipUser : MembershipUser
    {
        #region User Properties

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public ICollection<Role> Roles { get; set; }

        #endregion

        public CustomMembershipUser(User user):base("CustomMembership", user.Username, user.UserId, user.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            //Roles = user.Roles;
        }

        public CustomMembershipUser(UserInfo user) : base("CustomMembership", user.UserName, user.Id, user.Email, string.Empty, string.Empty, user.Active, user.IsLock, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            UserId = user.Id;
            FirstName = user.FullName;
            LastName = user.Code;
            //Roles = user.Roles;
        }
    }
}
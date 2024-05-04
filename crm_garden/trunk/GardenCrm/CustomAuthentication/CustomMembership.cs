using GardenCrm.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using GardenCrm.Helpers;

namespace GardenCrm.CustomAuthentication
{
    public class CustomMembership : MembershipProvider
    {
        private int _cacheTimeoutInMinutes = 30;

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
        /// 
        /// </summary>
        /// <param name="username">Dùng tên đăng nhập hoặc email</param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override bool ValidateUser(string username, string password)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            using (PortalEntities dbContext = new PortalEntities())
            {
                string encryptPass = EncryptBase64.EncodeBase64(Encoding.UTF8, password);
                //var user = (from us in dbContext.UserInfoes
                //            where (string.Compare(username, us.UserName, StringComparison.OrdinalIgnoreCase) == 0 | string.Compare(username, us.Email, StringComparison.OrdinalIgnoreCase) == 0)
                //            && string.Compare(password, encryptPass, StringComparison.OrdinalIgnoreCase) == 0
                //            && us.Active == true
                //            select us).FirstOrDefault();
                var user = dbContext.UserInfoes.FirstOrDefault(u =>
                    u.UserPass == encryptPass & (u.UserName.ToLower() == username.ToLower() | u.Email.ToLower() == username.ToLower()));

                return (user != null) ? true : false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="isApproved"></param>
        /// <param name="providerUserKey"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username">Lấy theo tên đăng nhập hoặc email</param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var cacheKey = string.Format("UserData_{0}", username);
            if (HttpRuntime.Cache[cacheKey] != null)
                return (CustomMembershipUser)HttpRuntime.Cache[cacheKey];

            using (PortalEntities dbContext = new PortalEntities())
            {
                var user = (from us in dbContext.UserInfoes
                            where string.Compare(username, us.UserName, StringComparison.OrdinalIgnoreCase) == 0 | string.Compare(username, us.Email, StringComparison.OrdinalIgnoreCase) == 0
                            select us).FirstOrDefault();

                if(user == null)
                {
                    return null;
                }

                var selectedUser = new CustomMembershipUser(user);
                //Store in cache
                HttpRuntime.Cache.Insert(cacheKey, selectedUser, null, DateTime.Now.AddMinutes(_cacheTimeoutInMinutes), Cache.NoSlidingExpiration);

                return selectedUser;
            }
        }

        public override string GetUserNameByEmail(string email)
        {
            using (PortalEntities dbContext = new PortalEntities())
            {
                string username = (from u in dbContext.UserInfoes
                                   where string.Compare(email, u.Email) == 0
                                   select u.UserName).FirstOrDefault();

                return !string.IsNullOrEmpty(username) ? username : string.Empty;
            }
        }

        #region Overrides of Membership Provider

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

        public override bool EnablePasswordReset
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
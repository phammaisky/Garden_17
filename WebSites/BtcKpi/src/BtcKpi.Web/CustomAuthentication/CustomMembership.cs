using System;
using System.Web.Mvc;
using System.Web.Security;
using BtcKpi.Service;
using System.Configuration;
using BtcKpi.Model;

namespace BtcKpi.Web
{
    public class CustomMembership : MembershipProvider
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override bool ValidateUser(string username, string password)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            var userService = DependencyResolver.Current.GetService<IUserService>();

            //using (AuthenticationDB dbContext = new AuthenticationDB())
            //{
            //    var user = (from us in dbContext.Users
            //                where string.Compare(username, us.Username, StringComparison.OrdinalIgnoreCase) == 0
            //                && string.Compare(password, us.Password, StringComparison.OrdinalIgnoreCase) == 0
            //                && us.IsActive == true
            //                select us).FirstOrDefault();

            //    return (user != null) ? true : false;
            //}
            string errorMsg = "";
            User currentUser = new User();
            return userService.ValidateUser(ConfigurationManager.AppSettings["AppKey"],username, password, out errorMsg);
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
        /// <param name="username"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var userService = DependencyResolver.Current.GetService<IUserService>();
            string errorMsg = "";
            User currentUser = new User();
            var user = userService.GetUser(username, ref currentUser, out errorMsg);
            var selectedUser = new CustomMembershipUser(currentUser);
            return selectedUser;
        }

        public override string GetUserNameByEmail(string email)
        {
            return "";
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
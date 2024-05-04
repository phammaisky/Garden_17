using System.Linq;
using System.Security.Principal;

namespace BtcKpi.Web
{
    public class CustomPrincipal : IPrincipal
    {
        #region Identity Properties

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
        public int? AdministratorShipId { get; set; }
        #endregion

        public IIdentity Identity
        {
            get; private set;
        }

        public bool IsInRole(string role)
        {
            //Remove Bod prefix
            if (!string.IsNullOrEmpty(role) && role.Contains("Bod"))
            {
                role = role.Replace("Bod", "");
            }
            if (string.IsNullOrEmpty(role))
                return false;
            
            if (role.Contains("Create") || role.Contains("View") || role.Contains("Edit") || role.Contains("Delete") ||
                role.Contains("Comment") || role.Contains("Approve"))
            {
                string actionNotCRUD = "";
                var roleItem = "";
                bool isValid = true;

                if (role.Contains("Create")) { 
                        actionNotCRUD = role.Replace("Create", "");
                        roleItem = Roles.FirstOrDefault(r => r.Contains(actionNotCRUD));
                        isValid = (roleItem == null) ? false : roleItem.Split('-')[1] == "1";
                }
                if (role.Contains("View")) { 
                        actionNotCRUD = role.Replace("View", "");
                        roleItem = Roles.FirstOrDefault(r => r.Contains(actionNotCRUD));
                        isValid = (roleItem == null) ? false : roleItem.Split('-')[2] == "1";
                }
                if (role.Contains("Edit")) { 
                        actionNotCRUD = role.Replace("Edit", "");
                        roleItem = Roles.FirstOrDefault(r => r.Contains(actionNotCRUD));
                        isValid = (roleItem == null) ? false : roleItem.Split('-')[3] == "1";
                }
                if (role.Contains("Delete")) { 
                        actionNotCRUD = role.Replace("Delete", "");
                        roleItem = Roles.FirstOrDefault(r => r.Contains(actionNotCRUD));
                        isValid = (roleItem == null) ? false : roleItem.Split('-')[4] == "1";
                }
                if (role.Contains("Comment")){
                    //actionNotCRUD = role.Replace("Comment", "");
                    //    roleItem = Roles.FirstOrDefault(r => r.Contains(actionNotCRUD));
                    //    isValid = (roleItem == null) ? false : roleItem.Split('-')[5] == "1";
                    isValid = true;
                }
                if (role.Contains("Approve")){ 
                        actionNotCRUD = role.Replace("Approve", "");
                        roleItem = Roles.FirstOrDefault(r => r.Contains(actionNotCRUD));
                        isValid = (roleItem == null) ? false : roleItem.Split('-')[6] == "1";
                }
                return isValid;
            }
            
            return Roles.Any(r => r.ToLower().Contains(role.ToLower()));
        }

        public CustomPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }
    }
}
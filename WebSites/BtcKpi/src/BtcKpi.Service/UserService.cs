using BtcKpi.Data.Infrastructure;
using BtcKpi.Data.Repositories;
using BtcKpi.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BtcKpi.Service.Common;

namespace BtcKpi.Service
{
    // operations you want to expose
    public interface IUserService
    {
        bool ValidateUser(string salt, string userName, string password, out string errorMessage);
        bool GetUser(string userName, ref User user, out string errorMessage);
        User GetUserFullInfo(int userId);
        User GetManagerByUserId(int userId);

        List<Department> GetDepartmentByUser(int userId);
        List<Department> GetDepartmentCrossByUser(int userId);
        List<Department> GetDepartmentByCompany(int companyId);

        void updatePassword(User user, string password);

        List<User> GetListManageInfo(int adminShipID);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository usersRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IRolesFunctionRepository rolesFunctionRepository;
        private readonly IRoleDepartmentRepository roleDepartmentRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly IAdministratorShipRepository administratorShipRepository;
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUserRepository usersRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, IRolesFunctionRepository rolesFunctionRepository, IRoleDepartmentRepository roleDepartmentRepository, IDepartmentRepository departmentRepository, ICompanyRepository companyRepository, IAdministratorShipRepository administratorShipRepository, IUnitOfWork unitOfWork)
        {
            this.usersRepository = usersRepository;
            this.roleRepository = roleRepository;
            this.userRoleRepository = userRoleRepository;
            this.rolesFunctionRepository = rolesFunctionRepository;
            this.roleDepartmentRepository = roleDepartmentRepository;
            this.departmentRepository = departmentRepository;
            this.companyRepository = companyRepository;
            this.administratorShipRepository = administratorShipRepository;
            this.unitOfWork = unitOfWork;
        }

        public void SaveUser()
        {
            unitOfWork.Commit();
        }

        //#endregion
        public bool ValidateUser(string salt, string userName, string password, out string errorMessage)
        {
            errorMessage = "";
            var user = usersRepository.GetUserByName(userName.ToLower());
            if (user == null)
            {
                errorMessage = string.Format("Không tồn tại tài khoản tên: {0}", userName);
                return false;
            }
            else
            {
                ////Get from Ap - Portal
                //UserInfo userInfo = usersRepository.GetUserInfo(user.Email).FirstOrDefault();
                //if (userInfo == null)
                //{
                //    errorMessage = string.Format("Không tồn tại tài khoản tên: {0}", userName);
                //    return false;
                //}

                ////Base64 encrypt
                //string encryptPass = MD5Crypt.EncodeBase64(Encoding.UTF8, password);
                //if (encryptPass != userInfo.UserPass)
                //{
                //    user.AccessFailedCount += 1;
                //    usersRepository.Update(user);
                //    unitOfWork.Commit();

                //    errorMessage = "Sai tên đăng nhập hoặc mật khẩu";
                //    return false;
                //}
                //else
                //{
                //    user.AccessFailedCount = 0;
                //    usersRepository.Update(user);
                //    unitOfWork.Commit();
                //}

                //Password Hasing Process   
                var encodingPasswordString = MD5Crypt.EncodePassword(password, salt);
                //Validate username and password    
                if (encodingPasswordString != user.Password)
                {
                    user.AccessFailedCount += 1;
                    usersRepository.Update(user);
                    unitOfWork.Commit();

                    errorMessage = "Sai tên đăng nhập hoặc mật khẩu";
                    return false;
                }
                else
                {
                    user.AccessFailedCount = 0;
                    usersRepository.Update(user);
                    unitOfWork.Commit();
                }
            }
            return true;
        }

        public bool GetUser(string userName, ref User user, out string errorMessage)
        {
            errorMessage = "";
            user = usersRepository.GetUserByName(userName);
            if (user == null)
            {
                errorMessage = string.Format("Không tồn tại tài khoản tên: {0}", userName);
                return false;
            }
            else
            {
                var userRole = userRoleRepository.GetUserRoleByUserId(user.ID);
                var role = roleRepository.GetById(userRole.RoleID);
                if (role != null)
                {
                    user.RolesFunctions = new List<RolesFunction>();
                    user.RolesDepartments = new List<RoleDepartment>();
                    var rolesFunctions = rolesFunctionRepository.GetByRoleId(role.ID);
                    
                    if (rolesFunctions != null && rolesFunctions.Any())
                    {
                        foreach (var rolesFunction in rolesFunctions)
                        {
                            user.RolesFunctions.Add(rolesFunction);
                        }
                    }
                    user.RolesDepartments = roleDepartmentRepository.GetByRoleId(role.ID);

                    
                }
            }
            return true;
        }

        public User GetUserFullInfo(int userId)
        {
            User user = usersRepository.GetById(userId);
            if (user != null)
            {
                if (user.DepartmentID != null)
                {
                    var department = departmentRepository.GetById((int)user.DepartmentID);
                    user.DepartmentName = department != null ? department.Name : "";
                    user.DepartmentEnName = department != null ? department.NameEn : "";

                    var company = companyRepository.GetById(department.CompanyId);
                    user.CompanyName = company != null ? company.Name : "";
                }
                if (user.AdministratorshipID != null && string.IsNullOrEmpty(user.AdministratorshipName))
                {
                    var administratorShip = administratorShipRepository.GetById((int)user.AdministratorshipID);
                    user.AdministratorshipName = administratorShip != null ? administratorShip.Name : "";
                }
            }
            return user;
        }

        public User GetManagerByUserId(int userId)
        {
            User staff = usersRepository.GetById(userId);
            User user = null;
            if (staff != null && staff.AdministratorshipID != null)
            {
                int adminShipID;
                if (staff.AdministratorshipID > 5)
                {
                    adminShipID = 5; // Dưới trưởng phòng thì quản lý là trưởng phòng
                }
                else if(staff.AdministratorshipID < 2)
                {
                    adminShipID = 2;  // Dưới tổng giám đốc thì quản lý là Tổng giám đốc
                }
                else
                {
                    adminShipID = 1;  // Tổng giám đốc thì quản lý là Chủ tịch
                }
                user = usersRepository.GetAll().Where(u => u.AdministratorshipID == adminShipID).FirstOrDefault();
            }

            if (user != null)
            {
                if (user.DepartmentID != null)
                {
                    var department = departmentRepository.GetById((int)user.DepartmentID);
                    user.DepartmentName = department != null ? department.Name : "";
                }
                if (user.AdministratorshipID != null && string.IsNullOrEmpty(user.AdministratorshipName))
                {
                    var administratorShip = administratorShipRepository.GetById((int)user.AdministratorshipID);
                    user.AdministratorshipName = administratorShip != null ? administratorShip.Name : "";
                }
            }
            else
            {
                user = staff;
            }
            return user;
        }

        public List<Department> GetDepartmentByUser(int userId)
        {
            var userRole = userRoleRepository.GetUserRoleByUserId(userId);
            if (userRole != null)
            {
                var items = departmentRepository.GetDepartmentByRoleId(userRole.RoleID);

                if (items != null && items.Any())
                {
                    var companiesCnt = items.GroupBy(t => t.CompanyId)
                        .OrderBy(group => group.Key)
                        .Select(group => Tuple.Create(group.Key, group.Count())).Count();
                    foreach (var item in items)
                    {
                        if (companiesCnt > 1)
                        {
                            item.Name = item.CompanyName + "-" + item.Name;
                        }
                    }

                    return items;
                }
            }
            return new List<Department>();
        }

        public List<Department> GetDepartmentByCompany(int companyId)
        {
            var items = departmentRepository.GetDepartmentByCompanyId(companyId);

            if (items != null && items.Any())
            {
                return items;
            }

            return new List<Department>();
        }

        public void updatePassword(User user, string password) {
            string decodePassword = MD5Crypt.EncodePassword(password, ConfigurationManager.AppSettings["AppKey"]);
            user.Password = decodePassword;
            usersRepository.Update(user);
            unitOfWork.Commit();
        }

        public List<Department> GetDepartmentCrossByUser(int userId)
        {
            var userRole = userRoleRepository.GetUserRoleByUserId(userId);
            if (userRole != null)
            {
                var items = departmentRepository.GetDepartmentCrossByRoleId(userRole.RoleID);
                if (items != null && items.Any())
                {
                    var companiesCnt = items.GroupBy(t => t.CompanyId)
                        .OrderBy(group => group.Key)
                        .Select(group => Tuple.Create(group.Key, group.Count())).Count();
                    var companies = companyRepository.GetAll();
                    foreach (var item in items)
                    {
                        var company = companies.FirstOrDefault(t => t.Id == item.CompanyId);
                        if (company != null)
                        {
                            item.CompanyName = company.Name;
                            if (companiesCnt > 1)
                            {
                                item.Name = company.Name + "-" + item.Name;
                            }
                        }

                        
                    }

                    return items;
                }
            }
            return new List<Department>();
        }

        public List<User> GetListManageInfo(int adminShipID)
        {
            return usersRepository.GetAll().Where(u => u.DeleteFlg == 0 && u.AdministratorshipID != 0 && u.AdministratorshipID < adminShipID)
                .OrderBy(o=> o.FullName).ToList();

        }
    }
}

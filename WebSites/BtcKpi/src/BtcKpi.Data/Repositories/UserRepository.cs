using System.Collections.Generic;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;
using System.Linq;

namespace BtcKpi.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public User GetUserByName(string userName)
        {
            var user = this.DbContext.Users.Where(c => c.UserName.ToLower() == userName | c.Email.ToLower() == userName).FirstOrDefault();
            if (user != null)
            {
                user.RolesFunctions = new List<RolesFunction>();
                user.RolesDepartments = new List<RoleDepartment>();
            }
            return user;
        }

        public List<UserInfo> GetUserInfo(string userName)
        {
            string sql =
                @"SELECT * FROM Portal.dbo.UserInfo WHERE 1 = 1  ";

            //Conditions
            if (!string.IsNullOrEmpty(userName))
            {
                sql += string.Format(" AND LOWER(UserName) = N'{0}' OR LOWER(Email) = N'{0}' ", userName);
            }

            var items = DbContext.Database.SqlQuery<UserInfo>(sql).ToList<UserInfo>();
            return items;
        }

        public override void Update(User entity)
        {
            base.Update(entity);
        }
        
    }

    public interface IUserRepository : IRepository<User>
    {
        User GetUserByName(string userName);

        List<UserInfo> GetUserInfo(string userName);
    }
}

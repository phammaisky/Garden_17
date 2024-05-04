using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;
using System.Linq;

namespace BtcKpi.Data.Repositories
{
    public class UserRoleRepository : RepositoryBase<UsersRole>, IUserRoleRepository
    {
        public UserRoleRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public UsersRole GetUserRoleByUserId(int userId)
        {
            var user = this.DbContext.UsersRoles.Where(c => c.UserID == userId).FirstOrDefault();

            return user;
        }

        public override void Update(UsersRole entity)
        {
            base.Update(entity);
        }
    }

    public interface IUserRoleRepository : IRepository<UsersRole>
    {
        UsersRole GetUserRoleByUserId(int userId);
    }
}

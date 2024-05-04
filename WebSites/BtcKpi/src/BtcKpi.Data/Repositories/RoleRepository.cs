using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
        
    }

    public interface IRoleRepository : IRepository<Role>
    {
        
    }
}

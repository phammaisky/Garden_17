using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class zPersonalPlanRepository : RepositoryBase<zPersonalPlan>, IzPersonalPlanRepository
    {
        public zPersonalPlanRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

    }

    public interface IzPersonalPlanRepository : IRepository<zPersonalPlan>
    {

    }
}

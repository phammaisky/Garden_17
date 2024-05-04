using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class zIpfDetailRepository : RepositoryBase<zIpfDetail>, IzIpfDetailRepository
    {
        public zIpfDetailRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

    }

    public interface IzIpfDetailRepository : IRepository<zIpfDetail>
    {

    }
}

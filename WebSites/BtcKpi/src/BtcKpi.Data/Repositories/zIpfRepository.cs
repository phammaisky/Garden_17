using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class zIpfRepository : RepositoryBase<zIpf>, IzIpfRepository
    {
        public zIpfRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

    }

    public interface IzIpfRepository : IRepository<zIpf>
    {

    }
}

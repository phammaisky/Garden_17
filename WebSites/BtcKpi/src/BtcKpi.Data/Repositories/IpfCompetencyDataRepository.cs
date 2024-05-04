using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class IpfCompetencyDataRepository : RepositoryBase<IpfCompetencyData>, IIpfCompetencyDataRepository
    {
        public IpfCompetencyDataRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

    }

    public interface IIpfCompetencyDataRepository : IRepository<IpfCompetencyData>
    {

    }
}

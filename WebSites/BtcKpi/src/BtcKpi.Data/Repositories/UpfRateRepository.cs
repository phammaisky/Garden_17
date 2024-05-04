using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class UpfRateRepository : RepositoryBase<UpfRate>, IUpfRateRepository

    {
        public UpfRateRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    }

    public interface IUpfRateRepository : IRepository<UpfRate>
    {
    }
}

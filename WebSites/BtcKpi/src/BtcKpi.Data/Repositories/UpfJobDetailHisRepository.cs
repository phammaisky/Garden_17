using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class UpfJobDetailHisRepository : RepositoryBase<UpfJobDetailHis>, IUpfJobDetailHisRepository
    {
        public UpfJobDetailHisRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

    }

    public interface IUpfJobDetailHisRepository : IRepository<UpfJobDetailHis>
    {

    }
}

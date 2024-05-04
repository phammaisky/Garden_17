using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class UpfPersRewPropHisRepository : RepositoryBase<UpfPersRewProposalHis>, IUpfPersRewPropHisRepository
    {
        public UpfPersRewPropHisRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

    }

    public interface IUpfPersRewPropHisRepository : IRepository<UpfPersRewProposalHis>
    {

    }
}

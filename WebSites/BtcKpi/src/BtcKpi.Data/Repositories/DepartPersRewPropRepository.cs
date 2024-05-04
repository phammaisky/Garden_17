using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;
using BtcKpi.Model.Enum;

namespace BtcKpi.Data.Repositories
{
    public class DepartPersRewPropRepository : RepositoryBase<UpfPersRewProposal>, IDepartPersRewPropRepository
    {
        public DepartPersRewPropRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<UpfPersRewProposal> GetPersRewPropByUpfId(int upfId)
        {
            var items = this.DbContext.UpfPersRewProposals.Where(t => t.UpfID == upfId && t.DeleteFlg == 0).OrderBy(t => t.Order);
            return items.ToList();
        }
    }
    public interface IDepartPersRewPropRepository : IRepository<UpfPersRewProposal>
    {
        List<UpfPersRewProposal> GetPersRewPropByUpfId(int upfId);
    }
}

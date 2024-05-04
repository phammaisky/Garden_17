using System;
using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class UpfCrossDetailRepository : RepositoryBase<UpfCrossDetail>, IUpfCrossDetailRepository
    {
        public UpfCrossDetailRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<UpfCrossDetail> GetByUpfCrossId(int ipfId)
        {
            var items = this.DbContext.UpfCrossDetails.Where(t => t.UpfCrossID == ipfId & t.DeleteFlg == 0).Distinct();
            if (items != null && items.Any())
            {
                return items.ToList();
            }
            return new List<UpfCrossDetail>();
        }
    }

    public interface IUpfCrossDetailRepository : IRepository<UpfCrossDetail>
    {
        List<UpfCrossDetail> GetByUpfCrossId(int ipfId);
    }
}

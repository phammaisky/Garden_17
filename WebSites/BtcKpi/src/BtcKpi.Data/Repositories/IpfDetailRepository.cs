using System;
using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class IpfDetailRepository : RepositoryBase<IpfDetail>, IIpfDetailRepository
    {
        public IpfDetailRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<IpfDetail> GetByIpfId(int ipfId)
        {
            var items = this.DbContext.IpfDetails.Where(t => t.IpfID == ipfId & t.DeleteFlg == 0).Distinct();
            if (items != null && items.Any())
            {
                return items.ToList();
            }
            return new List<IpfDetail>();
        }
    }

    public interface IIpfDetailRepository : IRepository<IpfDetail>
    {
        List<IpfDetail> GetByIpfId(int ipfId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class PersonalPlanRepository : RepositoryBase<PersonalPlan>, IPersonalPlanRepository
    {
        public PersonalPlanRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<PersonalPlan> GetByIpfId(int ipfId)
        {
            var items = this.DbContext.PersonalPlans.Where(t => t.IpfID == ipfId & t.DeleteFlg == 0).Distinct();
            if (items != null && items.Any())
            {
                return items.ToList();
            }
            return new List<PersonalPlan>();
        }
    }

    public interface IPersonalPlanRepository : IRepository<PersonalPlan>
    {
        List<PersonalPlan> GetByIpfId(int ipfId);
    }
}

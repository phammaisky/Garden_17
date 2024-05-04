using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class DepartmentRateRepository : RepositoryBase<UpfRate>, IDepartmentRateRepository
    {
        public DepartmentRateRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<UpfRate> GetUpfRateList()
        {
            var items = this.DbContext.UpfRates.OrderByDescending(t => t.ID);
            return items.ToList();
        }

    }

    public interface IDepartmentRateRepository : IRepository<UpfRate>
    {
        List<UpfRate> GetUpfRateList();
    }
}

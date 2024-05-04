using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class UpfCrossSummaryRepository : RepositoryBase<UpfCrossSummary>, IUpfCrossSummaryRepository

    {
        public UpfCrossSummaryRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public UpfCrossSummary GetUpfCrossSummary(int? departmentId, int? month, int? year)
        {
            var item = this.DbContext.UpfCrossSummaries.FirstOrDefault(t => t.Active == true && t.DepartmentID == departmentId
                                                                            && t.Month == month && t.Year == year);
            return item;
        }

        public UpfCrossSummary GetUpfCrossSummaryByYear(int? departmentId, int? year)
        {
            var item = this.DbContext.UpfCrossSummaries.FirstOrDefault(t => t.Active == true && t.DepartmentID == departmentId && t.Year == year && t.Month == null);
            return item;
        }
    }

    public interface IUpfCrossSummaryRepository : IRepository<UpfCrossSummary>
    {
        UpfCrossSummary GetUpfCrossSummary(int? departmentId, int? month, int? year);
        UpfCrossSummary GetUpfCrossSummaryByYear(int? departmentId, int? year);
    }
}

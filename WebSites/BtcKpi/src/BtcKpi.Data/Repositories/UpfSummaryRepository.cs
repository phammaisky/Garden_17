using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class UpfSummaryRepository : RepositoryBase<UpfSummary>, IUpfSummaryRepository

    {
        public UpfSummaryRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public UpfSummary GetUpfSummaryByDepartYear(int? departmentId, int? year)
        {
            var item = this.DbContext.UpfSummaries.FirstOrDefault(t => t.Active == 1 && t.DepartmentID == departmentId && t.Year == year);
            return item;
        }
    }

    public interface IUpfSummaryRepository : IRepository<UpfSummary>
    {
        UpfSummary GetUpfSummaryByDepartYear(int? departmentId, int? year);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;
using BtcKpi.Model.Enum;

namespace BtcKpi.Data.Repositories
{
    public class TypePerformanceRepository : RepositoryBase<TypePerformance>, ITypePerformanceRepository
    {
        public TypePerformanceRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<TypePerformance> GetListTypePerformance(int projectId)
        {
            if (projectId > 0)
            {
                var items = DbContext.TypePerformances.Where(w => w.DeleteFlg == 0 && w.ProjectId == projectId).ToList();
                return items;
            }
            else
            {
                var items = DbContext.TypePerformances.Where(w => w.DeleteFlg == 0).ToList();
                return items;
            }
        }

        public TypePerformance GetTypePerformanceById(int? typePerformanceId)
        {
            return DbContext.TypePerformances.FirstOrDefault(w => w.DeleteFlg == 0 && w.Id == typePerformanceId);
        }
    }

    public interface ITypePerformanceRepository : IRepository<TypePerformance>
    {
        List<TypePerformance> GetListTypePerformance(int projectId);
        TypePerformance GetTypePerformanceById(int? typePerformanceId);
    }
}

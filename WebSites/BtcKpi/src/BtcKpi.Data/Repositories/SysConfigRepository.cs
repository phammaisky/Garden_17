using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class SysConfigRepository : RepositoryBase<SysConfig>, ISysConfigRepository
    {
        public SysConfigRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public SysConfig GetByCode(string code)
        {
            return DbContext.SysConfigs.FirstOrDefault(t => t.Code == code);
        }

        public List<SysConfig> GetByType(string type)
        {
            var items = DbContext.SysConfigs.Where(t => t.Type == type).Distinct();
            if (items != null && items.Any())
            {
                return items.ToList();
            }
            return new List<SysConfig>();
        }
    }

    public interface ISysConfigRepository : IRepository<SysConfig>
    {
        SysConfig GetByCode(string code);
        List<SysConfig> GetByType(string type);
    }
}

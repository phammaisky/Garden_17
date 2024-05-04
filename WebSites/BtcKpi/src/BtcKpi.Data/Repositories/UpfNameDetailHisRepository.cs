using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class UpfNameDetailHisRepository : RepositoryBase<UpfNameDetailHis>, IUpfNameDetailHisRepository
    {
        public UpfNameDetailHisRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
        public int GetNameDetailHisID()
        {
            int identCurrent = (int)DbContext.Database.SqlQuery<decimal>("SELECT IDENT_CURRENT('kpi.UpfNameDetailHis')").FirstOrDefault();
            return (identCurrent + 1);
        }
    }

    public interface IUpfNameDetailHisRepository : IRepository<UpfNameDetailHis>
    {
        int GetNameDetailHisID();
    }
}

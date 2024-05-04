using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class UpfHisRepository : RepositoryBase<UpfHis>, IUpfHisRepository
    {
        public UpfHisRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public int GetUpfHisID()
        {
            int identCurrent = (int)DbContext.Database.SqlQuery<decimal>("SELECT IDENT_CURRENT('kpi.UpfHis')").FirstOrDefault();
            return (identCurrent + 1);
        }
    }

    public interface IUpfHisRepository : IRepository<UpfHis>
    {
        int GetUpfHisID();
    }
}

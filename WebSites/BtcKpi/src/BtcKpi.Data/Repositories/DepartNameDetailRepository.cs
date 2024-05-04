using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;
using BtcKpi.Model.Enum;

namespace BtcKpi.Data.Repositories
{
    public class DepartNameDetailRepository : RepositoryBase<UpfNameDetail>, IDepartNameDetailRepository
    {
        public DepartNameDetailRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public int GetNameDetailID()
        {
            int identCurrent = (int)DbContext.Database.SqlQuery<decimal>("SELECT IDENT_CURRENT('kpi.UpfNameDetail')").FirstOrDefault();
            return (identCurrent + 1);
        }

        public List<UpfNameDetail> GetNameDetailByUpfId(int upfId)
        {
            var items = this.DbContext.UpfNameDetails.Where(t => t.UpfID == upfId && t.DeleteFlg == 0).OrderBy(t => t.Order);
            return items.ToList();
        }
    }
    public interface IDepartNameDetailRepository : IRepository<UpfNameDetail>
    {
        int GetNameDetailID();

        List<UpfNameDetail> GetNameDetailByUpfId(int upfId);
    }
}

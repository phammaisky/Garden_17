using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;
using BtcKpi.Model.Enum;

namespace BtcKpi.Data.Repositories
{
    public class DepartJobDetailRepository : RepositoryBase<UpfJobDetail>, IDepartJobDetailRepository
    {
        public DepartJobDetailRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<UpfJobDetail> GetJobDetailByNameDetailId(int nameDetailId)
        {
            var items = this.DbContext.UpfJobDetails.Where(t => t.UpfNameDetailID == nameDetailId && t.DeleteFlg == 0).OrderBy(t => t.Order);
            return items.ToList();
        }

    }
    public interface IDepartJobDetailRepository : IRepository<UpfJobDetail>
    {
        List<UpfJobDetail> GetJobDetailByNameDetailId(int nameDetailId);
    }
}

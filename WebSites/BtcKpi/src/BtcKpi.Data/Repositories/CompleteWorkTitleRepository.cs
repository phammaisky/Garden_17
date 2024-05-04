using System;
using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class CompleteWorkTitleRepository : RepositoryBase<CompleteWorkTitle>, ICompleteWorkTitleRepository
    {
        public CompleteWorkTitleRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<CompleteWorkTitle> GetByDeparmentId(int deparmentId)
        {
            var items = this.DbContext.CompleteWorkTitles.Where(t => t.DeleteFlg == 0 &  t.DeparmentID == deparmentId).Distinct();
            //Lấy theo phòng ban
            if (items != null && items.Any())
            {
                return items.ToList();
            }
            //Nếu không có lấy mặc định
            else
            {
                items = this.DbContext.CompleteWorkTitles.Where(t => t.DeleteFlg == 0 & t.CompanyID == 0 & t.DeparmentID == 0).Distinct();
                return items.ToList();
            }
            return new List<CompleteWorkTitle>();
        }
    }

    public interface ICompleteWorkTitleRepository : IRepository<CompleteWorkTitle>
    {
        List<CompleteWorkTitle> GetByDeparmentId(int deparmentId);
    }
}

using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class DepartScheduleRepository : RepositoryBase<UpfSchedule>, IDepartScheduleRepository
    {
        public DepartScheduleRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<UpfSchedule> GetByDepartmentId(int departmentId, int companyId)
        {
            var items = this.DbContext.UpfSchedule.Where(t => t.DeleteFlg == 0 & t.CompanyID == companyId & t.DepartmentID == departmentId).Distinct();
            if (items != null && items.Any())
            {
                return items.ToList();
            }
            else
            {
                items = this.DbContext.UpfSchedule.Where(t => t.DeleteFlg == 0 & t.CompanyID == 0 & t.DepartmentID == 0).Distinct(); // Trường hợp không tìm thấy tìm theo mặc định
                return items.ToList();
            }
            return new List<UpfSchedule>();
        }

        public UpfSchedule GetScheduleById(int? id)
        {
            return this.DbContext.UpfSchedule.FirstOrDefault(w => w.DeleteFlg == 0 && w.ID == id);
        }

    }

    public interface IDepartScheduleRepository : IRepository<UpfSchedule>
    {
        List<UpfSchedule> GetByDepartmentId(int departmentId, int companyId);
        UpfSchedule GetScheduleById(int? id);
    }
}

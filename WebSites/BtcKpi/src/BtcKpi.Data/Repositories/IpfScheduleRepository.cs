using System.Collections.Generic;
using System.Linq;
using BtcKpi.Data.Infrastructure;
using BtcKpi.Model;

namespace BtcKpi.Data.Repositories
{
    public class IpfScheduleRepository : RepositoryBase<IpfSchedule>, IIpfScheduleRepository
    {
        public IpfScheduleRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<IpfSchedule> GetByDepartmentId(int departmentId)
        {
            var items = this.DbContext.IpfSchedules.Where(t => t.DeleteFlg == 0 & t.DepartmentID == departmentId).ToList();
            if (!items.Any())
            {
                items = this.DbContext.IpfSchedules.Where(t => t.DeleteFlg == 0 & t.DepartmentID == 0).ToList(); // Trường hợp không tìm thấy tìm theo mặc định
            }
            return items;
        }

    }

    public interface IIpfScheduleRepository : IRepository<IpfSchedule>
    {
        List<IpfSchedule> GetByDepartmentId(int departmentId);
    }
}

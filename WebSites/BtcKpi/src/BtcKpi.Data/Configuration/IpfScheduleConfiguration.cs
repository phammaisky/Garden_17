using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class IpfScheduleConfiguration : EntityTypeConfiguration<IpfSchedule>
    {
        public IpfScheduleConfiguration()
        {
            ToTable("kpi.IpfSchedule");
        }
    }
}

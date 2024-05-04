using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class UpfScheduleConfiguration : EntityTypeConfiguration<UpfSchedule>
    {
        public UpfScheduleConfiguration()
        {
            ToTable("kpi.UpfSchedule");
        }
    }
}

using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class PerformanceConfiguration : EntityTypeConfiguration<PerformanceLSFB>
    {
        public PerformanceConfiguration()
        {
            ToTable("kpi.PerformanceLSFB");
        }
    }
}

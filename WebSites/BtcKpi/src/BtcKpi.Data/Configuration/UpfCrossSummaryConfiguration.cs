using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class UpfCrossSummaryConfiguration : EntityTypeConfiguration<UpfCrossSummary>
    {
        public UpfCrossSummaryConfiguration()
        {
            ToTable("kpi.UpfCrossSummary");
        }
    }
}

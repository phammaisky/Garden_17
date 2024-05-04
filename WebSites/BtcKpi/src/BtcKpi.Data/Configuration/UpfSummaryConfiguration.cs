using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class UpfSummaryConfiguration : EntityTypeConfiguration<UpfSummary>
    {
        public UpfSummaryConfiguration()
        {
            ToTable("kpi.UpfSummary");
        }
    }
}

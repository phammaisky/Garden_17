using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class UpfCrossDetailConfiguration : EntityTypeConfiguration<UpfCrossDetail>
    {
        public UpfCrossDetailConfiguration()
        {
            ToTable("kpi.UpfCrossDetail");
        }
    }
}

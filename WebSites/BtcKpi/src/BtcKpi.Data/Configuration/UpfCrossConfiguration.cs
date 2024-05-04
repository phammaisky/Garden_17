using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class UpfCrossConfiguration : EntityTypeConfiguration<UpfCross>
    {
        public UpfCrossConfiguration()
        {
            ToTable("kpi.UpfCross");
        }
    }
}

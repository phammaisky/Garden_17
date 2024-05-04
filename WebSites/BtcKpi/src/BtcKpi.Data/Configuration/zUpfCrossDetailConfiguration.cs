using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class zUpfCrossDetailConfiguration : EntityTypeConfiguration<zUpfCrossDetail>
    {
        public zUpfCrossDetailConfiguration()
        {
            ToTable("kpi.zUpfCrossDetail");
        }
    }
}

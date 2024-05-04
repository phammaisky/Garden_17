using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class zUpfCrossConfiguration : EntityTypeConfiguration<zUpfCross>
    {
        public zUpfCrossConfiguration()
        {
            ToTable("kpi.zUpfCross");
        }
    }
}

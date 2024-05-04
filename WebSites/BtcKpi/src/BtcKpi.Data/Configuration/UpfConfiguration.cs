using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class UpfConfiguration : EntityTypeConfiguration<Upf>
    {
        public UpfConfiguration()
        {
            ToTable("kpi.Upf");
        }
    }
}

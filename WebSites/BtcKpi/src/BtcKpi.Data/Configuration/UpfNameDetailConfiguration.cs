using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class UpfNameDetailConfiguration : EntityTypeConfiguration<UpfNameDetail>
    {
        public UpfNameDetailConfiguration()
        {
            ToTable("kpi.UpfNameDetail");
        }
    }
}

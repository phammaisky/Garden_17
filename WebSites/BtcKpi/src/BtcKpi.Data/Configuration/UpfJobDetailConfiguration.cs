using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class UpfJobDetailConfiguration : EntityTypeConfiguration<UpfJobDetail>
    {
        public UpfJobDetailConfiguration()
        {
            ToTable("kpi.UpfJobDetail");
        }
    }
}

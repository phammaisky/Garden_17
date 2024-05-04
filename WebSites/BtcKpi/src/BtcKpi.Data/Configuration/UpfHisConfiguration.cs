using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class UpfHisConfiguration : EntityTypeConfiguration<UpfHis>
    {
        public UpfHisConfiguration()
        {
            ToTable("kpi.UpfHis");
        }
    }
}

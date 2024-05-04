using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class UpfJobDetailHisConfiguration : EntityTypeConfiguration<UpfJobDetailHis>
    {
        public UpfJobDetailHisConfiguration()
        {
            ToTable("kpi.UpfJobDetailHis");
        }
    }
}

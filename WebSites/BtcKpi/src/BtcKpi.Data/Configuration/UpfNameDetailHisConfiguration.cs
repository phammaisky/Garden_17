using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class UpfNameDetailHisConfiguration : EntityTypeConfiguration<UpfNameDetailHis>
    {
        public UpfNameDetailHisConfiguration()
        {
            ToTable("kpi.UpfNameDetailHis");
        }
    }
}

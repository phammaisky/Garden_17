using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class IpfDetailConfiguration : EntityTypeConfiguration<IpfDetail>
    {
        public IpfDetailConfiguration()
        {
            ToTable("kpi.IpfDetail");
        }
    }
}

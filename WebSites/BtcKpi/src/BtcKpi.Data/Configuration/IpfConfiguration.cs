using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class IpfConfiguration : EntityTypeConfiguration<Ipf>
    {
        public IpfConfiguration()
        {
            ToTable("kpi.Ipf");
        }
    }
}

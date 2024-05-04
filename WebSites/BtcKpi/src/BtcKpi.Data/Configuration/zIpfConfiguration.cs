using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class zIpfConfiguration : EntityTypeConfiguration<zIpf>
    {
        public zIpfConfiguration()
        {
            ToTable("kpi.zIpf");
        }
    }
}

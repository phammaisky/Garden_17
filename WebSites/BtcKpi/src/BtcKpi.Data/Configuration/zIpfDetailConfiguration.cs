using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class zIpfDetailConfiguration : EntityTypeConfiguration<zIpfDetail>
    {
        public zIpfDetailConfiguration()
        {
            ToTable("kpi.zIpfDetail");
        }
    }
}

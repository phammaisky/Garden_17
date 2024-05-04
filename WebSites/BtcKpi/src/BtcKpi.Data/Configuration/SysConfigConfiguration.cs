using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class SysConfigConfiguration : EntityTypeConfiguration<SysConfig>
    {
        public SysConfigConfiguration()
        {
            ToTable("dbo.SysConfig");
        }
    }
}

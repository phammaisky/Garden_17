using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class IpfCommentConfiguration : EntityTypeConfiguration<IpfComment>
    {
        public IpfCommentConfiguration()
        {
            ToTable("kpi.IpfComment");
        }
    }
}

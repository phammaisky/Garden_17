using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class UpfCommentConfiguration : EntityTypeConfiguration<UpfComment>
    {
        public UpfCommentConfiguration()
        {
            ToTable("kpi.UpfComment");
        }
    }
}

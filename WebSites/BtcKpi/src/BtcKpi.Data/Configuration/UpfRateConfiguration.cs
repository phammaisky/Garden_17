using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class UpfRateConfiguration : EntityTypeConfiguration<UpfRate>
    {
        public UpfRateConfiguration()
        {
            ToTable("dbo.UpfRate");
        }
    }
}

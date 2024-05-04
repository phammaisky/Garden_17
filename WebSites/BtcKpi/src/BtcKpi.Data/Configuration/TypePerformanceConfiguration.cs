using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class TypePerformanceConfiguration : EntityTypeConfiguration<TypePerformance>
    {
        public TypePerformanceConfiguration()
        {
            ToTable("dbo.TypePerformance");
        }
    }
}

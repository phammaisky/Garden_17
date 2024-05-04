using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class FunctionConfiguration : EntityTypeConfiguration<Function>
    {
        public FunctionConfiguration()
        {
            ToTable("dbo.Functions");
        }
    }
}

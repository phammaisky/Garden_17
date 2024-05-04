using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class CompleteWorkTitleConfiguration : EntityTypeConfiguration<CompleteWorkTitle>
    {
        public CompleteWorkTitleConfiguration()
        {
            ToTable("kpi.CompleteWorkTitle");
        }
    }
}

using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class zPersonalPlanConfiguration : EntityTypeConfiguration<zPersonalPlan>
    {
        public zPersonalPlanConfiguration()
        {
            ToTable("kpi.zPersonalPlan");
        }
    }
}

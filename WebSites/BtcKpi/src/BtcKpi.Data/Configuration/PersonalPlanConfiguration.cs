using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class PersonalPlanConfiguration : EntityTypeConfiguration<PersonalPlan>
    {
        public PersonalPlanConfiguration()
        {
            ToTable("kpi.PersonalPlan");
        }
    }
}

using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class IpfCompetencyDataConfiguration : EntityTypeConfiguration<IpfCompetencyData>
    {
        public IpfCompetencyDataConfiguration()
        {
            ToTable("kpi.IpfCompetencyData");
        }
    }
}

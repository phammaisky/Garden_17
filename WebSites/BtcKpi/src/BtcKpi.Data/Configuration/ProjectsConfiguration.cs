using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class ProjectsConfiguration : EntityTypeConfiguration<Projects>
    {
        public ProjectsConfiguration()
        {
            ToTable("dbo.Projects");
        }
    }
}

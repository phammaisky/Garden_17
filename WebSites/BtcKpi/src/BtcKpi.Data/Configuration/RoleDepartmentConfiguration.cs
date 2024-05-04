using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class RoleDepartmentConfiguration : EntityTypeConfiguration<RoleDepartment>
    {
        public RoleDepartmentConfiguration()
        {
            ToTable("dbo.RoleDepartment");
        }
    }
}

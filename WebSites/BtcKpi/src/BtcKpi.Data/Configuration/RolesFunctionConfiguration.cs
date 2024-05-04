using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class RolesFunctionConfiguration : EntityTypeConfiguration<RolesFunction>
    {
        public RolesFunctionConfiguration()
        {
            ToTable("dbo.RolesFunctions");
        }
    }
}

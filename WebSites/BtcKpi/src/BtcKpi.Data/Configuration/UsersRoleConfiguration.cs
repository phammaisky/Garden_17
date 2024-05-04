using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class UsersRoleConfiguration : EntityTypeConfiguration<UsersRole>
    {
        public UsersRoleConfiguration()
        {
            ToTable("dbo.UsersRoles");
        }
    }
}

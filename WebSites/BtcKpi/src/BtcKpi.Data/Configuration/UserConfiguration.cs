using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("dbo.Users");
            //Ignore(p => p.RolesFunctions);
            //Ignore(p => p.RolesDepartments);
            //Ignore(p => p.DepartmentName);
            //Ignore(p => p.CompanyID);
            //Ignore(p => p.CompanyName);
        }
    }
}

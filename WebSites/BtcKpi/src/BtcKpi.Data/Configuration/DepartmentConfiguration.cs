using BtcKpi.Model;
using System.Data.Entity.ModelConfiguration;

namespace BtcKpi.Data.Configuration
{
    public class DepartmentConfiguration : EntityTypeConfiguration<Department>
    {
        public DepartmentConfiguration()
        {
            ToTable("dbo.Departments");
        }
    }
}

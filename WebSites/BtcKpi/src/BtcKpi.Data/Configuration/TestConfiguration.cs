using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BtcKpi.Model;

namespace BtcKpi.Data.Configuration
{
    public class TestConfiguration : EntityTypeConfiguration<Test>
    {
        public TestConfiguration()
        {
            ToTable("store.Test");
            Property(c => c.Name).IsRequired().HasMaxLength(50);
        }
    }
}

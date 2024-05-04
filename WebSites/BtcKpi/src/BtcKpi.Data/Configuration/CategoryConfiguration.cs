using BtcKpi.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Data.Configuration
{
    public class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {
            ToTable("store.Categories");
            Property(c => c.Name).IsRequired().HasMaxLength(50);
        }
    }
}

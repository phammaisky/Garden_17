using System.ComponentModel.DataAnnotations.Schema;

namespace BtcKpi.Model
{
    public partial class Department
    {
        [NotMapped]
        public string CompanyName { get; set; }
    }
}

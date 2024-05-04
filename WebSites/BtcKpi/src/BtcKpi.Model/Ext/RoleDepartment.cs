using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Model
{
    public partial class RoleDepartment
    {
        [NotMapped]
        public int CompanyID { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }
        [NotMapped]
        public string DepartmentName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Model
{
    public partial class User
    {
        [NotMapped]
        public List<RolesFunction> RolesFunctions { get; set; }
        [NotMapped]
        public List<RoleDepartment> RolesDepartments { get; set; }
        [NotMapped]
        public string DepartmentName { get; set; }
        [NotMapped]
        public int CompanyID { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }
        [NotMapped]
        public string DepartmentEnName { get; set; }

    }
}

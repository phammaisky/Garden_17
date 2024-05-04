using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Model
{
    public partial class RolesFunction
    {
        [NotMapped]
        public string RoleName { get; set; }
        [NotMapped]
        public string FunctionName { get; set; }
        [NotMapped]
        public string FuncController { get; set; }
        [NotMapped]
        public string FuncAction { get; set; }
    }
}

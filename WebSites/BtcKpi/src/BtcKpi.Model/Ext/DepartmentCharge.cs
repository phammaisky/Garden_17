using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Model
{
    public partial class DepartmentCharge
    {
        public Nullable<int> DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<byte> Weight { get; set; }
        public Nullable<decimal> Score { get; set; }
    }
}

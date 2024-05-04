using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Model
{
    public partial class UpfMonthItem
    {
        public Nullable<int> DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public List<UpfMonth> Details { get; set; }
    }
}

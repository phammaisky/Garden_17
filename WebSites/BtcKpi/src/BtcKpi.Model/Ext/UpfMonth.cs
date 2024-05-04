using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Model
{
    public partial class UpfMonth
    {
        public Nullable<int> DepartmentID { get; set; }
        public Nullable<int> ScheduleID { get; set; } //0: Bình quân; -1: YTD; -2: BOD; 1~999: UpfSchedule.ID
        public Nullable<decimal> Score { get; set; }
        public string Rank { get; set; }
    }
}

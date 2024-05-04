using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Model
{
    public partial class IpfScore
    {
        public int ID { get; set; }
        public Nullable<byte> ScheduleType { get; set; }
        public Nullable<int> ScheduleID { get; set; }
        public string ScheduleName { get; set; }
        public Nullable<decimal> Score { get; set; }
        public Nullable<decimal> BodScore { get; set; }
    }
}

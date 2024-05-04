using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtcKpi.Model
{
    public partial class UpfJobDetail
    {
        [NotMapped]
        public string ScheduleTimeString
        {
            get { return Convert.ToDateTime(ScheduledTime).ToString("dd/MM/yyyy"); }
            set { ScheduleTimeString = value; }
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtcKpi.Model
{
    public partial class IpfDetail
    {
        [NotMapped]
        public bool IsWorkTitle;
        [NotMapped]
        public int Stt;

    }
}

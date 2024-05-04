using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtcKpi.Model
{
    public partial class UpfCrossDetail
    {
        [NotMapped]
        public bool IsWorkTitle;
        [NotMapped]
        public int Stt;
        [NotMapped]
        public string FromName;
        [NotMapped]
        public string ToName;
        [NotMapped]
        public byte Seq;

    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtcKpi.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class UpfNameDetailHis
    {
        [Key]
        public int ID { get; set; }
        public Nullable<int> WorkCompleteID { get; set; }
        public int UpfHisID { get; set; }
        public Nullable<byte> Action { get; set; }
        public string Descriptions { get; set; }
        public string NameKPI { get; set; }
        public Nullable<System.DateTime> Updated { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<byte> Order { get; set; }
        [NotMapped]
        public List<UpfJobDetail> JobDetails { get; set; }
    }
}
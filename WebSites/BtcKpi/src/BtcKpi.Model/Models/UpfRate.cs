namespace BtcKpi.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class UpfRate
    {
        [Key]
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<int> CreatedBy { get; set; }
    }
}
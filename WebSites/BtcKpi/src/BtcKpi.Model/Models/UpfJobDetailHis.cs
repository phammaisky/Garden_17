﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BtcKpi.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public partial class UpfJobDetailHis
    {
        [Key]
        public int ID { get; set; }
        public int UpfNameDetailHisID { get; set; }
        public Nullable<byte> Action { get; set; }
        public string Descriptions { get; set; }
        public string JobName { get; set; }
        public Nullable<System.DateTime> ScheduledTime { get; set; }
        public string NumberPlan { get; set; }
        public string PerformResults { get; set; }
        public Nullable<byte> Weight { get; set; }
        public Nullable<decimal> Point { get; set; }
        public Nullable<decimal> ManagePoint { get; set; }
        public Nullable<decimal> BodPoint { get; set; }
        public Nullable<System.DateTime> Updated { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<byte> Order { get; set; }
    }
}
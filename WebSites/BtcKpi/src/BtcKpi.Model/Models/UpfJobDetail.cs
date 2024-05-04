﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations.Schema;

namespace BtcKpi.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public partial class UpfJobDetail
    {
        [Key]
        public int ID { get; set; }
        public int UpfNameDetailID { get; set; }
        public string JobName { get; set; }
        public Nullable<System.DateTime> ScheduledTime { get; set; }
        public string NumberPlan { get; set; }
        public string PerformResults { get; set; }
        public Nullable<byte> Weight { get; set; }
        public Nullable<decimal> Point { get; set; }
        public Nullable<decimal> ManagePoint { get; set; }
        public Nullable<decimal> BodPoint { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<byte> DeleteFlg { get; set; }
        public Nullable<System.DateTime> Deleted { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<byte> Order { get; set; }
        [NotMapped]
        public string NameKPIEdit { get; set; }
        [NotMapped]
        public Nullable<byte> NameDetailID { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtcKpi.Model;

namespace BtcKpi.Web.ViewModels
{
    public class UpfSummaryViewModel : BaseViewModel
    {
        public int ID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<decimal> SumBODPoint { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<byte> Active { get; set; }
        public Nullable<System.DateTime> Updated { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public int UpfId { get; set; }
        public Nullable<int> Month { get; set; }
        public int UpfCrossId { get; set; }
        public int UpfCrossDetailId { get; set; }
        public Nullable<decimal> BodDependPoint { get; set; }
        public Nullable<decimal> BodPerforPoint { get; set; }
        public Nullable<byte> BodDependWeight { get; set; }
        public Nullable<byte> BodPerforWeight { get; set; }
        public Nullable<decimal> TotalYearPoint { get; set; }
    }
}
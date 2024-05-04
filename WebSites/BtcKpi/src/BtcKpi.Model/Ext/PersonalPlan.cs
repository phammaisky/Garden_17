using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtcKpi.Model
{
    public partial class PersonalPlan
    {
        [NotMapped]
        public string CompleteDateString
        {
            get { return Convert.ToDateTime(CompleteDate).ToString("dd/MM/yyyy"); }
            set { CompleteDateString = value; }
        }
    }
}

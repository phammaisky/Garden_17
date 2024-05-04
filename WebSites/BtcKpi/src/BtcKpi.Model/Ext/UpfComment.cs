using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Model
{
    public partial class UpfComment
    {
        [NotMapped]
        public string UserName { get; set; }

        [NotMapped]
        public int Order { get; set; }

        [NotMapped]
        public string AdminDepartName { get; set; }

        [NotMapped]
        public string CreatedDateString
        {
            get { return Convert.ToDateTime(CreatedDate).ToString("dd/MM/yyyy"); }
            set { CreatedDateString = value; }
        }
    }
}

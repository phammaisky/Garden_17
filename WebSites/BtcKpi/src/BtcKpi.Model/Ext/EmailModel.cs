using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Model
{
    public partial class EmailModel
    {
        [Required, Display(Name = "Your name")]
        public string ToName { get; set; }
        [Required, Display(Name = "Your email"), EmailAddress]
        public string ToEmail { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
    }
}

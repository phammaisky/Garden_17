using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Model
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual List<Gadget> Gadgets { get; set; }

        public Category()
        {
            DateCreated = DateTime.Now;
        }
    }
}

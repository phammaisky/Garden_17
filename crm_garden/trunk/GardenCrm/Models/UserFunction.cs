using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GardenCrm.Models
{
    public partial class UserFunction
    {
        public int UserId { get; set; }
        public int MenuId { get; set; }
        public int? CompId { get; set; }
        public int? DeptId { get; set; }
        public int FunctionId { get; set; }
        public string Operator { get; set; }
        public string UrlControlAction { get; set; }
        public bool HasExtend { get; set; }
    }
}
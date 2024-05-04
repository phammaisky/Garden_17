using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BtcKpi.Web.ViewModels
{
    public class BaseViewModel
    {
        public bool FirstLoad { get; set; }
        public string ErrorMessage { get; set; }
    }
}